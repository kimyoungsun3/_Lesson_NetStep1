using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer
{
	class CUserToken
	{
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, sendBuffer;

		public CUserToken(Socket _s, EventHandler<SocketAsyncEventArgs> _receiveCallback, EventHandler<SocketAsyncEventArgs> _sendCallback)
		{
			socket = _s;

			receiveBuffer = new byte[1024];
			receiveArgs = new SocketAsyncEventArgs();
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
		//private byte[] buffer = new byte[1024];
		private List<CUserToken> listUser = new List<CUserToken>();
		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs acceptArgs;

		static void Main(string[] args)
		{
			Console.Title = "Time Echo Server";
			Program _p = new Program();
			_p.StartupServer();
			
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		private void StartupServer()
		{
			Console.WriteLine("Setting up server");
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(5);

			//----------------------------------------
			//클라이언트 받는 비동기.
			//----------------------------------------
			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCallback);
			acceptSocket.AcceptAsync(acceptArgs);
		}

		//object _obj						-> acceptSocket
		//SocketAsyncEventArgs _acceptArgs	-> acceptArgs
		private void OnAcceptCallback(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client Connect {0}", ((Socket)_obj == acceptSocket));

			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;
			acceptSocket.AcceptAsync(acceptArgs);//재대기...
			//acceptArgs == _acceptArgs > Same SocketAsyncEventArgs
			//Console.WriteLine("acceptArgs == _acceptArgs:{0}", (acceptArgs == _acceptArgs));
			
			//----------------------------------------
			// 신규유저 서버에 등록...
			//----------------------------------------
			CUserToken _token = new CUserToken(_clientSocket, OnReceiveCallback, OnSendCallback);
			listUser.Add(_token);

			_token.socket.ReceiveAsync(_token.receiveArgs);
		}

		//object -> Socket(Client Socket)
		//_receiveArgs - CUserToken - _sendArgs
		private void OnReceiveCallback(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			//클라이언트로부터 데이타 받음....
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			Socket _socket = _token.socket;
			SocketAsyncEventArgs _sendArgs = _token.sendArgs;

			//-----------------------------------------
			//받은 데이타 빼내기... > 재등록...
			//-----------------------------------------
			int _transferred = _receiveArgs.BytesTransferred;
			byte[] _buffer = new byte[_transferred];
			Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _buffer, 0, _transferred);
			_socket.ReceiveAsync(_receiveArgs);
			//Console.WriteLine("_receiveArgs .Offset:{0}, _obj:{1}, Same:{2}", _receiveArgs.Offset, _obj, ((Socket)_obj == _token.socket);
			//Console.WriteLine("_receiveArgs Same:{0} / {1}", ((Socket)_obj == _token.socket), (_token.socket == acceptSocket));

			//-----------------------------------------
			// 데이타 보내기 > 전송등록....
			//-----------------------------------------
			string _text = Encoding.ASCII.GetString(_buffer);
			string _respone = string.Empty;
			if (_text.ToLower().Equals("get time"))
			{
				_respone = DateTime.Now.ToLongDateString();
			}
			else
			{
				_respone = "Echo:" + _text;
			}
			byte[] _data = Encoding.ASCII.GetBytes(_respone);
			Array.Copy(_data, 0, _sendArgs.Buffer, _sendArgs.Offset, _data.Length);
			_sendArgs.SetBuffer(_sendArgs.Offset, _data.Length);
			_socket.SendAsync(_sendArgs);
		}

		private void OnSendCallback(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			//Console.WriteLine("OnSendCallback >>");
		}
	}
}
