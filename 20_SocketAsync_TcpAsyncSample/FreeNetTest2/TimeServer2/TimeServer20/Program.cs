using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TimeServer20
{
	public class Protocol
	{
		public static string title = "Time Server 20(Sample)";
		public const bool DEBUG						= false;
		public const bool DEBUG_PACKET				= false;
		public const bool DEBUG_PACKET_LOOP_SHOW	= false;
		public const int DEBUG_PACKET_LOOP_COUNT	= 100_000;
	}

	class Program
	{
		public int identity, capability;
		public Queue<CUserToken> list_Pool = new Queue<CUserToken>();
		public List<CUserToken> list_Users = new List<CUserToken>();

		Socket acceptSocket;
		SocketAsyncEventArgs acceptArgs;

		static void Main(string[] args)
		{
			Console.Title	= Protocol.title;
			Program _p		= new Program();
			_p.StartupServer(2000);
			while (true)
			{
				//Debug.Log("Program Main Thread Wait");
				string _line = Console.ReadLine();
				if (_line.Equals("q"))
				{
					break;
				}
				else if (_line.Equals("c"))
				{
					Console.Clear();
					continue;
				}
				else if (_line.Equals("info") || _line.Equals("i"))
				{
					_p.DisplayInfo();
				}
			}
		}

		void DisplayInfo()
		{
			Debug.Log($" 접속자:{identity} list_Pool:{list_Pool.Count} list_Users:{list_Users.Count}");
		}

		void StartupServer(int _capability)
		{
			Debug.Log(Protocol.title);

			capability = _capability;
			CUserToken _token;
			for(int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
				list_Pool.Enqueue(_token);
			}

			//accept Socket
			acceptArgs				= new SocketAsyncEventArgs();
			acceptArgs.Completed	+= new EventHandler<SocketAsyncEventArgs>(OnAcceptAsync);
			acceptArgs.AcceptSocket	= null;

			acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(100);

			bool _isAcceptAsync = acceptSocket.AcceptAsync(acceptArgs);
			if(_isAcceptAsync == false)
			{
				Debug.Log(" StartupServer #### >> acceptSocket.AcceptAsync Die or Other error");
				OnAcceptAsync(acceptSocket, acceptArgs);
			}
		}

		void OnAcceptAsync(Object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Interlocked.Increment(ref identity);
			int _identity = identity;

			if (Protocol.DEBUG)
				Debug.Log($"[{identity}]OnAcceptAsync (callback) New Client Connect"
				   + $"\n   >> acceptSocket:{((Socket)_obj).Connected}"
				   //+ $" clientSocket:{_acceptArgs.AcceptSocket.Connected}"
				   + $" BytesTransferred:{_acceptArgs.BytesTransferred}"
				   + $" LastOperation:{_acceptArgs.LastOperation}"
				   + $" SocketError:{_acceptArgs.SocketError}");


			//---------------------------------------
			// acceptSocket -> new Client Socket -> New Client
			// acceptSocket -> Accept Register.
			//---------------------------------------
			Socket _acceptSocket		= acceptSocket;
			Socket _clientSocket		= _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket	= null;

			bool _isAcceptAsync = _acceptSocket.AcceptAsync(_acceptArgs);
			if (_isAcceptAsync == false)
			{
				Debug.Log($" [{identity}]OnAcceptAsync #### >> acceptSocket.AcceptAsync Die or Other error");
				OnAcceptAsync(_acceptSocket, _acceptArgs);
			}

			//---------------------------------------
			// Data				<- UserFree pool
			// UserConnect		<- Data
			// Register			<- clientSocket.ReceiveAsync(receiveArgs)
			//---------------------------------------
			CUserToken _token	= list_Pool.Dequeue();
			_token.socket		= _clientSocket;
			_token.identityID	= _identity;
			list_Users.Add(_token);

			bool _isReceiveAsync = _clientSocket.ReceiveAsync(_token.receiveArgs);
			if (_isReceiveAsync == false)
			{
				//등록하자마자 바로 데이타 받음...
				//
				// 상황1
				// 서버가 과부하 상태에서 신규 유저 접속하자 마자 바로 종료하는 경우...
				// 100% 발생함...데이타 대량으로 오고 가는중에 발생....
				// > 접속 종료에 대한 메세지를 받은 것임.. ㅎㅎㅎ
				// 접속 -> 대기중... 음... 바로 -> 바로 종료.... 발생....(종료 메세지)
				Debug.Log($" [{_identity}]OnAcceptAsync @@@@ >> 신규유저.ReceiveAsync 바로받음 {_token.socket.Connected}/{_token.receiveArgs.BytesTransferred}/{_token.receiveArgs.LastOperation}/{_token.receiveArgs.SocketError}");
				OnReceiveAsync(_clientSocket, _token.receiveArgs);
				Debug.Log($"[{_identity}]connect free:{list_Pool.Count} use:{list_Users.Count}");
			}
		}

		void OnConnectAsync(Object _obj, SocketAsyncEventArgs _connectArgs)
		{

		}

		void OnReceiveAsync(Object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token	= _receiveArgs.UserToken as CUserToken;
			int _identityID		= _token.identityID;
			int _debugWorkNum	= _token.GetWorkNum();
			if (Protocol.DEBUG) Debug.Log($"[{_identityID}/{_debugWorkNum}]OnReceiveAsync (callback) "
				+ $" \n   >> socket:{_token.socket.Connected} "
				+ $"BytesTransferred:{_receiveArgs.BytesTransferred} "
				+ $"LastOperation:{_receiveArgs.LastOperation} "
				+ $"SocketError:{_receiveArgs.SocketError}");

			if(_receiveArgs.LastOperation == SocketAsyncOperation.Receive
				&& _receiveArgs.SocketError == SocketError.Success
				&& _receiveArgs.BytesTransferred > 0)
			{
				//read byte
				Socket _clientSocket			= _token.socket;
				SocketAsyncEventArgs _sendArgs	= _token.sendArgs;

				//---------------------------------------
				// Client -> Socket -> (callback) -> OnReceiveAsync
				// Data Parsing
				//---------------------------------------
				int _byteTransferred = _receiveArgs.BytesTransferred;
				Buffer.BlockCopy(_receiveArgs.Buffer, _receiveArgs.Offset, _token.receiveBuffer2, 0, _byteTransferred);
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _byteTransferred);
				if (Protocol.DEBUG_PACKET)
					Debug.Log($"[{_identityID}/{_debugWorkNum}] _transferred:{_byteTransferred} >> {_text}");

				//-------------------------------
				// SendAsync....
				// Message Queue에 넣어두기... (보내는 byte[]가 반드시 다른 버퍼)
				//-------------------------------
				_token.SendCode(Encoding.ASCII.GetBytes(_text), _debugWorkNum);

				//-------------------------------
				//ReceiveAsync...
				//-------------------------------
				bool _isReceiveAsync = _clientSocket.ReceiveAsync(_receiveArgs);
				if(_isReceiveAsync == false)
				{
					//fail -> client socket is error
					Debug.Log($" [{_identityID}/{_debugWorkNum}]OnReceiveAsync .ReceiveAsync 재등록후(소켓꺼짐) "
						+ $"{_token.socket.Connected}/{_receiveArgs.BytesTransferred}"
						+ $"/{_receiveArgs.LastOperation}/{_receiveArgs.SocketError}");

					if (_token.socket.Connected == false)
					{
						Debug.Log(" >> Disconnect ");
						Disconnect("[정상종료2]", _token);

					}
					else
					{
						Debug.Log($" [{_identityID}] #### OnReceiveAsync > _socket.ReceiveAsync 메세지 받기(2) 등록하자마사 바로 받음.{_clientSocket.Connected}");
						OnReceiveAsync(_clientSocket, _receiveArgs);
					}
				}
			}
			else
			{
				//error 
				Debug.Log($"[{_identityID}/{_debugWorkNum}]OnReceiveAsync [정상종료] >>> "
					+ $"{_token.socket.Connected} / {_receiveArgs.BytesTransferred} : {_receiveArgs.LastOperation} : {_receiveArgs.SocketError}");
				Disconnect("[정상종료1]", _token);
			}
		}

		void OnSendAsync(Object _obj, SocketAsyncEventArgs _sendArgs)
		{
			CUserToken _token	= _sendArgs.UserToken as CUserToken;
			int _identityID		= _token.identityID;
			int _debugWorkNum	= _token.GetWorkNum();
			if (Protocol.DEBUG)
				Debug.Log($"[{_identityID}/{_debugWorkNum}]OnSendAsync (callback) \n   >> socket:{_token.socket.Connected} BytesTransferred:{_sendArgs.BytesTransferred} LastOperation:{_sendArgs.LastOperation} SocketError:{_sendArgs.SocketError}");


			if(_sendArgs.LastOperation == SocketAsyncOperation.Send
				&& _sendArgs.SocketError == SocketError.Success
				&& _sendArgs.BytesTransferred > 0)
			{
				_token.SendCode_CheckAndResend(_debugWorkNum);
			}
			else
			{
				Debug.Log($"[{_identityID}/{_debugWorkNum}]OnSendAsync [정상종료] >>> :{_token.socket.Connected} :{_sendArgs.BytesTransferred} :{_sendArgs.LastOperation} :{_sendArgs.SocketError}");
				//종료는 받는 쪽에서 처리하는 것으로...
				//Disconnect("[정상종료1]", _token);
			}
		}

		void Disconnect(string _msg, CUserToken _token)
		{
			lock (list_Users)
			{
				list_Users.Remove(_token);
			}

			lock (list_Pool)
			{
				list_Pool.Enqueue(_token);
			}
			Debug.Log($" [{_token.identityID}] release {_msg} free:{list_Pool.Count} use:{list_Users.Count}");
			_token.OnRemoved();
		}

	}

	class CUserToken
	{
		public int identityID, workNum;
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, receiveBuffer2, sendBuffer, sendBuffer2;
		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveAsync, EventHandler<SocketAsyncEventArgs> _onSendAsync)
		{
			receiveBuffer			= new byte[1024];
			receiveBuffer2			= new byte[1024];
			receiveArgs				= new SocketAsyncEventArgs();
			receiveArgs.Completed	+= _onReceiveAsync;
			receiveArgs.UserToken	= this;
			receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

			sendBuffer				= new byte[1024];
			sendBuffer2				= new byte[1024];
			sendArgs				= new SocketAsyncEventArgs();
			sendArgs.Completed		+= _onSendAsync;
			sendArgs.UserToken		= this;
			sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
		}

		public void OnRemoved()
		{
			identityID	= -1;
			workNum		= -1;
			try
			{
				socket.Disconnect(false);
			}
			catch (Exception _e)
			{
			}
			finally{
				socket.Close();
			}
			socket		= null;
		}

		public void SetSocket(Socket _s) { socket = _s; }
		public int GetWorkNum()
		{
			Interlocked.Increment(ref workNum);
			return workNum;
		}

		Queue<byte[]> sendQueue = new Queue<byte[]>();
		public void SendCode(byte[] _data, int _debugWorkNum = -1)
		{
			if (Protocol.DEBUG_PACKET)
				Debug.Log($"[{identityID}/{_debugWorkNum} SendMessage >> sendQueue.Count:{sendQueue.Count}");

			lock (sendQueue)
			{
				bool _isSending = sendQueue.Count > 0;
				sendQueue.Enqueue(_data);
				if (_isSending)
				{
					//전송중이시네요.... 입력만 해두기....
				}
				else
				{
					//전송중인것이 없네요.... >>. 전송하자~
					if (Protocol.DEBUG_PACKET)
						Debug.Log(" >> 전송 등록하러가기");
					SendCode_Sending(_debugWorkNum);
				}
			}
		}

		//OnSendAsync에서 호출.....
		public void SendCode_CheckAndResend(int _debugWorkNum = -1)
		{
			lock (sendQueue)
			{
				//1. 하나 뺴서 버린다....
				//2. 남아 있는가 확인한다...
				sendQueue.Dequeue();
				if(sendQueue.Count > 0)
				{
					if (sendQueue.Count > 1)
						Debug.Log("대기명령:" + sendQueue.Count);
					SendCode_Sending(_debugWorkNum);
				}
			}
		}

		public void SendCode_Sending(int _debugWorkNum)
		{
			//보낼 메세지를 sendArgs에 복사해서 보내기 등록을 한다...
			if (Protocol.DEBUG_PACKET)
				Debug.Log($"[{identityID}/{_debugWorkNum}]SendCode_Sending >> ");

			byte[] _data = sendQueue.Peek();
			Buffer.BlockCopy(_data, 0, sendArgs.Buffer, sendArgs.Offset, _data.Length);
			sendArgs.SetBuffer(sendArgs.Offset, _data.Length);

			bool _isSendAsync = socket.SendAsync(sendArgs);
			if(_isSendAsync == false)
			{
				// 보내기 등록 오류는 .... (socket killed) 
				// 1. 큐만 클리어해주고....
				// 2. 여기서 처리 안하고 OnReceiveAsync에서 처리하자..
				Debug.Log(" [{identityID}/{_debugWorkNum}]SendCode_Sending @@@@ >> .SendAsync 보내기 등록 >> 소켓죽음...");
				sendQueue.Clear();
				//.ReceiveAsynce에서 처리 하기 때문에 여기는 필요는 없다...
				//if(socket.Connected == false)
				//{
				//   //자원정리...
				//   //Disconnect(" <<<< 보낼때1", _token);
				//}
				//else
				//{
				//   //OnSendAsync(socket, sendArgs);
				//}
			}
		}
		//object obj = new object();
		//object obj2 = new object();
		//Queue<byte[]> queue = new Queue<byte[]>();
		//public void Fun()
		//{
		//   lock (obj)
		//   {
		//      queue.Enqueue(new byte[12]);
		//   }
		//}

		//public void Fun2()
		//{
		//   lock (obj2)
		//   {
		//      queue.Dequeue();
		//   }
		//}
	}
}