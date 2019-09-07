using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _112_Thread
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.VariableThreadStart();
			_p.OtherClassThreadStart();
			_p.ParamterThreadStart();
			Console.ReadKey();
		}

		void DoTest()
		{
			Console.WriteLine("======[Thread Start]=======");
			//Thread _t1 = new Thread(new ThreadStart(Run));
			//new ThreadStart				
			// => public delegate void ThreadStart();
			//new ParameterizedThreadStart	
			// => public delegate void ParameterizedThreadStart(object obj);
			Thread _t1 = new Thread(Run);
			_t1.Start();
			Run();
		}
		void Run()
		{
			Console.WriteLine("Thread#{0}: Begin", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(3000);
			Console.WriteLine("Thread#{0}: End", Thread.CurrentThread.ManagedThreadId);
		}
		void VariableThreadStart()
		{

			Console.WriteLine("======[다양한 Thread Start]=======");
			Thread _t1 = new Thread(new ThreadStart(Run));
			_t1.Start();

			Thread _t2 = new Thread(Run);
			_t2.Start();

			Thread _t3 = new Thread(delegate()
			{
				Run();
			});
			_t3.Start();

			Thread _t4 = new Thread(() => Run());
			_t4.Start();

			new Thread(() => Run()).Start();
		}
		void OtherClassThreadStart()
		{

			Console.WriteLine("======[다른 클래스의 함수 Thread Start]=======");
			Helper _h = new Helper();
			Thread _t1 = new Thread(_h.Run);
			_t1.Start();
		}
		void ParamterThreadStart()
		{
			Console.WriteLine("======[파라미터 전달 Thread Start]=======");
			Thread _t1 = new Thread(Run);
			_t1.Start();

			Thread _t2 = new Thread(new ParameterizedThreadStart(Calc));
			_t2.Start(10.00);

			Thread _t3 = new Thread(() => Sum(10, 20, 30));
			_t3.Start();

		}

		void Calc(object _obj)
		{
			double _r = (double)_obj;
			double _area = _r * _r * 3.14;
			Console.WriteLine("r={0}, area={1}", _r, _area);
		}

		void Sum(int _d1, int _d2, int _d3)
		{
			int loop = 5;
			while (loop > 0)
			{
				int _sum = _d1 + _d2 + _d3;
				Console.WriteLine("sum:" + _sum);
				loop--;
				Thread.Sleep(1000);
			}
		}
	}

	class Helper
	{
		public void Run()
		{
			Console.WriteLine("Helper.Run");
		}
	}
}
