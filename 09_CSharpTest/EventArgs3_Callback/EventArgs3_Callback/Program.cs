using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs3_Callback
{
	class CustomeEventArgs : EventArgs
	{
		public int callCount;
		public string msg;
		public CustomSocket socket;
		public object UserToken;

		public event EventHandler<CustomeEventArgs> Complete;
		public CustomeEventArgs(string _msg)
		{
			callCount = 0;
			msg = _msg;
		}

		public void OnComplete(object _obj)
		{
			if(Complete != null)
			{
				callCount++;
				Complete(_obj, this);
			}
		}
	}

	class CustomSocket
	{
		CustomeEventArgs acceptArgs, receiveArgs, sendArgs;
		public void AcceptAsync(CustomeEventArgs _args)		{ acceptArgs = _args; }
		public void ReceiveAsync(CustomeEventArgs _args)	{ receiveArgs = _args; }
		public void SendAsync(CustomeEventArgs _args)		{ sendArgs = _args; }

		public void Startup()
		{
			Random _rand = new Random();
			int _kind;
			while (true)
			{
				_kind = _rand.Next() % 3;
				switch (_kind)
				{
					case 0:
						acceptArgs.OnComplete(this);
						break;
					case 1:
						receiveArgs.OnComplete(this);
						break;
					case 2:
						sendArgs.OnComplete(this);
						break;
				}

				System.Threading.Thread.Sleep(100);
			}
		}
	}


	class Program
	{
		public CustomSocket socket = new CustomSocket();
		void Startup()
		{
			CustomeEventArgs _args1 = new CustomeEventArgs("AcceptAsync");
			_args1.Complete += OnAcceptCallback;
			socket.AcceptAsync(_args1);

			CustomeEventArgs _args2 = new CustomeEventArgs("ReceiveAsync");
			_args2.Complete += OnReceiveCallback;
			socket.ReceiveAsync(_args2);

			CustomeEventArgs _args3 = new CustomeEventArgs("SendAsync");
			_args3.Complete += OnSendCallback;
			socket.SendAsync(_args3);

			System.Threading.Thread _thread = new System.Threading.Thread(socket.Startup);
			_thread.Start();
		}


		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Startup();

			while (true)
			{
				Console.WriteLine(".");
				System.Threading.Thread.Sleep(1000);
			}
		}

		void OnAcceptCallback(object _obj, CustomeEventArgs _acceptArgs){	Console.WriteLine(" > OnAcceptCallback:" + _acceptArgs.msg + " > " + _acceptArgs.callCount);}
		void OnReceiveCallback(object _obj, CustomeEventArgs _receiveArgs){	Console.WriteLine(" > OnReceiveCallback:" + _receiveArgs.msg + " > " + _receiveArgs.callCount);}
		void OnSendCallback(object _obj, CustomeEventArgs _sendArgs){		Console.WriteLine(" > OnSendCallback:" + _sendArgs.msg + " > " + _sendArgs.callCount);	}
	}
}
