using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs5
{
	class Program
	{
		public CustomSocket socket = new CustomSocket();
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

		void Startup()
		{
			CustomEventArgs _args1 = new CustomEventArgs("AcceptAsync");
			_args1.Complete += OnAcceptCallback;
			socket.AcceptAysnc(_args1);

			CustomEventArgs _args2 = new CustomEventArgs("ReceiveAsync");
			_args2.Complete += OnReceiveCallback;
			socket.ReceiveAysnc(_args2);

			CustomEventArgs _args3 = new CustomEventArgs("SendAsync");
			_args3.Complete += OnSendCallback;
			socket.SendAysnc(_args3);

			System.Threading.Thread _thread = new System.Threading.Thread(socket.Startup);
			_thread.Start();
		}

		void OnAcceptCallback(object _obj, CustomEventArgs _args) { Console.WriteLine(" > OnAcceptCallback : " + _args.msg); }
		void OnReceiveCallback(object _obj, CustomEventArgs _args) { Console.WriteLine(" > OnReceiveCallback : " + _args.msg); }
		void OnSendCallback(object _obj, CustomEventArgs _args) { Console.WriteLine(" > OnSendCallback : " + _args.msg); }
	}

	class CustomSocket
	{
		CustomEventArgs acceptArgs, receiveArgs, sendArgs;
		public void AcceptAysnc(CustomEventArgs _args) { acceptArgs = _args; }
		public void ReceiveAysnc(CustomEventArgs _args) { receiveArgs = _args; }
		public void SendAysnc(CustomEventArgs _args) { sendArgs = _args; }

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
						acceptArgs.OnComplete(this, acceptArgs);
						break;
					case 1:
						receiveArgs.OnComplete(this, receiveArgs);
						break;
					case 2:
						sendArgs.OnComplete(this, sendArgs);
						break;
				}

				System.Threading.Thread.Sleep(500);
			}
		}
	}

	class CustomEventArgs : EventArgs
	{
		public string msg;

		public event EventHandler<CustomEventArgs> Complete;
		public CustomSocket socket;
		public object UserToken;
		public byte[] buffer;

		public CustomEventArgs(string _msg)
		{
			msg = _msg;
		}

		public void OnComplete(object _obj, CustomEventArgs _args)
		{
			Console.WriteLine(this == _args);
			if(Complete != null)
			{
				Complete(_obj, this);
			}
		}
	}
}
