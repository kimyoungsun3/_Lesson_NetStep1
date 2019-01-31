using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer5
{
	class CUserToken
	{
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, sendBuffer;

		public CUserToken (Socket _s, EventHandler<SocketAsyncEventArgs> _onReceiveCallback, EventHandler<SocketAsyncEventArgs> _onSendCallback)
		{
			socket = _s;

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
	}


	class Program
	{
		Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs acceptArgs;
		List<CUserToken> listUser = new List<CUserToken>();

		static void Main(string[] args)
		{
			Console.Title = "Time Server5";
			Program _p = new Program();
			_p.StartupServer();

			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		void StartupServer()
		{
			Console.WriteLine("Server Start up5");
			acceptSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
			acceptSocket.Listen(5);

			acceptArgs = new SocketAsyncEventArgs();
			acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCallback);
			acceptSocket.AcceptAsync(acceptArgs);
		}

		void OnAcceptCallback(object _obj, SocketAsyncEventArgs _acceptArgs)
		{
			Console.WriteLine("Client connect");
			Socket _clientSocket = _acceptArgs.AcceptSocket;
			_acceptArgs.AcceptSocket = null;//반드시 비워줘야함....
			acceptSocket.AcceptAsync(_acceptArgs);

			CUserToken _token = new CUserToken(_clientSocket, OnReceiveCallbak, OnSendCallback);
			_token.socket.ReceiveAsync(_token.receiveArgs);
			listUser.Add(_token);
		}

		void OnReceiveCallbak(object _obj, SocketAsyncEventArgs _receiveArgs)
		{
			CUserToken _token = _receiveArgs.UserToken as CUserToken;
			Socket _socket = _token.socket;
			SocketAsyncEventArgs _sendArgs = _token.sendArgs;

			int _transferred = _receiveArgs.BytesTransferred;
			byte[] _readBuffer = new byte[_transferred];
			Array.Copy(_receiveArgs.Buffer, _receiveArgs.Offset, _readBuffer, 0, _transferred);
			_socket.ReceiveAsync(_receiveArgs);

			string _text = Encoding.ASCII.GetString(_readBuffer);
			string _response = string.Empty;
			if(_text.Equals("get time"))
			{
				_response = " Ok Time Server5";
			}
			else
			{
				_response = " No ...";
			}
			byte[] _sendBuffer = Encoding.ASCII.GetBytes(_response);
			Array.Copy(_sendBuffer, 0, _sendArgs.Buffer, _sendArgs.Offset, _sendBuffer.Length);
			_sendArgs.SetBuffer(_sendArgs.Offset, _sendBuffer.Length);
			_socket.SendAsync(_sendArgs);
		}

		void OnSendCallback(object _obj, SocketAsyncEventArgs _sendArgs)
		{

		}


	}
}
