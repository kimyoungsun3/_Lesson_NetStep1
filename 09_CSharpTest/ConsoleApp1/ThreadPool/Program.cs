using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPoolDD
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.DoTest2(50);
			_p.DoTest2(100);
			_p.DoTest2(200);

			SubClass _sub = new SubClass();
			_sub.Startup(100);		//200 x 2 => 400개
			_sub.StartupPool(100);//200 x 2 => 400개

			Console.ReadKey();
		}

		void DoTest()
		{
			Console.WriteLine("======[ThreadPool 100 -> 3개사용]=======");

			//스레드 풀에 있는 쓰레드를 이용하여 실행한후에 리턴값이 없는 경우...
			ThreadPool.SetMinThreads(100, 100);
			ThreadPool.QueueUserWorkItem(Calc);
			ThreadPool.QueueUserWorkItem(Calc, 10.0);
			ThreadPool.QueueUserWorkItem(Calc, 20.0);
		}

		void Calc(object _obj)
		{
			if (_obj == null) return;
			double _r = (double)_obj;
			double _area = _r * _r * 3.14;
			Console.Write("r={0}, area={1}\t", _r, _area);
		}

		void DoTest2(int _count)
		{
			Console.WriteLine("======[ThreadPool 100 -> for X "+ _count + "]=======");
			//스레드 풀에 있는 쓰레드를 이용하여 실행한후에 리턴값이 없는 경우...
			ThreadPool.SetMinThreads(100, 100);
			for(int i = 0; i < _count; i++)
			{
				ThreadPool.QueueUserWorkItem(Calc, i*10.0);
			}
		}
	}
}
