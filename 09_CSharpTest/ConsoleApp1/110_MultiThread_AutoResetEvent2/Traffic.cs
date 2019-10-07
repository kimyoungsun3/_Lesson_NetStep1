using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _110_MultiThread_AutoResetEvent2
{

	class Traffic
	{
		Queue<int> queueVertical = new Queue<int>();
		Queue<int> queueHorizontal = new Queue<int>();
		AutoResetEvent autoVertical = new AutoResetEvent(true);
		AutoResetEvent autoHorizontal = new AutoResetEvent(false);
		public bool running = true;

		public void ProcessInputQueue()
		{
			while (true)
			{
				Console.WriteLine("====[Push Start]======");
				for (int i = 0; i < 6; i += 3)
				{
					Console.WriteLine("Push Start V>> " + i);
					AddVertical(new int[] { i, i + 1, i + 2 });
					Console.WriteLine("Push Start H>> " + i);
					AddHorizontal(new int[] { i, i + 1, i + 2 });
					Console.WriteLine("Push End >> " + i);
					Thread.Sleep(1000);
				}
				Console.WriteLine("====[Push End]======");
			}

		}

		public void ProcessVertical()
		{
			string _m = "\t\t[" + Thread.CurrentThread.ManagedThreadId + "] ";
			Console.WriteLine(_m + "========[ProcessVertical start]========");
			while (running)
			{
				Console.WriteLine(_m + " V ... WaitOne");
				autoVertical.WaitOne();
				Console.WriteLine(_m + " >> V Processing..." + Thread.CurrentThread.ManagedThreadId); ;

				lock (queueVertical)
				{
					while (queueVertical.Count > 0)
					{
						int _val = queueVertical.Dequeue();
						Console.WriteLine(_m + "  >> V:" + _val + " :" + queueVertical.Count + " :" + Thread.CurrentThread.ManagedThreadId);
						Thread.Sleep(500);
					}
				}

				autoHorizontal.Set();
				Console.WriteLine(_m + " V -> H .set");
			}
			Console.WriteLine(_m + "========[ProcessVertical end]========");
		}

		public void ProcessHorizontal()
		{
			string _m = "\t\t\t\t[" + Thread.CurrentThread.ManagedThreadId + "] ";
			Console.WriteLine(_m + "========[ProcessHorizontal start]========");
			while (running)
			{
				Console.WriteLine(_m + " H ... WaitOne");
				autoHorizontal.WaitOne();
				Console.WriteLine(_m + " >> H Processing...");

				lock (queueHorizontal)
				{
					while (queueHorizontal.Count > 0)
					{
						int _val = queueHorizontal.Dequeue();
						Console.WriteLine(_m + "  >> H:" + _val + " :" + queueHorizontal.Count + " :" + Thread.CurrentThread.ManagedThreadId);
						Thread.Sleep(500);
					}
				}

				autoVertical.Set();
				Console.WriteLine(_m + " H -> V .set");
			}
			Console.WriteLine(_m + "========[ProcessHorizontal end]========");
		}

		public void AddVertical(int[] _array)
		{
			lock (queueVertical)
			{
				for (int i = 0; i < _array.Length; i++)
				{
					Console.WriteLine("  AddVertical >>" + i);
					queueVertical.Enqueue(_array[i]);
				}
			}
		}

		public void AddHorizontal(int[] _array)
		{
			lock (queueHorizontal)
			{
				for (int i = 0; i < _array.Length; i++)
				{
					Console.WriteLine("    AddHorizontal >>" + i);
					queueHorizontal.Enqueue(_array[i]);
				}
			}
		}
	}
}
