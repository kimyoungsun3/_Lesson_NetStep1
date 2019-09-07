using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreadQueue3_Resolution_objectother
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "MultiThreadQueue3_3_Resolution_objectother";
			Console.WriteLine(Console.Title);
			Program _p = new Program();
			_p.Startup(100);

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
			for (int i = 0; i < _count; i++)
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
			int _loop = 0;
			while (true)
			{
				_loop++;
				if (_loop % 1000 == 0) Console.WriteLine("I[{0}] >> {1}", _id, _loop);

				_x++;
				//Thread.Sleep(1);
				Enqueue(_id * 100000 + _x);
				//Thread.Sleep(1);
				//Console.WriteLine("InsertThread [{0}/{1}] >> {2}", _id, Thread.CurrentThread.ManagedThreadId, queue.Count);
				Thread.Sleep(1);
			}
		}

		void OutputThread(object _number)
		{
			int _id = (int)_number;
			int _x = 0;
			int _loop = 0;
			while (true)
			{
				_loop++;
				if (_loop % 1000 == 0) Console.WriteLine("O[{0}] >> {1}", _id, _loop);

				_x = -1;
				//Thread.Sleep(1);
				if (CheckQueue())
					_x = Dequeue();
				//Thread.Sleep(1);
				//Console.WriteLine("OutputThread [{0}/{1}] >> {2} >> {3}", _id, Thread.CurrentThread.ManagedThreadId, queue.Count, _x);
				Thread.Sleep(1);
			}
		}

		bool CheckQueue()
		{
			//lock (queue)
			{
				return queue.Count > 0;
			}
		}

		//2 object로 테스트
		object obj1 = new object();
		object obj2 = new object();
		void Enqueue(int _value)
		{
			lock (obj1)
			{
				//System.ArgumentException: '소스 배열의 길이가 짧습니다. srcIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '대상 배열의 길이가 짧습니다. destIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '소스 배열의 길이가 짧습니다. srcIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '대상 배열의 길이가 짧습니다. destIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '대상 배열의 길이가 짧습니다. destIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '대상 배열의 길이가 짧습니다. destIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '소스 배열의 길이가 짧습니다. srcIndex, length 및 배열의 하한을 확인하십시오.'
				//System.ArgumentException: '소스 배열의 길이가 짧습니다. srcIndex, length 및 배열의 하한을 확인하십시오.'
				queue.Enqueue(_value);
			}
		}

		int Dequeue()
		{
			lock (obj2)
			{
				//System.InvalidOperationException: '큐가 비어 있습니다.'
				//System.InvalidOperationException: '큐가 비어 있습니다.'
				if (queue.Count > 0)
					return queue.Dequeue();
				else
					return -1;
			}
		}
	}
}
