using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreadQueue
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "MultiThreadQueue Test";
			Program _p = new Program();
			_p.Startup(2);
		}

		void Startup(int _count)
		{
			Thread[] _t = new Thread[_count];
			for (int i = 0; i < _count; i++)
			{
				_t[i] = new Thread(new ParameterizedThreadStart(SubThread));
				_t[i].Start(i + 1);
			}
		}

		Queue<int> queue = new Queue<int>();
		void SubThread(object _number)
		{
			int _id = (int)_number;
			int _x = 0;
			int _value;
			string _msg;
			while (true)
			{
				_x++;
				_value = -1;
				Thread.Sleep(1);
				if (CheckQueue())
				{
					Thread.Sleep(1);
					_value = Dequeue();
					_msg = "Dequeue";
				}
				else
				{
					Thread.Sleep(1);
					Enqueue(_id * 100000 + _x);
					_msg = "Enqueue";
				}
				Thread.Sleep(1);
				Console.WriteLine("[{0}/{1}] {2} >> {3} >> {4}", 
					_id, Thread.CurrentThread.ManagedThreadId,
					_msg,
					queue.Count,
					_value
					);
				Thread.Sleep(1000);
			}
		}

		bool CheckQueue()
		{
			return queue.Count > 0;
		}

		void Enqueue(int _value)
		{
			queue.Enqueue(_value);
		}

		int Dequeue()
		{
			return queue.Dequeue();
		}
	}
}
