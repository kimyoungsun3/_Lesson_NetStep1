using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Task;
using System.Threading.Tasks;

namespace _100_async_await
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Run();
		}

		private async void Run()
		{
			int sum = await LongCalc2(10);
		}

		private async Task<int> LongCalc2(int times)
		{
			//UI Thread에서 실행
			Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
			int result = 0;
			for (int i = 0; i < times; i++)
			{
				result += i;
				await Task.Delay(1000);
			}
			return result;
		}
	}
}
