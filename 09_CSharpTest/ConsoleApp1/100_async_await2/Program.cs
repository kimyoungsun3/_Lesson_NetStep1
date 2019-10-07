using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _100_async_await2
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Run1();
			Thread.Sleep(5000);

			_p.Run2();

			Console.ReadLine();
		}

		private async void Run1()
		{
			Console.WriteLine("-----Run1-----");
			await LongCalc2(1, 4);
			await LongCalc2(2, 4);
			await LongCalc2(3, 4);
			Console.WriteLine("------Run1 end----");
		}

		private async void Run2()
		{
			Console.WriteLine("----Run2------");
			Task<int>.Run(() => LongCalc2(1, 4));
			Task<int>.Run(() => LongCalc2(2, 4));
			Task<int>.Run(() => LongCalc2(3, 4));
			Console.WriteLine("----Run2 end------");
		}

		private async Task<int> LongCalc2(int _id, int _times)
		{
			int result = 0;
			for (int i = 0; i < _times; i++)
			{
				result += i;
				Console.WriteLine(_id + ":" + Thread.CurrentThread.ManagedThreadId + " >> " + i);
				await Task.Delay(100);
			}
			return result;
		}
	}
}
