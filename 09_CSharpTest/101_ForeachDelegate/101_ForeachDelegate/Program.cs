using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //.NET 4.5
using System.Threading;

namespace _101_ForeachDelegate
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();

			_p.TestTask();
			_p.DoTest();

			Console.ReadLine();
		}
		
		void TestTask()
		{
			Console.WriteLine("Fun() -> 리스트에담아... foreach ..");
			var heavyQuery = Enumerable.Range(0, 10).Where(c =>
			{
				Task.Delay(1000).Wait();
				//Thread.Sleep(1000);
				return true;
			});

			DateTime _start = DateTime.Now;
			foreach (var _item in heavyQuery)
			{
				Console.Write(_item);
			}
			Console.WriteLine(" 소요시간:{0}", DateTime.Now - _start);
		}

		void DoTest()
		{
			Console.WriteLine("Fun() -> 리스트에담아... foreach ..");
			var _query = Enumerable.Range(0, 10).Where(_c =>
			{
				Thread.Sleep(1000);
				return true;
			});

			foreach(var _item in _query)
			{
				Console.Write(_item);
			}
		}
	}
}
