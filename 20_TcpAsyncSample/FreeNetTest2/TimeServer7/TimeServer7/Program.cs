using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer7
{
	class CUserToken
	{
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, sendBuffer, receiveBuffer2, sendBuffer2;

		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveCallback, EventHandler<SocketAsyncEventArgs> _onSendCallback)
		{
			receiveBuffer = new byte[1024];
			receiveArgs = new SocketAsyncEventArgs();
			receiveArgs.Completed += _onReceiveCallback;
			receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
			receiveArgs.UserToken = this;

			sendBuffer = new byte[1024];
			sendArgs = new SocketAsyncEventArgs();
			sendArgs.Completed += _onSendCallback;
			sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			sendArgs.UserToken = this;


			receiveBuffer2 = new byte[1024];
			sendBuffer2 = new byte[1024];
		}

		public void SetSocket(Socket _s)
		{
			socket = _s;
		}
	}
	class Program
	{
		private object lockListUser = new object();
		private int capability;
		public Queue<CUserToken> listFreeUser = new Queue<CUserToken>();

		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private SocketAsyncEventArgs acceptArgs;
		public List<CUserToken> listConnectUser = new List<CUserToken>();

		static void Main(string[] args)
		{
			Console.Title = "Time Server7";
			Program _p = new Program();
			_p.StartupServer(2000);

			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		void StartupServer(int _capability)
		{
			Console.WriteLine("Time Server Start7 (pooling)");
			//---------------------------------------
			// 풀링화 하기.
			//---------------------------------------
			CUserToken _token;
			capability = _capability;
			for (int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveCallback, OnSendCallback);
				listFreeUser.Enqueue(_token);
			}

			//---------------------------------------
			// 신규유저 받기 전용 소켓을 통해서 등록....
			//---------------------------------------
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(5);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCallback);
			acceptSocket.AcceptAsync(acceptArgs);

		}

		void OnAcceptCallback(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client Connect");

			//---------------------------------------
			// 접속한 유저에게서 소켓만 떼어내기. 
			// 접속 콜백 재등록..
			//---------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;
			acceptSocket.AcceptAsync(_acceptArgs);

			//---------------------------------------
			// 유저를 풀에서 꺼내서 연결해주기...
			// 받기 콜백 등록.
			//---------------------------------------
			CUserToken _token = listFreeUser.Dequeue();
			_token.SetSocket(_clientSocket);
			listConnectUser.Add(_token);

			bool _bReceiveRegister = _token.socket.ReceiveAsync(_token.receiveArgs);
			if (_bReceiveRegister)
			{
				//정상등록...
			}
			else
			{
				//등록하자마자 데이타 받음...
				OnReceiveCallback(null, _token.receiveArgs);
			}
			Console.WriteLine("connect free:{0} use:{1}", listFreeUser.Count, listConnectUser.Count);
		}

		void OnReceiveCallback(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;

			//if (_receiveArgs.LastOperation == SocketAsyncOperation.Receive)
			//{
			//	return;
			//}
			///else 
			if(_receiveArgs.BytesTransferred > 0 && _receiveArgs.SocketError == SocketError.Success)
			{
				Socket _socket = _token.socket;
				SocketAsyncEventArgs _sendArgs = _token.sendArgs;

				//---------------------------------------
				// Client -> Socket -> receiveArgs(데이타)
				//---------------------------------------
				int _transferred = _receiveArgs.BytesTransferred;
				Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _token.sendBuffer2, 0, _transferred);
				bool _bReceiveRegister = _socket.ReceiveAsync(_receiveArgs);
				if (_bReceiveRegister)
				{
					//정상등록...
				}
				else
				{
					//등록하자마자 데이타 받음...
					OnReceiveCallback(null, _token.receiveArgs);
				}

				//---------------------------------------
				// Data distribute
				//---------------------------------------
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _transferred);
				string _respone = string.Empty;
				if(_text.ToLower().Equals("get time"))
				{
					_respone = "[C <- S] OK Time Server7";
				}
				else
				{
					_respone = "[C <- S] Fail Time Server7";
				}
				byte[] _sendBuffer = Encoding.ASCII.GetBytes(_respone);
				int _sendSize = _sendBuffer.Length;
				Array.Copy(_sendBuffer, 0, _sendArgs.Buffer, _sendArgs.Offset, _sendSize);
				_sendArgs.SetBuffer(_sendArgs.Offset, _sendSize);

				bool _bSendRegister = _socket.SendAsync(_sendArgs);
				if (_bSendRegister)
				{
					//정상등록...
				}
				else
				{
					//등록하자마자 데이타 받음...
					OnSendCallback(null, _token.sendArgs);
				}
			}
			else
			{
				_token.socket = null;
				lock (lockListUser)
				{
					listConnectUser.Remove(_token);
				}
				listFreeUser.Enqueue(_token);
				Console.WriteLine("release free:{0} use:{1}", listFreeUser.Count, listConnectUser.Count);
			}
		}

		void OnSendCallback(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			//Console.WriteLine(" > OnSendCallback");

		}
	}
}
