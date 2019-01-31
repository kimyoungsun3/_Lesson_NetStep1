using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEventArgs
{
	public class CustomEventArgs : EventArgs
	{
		public string msg;
		public CustomEventArgs(string _msg)
		{
			msg = _msg;
		}
	}

	public class Publisher
	{
		//public delegate void EventHandler(object _obj, EventArgs _e);
		public event EventHandler<CustomEventArgs> onEventHandler;
		
		public void DoSomething()
		{
			if(onEventHandler != null)
			{
				CustomEventArgs _e = new CustomEventArgs(" > Message");
				onEventHandler( this, _e);
			}
		}
	}

	public class Subscriber
	{
		public string id;
		public Subscriber(string _id, Publisher _pub)
		{
			id = _id;
			_pub.onEventHandler += OnEventHandler;
		}

		void OnEventHandler(object _obj, CustomEventArgs _e)
		{
			Console.WriteLine(id + _e.msg);
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			Publisher _pub		= new Publisher();
			Subscriber _sub1	= new Subscriber("sub1", _pub);
			Subscriber _sub2	= new Subscriber("sub2", _pub);
			_pub.DoSomething();
		}
	}
}
