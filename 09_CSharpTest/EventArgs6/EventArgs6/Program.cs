using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs6
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Startup();
			Console.Title = "EventArgs6";
			while (true)
			{
				Console.WriteLine(".");
				System.Threading.Thread.Sleep(1000);
			}
		}

		CustomSocket socket = new CustomSocket();
		void Startup()
		{
			CustomEventArgs _args1 = new CustomEventArgs("AcceptAsync");
			_args1.Complete += OnAcceptAsync;
			socket.AcceptAsync(_args1);

			CustomEventArgs _args2 = new CustomEventArgs("ReceiveAsync");
			_args2.Complete += OnReceiveAsync;
			socket.ReceiveAsync(_args2);

			CustomEventArgs _args3 = new CustomEventArgs("SendAsync");
			_args3.Complete += OnSendAsync;
			socket.SendAsync(_args3);


			System.Threading.Thread _thread = new System.Threading.Thread(socket.Startup);
			_thread.Start();
		}

		void OnAcceptAsync(object _obj, CustomEventArgs _args)	{ Console.WriteLine(" OnAcceptAsync > " + _args.msg); }
		void OnReceiveAsync(object _obj, CustomEventArgs _args)	{ Console.WriteLine(" OnReceiveAsync > " + _args.msg); }
		void OnSendAsync(object _obj, CustomEventArgs _args)	{ Console.WriteLine(" OnSendAsync > " + _args.msg); }
	}

	class CustomEventArgs : EventArgs
	{
		public string msg;
		//object, TEventArgs
		public event EventHandler<CustomEventArgs> Complete;
		public CustomSocket socket;
		public byte[] buffer;

		public CustomEventArgs(string _msg)
		{
			msg = _msg;
		}

		public void RunComplete(object _obj)
		{
			if(Complete != null)
			{
				Complete(_obj, this);
			}
		}
	}

	class CustomSocket
	{
		CustomEventArgs acceptArgs, receiveArgs, sendArgs;
		public void AcceptAsync(CustomEventArgs _args)	{ acceptArgs = _args;	}
		public void ReceiveAsync(CustomEventArgs _args) { receiveArgs = _args;	}
		public void SendAsync(CustomEventArgs _args)	{ sendArgs = _args;		}

		public void Startup()
		{
			Random _rand = new Random();
			int _kind;
			while (true)
			{
				_kind = _rand.Next() % 3;
				switch (_kind)
				{

					case 0: acceptArgs.RunComplete(this);	break;
					case 1: receiveArgs.RunComplete(this);	break;
					case 2: sendArgs.RunComplete(this);		break;
				}

				System.Threading.Thread.Sleep(100);
			}
		}
	}
}
