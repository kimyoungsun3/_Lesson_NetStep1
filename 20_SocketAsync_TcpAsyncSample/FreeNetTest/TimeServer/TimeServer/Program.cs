using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace TimeServer
{
	class CUserToken
	{
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, sendBuffer;

		public CUserToken(Socket _s, 
			EventHandler<SocketAsyncEventArgs> _receiveCallback,
			EventHandler<SocketAsyncEventArgs> _sendCallback)
		{
			socket = _s;

			receiveBuffer = new byte[1024];
			receiveArgs	= new SocketAsyncEventArgs();
			receiveArgs.Completed += _receiveCallback;
			receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
			receiveArgs.UserToken = this;

			sendBuffer = new byte[1024];
			sendArgs = new SocketAsyncEventArgs();
			sendArgs.Completed += _sendCallback;
			sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			sendArgs.UserToken = this;
		}
	}

	class Program
	{
		private byte[] buffer = new byte[1024];
		private List<CUserToken> listUser = new List<CUserToken>(); 
		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs acceptArgs;
		static int sendpack = 0;


		static void Main(string[] args)
		{
			Console.Title = "Time Echo Server";
			Program _p = new Program();
			_p.StartupServer();

			while (true)
			{
				//Console.Clear();
				//Console.WriteLine("SendPack:" + sendpack);
				System.Threading.Thread.Sleep(1000);
			}

		}

		private void StartupServer()
		{
			Console.WriteLine("Setting up server...");
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(5);

			//-------------------------------------
			//
			//-------------------------------------
			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed	+= new EventHandler<SocketAsyncEventArgs>(OnAcceptCallback);
			acceptSocket.AcceptAsync(acceptArgs);
		}

		private void OnAcceptCallback(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client Connect");
			//-------------------------------------
			// 접속은 다시 대기로 돌리기....
			//-------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;
			acceptSocket.AcceptAsync(acceptArgs);


			//-------------------------------------
			// 신규유저 연결버퍼 생성...
			//-------------------------------------
			CUserToken _token = new CUserToken(_clientSocket, OnReceiveCallback, OnSendCallback);
			listUser.Add(_token);

			//신규 유저들 받기대기로 돌리기...
			_token.socket.ReceiveAsync(_token.receiveArgs);
		}

		private void OnReceiveCallback(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			//받은 유저Socket + 데이타 빼기.
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			Socket _socket = _token.socket;
			SocketAsyncEventArgs _sendArgs = _token.sendArgs;

			//받은 데이타만 빼내기...
			int _transferred = _receiveArgs.BytesTransferred;
			byte[] _buffer = new byte[_transferred];
			Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _buffer, 0, _transferred);
			_token.socket.ReceiveAsync(_token.receiveArgs);

			sendpack += _transferred;
			//-------------------------------------
			//
			//-------------------------------------
			string _text = Encoding.ASCII.GetString(_buffer);
			//Console.WriteLine("Text received:" + _text);

			string _respone = string.Empty;
			if(_text.ToLower().Equals("get time"))
			{
				//Console.WriteLine(" > OK");
				_respone = DateTime.Now.ToLongDateString();
			}
			else
			{
				//Console.WriteLine(" > No");
				_respone = "Invalid Request";
			}
			byte[] _data = Encoding.ASCII.GetBytes(_respone);
			Array.Copy(_data, 0, _sendArgs.Buffer, _sendArgs.Offset, _data.Length);
			_sendArgs.SetBuffer(_sendArgs.Offset, _data.Length);
			_socket.SendAsync(_sendArgs);

		}

		private void OnSendCallback(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			//CUserToken _token = _sendArgs.UserToken as CUserToken;
			//Socket _socket = _token.socket;
		}
	}
}
