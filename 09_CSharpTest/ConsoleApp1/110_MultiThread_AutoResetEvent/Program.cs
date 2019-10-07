using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _110_MultiThread_AutoResetEvent
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			Console.ReadLine();
		}

		AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		void DoTest()
		{
			string _msg = Thread.CurrentThread.ManagedThreadId.ToString();
			Console.WriteLine("======[1. DoTest("+ _msg + ")]=========");
			Thread _t1 = new Thread(Run);
			_t1.Start();

			Console.WriteLine(_msg + " >> sleep");
			Thread.Sleep(3000);
			Console.WriteLine(_msg + " >> wait >> Set1");
			autoResetEvent.Set();
			Console.WriteLine(_msg + " >> wait >> Set2");
		}

		void Run()
		{
			string _msg = Thread.CurrentThread.ManagedThreadId.ToString();
			Console.WriteLine("======[Run(" + _msg + ")]======");

			autoResetEvent.WaitOne();

			Console.WriteLine(_msg + " >> Run Wake");

		}
	}
}
