using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _113_02_TaskTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			Console.ReadKey();
		}

		async void DoTest()
		{
			Console.WriteLine(11);
			Task<double> _t1 = Task<double>.Factory.StartNew(
				() => LongCalc(10)
			);

			Console.WriteLine(12);
			await _t1;

			Console.WriteLine(13);
			Console.WriteLine("result:{0}", _t1.Result.ToString());
		}

		double LongCalc(double _r)
		{
			Thread.Sleep(5000);
			return 3.14 * _r * _r;
		}
	}
}
