using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPoolDD
{
	class SubClass
	{
		public void Startup(int _count = 100)
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

		public void StartupPool(int _count)
		{
			ThreadPool.SetMinThreads(_count * 2, _count * 2);
			for (int i = 0; i < _count; i++)
			{
				ThreadPool.QueueUserWorkItem(InsertThread, i + 1);
				ThreadPool.QueueUserWorkItem(OutputThread, i + 1);
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
				if (_id == 1 && _loop % 1000 == 0) Console.WriteLine("I[{0}] >> {1}", _id, _loop);

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
				if (_id == 1 && _loop % 1000 == 0) Console.WriteLine("O[{0}] >> {1}", _id, _loop);

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

		//1. 해당 오브젝트로 묶어주기..
		//   queue
		void Enqueue(int _value)
		{
			lock (queue)
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
			lock (queue)
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
