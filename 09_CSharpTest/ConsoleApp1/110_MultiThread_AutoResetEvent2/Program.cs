using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _110_MultiThread_AutoResetEvent2
{
	class Program
	{
		static void Main(string[] args)
		{

			Program _p = new Program();
			//_p.DoTest();
			_p.DoTest2();

			Console.ReadLine();
		}

		void DoTest()
		{
			Traffic _traffic = new Traffic();

			Thread _tInput			= new Thread(_traffic.ProcessInputQueue);
			Thread _tVertical		= new Thread(_traffic.ProcessVertical);
			Thread _tHorizontal		= new Thread(_traffic.ProcessHorizontal);
			_tInput.Start();
			_tVertical.Start();
			_tHorizontal.Start();
			Task.Factory.StartNew(_traffic.ProcessInputQueue);
			Task.Factory.StartNew(_traffic.ProcessInputQueue);
			Task.Factory.StartNew(_traffic.ProcessInputQueue);
		}

		void DoTest2()
		{
			Traffic2 _traffic = new Traffic2();

			Thread _tInput		= new Thread(_traffic.ProcessInputQueue);
			Thread _tVertical	= new Thread(_traffic.ProcessVertical);
			Thread _tHorizontal = new Thread(_traffic.ProcessHorizontal);

			_tInput.Start();
			_tVertical.Start();
			_tHorizontal.Start();

			new Thread(_traffic.ProcessInputQueue).Start();
			new Thread(_traffic.ProcessInputQueue).Start();
			new Thread(_traffic.ProcessInputQueue).Start();
		}
	}

}
