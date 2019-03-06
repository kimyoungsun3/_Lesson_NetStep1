using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer12
{
	class Program
	{
		class Protocol
		{
			public const bool DEBUG = false;
			public const bool DEBUG_PACKET = true;
			public const bool DEBUG_PACKET_SHOW = true;
		}

		static void Main(string[] args)
		{
			Console.Title = "TimeServer12";
			Program _p = new Program();
			_p.StartupServer(1000);
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
			Console.WriteLine("ServerTime12 pool Start");
			capability = _capability;

			//유저 받을것 풀링해두기...
			CUserToken _token;
			for (int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
				listFreeUser.Enqueue(_token);
			}

			//------------------------------------
			//신규유저 받기 전용 소켓등록.
			//------------------------------------
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(100);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptAsync);

			//------------------------------------------------------------------------------------------------------------------------------------
			//		이중 비동기 등록 오류...
			//_bAccept = acceptSocket.AcceptAsync(acceptArgs);
			//_bAccept = acceptSocket.AcceptAsync(acceptArgs); <- 이놈이 발생시킴...
			//
			// System.InvalidOperationException: "이 SocketAsyncEventArgs인스턴스를 사용하여 비동기 소켓 작업이 이미 진행되고 있습니다.";
			// System.Net.Sockets.SocketAsyncEventArgs.StartOperationCommon(Socket socket)
			// System.Net.Sockets.Socket.AcceptAsync(SocketAsyncEventArgs e)
			// TimeServer10.Program.StartupServer(Int32 _capability) 파일 D:\devtool\study\study\_Lesson_NetStep1\20_TcpAsyncSample\FreeNetTest2\TimeServer10\TimeServer10\Program.cs:줄 58
			// TimeServer10.Program.Main(String[] args) 파일 D:\devtool\study\study\_Lesson_NetStep1\20_TcpAsyncSample\FreeNetTest2\TimeServer10\TimeServer10\Program.cs:줄 21
			//------------------------------------------------------------------------------------------------------------------------------------
			bool _bAccept = acceptSocket.AcceptAsync(acceptArgs);
			if (_bAccept == false)
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
			//신규접속 유저 신규소켓 > Pool에서 하나 꺼내서 달아주기.
			//신규유저 받는 전용 소켓은 재등록 해주고...
			//---------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			Socket _acceptSocket = acceptSocket;
			_acceptArgs.AcceptSocket = null;
			bool _bAccept = _acceptSocket.AcceptAsync(_acceptArgs);
			if (Protocol.DEBUG) Console.WriteLine((_acceptSocket == acceptSocket) + ":" + (acceptArgs == _acceptArgs));

			if (_bAccept == false)
			{
				//접속대기 등록하자마자 -> 신규유저 바로 접속은 없다
				//
				//50  대기중 발생안함
				//100 대기중 발생안함
				//120 대기중 발생안함.
				Console.WriteLine("[{0}] >>> #### OnAcceptAsync2 > _acceptSocket.AcceptAsync 신규유저받기 후 다시 신규유저 들어옴...", identity);
				OnAcceptAsync(_acceptSocket, _acceptArgs);
			}

			//---------------------------------------
			// pool -> client setting infomation
			//---------------------------------------
			CUserToken _token = listFreeUser.Dequeue();
			//if (_token.bProblemData)
			//{
			//	//_token 문제가 있는놈이다 
			//	//       > 뎅글링 클래스로 나둬버림... 
			//	//       > GC가 호출해감....
			//	//새것으로 다시 뺴오자..~~~
			//	_token = listFreeUser.Dequeue();
			//}

			_token.socket = _clientSocket;
			listConnectUser.Add(_token);
			_token.identityID = identity++;

			//-------------------------------------------------------------------
			//	이중으로 ReceiveAsync 등록시 오류
			//_bReceive = _clientSocket.ReceiveAsync(_token.receiveArgs);
			//_bReceive = _clientSocket.ReceiveAsync(_token.receiveArgs); < -이놈이 발생...
			//System.InvalidOperationException: '"이 SocketAsyncEventArgs 인스턴스를 사용하여 비동기 소켓 작업이 이미 진행되고 있습니다.";'
			//
			// 서버가 오류발생....
			//
			// 클라에 오류가 발생 전달... 클라를 닫으면 아래의 메세지가 그제서야 들어옴...
			//System.InvalidOperationException: "이 SocketAsyncEventArgs인스턴스를 사용하여 비동기 소켓 작업이 이미 진행되고 있습니다.";
			//System.Net.Sockets.SocketAsyncEventArgs.StartOperationCommon(Socket socket)
			//System.Net.Sockets.Socket.ReceiveAsync(SocketAsyncEventArgs e)
			//TimeServer10.Program.OnAcceptAsync(Object _obj, SocketAsyncEventArgs _acceptArgs) 파일 D:\devtool\study\study\_Lesson_NetStep1\20_TcpAsyncSample\FreeNetTest2\TimeServer10\TimeServer10\Program.cs:줄 123
			//System.Net.Sockets.SocketAsyncEventArgs.OnCompleted(SocketAsyncEventArgs e)
			//System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
			//System.Net.Sockets.SocketAsyncEventArgs.FinishOperationSuccess(SocketError socketError, Int32 bytesTransferred, SocketFlags flags)
			//System.Net.Sockets.SocketAsyncEventArgs.CompletionPortCallback(UInt32 errorCode, UInt32 numBytes, NativeOverlapped * nativeOverlapped)
			//System.Threading._IOCompletionCallback.PerformIOCompletionCallback(UInt32 errorCode, UInt32 numBytes, NativeOverlapped * pOVERLAP)
			//-------------------------------------------------------------------

			bool _bReceive = _clientSocket.ReceiveAsync(_token.receiveArgs);
			//_bReceive = _clientSocket.ReceiveAsync(_token.receiveArgs);
			if (_bReceive == false)
			{
				//50  대기중 발생안함
				//100 대기중 발생안함
				//120 대기중 발생안함.
				//등록하자마자 바로 데이타 받음...
				//
				//서버가 과부하 상태에서 신규 유저 접속하자 마자 바로 종료하는 경우...
				//100% 발생함...데이타 대량으로 오고 가는중에 발생....
				// > 접속 종료에 대한 메세지를 받은 것임.. ㅎㅎㅎ
				// 접속 -> 대기중... 음... 바로 -> 바로 종료.... 발생....(종료 메세지)
				Console.WriteLine("[{0}] #### OnAcceptAsync1 > _clientSocket.ReceiveAsync  메세지 받기(1) 등록하자마사 바로 받음.", _token.identityID);
				OnReceiveAsync(_clientSocket, _token.receiveArgs);
			}
			Console.WriteLine("[{0}]connect free:{1} use:{2}", _token.identityID, listFreeUser.Count, listConnectUser.Count);
		}

		void OnReceiveAsync(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			if (Protocol.DEBUG) Console.WriteLine("[{0}] OnReceiveAsync >> socket:{1} BytesTransferred:{2} LastOperation:{3} SocketError:{4}",
				_token.identityID,
				_token.socket.Connected,
				_receiveArgs.BytesTransferred,
				_receiveArgs.LastOperation,
				_receiveArgs.SocketError);

			if (_receiveArgs.LastOperation == SocketAsyncOperation.Receive
				&& _receiveArgs.SocketError == SocketError.Success
				&& _receiveArgs.BytesTransferred > 0)
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

				//--------------------------------------
				//중간에 끼워들어옴... 음...
				if (_socket.Connected == false)
				{
					//갑자기 종료하면 발생함....
					//20개에서도 발생함...
					Disconnect(" >>>> OnReceiveAsync 받을때 소켓꺼짐", _token);
					return;
				}

				if (_bReceive == false)
				{
					//등록하자마사 바로 데이타 받음...
					Console.WriteLine("[{0}] #### OnReceiveAsync > _socket.ReceiveAsync 메세지 받기(2) 등록하자마사 바로 받음.{1}", _token.identityID, _socket.Connected);
					OnReceiveAsync(null, _receiveArgs);
				}

				//Data Parse and Send...
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _transferred);
				if (Protocol.DEBUG_PACKET_SHOW) Console.WriteLine("message:{0}", _text);

				string _response = string.Empty;
				if (Protocol.DEBUG_PACKET) Console.WriteLine(_token.identityID + ":" + _text);
				if (_text.ToLower().Equals("get time"))
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

				//이부분에서 130개를 넘어가면 오류가 메모리 오류...
				//그 근처에서 메모리 오류가 먼저 발생해준다. (메모리는 충분한데... win7내 컴에서 종종 나타난다...)
				//2G까지만 (Person win7)
				//bool _bSend = true;
				//try
				//{
				//	_bSend = _socket.SendAsync(_sendArgs);
				//}
				//catch (Exception _e)
				//{
				//	Console.WriteLine(" >>> OnReceiveAsync _socket.SendAsync _e:{0}", _e);
				//	Disconnect(" <<<< ***보낼려고 하는데 어 아직 안보낸것이 있네...***", _token, true);
				//	return;
				//}
				//if (Protocol.DEBUG) Console.WriteLine("[{0}] _bSend {1} {2}", _token.identityID, _bSend, _socket.Connected);
				//
				//
				//if (_socket.Connected == false)
				//{
				//	Disconnect(" <<<< 보낼때1", _token);
				//	return;
				//}
				//if (_bSend == false)
				//{
				//	Console.WriteLine("[{0}] #### OnReceiveAsync _socket.SendAsync -> 보내기(1) 등록하자마사 바로 받음.{1}", _token.identityID, _socket.Connected);
				//	OnSendAsync(_socket, _sendArgs);
				//}
			}
			else
			{
				Console.WriteLine("OnReceiveAsync [종료인듯] >>> :{0} :{1} :{2} :{3} :{4}",
					_token.identityID,
					_token.socket.Connected,
					_receiveArgs.BytesTransferred,
					_receiveArgs.LastOperation,
					_receiveArgs.SocketError
					);
				Disconnect("정상", _token);
			}
		}

		void Disconnect(string _branch, CUserToken _token, bool _bDestroyAndRemake = false)
		{
			//종료 -> 사용중에서 풀로 돌려주고, 소켓을 클리어한다.
			lock (lockListUser)
			{
				listConnectUser.Remove(_token);
			}

			if (_token.socket != null)
			{
				if (_token.socket.Connected)
				{
					try
					{
						Console.WriteLine("[{0}] >>>>>>> socke is alive and Shutdown.Send _branch:{1}, _token:{2} _bDestroyAndRemake:{3} ", _token.identityID, _branch, _token.identityID, _bDestroyAndRemake);
						_token.socket.Shutdown(SocketShutdown.Send);
						_token.socket.Close();
					}
					catch (Exception _e)
					{
						Console.WriteLine("[{0}] >> remake >> _e:{1}", _token.identityID, _e);
						_bDestroyAndRemake = true;
					}
				}
			}
			_token.socket = null;

			bool _bDebugRemake = false;
			int _debugIdentity = -1;
			if (_bDestroyAndRemake)
			{
				_debugIdentity = _token.identityID;
				_bDebugRemake = true;

				_token.bProblemData = true;//기존것 null처리...
				Console.WriteLine(" ***** Destroy:{0} and Remake:{1} *****", _token.identityID, ++destroyAndRemakeCount);
				_token = null;
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);

			}

			if (!_token.bProblemData)
			{
				listFreeUser.Enqueue(_token);
			}
			else
			{
				Console.WriteLine(" **** [{0}] Problem Data not return Pool *** ", (_bDebugRemake ? _debugIdentity : _token.identityID));
			}
			Console.WriteLine(" [{0}] release branch:{3} free:{1} use:{2}", (_bDebugRemake ? _debugIdentity : _token.identityID), listFreeUser.Count, listConnectUser.Count, _branch);
		}
		int destroyAndRemakeCount = 0;

		void OnSendAsync(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			Console.WriteLine("OnSendAsync");
		}
	}

	class CUserToken
	{
		public bool bProblemData;
		public int identityID;
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, receiveBuffer2;
		public byte[] sendBuffer, sendBuffer2;
		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveCallback, EventHandler<SocketAsyncEventArgs> _onSendCallback)
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

		//public int ReceiveToRead()
		//{
		//	int _transferred = receiveArgs.BytesTransferred;
		//	Array.Copy(receiveArgs.Buffer, receiveArgs.Offset, receiveBuffer2, 0, _transferred);
		//	return _transferred;
		//}

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
