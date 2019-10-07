using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer4
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
		private List<CUserToken> listUser = new List<CUserToken>();
		private Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs acceptArgs;

		static void Main(string[] args)
		{
			Console.Title = "Time Echo Server4";
			Program _p = new Program();
			_p.StartupServer();

			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		private void StartupServer()
		{
			Console.WriteLine("Setting up Sever4");
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(5);

			//----------------------------------------
			// accept async
			//----------------------------------------
			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCallback);
			acceptSocket.AcceptAsync(acceptArgs);
		}

		private void OnAcceptCallback(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client connect");

			//----------------------------------------
			//
			//----------------------------------------
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;
			acceptSocket.AcceptAsync(_acceptArgs);

			//----------------------------------------
			//
			//----------------------------------------
			CUserToken _token = new CUserToken(_clientSocket, OnReceiveCallback, OnSendCallback);
			_token.socket.ReceiveAsync(_token.receiveArgs);

			listUser.Add(_token);
		}

		private void OnReceiveCallback(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			//----------------------------------------
			//
			//----------------------------------------
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			Socket _socket = _token.socket;
			SocketAsyncEventArgs _sendArgs = _token.sendArgs;

			//----------------------------------------
			//
			//----------------------------------------
			int _transferred = _receiveArgs.BytesTransferred;
			byte[] _receiveBuffer = new byte[_transferred];
			Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _receiveBuffer, 0, _transferred);
			_socket.ReceiveAsync(_receiveArgs);

			//----------------------------------------
			//
			//----------------------------------------
			string _text = Encoding.ASCII.GetString(_receiveBuffer);
			string _response = string.Empty;
			if(_text.ToLower().Equals("get time"))
			{
				_response = " Time server4";
			}
			else
			{
				_response = " not command";
			}
			byte[] _sendBuffer = Encoding.ASCII.GetBytes(_response);
			Array.Copy(_sendBuffer, 0, _sendArgs.Buffer, _sendArgs.Offset, _sendBuffer.Length);
			_sendArgs.SetBuffer(_sendArgs.Offset, _sendBuffer.Length);
			_socket.SendAsync(_sendArgs);

			//----------------------------------------
			//
			//----------------------------------------

		}
		private void OnSendCallback(object _obj, SocketAsyncEventArgs _sendArgs)
		{
			//----------------------------------------
			//
			//----------------------------------------

		}

	}
}
