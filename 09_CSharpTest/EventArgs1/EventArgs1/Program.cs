using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs1
{

	class CustomEventArgs : EventArgs
	{
		public string msg;
		public CustomEventArgs(string _msg)
		{
			msg = _msg;
		}
	}

	class Publisher
	{
		//public delegate void System.EventHandler<TEventArgs>(object _sender, TEventArgs _e) 
		//										  where TEventArgs:System.EventArgs
		public event EventHandler<CustomEventArgs> onXXXCallback;
		public void DoSomething()
		{
			if(onXXXCallback != null)
			{
				CustomEventArgs _args = new CustomEventArgs("Publisher");
				onXXXCallback(this, _args);
			}
		}
	}

	class SubScriber
	{
		string id;
		public SubScriber(string _id, Publisher _pub)
		{
			id					= _id;
			_pub.onXXXCallback += OnXXXCallback;
		}

		private void OnXXXCallback(object _pub, CustomEventArgs _args)
		{
			Console.WriteLine(_args.msg + " > " + id);
		}
	}



	class Program
	{
		static void Main(string[] args)
		{
			Publisher _pub = new Publisher();
			SubScriber _sub1 = new SubScriber("sub1", _pub);
			SubScriber _sub2 = new SubScriber("sub2", _pub);

			_pub.DoSomething();
		}
	}
}
