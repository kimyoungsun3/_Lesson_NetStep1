using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreadQueue3
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "MultiThreadQueue2 Test111";
			Program _p = new Program();
			_p.Startup(91);
		}

		void Startup(int _count)
		{
			Thread[] _t = new Thread[_count];
			for (int i = 0; i < _count; i++)
			{
				_t[i] = new Thread(new ParameterizedThreadStart(InsertThread));
				_t[i].Start(i + 1);
			}

			Thread[] _t2 = new Thread[_count];
			for (int i = 0; i < 1; i++)
			{
				_t2[i] = new Thread(new ParameterizedThreadStart(OutputThread));
				_t2[i].Start(i + 1);
			}
		}

		Queue<int> queue = new Queue<int>();
		void InsertThread(object _number)
		{
			int _id = (int)_number;
			int _x = 0;
			while (true)
			{
				_x++;
				Thread.Sleep(1);
				Enqueue(_id * 100000 + _x);
				Thread.Sleep(1);
				Console.WriteLine("IH [{0}/{1}] >> {2}", _id, Thread.CurrentThread.ManagedThreadId, queue.Count);
				Thread.Sleep(1000);
			}
		}

		void OutputThread(object _number)
		{
			int _id = (int)_number;
			int _x = 0;
			while (true)
			{
				Thread.Sleep(1);
				_x = -1;
				if (CheckQueue())
					_x = Dequeue();
				Thread.Sleep(1);
				Console.WriteLine("OT [{0}/{1}] >> {2} >> {3}",
					_id, Thread.CurrentThread.ManagedThreadId, queue.Count,
					_x);

				if (CheckQueue())
					Thread.Sleep(1);
				else
					Thread.Sleep(1000);
			}
		}

		bool CheckQueue()
		{
			return queue.Count > 0;
		}

		void Enqueue(int _value)
		{
			lock (queue)
			{
				queue.Enqueue(_value);
			}
		}

		int Dequeue()
		{
			lock (queue)
			{
				return queue.Dequeue();
			}
		}
	}
}
