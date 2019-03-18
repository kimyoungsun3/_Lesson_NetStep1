using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThread
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "MultiThread Test";
			Program _p = new Program();
			_p.Startup();
		}

		void Startup()
		{
			Thread[] _t = new Thread[2];
			for (int i = 0; i < _t.Length; i++)
			{
				_t[i] = new Thread(new ParameterizedThreadStart(SubThread));
				_t[i].Start(i);
				//_t[i].Join();
			}
		}

		int x = 0;
		void SubThread(object _number)
		{
			int _id = (int)_number;
			int _count = 0;
			while (true)
			{
				//Thread.Sleep(1);
				//Console.WriteLine("[{0}] 1>> {1}", _id, x);
				//Thread.Sleep(1);
				x++;
				//Thread.Sleep(1);
				Console.WriteLine("[{0}/{2}] >> {1}", _id, x, Thread.CurrentThread.ManagedThreadId);
				Thread.Sleep(1000);
			}
		}
	}
}
