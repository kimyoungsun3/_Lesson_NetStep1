using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _118_delegate
{
	delegate void VOID_FUN_VOiD();
	delegate int INT_FUN_INT_INT(int _a, int _b);
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.DoTest2();
			_p.DoTest3();

			Console.ReadLine();
		}

		void DoTest()
		{
			VOID_FUN_VOiD _on;
			INT_FUN_INT_INT _on2;

			_on = delegate ()
			{
				Console.WriteLine("Hi");
			};
			if (_on != null)
				_on();


			_on = () => Console.WriteLine("Hi2");
			_on();

			_on2 = (int _a, int _b) => (_a + _b);
			Console.WriteLine(_on2(1, 2));

			_on2 = (_a, _b) => (_a * _b);
			Console.WriteLine(_on2(100, 100));

			_on2 = (_a, _b) =>
			{
				if (_a > _b)
					return _a + _b;
				else
					return _a * _b;
			};
			Console.WriteLine(_on2(10, 20));
			Console.WriteLine(_on2(20, 10));
		}

		void DoTest2()
		{
			Console.WriteLine("======[Dictionary]========");
			Dictionary<string, int> _dic = new Dictionary<string, int>
			{
				{"dog", 2 },
				{"iguana", -1 },
				{"cat", 1 },
				{"cow", 0 }
			};
			Console.WriteLine(_dic["dog"]);

			foreach (KeyValuePair<string, int> _pair in _dic)
			{
				Console.WriteLine(_pair.Key + " => " + _pair.Value);
			}

			List<string> _list = new List<string>(_dic.Keys);
			for(int i = 0; i < _list.Count; i++)
			{
				Console.WriteLine(i + " => " + _list[i]);
			}
		}

		void DoTest3()
		{
			Console.WriteLine("=====[3. CPU 1]======");
			for (int i = 0; i < 10; i++)
				Console.WriteLine("{0} => {1}", Thread.CurrentThread.ManagedThreadId, i);

			Console.WriteLine("=====[3. CPU n]======");
			Parallel.For (0, 10, (i) => {
				Console.WriteLine("{0} => {1}", Thread.CurrentThread.ManagedThreadId, i);
			});
		}
	}
}
