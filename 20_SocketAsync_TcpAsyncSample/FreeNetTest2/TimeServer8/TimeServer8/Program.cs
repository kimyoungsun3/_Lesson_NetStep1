using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer8
{
	class Protocol
	{
		public const bool DEBUG	= false;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "TimeServer8";
			Program _p = new Program();
			_p.StartupServer(100);
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		private int identity = 0;
		private int capability;
		private object lockListUser = new object();
		public Queue<CUserToken> listFreeUser = new Queue<CUserToken>();
		public List<CUserToken> listConnectUser = new List<CUserToken>();

		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private SocketAsyncEventArgs acceptArgs;
		void StartupServer(int _capability)
		{
			Console.WriteLine("ServerTime8 start");
			capability = _capability;

			//---------------------------------------
			// 풀링화 하기.
			//---------------------------------------
			CUserToken _token;
			for(int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
				listFreeUser.Enqueue(_token);
			}

			//---------------------------------------
			// 신규유저 받기 전용 소켓을 통해서 등록....
			//---------------------------------------
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(10);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptAsync);
			bool _bAccept = acceptSocket.AcceptAsync(acceptArgs);
			if(_bAccept == false)
			{
				if(Protocol.DEBUG) Console.WriteLine("@@@@ 접속등록하자마자 > 바로접속");
				OnAcceptAsync(null, acceptArgs);
			}
		}

		void OnAcceptAsync(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client connect");

			//---------------------------------------
			//접속한 유저에게 Pool에서 하나 꺼래서 연결해주기.
			//---------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			Socket _acceptSocket = (Socket)_obj;
			_acceptArgs.AcceptSocket = null;
			acceptSocket.AcceptAsync(_acceptArgs);
			if (Protocol.DEBUG) Console.WriteLine(((Socket)_obj == acceptSocket) + ":" + (acceptArgs == _acceptArgs));

			//---------------------------------------
			//접속에 따른 소켓만 꺼내고 받기 다시 대기모드로 보내버림...
			// 받기 콜백 등록.
			//---------------------------------------
			CUserToken _token = listFreeUser.Dequeue();
			_token.socket = _clientSocket;
			listConnectUser.Add(_token);
			_token.identityID = identity++;

			bool _bReceive = _clientSocket.ReceiveAsync(_token.receiveArgs);
			if(_bReceive)
			{
				// > 메세지 받기 등록 성공 > 메세지 오면 콜백에서 처리.....
			}
			else
			{
				//등록하자마자 바로 데이타 받음...
				if (Protocol.DEBUG) Console.WriteLine("[{0}]@@@@ OnAcceptAsync 메세지 받기(1) 등록하자마사 바로 받음.", _token.identityID);
				OnReceiveAsync(null, _token.receiveArgs);
			}
			Console.WriteLine("[{0}]connect free:{1} use:{2}", _token.identityID, listFreeUser.Count, listConnectUser.Count);
		}

		void OnReceiveAsync(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			if (Protocol.DEBUG) Console.WriteLine("[{0}] >> socket:{1} BytesTransferred:{2} ", _token.identityID, _token.socket.Connected, _receiveArgs.BytesTransferred);

			if (_receiveArgs.BytesTransferred > 0 && _receiveArgs.SocketError == SocketError.Success)
			{
				SocketAsyncEventArgs _sendArgs = _token.sendArgs;
				Socket _socket = _token.socket;

				//---------------------------------------
				// Client -> Socket -> Receive
				//---------------------------------------
				int _transferred = _receiveArgs.BytesTransferred;
				Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _token.receiveBuffer2, 0, _transferred);
				bool _bReceive = _socket.ReceiveAsync(_receiveArgs);
				if (Protocol.DEBUG) Console.WriteLine("[{0}] _bReceive {1} {2}", _token.identityID, _bReceive, _socket.Connected);
				if (_bReceive)
				{
					//메세지 받기 정상등록됨...
				}
				else
				{
					//등록하자마사 바로 데이타 받음...
					if (Protocol.DEBUG) Console.WriteLine("[{0}] @@@@ OnReceiveAsync 메세지 받기(2) 등록하자마사 바로 받음.{1}", _token.identityID, _socket.Connected);
					OnReceiveAsync(null, _receiveArgs);
				}

				//----------------------------------------------
				//Data 분석
				//----------------------------------------------
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _transferred);
				string _response = string.Empty;
				if(_text.ToLower().Equals("get time"))
				{
					_response = "[C <- S] OK Time Server8";
				}
				else
				{
					_response = "[C <- S] Fail";
				}
				byte[] _sendBuffer2 = Encoding.ASCII.GetBytes(_response);
				int _sendSize = _sendBuffer2.Length;
				Array.Copy(_sendBuffer2, 0, _sendArgs.Buffer, _sendArgs.Offset, _sendSize);
				_sendArgs.SetBuffer(_sendArgs.Offset, _sendSize);

				bool _bSend = _socket.SendAsync(_sendArgs);
				if (Protocol.DEBUG) Console.WriteLine("[{0}] _bSend {1} {2}", _token.identityID, _bSend, _socket.Connected);
				if (_bSend)
				{
					//정상등록...
				}
				else
				{

					if (Protocol.DEBUG) Console.WriteLine("[{0}] @@@@ OnReceiveAsync(SendAsync) -> 보내기(1) 등록하자마사 바로 받음.{1}", _token.identityID, _socket.Connected);
					OnSendAsync(_socket, _sendArgs);
				}
			}
			else
			{
				lock (lockListUser)
				{
					listConnectUser.Remove(_token);
				}
				_token.socket = null;
				listFreeUser.Enqueue(_token);
				Console.WriteLine("[{0}] release free:{1} use:{2}", _token.identityID, listFreeUser.Count, listConnectUser.Count);
			}
		}

		void OnSendAsync(object _obj, SocketAsyncEventArgs _sendArgs)
		{

		}
	}

	class CUserToken
	{
		public int identityID;
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, sendBuffer, receiveBuffer2, sendBuffer2;

		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveAsync, EventHandler<SocketAsyncEventArgs> _onSendAsync)
		{
			receiveBuffer = new byte[1024];
			receiveArgs = new SocketAsyncEventArgs();
			receiveArgs.Completed += _onReceiveAsync;
			receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
			receiveArgs.UserToken = this;

			sendBuffer = new byte[1024];
			sendArgs = new SocketAsyncEventArgs();
			sendArgs.Completed += _onSendAsync;
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
}
