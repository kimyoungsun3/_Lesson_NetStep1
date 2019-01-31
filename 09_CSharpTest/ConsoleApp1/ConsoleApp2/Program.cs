using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
	public class CustomEventArgs : EventArgs
	{
		private string message;
		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		public CustomEventArgs(string _str)
		{
			message = _str;
		}
	}

	class Publisher
	{
		public event EventHandler<CustomEventArgs> onHandler;
		public void DoSomething()
		{
			if(onHandler != null)
			{
				CustomEventArgs _e = new CustomEventArgs("Did something");
				_e.Message += $" at {DateTime.Now}";
				onHandler(this, _e);
			}
		}
	}

	class Subscriber
	{
		private string id;
		public Subscriber(string _id, Publisher _pub)
		{
			id = _id;
			_pub.onHandler += OnHandler;
		}

		void OnHandler(object _sender, CustomEventArgs _e)
		{
			Console.WriteLine(id + " received this message:{0}", _e.Message);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Publisher _pub = new Publisher();
			Subscriber _sub1 = new Subscriber("sub1", _pub);
			Subscriber _sub2 = new Subscriber("sub2", _pub);

			_pub.DoSomething();
		}
	}
}
