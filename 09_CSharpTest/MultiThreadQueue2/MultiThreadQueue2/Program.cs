using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreadQueue2
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "MultiThreadQueue2 Test";
			Program _p = new Program();
			_p.Startup(9);
		}

		void Startup(int _count)
		{
			Thread[] _t = new Thread[_count];
			for (int i = 0; i < _count; i++)
			{
				_t[i] = new Thread(new ParameterizedThreadStart(InsertThread));
				_t[i].Start(i + 1);
			}

			Thread[] _t2 = new Thread[1];
			for (int i = 0; i < 1; i++)
			{
				_t[i] = new Thread(new ParameterizedThreadStart(OutputThread));
				_t[i].Start(i + 1);
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
				if(CheckQueue())
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
			queue.Enqueue(_value);
		}

		//처리되지 않은 예외:처리되지 않은 예외:  처리되지 않은 예외: System.ArgumentException: 소스 배열의 길이가 짧습니다.srcIndex, length 및 배열의 하한을 확인하십시오.
		//   위치: System.Array.Copy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length, Boolean reliable)
		//   위치: System.Collections.Generic.Queue`1.SetCapacity(Int32 capacity)
		//   위치: System.Collections.Generic.Queue`1.Enqueue(T item)
		//   위치: MultiThreadQueue2.Program.Enqueue(Int32 _value) 파일 D:\devtool\study\study\_Lesson_NetStep1\09_CSharpTest\MultiThreadQueue2\MultiThreadQueue2\Program.cs:줄 80
		//   위치: MultiThreadQueue2.Program.InsertThread(Object _number) 파일 D:\devtool\study\study\_Lesson_NetStep1\09_CSharpTest\MultiThreadQueue2\MultiThreadQueue2\Program.cs:줄 44
		//   위치: System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
		//   위치: System.Threading.ThreadHelper.ThreadStart(Object obj) System.ArgumentException: 소스 배열의 길이가 짧습니다. srcIndex, length 및 배열의 하한을 확인하십시오.

		//   위치: System.Array.Copy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length, Boolean reliable)
		//   위치: System.Collections.Generic.Queue`1.SetCapacity(Int32 capacity)
		//   위치: System.Collections.Generic.Queue`1.Enqueue(T item)
		//   위치: MultiThreadQueue2.Program.Enqueue(Int32 _value) 파일 D:\devtool\study\study\_Lesson_NetStep1\09_CSharpTest\MultiThreadQueue2\MultiThreadQueue2\Program.cs:줄 80
		//   위치: MultiThreadQueue2.Program.InsertThread(Object _number) 파일 D:\devtool\study\study\_Lesson_NetStep1\09_CSharpTest\MultiThreadQueue2\MultiThreadQueue2\Program.cs:줄 44
		//   위치: System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
		//   위치: System.Threading.ThreadHelper.ThreadStart(Object obj) 계속하려면 아무 키나 누르십시오. . .



				int Dequeue()
		{
			return queue.Dequeue();
		}
	}
}
