using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _100_async_await3
{
	public class Program
	{
		public delegate void Func();
		public Func pF;

		static void Main(string[] args)
		{
			//Environment.Exit(0); 
			Program p = new Program();
			p.log("main");
			Task<int> t1 = p.test2("P1");
			Task<int> t2 = p.test2("P2");
			Task<int> t3 = p.test2("P3");
			Thread.Sleep(3000);

		}


		public async Task<int> test2(string name)
		{
			//await 
			await Call(name);   //
			Console.WriteLine("======END " + name);
			return 0;
		}

		//
		private async Task<int> Call(string name)
		{
			string n = name;
			for (int i = 0; i < 10; i++)
			{
				await Task.Delay(1); //
				log("[call]" + n + " <" + i);
				if (pF != null)
				{
					pF();
				}
			}
			return 0;

			//
			/*
			Task.Run(()=>{  // <- task => Thread -> 

			   for (int i = 0; i < 100; i++) {
				  log("[call]" + name + " <" + i);
				  Thread.Sleep(1);
			   }

			});

			return 0;
			*/
		}

		public static void test1()
		{
			Console.Title = "Study App";

			Program p = new Program();

			Thread t = new Thread(p.workJob);
			t.Start();

			int l = Thread.CurrentThread.ManagedThreadId;
			Thread.Sleep(10);
			Console.WriteLine("[Thread-" + l + "]@@");
		}

		public void log(string str)
		{
			int l = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine("[Thread-" + l + "]" + str);
		}

		public void workJob()
		{
			bool b = true;

			Thread t2 = new Thread(workJob2);
			t2.Start();
			while (b)
			{

				log("1");
			}


		}

		public void workJob2()
		{
			bool b = true;
			while (b)
			{

				log("1");
			}
		}
	}
}