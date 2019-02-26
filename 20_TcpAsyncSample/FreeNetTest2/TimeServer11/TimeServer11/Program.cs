using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer11
{
	class Protocol
	{
		public const bool DEBUG = false;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "Time Server11";
			Program _p = new Program();
			_p.StartupServer(1000);
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		int capability;
		int identity = 0;
		public Queue<CUserToken> list_FreeUser = new Queue<CUserToken>();
		public List<CUserToken> list_ConnectUser = new List<CUserToken>();

		Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs acceptArgs;
		void StartupServer(int _capability)
		{
			Console.WriteLine("ServerTime 11 Pool start");
			capability = _capability;

			CUserToken _token;
			for(int i = 0; i < _capability; i++)
			{
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
				list_FreeUser.Enqueue(_token);
			}

			//신규유저 받기 전용소켓등록...
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(10);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptAsync);

			bool _bAccept = acceptSocket.AcceptAsync(acceptArgs);
			if(_bAccept == false)
			{
				OnAcceptAsync(acceptSocket, acceptArgs);
			}

		}

		void OnAcceptAsync(object _object, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("New Client Connect");

			//신규 -> Pool
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;
			Socket _acceptSocket = acceptSocket;
			bool _bAccept = _acceptSocket.AcceptAsync(_acceptArgs);
			if(_bAccept == false)
			{
				OnAcceptAsync(_acceptSocket, _acceptArgs);
			}

			CUserToken _token = list_FreeUser.Dequeue();
			_token.socket = _clientSocket;
			_token.identityID = identity++;
			list_ConnectUser.Add(_token);

			//유저 ReceiveAsync등록
			bool _bReceive = _clientSocket.ReceiveAsync(_token.receivArgs);
			if(_bReceive == false)
			{
				OnReceiveAsync(_clientSocket, _token.receivArgs);
			}
		}

		void OnReceiveAsync(object _object, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			if(_receiveArgs.LastOperation == SocketAsyncOperation.Receive
				&& _receiveArgs.SocketError == SocketError.Success
				&& _receiveArgs.BytesTransferred > 0)
			{
				SocketAsyncEventArgs _sendArgs = _token.sendArgs;
				Socket _clientSocket = _token.socket;

				//client -> socket -> receive
				int _transferred = _receiveArgs.BytesTransferred;
				Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _token.receiveBuffer2, 0, _transferred);
				bool _bReceive = _clientSocket.ReceiveAsync(_receiveArgs);
				if(_clientSocket.Connected == false)
				{
					Disconnect("받을 때 꺼짐", _token);
					return;
				}

				if (_bReceive == false)
				{
					//등록하자마사 바로 데이타 받음...
					Console.WriteLine("[{0}] #### OnReceiveAsync > _socket.ReceiveAsync 메세지 받기(2) 등록하자마사 바로 받음.{1}", _token.identityID, _clientSocket.Connected);
					OnReceiveAsync(null, _receiveArgs);
				}

				//Data Parse and Send...
				string _text = Encoding.ASCII.GetString(_token.receiveBuffer2, 0, _transferred);
				string _response = string.Empty;
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


				bool _bSend = true;
				try
				{
					_bSend = _clientSocket.SendAsync(_sendArgs);
				}
				catch (Exception _e)
				{
					Console.WriteLine(" >>> OnReceiveAsync _socket.SendAsync _e:{0}", _e);
					Disconnect(" <<<< ***보낼려고 하는데 어 아직 안보낸것이 있네...***", _token, true);
					return;
				}
				if (Protocol.DEBUG) Console.WriteLine("[{0}] _bSend {1} {2}", _token.identityID, _bSend, _clientSocket.Connected);


				if (_clientSocket.Connected == false)
				{
					Disconnect(" <<<< 보낼때1", _token);
					return;
				}
				if (_bSend == false)
				{
					Console.WriteLine("[{0}] #### OnReceiveAsync _socket.SendAsync -> 보내기(1) 등록하자마사 바로 받음.{1}", _token.identityID, _clientSocket.Connected);
					OnSendAsync(_clientSocket, _sendArgs);
				}
			}
			else
			{

				Disconnect("정상", _token);
			}

		}

		void OnSendAsync(object _object, SocketAsyncEventArgs _sendArgs)
		{

		}

		int destroyAndRemakeCount = 0;
		void Disconnect(string _branch, CUserToken _token, bool _bDestroyAndRemake = false)
		{
			lock (list_ConnectUser)
			{
				list_ConnectUser.Remove(_token);
			}

			if(_token.socket != null)
			{
				if (_token.socket.Connected)
				{
					try
					{
						_token.socket.Shutdown(SocketShutdown.Send);
						_token.socket.Close();

					}catch(Exception _e)
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

				_token.bProblemData = true;
				_token = null;
				_token = new CUserToken(OnReceiveAsync, OnSendAsync);
			}

			if (!_token.bProblemData)
			{
				list_FreeUser.Enqueue(_token);
			}
			else
			{
				Console.WriteLine(" **** [{0}] Problem Data not return Pool *** ", (_bDebugRemake ? _debugIdentity : _token.identityID));
			}
			Console.WriteLine(" [{0}] release branch:{3} free:{1} use:{2}", (_bDebugRemake ? _debugIdentity : _token.identityID), list_FreeUser.Count, list_ConnectUser.Count, _branch);
		}
	}

	class CUserToken
	{
		public bool bProblemData;
		public SocketAsyncEventArgs receivArgs;
		public SocketAsyncEventArgs sendArgs;
		public byte[] receiveBuffer, receiveBuffer2;
		public byte[] sendBuffer, sendBuffer2;
		public Socket socket;
		public int identityID;

		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveAsync, EventHandler<SocketAsyncEventArgs> _onSendAsync)
		{
			receiveBuffer	= new byte[1024];
			sendBuffer		= new byte[1024];
			receiveBuffer2 = new byte[1024];
			sendBuffer2 = new byte[1024];

			receivArgs = new SocketAsyncEventArgs();
			receivArgs.Completed += _onReceiveAsync;
			receivArgs.UserToken = this;
			receivArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

			sendArgs = new SocketAsyncEventArgs();
			sendArgs.Completed += _onSendAsync;
			sendArgs.UserToken = this;
			sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
		}
	}
}
