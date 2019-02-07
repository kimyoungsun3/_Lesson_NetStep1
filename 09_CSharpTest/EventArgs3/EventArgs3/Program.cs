using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgs3
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
		public event EventHandler<CustomEventArgs> Complete;

		public void DoSomething()
		{
			if(Complete != null)
			{
				Complete(this, new CustomEventArgs("hi"));
			}
		}
	}

	class SubScriber
	{
		string id;
		public SubScriber(string _id, Publisher _pub)
		{
			id = _id;
			_pub.Complete += OnHandler;
		}

		void OnHandler(object _obj, CustomEventArgs _args)
		{
			Console.WriteLine(id + " > " + _args.msg);
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
