using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs4
{
	class CustomEventArgs : EventArgs
	{
		public string msg;
		public event EventHandler<CustomEventArgs> Complete;
		public CustomEventArgs(string _msg)
		{
			msg = _msg;
		}

		public void CompleteJob(object _obj)
		{
			if(Complete != null)
			{
				Complete(_obj, this);
			}
		}
	}

	class Publisher
	{
		CustomEventArgs acceptArgs, receiveArgs, sendArgs;
		public void AcceptAsync(CustomEventArgs _args) { acceptArgs = _args; }
		public void ReceiveAsync(CustomEventArgs _args) { receiveArgs = _args; }
		public void SendAsync(CustomEventArgs _args) { sendArgs = _args; }

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
						acceptArgs.CompleteJob(this);
						break;
					case 1:
						receiveArgs.CompleteJob(this);
						break;
					case 2:
						sendArgs.CompleteJob(this);
						break;

				}
				System.Threading.Thread.Sleep(100);
			}
		}
	}

	class Program
	{
		Publisher pub = new Publisher();
		static void Main(string[] args)
		{

			while (true)
			{
				Program _p = new Program();
				_p.Startup();
				while (true)
				{
					Console.WriteLine(".");
					System.Threading.Thread.Sleep(1000);
				}
			}
		}

		void Startup()
		{
			CustomEventArgs _args1 = new CustomEventArgs("AcceptAsync");
			_args1.Complete += OnAcceptCallback;
			pub.AcceptAsync(_args1);

			CustomEventArgs _args2 = new CustomEventArgs("ReceiveAsync");
			_args2.Complete += OnReceiveCallback;
			pub.ReceiveAsync(_args2);

			CustomEventArgs _args3 = new CustomEventArgs("SendAsync");
			_args3.Complete += OnSendCallback;
			pub.SendAsync(_args3);

			System.Threading.Thread _th = new System.Threading.Thread(pub.Startup);
			_th.Start();

		}

		void OnAcceptCallback(object _obj, CustomEventArgs _args) { Console.WriteLine(_args.msg + " > "); }
		void OnReceiveCallback(object _obj, CustomEventArgs _args) { Console.WriteLine(_args.msg + " > "); }
		void OnSendCallback(object _obj, CustomEventArgs _args) { Console.WriteLine(_args.msg + " > "); }
	}
}
