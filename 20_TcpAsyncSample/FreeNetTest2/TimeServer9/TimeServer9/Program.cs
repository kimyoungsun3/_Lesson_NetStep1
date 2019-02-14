using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer9
{
	class Program
	{
		class Protocol
		{
			public const bool DEBUG = false;
		}

		static void Main(string[] args)
		{
			Console.Title = "TimeServer9";
			Program _p = new Program();
			_p.StartupServer(100);
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		int capability;
		int identity = 0;
		private object lockListUser = new object();
		public Queue<CUserToken> listFreeUser = new Queue<CUserToken>();
		public List<CUserToken> listConnectUser = new List<CUserToken>();

		Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs acceptArgs;
		void StartupServer(int _capability)
		{
			Console.WriteLine("ServerTime9 Start");
			capability = _capability;

			//유저 받을것 풀링해두기...
			CUserToken _token;
			for(int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
				listFreeUser.Enqueue(_token);
			}

			//------------------------------------
			//신규유저 받기 전용 소켓등록.
			//------------------------------------
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(10);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptAsync);
			bool _bAccept = acceptSocket.AcceptAsync(acceptArgs);
			if(_bAccept == false)
			{
				//50 대기중 발생안함
				//100 대기중 발생안함
				//120 대기중 발생안함.
				Console.WriteLine("@@@@ 신규유저접속받기 등록하자마사 > 바로접속");
				OnAcceptAsync(acceptSocket, acceptArgs);
			}
		}

		void OnAcceptAsync(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("New Client connect");

			//---------------------------------------
			//접속한 유저에게 Pool에서 하나 꺼래서 연결해주기.
			//신규유저 받는 전용 소켓은 재등록 해주고...
			//---------------------------------------
			Socket _acceptSocket = (Socket)_obj;
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;    //must clear....
			if (Protocol.DEBUG) Console.WriteLine((_acceptSocket == acceptSocket) + ":" + (acceptArgs == _acceptArgs));

			bool _bAccept = _acceptSocket.AcceptAsync(_acceptArgs);
			//접속대기 등록하자마자 -> 신규유저 바로 접속은 없다
		
			//50 대기중 발생안함
			//100 대기중 발생안함
			//120 대기중 발생안함.
			if (_bAccept == false)
			{
				Console.WriteLine(" >>> #### 신규유저받기 등록하자마자 바로옴... > 재귀호출...");
				OnAcceptAsync(_acceptSocket, _acceptArgs);
			}

			//---------------------------------------
			// pool -> client setting infomation
			//---------------------------------------
			CUserToken _token = listFreeUser.Dequeue();
			listConnectUser.Add(_token);
			_token.socket = _clientSocket;
			_token.identityID = identity++;
			bool _bReceive = _clientSocket.ReceiveAsync(_token.receiveArgs);
			if(_bReceive == false)
			{
				//등록하자마자 바로 데이타 받음...
				Console.WriteLine("[{0}]@@@@ OnAcceptAsync 메세지 받기(1) 등록하자마사 바로 받음.", _token.identityID);
				OnReceiveAsync(_clientSocket, _token.receiveArgs);
			}
			Console.WriteLine("[{0}]connect free:{1} use:{2}", _token.identityID, listFreeUser.Count, listConnectUser.Count);
		}

		void OnReceiveAsync(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			if (Protocol.DEBUG) Console.WriteLine("[{0}] >> socket:{1} BytesTransferred:{2} ", _token.identityID, _token.socket.Connected, _receiveArgs.BytesTransferred);

			if (_receiveArgs.LastOperation == SocketAsyncOperation.Receive
				&& _receiveArgs.SocketError == SocketError.Success 
				&& _receiveArgs.BytesTransferred > 0)
			{
				SocketAsyncEventArgs _sendArgs = _token.sendArgs;
				Socket _socket = _token.socket;

				//---------------------------------------
				// Client -> Socket -> Receive
				//---------------------------------------
				int _transferred = _token.ReceiveToRead();
				bool _bReceive = _socket.ReceiveAsync(_receiveArgs);
				if (Protocol.DEBUG) Console.WriteLine("[{0}] _bReceive {1} {2}", _token.identityID, _bReceive, _socket.Connected);

				if (_socket.Connected == false)
				{
					//갑자기 종료하면 발생함....
					//20개에서도 발생함...
					Disconnect("받을때1", _token);
					return;
				}
				if (_bReceive == false)
				{
					//등록하자마사 바로 데이타 받음...
					if (Protocol.DEBUG) Console.WriteLine("[{0}] @@@@ OnReceiveAsync 메세지 받기(2) 등록하자마사 바로 받음.{1}", _token.identityID, _socket.Connected);
					OnReceiveAsync(_socket, _receiveArgs);
				}

				//Data Parse and Send...
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _transferred);
				string _response = string.Empty;
				if(_text.ToLower().Equals("get time"))
				{
					_response = "[C <- S] OK Time Server9";
				}
				else
				{
					_response = "[C <- S] Fail";
				}
				byte[] _sendBuffer2 = Encoding.ASCII.GetBytes(_response);
				int _sendSize = _sendBuffer2.Length;
				Array.Copy(_sendBuffer2, 0, _sendArgs.Buffer, _sendArgs.Offset, _sendSize);
				_sendArgs.SetBuffer(_sendArgs.Offset, _sendSize);


				bool _bSend = false;
				try
				{
					//이부분에서 130개를 넘어가면 오류가 발생하기 한다...
					// 왜일까? 
					//그 근처에서 메모리 오류가 먼저 발생해준다. (메모리는 충분한데... win7내 컴에서 종종 나타난다...)
					// 이원이일까? 
					//
					_socket.SendAsync(_sendArgs);
				}catch(Exception _ee)
				{
					Console.WriteLine(" >>> e:" + _ee);
					Disconnect("받을때2", _token);
					return;
				}
				if (Protocol.DEBUG) Console.WriteLine("[{0}] _bSend {1} {2}", _token.identityID, _bSend, _socket.Connected);
				if(_bSend == false)
				{
					if (_socket.Connected == false)
					{
						Disconnect("보낼때", _token);
						return;
					}
					else
					{
						if (Protocol.DEBUG) Console.WriteLine("[{0}] @@@@ OnReceiveAsync(SendAsync) -> 보내기(1) 등록하자마사 바로 받음.{1}", _token.identityID, _socket.Connected);
						OnSendAsync(_socket, _sendArgs);
					}
				}
			}
			else
			{
				Disconnect("정상", _token);
			}
		}

		void Disconnect(string _branch, CUserToken _token)
		{
			//종료 -> 사용중에서 풀로 돌려주고, 소켓을 클리어한다.
			lock (lockListUser)
			{
				listConnectUser.Remove(_token);
			}

			bool _bPass = false;
			if (_token.socket != null)
			{
				if (_token.socket.Connected)
				{
					try
					{
						Console.WriteLine(" >>>>>>> socke is alive and Shutdown.Send ");
						_token.socket.Shutdown(SocketShutdown.Send);
					}
					catch (Exception _e)
					{
						Console.WriteLine(" >> _e:" + _e);
					}
					_token.socket.Close();
				}
			}
			_token.socket = null;
			listFreeUser.Enqueue(_token);
			Console.WriteLine("[{0}] release branch:{3} free:{1} use:{2}", _token.identityID, listFreeUser.Count, listConnectUser.Count, _branch);
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
		public byte[] receiveBuffer, receiveBuffer2;
		public byte[] sendBuffer, sendBuffer2;
		public CUserToken (EventHandler<SocketAsyncEventArgs> _onReceiveCallback, EventHandler<SocketAsyncEventArgs> _onSendCallback)
		{
			receiveBuffer = new byte[1024];
			receiveBuffer2 = new byte[1024];
			receiveArgs = new SocketAsyncEventArgs();
			receiveArgs.Completed += _onReceiveCallback;
			receiveArgs.UserToken = this;
			receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

			sendBuffer = new byte[1024];
			sendBuffer2 = new byte[1024];
			sendArgs = new SocketAsyncEventArgs();
			sendArgs.Completed += _onSendCallback;
			sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			sendArgs.UserToken = this;
		}

		public int ReceiveToRead()
		{
			int _transferred = receiveArgs.BytesTransferred;
			Array.Copy(receiveArgs.Buffer, receiveArgs.Offset, receiveBuffer2, 0, _transferred);
			return _transferred;
		}

		public string TempGetMessage(int _transferred)
		{
			return Encoding.ASCII.GetString(receiveBuffer2, 0, _transferred);
		}


		//public void SetSocket(Socket _s)
		//{
		//	socket = _s;
		//}
	}
}
