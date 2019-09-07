using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _113_01_TaskTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.DoTest2();
			Console.ReadKey();
		}

		void DoTest()
		{
			Console.WriteLine("====[Create and Start]=======");
			Task.Factory.StartNew(Run, null);
			Task.Factory.StartNew(Run, "2nd");
			Task.Factory.StartNew(new Action<object>(Run), null);
			Task.Factory.StartNew(new Action<object>(Run), "3nd");
		}

		void Run(object _obj)
		{
			Console.WriteLine(_obj == null ? "NULL" : _obj);
		}

		void Run()
		{
			Thread.Sleep(1000);
			Console.WriteLine("Empty Run1");
			Thread.Sleep(1000);
		}

		void DoTest2()
		{
			Console.WriteLine("====[Create / Start / Wait]=======");
			//Task 생성자
			Task _t1 = new Task(new Action(Run));
			Task _t2 = new Task(() =>
			{
				Thread.Sleep(1000);
				Console.WriteLine("Empty Run2");
				Thread.Sleep(1000);
			});
			Console.WriteLine(11);

			//Task 시작
			_t1.Start();
			Console.WriteLine(12);
			_t2.Start();
			Console.WriteLine(13);

			//Task 가 끝날 떄까지 대기...
			_t1.Wait();
			Console.WriteLine(14);
			_t2.Wait();
			Console.WriteLine(15);

		}
	}
}
