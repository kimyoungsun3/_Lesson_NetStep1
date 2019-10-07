using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer14
{
	class Protocol
	{
		public static string title = "Time Server 14(Sample)";
		public const bool DEBUG = true;
		public const bool DEBUG_PACKET = true;
		public const bool DEBUG_PACKET_LOOP_SHOW = true;
		public const int DEBUG_PACKET_LOOP_COUNT = 100000;
	}
	class Program
	{
		public int identity = 0;
		public int capability;
		public Queue<CUserToken> list_UserFree = new Queue<CUserToken>();
		public List<CUserToken> list_UserConnect = new List<CUserToken>();

		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private SocketAsyncEventArgs acceptArgs;
		static void Main(string[] args)
		{
			Console.Title = Protocol.title;
			Program _p = new Program();
			_p.StartupServer(200);

			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		void StartupServer(int _capability)
		{
			Console.WriteLine(Protocol.title);

			CUserToken _token;
			capability = _capability;
			for(int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
				list_UserFree.Enqueue(_token);
			}

			//accept socket...
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(10);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptAsync);
			acceptArgs.AcceptSocket = null;
			bool _bAcceptAsync = acceptSocket.AcceptAsync(acceptArgs);
			if (_bAcceptAsync == false)
			{
				Console.WriteLine(" StartupServer #### >> acceptSocket.AcceptAsync Die or Other error");
				OnAcceptAsync(acceptSocket, acceptArgs);
			}
		}

		void OnAcceptAsync(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			identity++;
			if (Protocol.DEBUG) Console.WriteLine("[{0}]OnAcceptAsync (callback) New Client Connect \n   >> socket:{1} BytesTransferred:{2} LastOperation:{3} SocketError:{4}", identity, acceptSocket.Connected, _acceptArgs.BytesTransferred, _acceptArgs.LastOperation, _acceptArgs.SocketError);

			//---------------------------------------
			// acceptSocket -> new Client Socket -> New Client
			// acceptSocket -> Accept Register.
			//---------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			Socket _acceptSocket = acceptSocket;
			_acceptArgs.AcceptSocket = null;
			bool _bAcceptAsync = _acceptSocket.AcceptAsync(_acceptArgs);
			if(_bAcceptAsync == false)
			{
				Console.WriteLine(" [{0}]OnAcceptAsync #### >> acceptSocket.AcceptAsync Die or Other error", identity);
				OnAcceptAsync(_acceptSocket, _acceptArgs);
			}

			//---------------------------------------
			// Data			<- UserFree pool
			// UserConnect	<- Data
			// Register		<- clientSocket.ReceiveAsync(receiveArgs)
			//---------------------------------------
			CUserToken _token = list_UserFree.Dequeue();
			_token.socket = _clientSocket;
			list_UserConnect.Add(_token);
			_token.identityID = identity;
			int _identityID = _token.identityID;

			bool _bNewClientReceiveAsync = _clientSocket.ReceiveAsync(_token.receiveArgs);
			if (_bNewClientReceiveAsync == false)
			{

				//등록하자마자 바로 데이타 받음...
				//
				// 상황1
				// 서버가 과부하 상태에서 신규 유저 접속하자 마자 바로 종료하는 경우...
				// 100% 발생함...데이타 대량으로 오고 가는중에 발생....
				// > 접속 종료에 대한 메세지를 받은 것임.. ㅎㅎㅎ
				// 접속 -> 대기중... 음... 바로 -> 바로 종료.... 발생....(종료 메세지)
				Console.WriteLine(" [{0}]OnAcceptAsync @@@@ >> 신규유저.ReceiveAsync 바로받음 {1}/{2}/{3}/{4}", _identityID, _token.socket.Connected, _token.receiveArgs.BytesTransferred, _token.receiveArgs.LastOperation, _token.receiveArgs.SocketError);
				OnReceiveAsync(_clientSocket, _token.receiveArgs);
				Console.WriteLine("[{0}]connect free:{1} use:{2}", _identityID, list_UserFree.Count, list_UserConnect.Count);
			}
		}

		void OnReceiveAsync(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			int _debugWorkNum = _token.GetWorkNum();
			int _identityID = _token.identityID;
			if (Protocol.DEBUG) Console.WriteLine("[{0}]OnReceiveAsync (callback) \n   >> socket:{1} BytesTransferred:{2} LastOperation:{3} SocketError:{4}", _identityID + "/" + _debugWorkNum, _token.socket.Connected, _receiveArgs.BytesTransferred, _receiveArgs.LastOperation, _receiveArgs.SocketError);
			if (Protocol.DEBUG_PACKET_LOOP_SHOW && _debugWorkNum % Protocol.DEBUG_PACKET_LOOP_COUNT <= 1) Console.WriteLine("[{0}]OnReceiveAsync (callback) \n   >> socket:{1} BytesTransferred:{2} LastOperation:{3} SocketError:{4}", _identityID + "/" + _debugWorkNum, _token.socket.Connected, _receiveArgs.BytesTransferred, _receiveArgs.LastOperation, _receiveArgs.SocketError);

			if (_receiveArgs.LastOperation == SocketAsyncOperation.Receive
				&& _receiveArgs.SocketError == SocketError.Success
				&& _receiveArgs.BytesTransferred > 0)
			{
				Socket _clientSocket = _token.socket;
				SocketAsyncEventArgs _sendArgs = _token.sendArgs;

				//---------------------------------------
				// Client -> Socket -> (callback) -> OnReceiveAsync
				// Data Parsing
				//---------------------------------------
				int _transferred = _receiveArgs.BytesTransferred;
				Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _token.receiveBuffer2, 0, _transferred);
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _transferred);
				if (Protocol.DEBUG_PACKET) Console.WriteLine("[{0}] size:{1} >> {2}", _identityID + "/" + _debugWorkNum, _transferred, _text);

				//-------------------------------
				// SendAsync....
				// Message Queue에 넣어두기... (보내는 byte[]가 반드시 다른 버퍼)
				//-------------------------------
				_token.SendMessage(Encoding.ASCII.GetBytes(_text), _debugWorkNum);

				//-------------------------------
				//ReceiveAsync...
				//-------------------------------
				bool _bReceiveAsync = _clientSocket.ReceiveAsync(_receiveArgs);
				if (_bReceiveAsync == false)
				{
					Console.WriteLine(" [{0}]OnReceiveAsync .ReceiveAsync 재등록후(소켓꺼짐) {1}/{2}/{3}/{4}", _identityID + "/" + _debugWorkNum, _token.socket.Connected, _receiveArgs.BytesTransferred, _receiveArgs.LastOperation, _receiveArgs.SocketError);
					if (_token.socket.Connected == false)
					{
						Console.WriteLine(" >> Disconnect ");
						Disconnect("[정상종료2]", _token);
					}
					else
					{
						Console.WriteLine(" [{0}] #### OnReceiveAsync > _socket.ReceiveAsync 메세지 받기(2) 등록하자마사 바로 받음.{1}", _identityID, _clientSocket.Connected);
						OnReceiveAsync(_clientSocket, _receiveArgs);
					}
				}
			}
			else
			{
				Console.WriteLine("[{0}]OnReceiveAsync [정상종료] >>> :{1} :{2} :{3} :{4}", _identityID + "/" + _debugWorkNum, _token.socket.Connected, _receiveArgs.BytesTransferred, _receiveArgs.LastOperation, _receiveArgs.SocketError);
				Disconnect("[정상종료1]", _token);
			}
		}

		void OnSendAsync(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			CUserToken _token = _sendArgs.UserToken as CUserToken;
			int _debugWorkNum = _token.GetWorkNum();
			int _identityID = _token.identityID;
			if (Protocol.DEBUG) Console.WriteLine("[{0}]OnSendAsync (callback) \n   >> socket:{1} BytesTransferred:{2} LastOperation:{3} SocketError:{4}", _identityID + "/" + _debugWorkNum, _token.socket.Connected, _sendArgs.BytesTransferred, _sendArgs.LastOperation, _sendArgs.SocketError);

			if (_sendArgs.LastOperation == SocketAsyncOperation.Send
				&& _sendArgs.SocketError == SocketError.Success
				&& _sendArgs.BytesTransferred > 0)
			{
				//큐에서 빼고 > 더있는지 검사후에 다음것 등록...
				_token.SendMessage_CheckAndReSend(_debugWorkNum);
			}
			else
			{

				Console.WriteLine("[{0}]OnSendAsync [정상종료] >>> :{1} :{2} :{3} :{4}", _identityID + "/" + _debugWorkNum, _token.socket.Connected, _sendArgs.BytesTransferred, _sendArgs.LastOperation, _sendArgs.SocketError);
				Disconnect("[정상종료1]", _token);
			}
		}

			void Disconnect(string _msg, CUserToken _token)
		{
			list_UserConnect.Remove(_token);
			lock (list_UserFree)
			{
				list_UserFree.Enqueue(_token);
			}
			Console.WriteLine(" [{0}] release {3} free:{1} use:{2}", _token.identityID, list_UserFree.Count, list_UserConnect.Count, _msg);
			_token.ClearData();
		}
	}

	class CUserToken
	{
		public int identityID;
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, receiveBuffer2, sendBuffer;//, sendBuffer2;

		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveAsync, EventHandler<SocketAsyncEventArgs> _onSendAsync)
		{
			receiveBuffer = new byte[1024];
			receiveBuffer2 = new byte[1024];
			receiveArgs = new SocketAsyncEventArgs();
			receiveArgs.Completed += _onReceiveAsync;
			receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
			receiveArgs.UserToken = this;

			sendBuffer = new byte[1024];
			//sendBuffer2 = new byte[1024];
			sendArgs = new SocketAsyncEventArgs();
			sendArgs.Completed += _onSendAsync;
			sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			sendArgs.UserToken = this;
		}

		public void ClearData()
		{
			identityID	= 0;
			workNum		= 0;
			socket		= null;
		}
		public void SetSocket(Socket _s) { socket = _s; }
		public int workNum;
		object objectWorkNum = new object();
		public int GetWorkNum()
		{
			lock (objectWorkNum)
			{
				workNum++;
			}
			return workNum;
		}

		Queue<byte[]> sendQueue = new Queue<byte[]>();
		object objectSendLock = new object();
		public void SendMessage(byte[] _data, int _debugWorkNum = -1)
		{
			if (Protocol.DEBUG_PACKET) Console.WriteLine("[{0}]SendMessage >> sendQueue.Count:{1}", identityID + "/" + _debugWorkNum, sendQueue.Count);
			lock (objectSendLock)
			{
				bool _bSending = sendQueue.Count > 0;
				sendQueue.Enqueue(_data);

				if (!_bSending)
				{
					if (Protocol.DEBUG_PACKET) Console.WriteLine(" >> 전송 등록하러가기");
					SendMessage_Sending(_debugWorkNum);
				}
			}
		}

		public void SendMessage_CheckAndReSend(int _debugWorkNum = -1)
		{
			lock (objectSendLock)
			{
				sendQueue.Dequeue();
				if(sendQueue.Count > 0)
				{
					SendMessage_Sending(_debugWorkNum);
				}
			}
		}

		public void SendMessage_Sending(int _debugWorkNum)
		{
			if (Protocol.DEBUG_PACKET) Console.WriteLine("[{0}]SendMessage_Sending >> ", identityID + "/" + _debugWorkNum);
			byte[] _data = sendQueue.Peek();
			Array.Copy(_data, 0, sendArgs.Buffer, sendArgs.Offset, _data.Length);
			sendArgs.SetBuffer(sendArgs.Offset, _data.Length);

			bool _bSendAsync = socket.SendAsync(sendArgs);
			Console.WriteLine(socket.Connected + ":" + _bSendAsync);
			if (_bSendAsync == false)
			{
				// socket killed
				Console.WriteLine(" [{0}]SendMessage_Sending @@@@ >> .SendAsync 보내기 등록 >> 소켓죽음...", identityID + "/" + _debugWorkNum);
				sendQueue.Clear();

				//.ReceiveAsynce에서 처리 하기 때문에 여기는 필요는 없다...
				//if(socket.Connected == false)
				//{
				//	//자원정리...
				//	//Disconnect(" <<<< 보낼때1", _token);
				//}
				//else
				//{
				//	//OnSendAsync(socket, sendArgs);
				//}
			}
		}
	}
}
