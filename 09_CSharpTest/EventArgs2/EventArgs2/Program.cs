using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs2
{
	class CustomEventArgs : EventArgs
	{
		public int callCount;
		public string msg;
		public event EventHandler<CustomEventArgs> Complete;
		public CustomEventArgs(string _msg)
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

	class Publisher
	{
		CustomEventArgs acceptArgs;
		CustomEventArgs receiveArgs;
		CustomEventArgs sendArgs;
		public void AcceptAsync(CustomEventArgs _args)	{	acceptArgs	= _args;	}
		public void ReceiveAsync(CustomEventArgs _args)	{	receiveArgs = _args;	}
		public void SendAsync(CustomEventArgs _args)	{	sendArgs	= _args;	}

		public void Startup()
		{
			Random _rand = new Random();
			int _kind;
			while (true)
			{
				_kind = _rand.Next() % 3;
				switch (_kind) {
					case 0:
						if (acceptArgs != null)
						{
							acceptArgs.OnComplete(this);
						}
						break;
					case 1:
						if (receiveArgs != null)
						{
							receiveArgs.OnComplete(this);
						}
						break;
					case 2:
						if (sendArgs != null)
						{
							sendArgs.OnComplete(this);
						}
						break;
				}
				System.Threading.Thread.Sleep(100);
			}
		}
	}

	class Program
	{
		public Publisher pub = new Publisher();
		void Startup()
		{
			CustomEventArgs _args1	= new CustomEventArgs("AcceptAsync");
			_args1.Complete			+= OnAcceptCallback;
			pub.AcceptAsync(_args1);

			CustomEventArgs _args2	= new CustomEventArgs("ReceiveAsync");
			_args2.Complete			+= OnReceiveCallback;
			pub.ReceiveAsync(_args2);

			CustomEventArgs _args3	= new CustomEventArgs("SendAsync");
			_args3.Complete			+= OnSendCallback;
			pub.SendAsync(_args3);

			System.Threading.Thread _th = new System.Threading.Thread(pub.Startup);
			_th.Start();
		}

		void OnAcceptCallback(object _obj, CustomEventArgs _args){	Console.WriteLine(_args.msg + " > " + _args.callCount);}
		void OnReceiveCallback(object _obj, CustomEventArgs _args){	Console.WriteLine(_args.msg + " > " + _args.callCount);}
		void OnSendCallback(object _obj, CustomEventArgs _args)	{	Console.WriteLine(_args.msg + " > " + _args.callCount);}

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
	}
}
