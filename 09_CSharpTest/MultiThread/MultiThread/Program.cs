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
			//Console.Title = "MultiThread Test";
			//ThreadTest1 _p = new ThreadTest1();
			//_p.Startup();

			ThreadTest2 _p = new ThreadTest2();
			_p.Start();

		}
	}
}
