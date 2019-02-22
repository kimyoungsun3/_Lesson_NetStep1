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
			_a.CountDown(10);
			_b.CountDown(10);
			Console.WriteLine("---- Main 1 ----");
			AutoResetEvent.WaitAll(new[] { _a.done, _b.done });
			Console.WriteLine("---- Main 2 ----");
		}
	}

	class CountDownWrapper
	{
		public AutoResetEvent done = new AutoResetEvent(false);
		public void CountDown(int _cc)
		{
			Console.WriteLine(_cc--);
			if(_cc >= 0)
			{
				Task.Delay(1000).ContinueWith((c) =>
				{
					CountDown(_cc);
				});
				Console.WriteLine(" ---->");
			}
			else
			{
				done.Set();
			}
			Console.WriteLine("----{0}----", _cc);
		}
	}
}
