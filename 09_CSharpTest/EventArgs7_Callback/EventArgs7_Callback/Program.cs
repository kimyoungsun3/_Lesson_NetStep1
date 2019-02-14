using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs7_Callback
{
	class CustomEventArgs : EventArgs
	{
		public string msg;

		public event EventHandler<CustomEventArgs> Complete;
		public CustomSocket socket;
		public object userToken;
		public byte[] buffer;
		public int offset, transferred;

		public CustomEventArgs(string _msg)
		{
			msg = _msg;
		}

		public void RunComplete(object _obj)
		{
			//public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e) 
			//where TEventArgs : EventArgs;
			if (Complete!= null)
			{
				Complete(_obj, this);
			}
		}
	}

	class CustomSocket
	{
		CustomEventArgs acceptAgrs, receiveArgs, sendArgs;
		public void AcceptAsync(CustomEventArgs _args)	{ acceptAgrs = _args; }
		public void ReceiveAsync(CustomEventArgs _args) { receiveArgs = _args; }
		public void SendAsync(CustomEventArgs _args)	{ sendArgs = _args; }

		public void Startup()
		{
			Random _rand = new Random();
			int _kind;
			while (true)
			{
				_kind = _rand.Next() % 3;
				switch (_kind)
				{
					case 0:	acceptAgrs.RunComplete(this);	break;
					case 1: receiveArgs.RunComplete(this);	break;
					case 2: sendArgs.RunComplete(this);		break;
				}
				System.Threading.Thread.Sleep(100);
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Startup();
			Console.Title = "EventArgs7_Callback";
			while (true)
			{
				Console.WriteLine(".");
				System.Threading.Thread.Sleep(1000);
			}
		}

		CustomSocket socket = new CustomSocket();
		void Startup()
		{
			CustomEventArgs _acceptArgs = new CustomEventArgs("Accept");
			_acceptArgs.Complete += OnAcceptAsync;
			socket.AcceptAsync(_acceptArgs);

			CustomEventArgs _receiveArgs = new CustomEventArgs("Receive");
			_receiveArgs.Complete += OnReceiveAsync;
			socket.ReceiveAsync(_receiveArgs);

			CustomEventArgs _sendArgs = new CustomEventArgs("Send");
			_sendArgs.Complete += OnSendAsync;
			socket.SendAsync(_sendArgs);

			System.Threading.Thread _thread = new System.Threading.Thread(socket.Startup);
			_thread.Start();
		}

		void OnAcceptAsync(object _obj, CustomEventArgs _args)	{ Console.WriteLine(" > OnAcceptAsync:" + _args.msg); }
		void OnReceiveAsync(object _obj, CustomEventArgs _args) { Console.WriteLine(" > OnReceiveAsync:" + _args.msg); }
		void OnSendAsync(object _obj, CustomEventArgs _args)	{ Console.WriteLine(" > OnSendAsync:" + _args.msg); }
	}
}
