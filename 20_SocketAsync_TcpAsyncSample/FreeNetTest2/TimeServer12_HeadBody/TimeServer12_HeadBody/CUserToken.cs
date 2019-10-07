using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer12_HeadBody
{

	public class CUserToken
	{
		public bool bProblemData;
		public int identityID;
		public Socket socket;
		public SocketAsyncEventArgs receiveArgs, sendArgs;
		public byte[] receiveBuffer, receiveBuffer2;
		public byte[] sendBuffer, sendBuffer2;
		CMessageResolver messageResolver;
		public CUserToken(EventHandler<SocketAsyncEventArgs> _onReceiveCallback, EventHandler<SocketAsyncEventArgs> _onSendCallback)
		{
			messageResolver = new CMessageResolver();

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

		public void ReceiveRead(byte[] _buffer, int _offset, int _transferred)
		{
			Console.WriteLine(this + " ReceiveRead _buffer:{0} _offset:{1} _transferred:{2}", _buffer, _offset, _transferred);
			messageResolver.ReadReceive(_buffer, _offset, _transferred, OnParseCode);
		}

		void OnParseCode(byte[] _buffer, int _size)
		{
			string _text = Encoding.ASCII.GetString(_buffer, 2, _size);
			Console.WriteLine("message:[{0}]", _text);
		}
	}
}
