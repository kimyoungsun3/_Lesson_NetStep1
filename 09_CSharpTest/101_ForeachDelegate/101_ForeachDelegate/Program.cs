using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //.NET 4.5

namespace _101_ForeachDelegate
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();

			_p.TestTask();
		}
		
		void TestTask()
		{
			Console.WriteLine("Fun() -> 리스트에담아... foreach ..");
			var heavyQuery = Enumerable.Range(0, 10).Where(c =>
			{
				Task.Delay(1000).Wait();
				return true;
			});

			DateTime _start = DateTime.Now;
			foreach (var _item in heavyQuery)
			{
				Console.Write(_item);
			}
			Console.WriteLine(" 소요시간:{0}", DateTime.Now - _start);
		}
	}
}
