using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _115_02_System.Times.Timer
{

	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		System.Threading.AutoResetEvent autoReset = new System.Threading.AutoResetEvent(false);
		System.Timers.Timer stateTimer;
		void DoTest()
		{
			Console.WriteLine("[*** System.Timer.Timer ***]");
			StatusChecker _status = new StatusChecker(10, autoReset);

			//string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss");
			string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
			Console.WriteLine($"{_time} Creating timer.");
			stateTimer = new System.Timers.Timer(250);
			stateTimer.Elapsed += new System.Timers.ElapsedEventHandler(_status.CheckStatus);
			stateTimer.Start();
			autoReset.WaitOne();

			//when AutoEvent signal, change the period to every half Seconds.
			stateTimer.Interval = 500;
			Console.WriteLine("changing period to 500ms");
			autoReset.WaitOne();

			//When auto signals the second time, 
			stateTimer.Dispose();
			Console.WriteLine("Destroying Timer");
		}
	}


	class StatusChecker
	{
		private int count, maxCount;
		System.Threading.AutoResetEvent autoReset;

		public StatusChecker(int _maxCount, System.Threading.AutoResetEvent _autoReset)
		{
			count		= 0;
			maxCount	= _maxCount;
			autoReset	= _autoReset;
		}

		//This method is called by the timer delegate...
		public void CheckStatus(object _obj, System.Timers.ElapsedEventArgs _args)
		{
			count++;
			string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
			Log($"\t {count} >> {_time}");
			System.Threading.Thread.Sleep(2000);
			Log($"\t\t {count} >> 2000ms");

			if (count > maxCount)
			{
				Log("\t autoReset.Set");
				//Reset the counter and signal the waiting...
				count = 0;
				autoReset.Set();
			}
			System.Threading.Thread.Sleep(2000);
			Log($"\t\t\t >>>>> {count}");
		}

		void Log(string _str)
		{
			Console.WriteLine($"[{System.Threading.Thread.CurrentThread.ManagedThreadId} {_str}");
		}
	}
}
/*
 * [*** System.Timer.Timer ***]
20191001_084411_552 Creating timer.
[4       0 >> 20191001_084411_806
[5       1 >> 20191001_084412_053
[6       2 >> 20191001_084412_315
[7       3 >> 20191001_084412_580
[8       4 >> 20191001_084412_830
[9       5 >> 20191001_084413_095
[10      6 >> 20191001_084413_360
[11      7 >> 20191001_084413_610
[4               8 >> 2000ms
[4       9 >> 20191001_084413_875
[5               10 >> 2000ms
[5       autoReset.Set
changing period to 500ms
[6               0 >> 2000ms
[6       1 >> 20191001_084414_555
[7               2 >> 2000ms
[8               3 >> 2000ms
[7       4 >> 20191001_084415_061
[9               5 >> 2000ms
[10              6 >> 2000ms
[5       7 >> 20191001_084415_562
[11              8 >> 2000ms
[4               9 >> 2000ms
[10      10 >> 20191001_084416_076
[6               11 >> 2000ms
[6       autoReset.Set
 

	*/
