using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer6
{
	class CUserToken
	{
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, sendBuffer;

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
		}

		public void SetSocket(Socket _s)
		{
			socket = _s;
		}

		public void ClearSocket()
		{
			socket = null;
		}

		//public void ReceiveAsync()
		//{
		//	socket.ReceiveAsync(receiveArgs);
		//}

		//public void SendAsync(int _offset, int _size)
		//{
		//	sendArgs.SetBuffer(_offset, _size);
		//	socket.SendAsync(sendArgs);
		//}
	}

	class Program
	{
		public int capablity;
		public Queue<CUserToken> freeUser = new Queue<CUserToken>();
		
		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private SocketAsyncEventArgs acceptArgs;
		public List<CUserToken> listUser = new List<CUserToken>();

		static void Main(string[] args)
		{
			Console.Title = "Time Server6(pooling)";
			Program _p = new Program();
			_p.StartupServer(100);

			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		private void StartupServer(int _capabilty)
		{
			Console.WriteLine("Time Server Start6 (pooling)");
			//---------------------------------------
			// 풀링화 하기.
			//---------------------------------------
			CUserToken _token;
			capablity = _capabilty;
			for (int i = 0; i < _capabilty; i++)
			{
				_token = new CUserToken(OnReceiveCallback, OnSendCallback);
				freeUser.Enqueue(_token);
			}


			//---------------------------------------
			// 신규유저 받기 전용 소켓을 통해서 등록....
			//---------------------------------------
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(5);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCallback);
			//acceptArgs.AcceptSocket = null;
			//Console.WriteLine("1-1 acceptArgs.AcceptSocket:[" + acceptArgs.AcceptSocket + "]");
			acceptSocket.AcceptAsync(acceptArgs);
			//Console.WriteLine("1-2 acceptArgs.AcceptSocket:[" + acceptArgs.AcceptSocket + "]");



		}

		private void OnAcceptCallback(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client Connect");

			//---------------------------------------
			// 접속한 유저에게서 소켓만 떼어내기. 
			// 접속 콜백 재등록..
			//---------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;
			acceptSocket.AcceptAsync(_acceptArgs);

			////---------------------------------------
			// 유저를 풀에서 꺼내서 연결해주기...
			// 받기 콜백 등록.
			//---------------------------------------
			CUserToken _token = freeUser.Dequeue();
			_token.SetSocket(_clientSocket);
			_token.socket.ReceiveAsync(_token.receiveArgs);
			listUser.Add(_token);

			Console.WriteLine("connect free:{0} use:{1}", freeUser.Count, listUser.Count);
		}

		private void OnReceiveCallback(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;

			//int i = 0;
			//foreach (CUserToken _c in listUser)
			//	Console.WriteLine("{0} = {1} / {2}", i++, _c.socket, _c.socket.Connected );


			//if (_receiveArgs.LastOperation == SocketAsyncOperation.Receive)
			//{
			//	return;
			//}
			///else 
			if (_receiveArgs.BytesTransferred > 0 && _receiveArgs.SocketError == SocketError.Success)
			{
				Socket _socket = _token.socket;
				SocketAsyncEventArgs _sendArgs = _token.sendArgs;

				//---------------------------------------
				// Client -> Socket -> receiveArgs(데이타)
				//---------------------------------------
				int _transfereed = _receiveArgs.BytesTransferred;
				byte[] _readBuffer = new byte[_transfereed];
				Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _readBuffer, 0, _transfereed);
				_socket.ReceiveAsync(_receiveArgs);
				//_token.ReceiveAsync();

				//---------------------------------------
				// Data distribute
				//---------------------------------------
				string _text = Encoding.ASCII.GetString(_readBuffer);
				string _respone = string.Empty;
				if (_text.ToLower().Equals("get time"))
				{
					_respone = " OK Time Server6";
				}
				else
				{
					_respone = " No ....";
				}
				Console.WriteLine(_text);

				byte[] _sendBuffer = Encoding.ASCII.GetBytes(_respone);
				Array.Copy(_sendBuffer, 0, _sendArgs.Buffer, _sendArgs.Offset, _sendBuffer.Length);
				//_token.SendAsync(_sendArgs.Offset, _sendBuffer.Length);
				_sendArgs.SetBuffer(_sendArgs.Offset, _sendBuffer.Length);
				//_socket.SendAsync(_sendArgs);
			}
			else
			{
				_token.socket = null;
				listUser.Remove(_token);
				freeUser.Enqueue(_token);
				Console.WriteLine("release free:{0} use:{1}", freeUser.Count, listUser.Count);
			}
		}

		private void OnSendCallback(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			//Console.WriteLine(" > OnSendCallback");

			//---------------------------------------
			//
			//---------------------------------------

		}
	}
}
