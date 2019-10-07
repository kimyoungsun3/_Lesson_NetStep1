using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace _113_03_TaskTest
{
	class Program
	{
		const int LOOM_MAX = 1000000;
		int num = 0;
		static void Main(string[] args)
		{
			Program _p = new Program();
			for(int i = 0; i < 5; i++)
				_p.DoTest();
			for (int i = 0; i < 5; i++)
				_p.DoTest2();
			for (int i = 0; i < 5; i++)
				_p.DoTest3();

			_p.DoTest4(); Thread.Sleep(3000);
			_p.DoTest5(); Thread.Sleep(3000);
			_p.DoTest6(); Thread.Sleep(3000);
			_p.DoTest7(); Thread.Sleep(3000);
			_p.DoTest8(); Thread.Sleep(3000);
			_p.DoTest9(); Thread.Sleep(3000);
			_p.DoTest0(); 

			Console.ReadLine();
		}

		void DoTest()
		{
			num = 0;
			Stopwatch _watch = Stopwatch.StartNew();
			Task _t1 = Task.Factory.StartNew(Fun1);
			Task _t2 = Task.Factory.StartNew(Fun2);
			Task.WaitAll(new Task[] { _t1, _t2 });
			_watch.Stop();
			Console.WriteLine("비락 num:{0} time:{1}", num, _watch.ElapsedMilliseconds);
		}

		void Fun1()
		{
			for(int i = 0; i < LOOM_MAX; i++)
			{
				num++;
			}
		}

		void Fun2()
		{
			for(int i = 0; i < LOOM_MAX; i++)
			{
				num--;
			}
		}

		void DoTest2()
		{
			num = 0;
			Stopwatch _watch = Stopwatch.StartNew();
			Task _t1 = Task.Factory.StartNew(Fun21);
			Task _t2 = Task.Factory.StartNew(Fun22);
			Task.WaitAll(new Task[] { _t1, _t2 });
			_watch.Stop();
			Console.WriteLine("lock num:{0} time:{1}", num, _watch.ElapsedMilliseconds);
		}

		object obj2 = new object();
		void Fun21()
		{
			for (int i = 0; i < LOOM_MAX; i++)
			{
				lock (obj2)
				{
					num++;
				}
			}
		}

		void Fun22()
		{
			for (int i = 0; i < LOOM_MAX; i++)
			{
				lock (obj2)
				{
					num--;
				}
			}
		}

		void DoTest3()
		{
			num = 0;
			Stopwatch _watch = Stopwatch.StartNew();
			Task _t1 = Task.Factory.StartNew(Fun31);
			Task _t2 = Task.Factory.StartNew(Fun32);
			Task.WaitAll(new Task[] { _t1, _t2 });
			_watch.Stop();
			Console.WriteLine("Interlocked num:{0} time:{1}", num, _watch.ElapsedMilliseconds);
		}

		object obj3 = new object();
		void Fun31()
		{
			for (int i = 0; i < LOOM_MAX; i++)
			{
				//Interlocked.Increment(ref num);
				//System.Threading.Interlocked(ref num);
				//Interlocked.Increment(ref num);
				//Interlocked.Increment(ref num);
				//Interlocked.Increment(ref num);
				Interlocked.Increment(ref num);
			}
		}

		void Fun32()
		{
			for (int i = 0; i < LOOM_MAX; i++)
			{
				//Interlocked.Decrement(ref num);
				//Interlocked.Decrement(ref num);
				//Interlocked.Decrement(ref num);
				//Interlocked.Decrement(ref num);
				//Interlocked.Decrement(ref num);
				//Interlocked.Decrement(ref num);
				Interlocked.Decrement(ref num);
			}
		}

		void DoTest4()
		{
			Console.WriteLine("=======[4. FunA -> FunB -> FunC ]============");
			Stopwatch _watch = Stopwatch.StartNew();

			FunA();
			FunB();
			FunC();
			_watch.Stop();
			Console.WriteLine(" 함수 실행:{0}", _watch.ElapsedMilliseconds);
		}

		void DoTest5()
		{
			Console.WriteLine("=======[5. Task, .Start .Wait ]============");
			Stopwatch _watch = Stopwatch.StartNew();
			Task _ta = new Task(FunA);
			Task _tb = new Task(FunB);
			Task _tc = new Task(FunC);

			_ta.Start();
			_tb.Start();
			_tc.Start();

			_ta.Wait();
			_tb.Wait();
			_tc.Wait();
			_watch.Stop();
			Console.WriteLine("task 함수 실행:{0}", _watch.ElapsedMilliseconds);
		}

		void DoTest6()
		{
			Console.WriteLine("=======[6. Task .Start...]============");
			Stopwatch _watch = Stopwatch.StartNew();
			Task _ta = new Task(FunA);
			Task _tb = new Task(FunB);
			Task _tc = new Task(FunC);
			_ta.Start();
			_tb.Start();
			_tc.Start();

			_watch.Stop();
			Console.WriteLine("task 함수 실행:{0}", _watch.ElapsedMilliseconds);
		}

		void DoTest7()
		{
			Console.WriteLine("=======[7. Task.Factory.StartNew -> Task.WaitAll...]============");
			Stopwatch _watch = Stopwatch.StartNew();
			Task _ta = Task.Factory.StartNew(FunA);
			Task _tb = Task.Run(new Action(FunB));
			Task _tc = Task.Factory.StartNew(FunC);
			Console.WriteLine(11);
			Thread.Sleep(1000);
			Console.WriteLine(12);
			Task.WaitAll(new Task[] { _ta, _tb, /*_tc*/ });

			_watch.Stop();
			Console.WriteLine("task 함수 실행:{0}", _watch.ElapsedMilliseconds);
		}

		void FunA()
		{
			Console.WriteLine("[{0}]A함수 시작", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(1000);
			Console.WriteLine("[{0}]A함수 끝", Thread.CurrentThread.ManagedThreadId);
		}

		void FunB()
		{
			Console.WriteLine("[{0}]B함수 시작", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(2000);
			Console.WriteLine("[{0}]B함수 끝", Thread.CurrentThread.ManagedThreadId);
		}

		void FunC()
		{
			Console.WriteLine("[{0}]C함수 시작", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(3000);
			Console.WriteLine("[{0}]C함수 끝", Thread.CurrentThread.ManagedThreadId);
		}

		void DoTest8()
		{
			Console.WriteLine("=======[8. Task 전달-> Task 받아처리..]=========");
			var _test = Task.Factory.StartNew(TestA).ContinueWith(
							_task => TestB(_task.Result)
						);
			Console.WriteLine("result:" + _test.Result);
		}

		int TestA()
		{
			Console.WriteLine("[{0}]TestA함수 시작", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(1000);
			Console.WriteLine("[{0}]TestA함수 끝", Thread.CurrentThread.ManagedThreadId);
			return 10;
		}

		int TestB(int _val)
		{
			Console.WriteLine("[{0}]TestB함수 시작", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(2000);
			Console.WriteLine("[{0}]TestB함수 끝", Thread.CurrentThread.ManagedThreadId);
			return _val + 10;
		}


		void DoTest9()
		{
			Console.WriteLine("=======[9. Task(outer) -> Task(Inner) .]=========");

			Console.WriteLine("Main Start[{0}]", Thread.CurrentThread.ManagedThreadId);
			Task _outer = Task.Factory.StartNew(()=>
			{
				Console.WriteLine("Outer Task Start[{0}]", Thread.CurrentThread.ManagedThreadId);
				Task _inner = Task.Factory.StartNew(() =>
				{
					Console.WriteLine(" Inner Task Start[{0}]", Thread.CurrentThread.ManagedThreadId);
					Thread.Sleep(2000);
					Console.WriteLine(" Inner Task End[{0}]", Thread.CurrentThread.ManagedThreadId);
				});
				Thread.Sleep(1000);
				Console.WriteLine("Outer Task End[{0}]", Thread.CurrentThread.ManagedThreadId);
			});
			_outer.Wait();
			Console.WriteLine("Main End[{0}]", Thread.CurrentThread.ManagedThreadId);
		}
		void DoTest0()
		{
			Console.WriteLine("=======[0. Task(outer) -> Task(Inner) .]=========");

			Console.WriteLine("Main Start[{0}]", Thread.CurrentThread.ManagedThreadId);
			Task _outer = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Outer Task Start[{0}]", Thread.CurrentThread.ManagedThreadId);
				Task _inner = Task.Factory.StartNew(() =>
				{
					Console.WriteLine(" Inner Task Start[{0}]", Thread.CurrentThread.ManagedThreadId);
					Thread.Sleep(2000);
					Console.WriteLine(" Inner Task End[{0}]", Thread.CurrentThread.ManagedThreadId);
				}, TaskCreationOptions.AttachedToParent);
				//Thread.Sleep(1000);
				Console.WriteLine("Outer Task End[{0}]", Thread.CurrentThread.ManagedThreadId);
			});
			_outer.Wait();
			Console.WriteLine("Main End[{0}]", Thread.CurrentThread.ManagedThreadId);
		}
	}
}
