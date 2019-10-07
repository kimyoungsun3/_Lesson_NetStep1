using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace _116_LINQTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
							Thread.Sleep(2000);
			_p.DoTest();	Thread.Sleep(2000);
			_p.DoTest2();	Thread.Sleep(2000);

			Console.ReadLine();
		}

		void DoTest()
		{
			Console.WriteLine("=======[1. 1-> 5억 나누기 (Single)... ]========");
			Stopwatch _watch = Stopwatch.StartNew();
			_watch.Start();
			IEnumerable<int> _numInfo = Enumerable.Range(1, 50_000_000);
			var _result = _numInfo.Select(_num => _num / 2.1).ToArray();

			_watch.Stop();
			Console.WriteLine("time:" + _watch.ElapsedMilliseconds);
		}

		void DoTest2()
		{
			Console.WriteLine("=======[1. 2-> 5억 나누기 (Single)... ]========");
			Stopwatch _watch = Stopwatch.StartNew();
			_watch.Start();
			IEnumerable<int> _numInfo = Enumerable.Range(1, 50_000_000);
			var _result = _numInfo.AsParallel().Select(_num => _num / 2.1).ToArray();

			_watch.Stop();
			Console.WriteLine("time:" + _watch.ElapsedMilliseconds);
		}
	}
}
