using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _100_AsyncRun
{
	class Program
	{
		static void Main(string[] args)
		{
			CountDownWrapper _a = new CountDownWrapper();
			CountDownWrapper _b = new CountDownWrapper();
			_a.CountDown(1, 4);
			_b.CountDown(2, 4);
			Console.WriteLine("---- Main 1 ----");
			AutoResetEvent.WaitAll(new[] { _a.done, _b.done });
			Console.WriteLine("---- Main 2 ----");
			Console.ReadLine();
		}
	}

	class CountDownWrapper
	{
		public AutoResetEvent done = new AutoResetEvent(false);
		int id;
		public void CountDown(int _id, int _cc)
		{
			id = _id;
			_cc--;
			string _str = "["+ id+"/"+ _cc + "/" + Thread.CurrentThread.ManagedThreadId + "]";
			Console.WriteLine(_str + " >> " + _cc);
			if(_cc >= 0)
			{
				Task.Delay(3000).ContinueWith((c) =>
				{
					CountDown(id, _cc);
				});
				Console.WriteLine(_str + " >> ---->");
			}
			else
			{
				done.Set();
				Console.WriteLine(_str + " >> autoResetEvent.()");
			}
			Console.WriteLine(_str + "----end----");
		}
	}
}
