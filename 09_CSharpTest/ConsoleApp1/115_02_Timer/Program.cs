using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _115_02_Timer
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			while (true)
			{
				Thread.Sleep(1000);
			}
		}

		AutoResetEvent autoReset = new AutoResetEvent(false);
		System.Threading.Timer stateTimer;
		void DoTest()
		{
			Console.WriteLine("[*** System.Threading.Timer ***]");
			StatusChecker _status = new StatusChecker(10);

			//string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss");
			string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
			Console.WriteLine($"{_time} Creating timer.");
			stateTimer = new Timer(_status.CheckStatus, autoReset, 1000, 250);
			autoReset.WaitOne();

			//when AutoEvent signal, change the period to every half Seconds.
			stateTimer.Change(0, 500);
			Console.WriteLine("changing period to 500ms");
			autoReset.WaitOne();

			//When auto signals the second time, 
			stateTimer.Dispose();
			Console.WriteLine("Destroying Timer");
		}
	}


	class StatusChecker {
		private int count, maxCount;
		AutoResetEvent autoReset;

		public StatusChecker(int _maxCount)
		{
			count		= 0;
			maxCount	= _maxCount;
		}

		//This method is called by the timer delegate...
		public void CheckStatus(object _obj)
		{
			count++;
			autoReset = (AutoResetEvent)_obj;
			string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
			Log($"\t {count} >> {_time}");
			Thread.Sleep(2000);
			Log($"\t\t {count} >> 2000ms");

			if(count > maxCount)
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
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId} {_str}");
		}
	}
}

/*
[*** System.Threading.Timer ***]
20191001_085259_552 Creating timer.
[4       1 >> 20191001_085300_570
[5       2 >> 20191001_085300_817
[6       3 >> 20191001_085301_067
[7       4 >> 20191001_085301_331
[8       5 >> 20191001_085301_596
[9       6 >> 20191001_085301_846
[10      7 >> 20191001_085302_111
[11      8 >> 20191001_085302_376
[4               8 >> 2000ms
[4       9 >> 20191001_085302_626
[5               9 >> 2000ms
[5       10 >> 20191001_085302_891
[6               10 >> 2000ms
[6       11 >> 20191001_085303_156
[7               11 >> 2000ms
[7       autoReset.Set
changing period to 500ms
[7       1 >> 20191001_085303_333
[8               1 >> 2000ms
[8       2 >> 20191001_085303_842
[9               2 >> 2000ms
[10              2 >> 2000ms
[10      3 >> 20191001_085304_342
[11              3 >> 2000ms
[4               3 >> 2000ms
[11      4 >> 20191001_085304_856
[5               4 >> 2000ms
[6               4 >> 2000ms
[7               4 >> 2000ms
[4       5 >> 20191001_085305_356
[8               5 >> 2000ms
[5       6 >> 20191001_085305_870
[10              6 >> 2000ms
[7       7 >> 20191001_085306_370
[11              7 >> 2000ms
[11      8 >> 20191001_085306_884
[4               8 >> 2000ms
[4       9 >> 20191001_085307_384
[5               9 >> 2000ms
[10      10 >> 20191001_085307_898
[7               10 >> 2000ms
[8       11 >> 20191001_085308_398
[11              11 >> 2000ms
[11      autoReset.Set
Destroying Timer
[4               0 >> 2000ms
[10              0 >> 2000ms
[8               0 >> 2000ms


	*/
