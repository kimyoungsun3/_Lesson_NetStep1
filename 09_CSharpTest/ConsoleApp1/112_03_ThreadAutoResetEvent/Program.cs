using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _112_03_ThreadAutoResetEvent
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			Console.ReadKey();
		}

		void DoTest()
		{
			Thread _t1 = new Thread(new ThreadStart(RunAndWait));
			_t1.Start();

			Thread _t2 = new Thread(RunAndWake);
			_t2.Start();
		}

		AutoResetEvent autoResetEvent;
		void RunAndWait()
		{
			autoResetEvent = new AutoResetEvent(false);
			while (true)
			{
				Console.WriteLine("RunAndWait -> Fun ....");
				autoResetEvent.WaitOne();
			}
		}

		void RunAndWake()
		{
			while (true)
			{
				Thread.Sleep(2000);
				Console.WriteLine("New Client accept -> Wake and SocketWait");
				autoResetEvent.Set();
			}
		}
	}
}
