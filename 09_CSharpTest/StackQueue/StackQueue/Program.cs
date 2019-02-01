using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackQueue
{
	class CUserToken {
		public byte[] buffer = new byte[1024];
	}

	class Program
	{
		public int LOOP = 1000000;
		public Stack<int> stack = new Stack<int>();
		public Queue<int> queue = new Queue<int>();
		static DateTime[] dt = new DateTime[10];
		byte[] buffer = new byte[1024];
		byte[] buffer2 = new byte[1024];
		public Queue<CUserToken> freeUser = new Queue<CUserToken>();

		void Init()
		{
			CUserToken _token;
			for (int i = 0; i < 2000; i++)
			{
				_token = new CUserToken();
				freeUser.Enqueue(_token);
			}
		}

		static void Main(string[] args)
		{
			Program p = new Program();
			p.Init();

			dt[0] = DateTime.Now;
			p.StackTest();
			dt[1] = DateTime.Now;
			p.QueueTest();
			dt[2] = DateTime.Now;
			p.ClearBuffer();
			dt[3] = DateTime.Now;
			p.CopyBuffer();
			dt[4] = DateTime.Now;
			p.CreateClass();
			dt[5] = DateTime.Now;
			p.UseQueueClass();
			dt[6] = DateTime.Now;
			dt[7] = DateTime.Now;
			dt[8] = DateTime.Now;
			dt[9] = DateTime.Now;

			Console.WriteLine("StackTest:" + (int)((TimeSpan)(dt[1] - dt[0])).TotalMilliseconds);
			Console.WriteLine("QueueTest:" + (int)((TimeSpan)(dt[2] - dt[1])).TotalMilliseconds);
			Console.WriteLine("ClearBuffer:" + (int)((TimeSpan)(dt[3] - dt[2])).TotalMilliseconds);
			Console.WriteLine("CopyBuffer:" + (int)((TimeSpan)(dt[4] - dt[3])).TotalMilliseconds);
			Console.WriteLine("CreateClass:"+(int)((TimeSpan)(dt[5] - dt[4])).TotalMilliseconds);
			Console.WriteLine("UseQueueClass:" + (int)((TimeSpan)(dt[6] - dt[5])).TotalMilliseconds);
			Console.WriteLine("" + (int)((TimeSpan)(dt[7] - dt[6])).TotalMilliseconds);
			Console.WriteLine("" + (int)((TimeSpan)(dt[8] - dt[7])).TotalMilliseconds);
			Console.WriteLine("" + (int)((TimeSpan)(dt[9] - dt[8])).TotalMilliseconds);
		}

		public void UseQueueClass()
		{
			CUserToken _token;
			for (int i = 0; i < LOOP; i++)
			{
				_token = freeUser.Dequeue();
				freeUser.Enqueue(_token);
			}
		}

		public void CreateClass()
		{
			CUserToken _token;
			for (int i = 0; i < LOOP; i++)
			{
				_token = new CUserToken();
			}
		}

		public void ClearBuffer()
		{
			for (int i = 0; i < LOOP; i++)
				Array.Clear(buffer, 0, buffer.Length);
		}

		public void CopyBuffer()
		{
			int _len = buffer.Length;
			for (int i = 0; i < _len; i++)
				buffer2[i] = (byte)(i % 20);

			for (int i = 0; i < LOOP; i++)
				Array.Copy(buffer2, 0, buffer, 0, _len);
		}

		public void QueueTest()
		{
			Random _rand = new Random();
			int _value;
			for (int i = 0; i < LOOP; i++)
			{
				_value = _rand.Next();
				if (_value % 2 == 0)
				{
					queue.Enqueue(i);
				}
				else
				{
					if (queue.Count > 0)
					{
						queue.Dequeue();
					}
				}
			}
		}

		public void StackTest()
		{
			Random _rand = new Random();
			int _value;
			for (int i = 0; i < LOOP; i++)
			{
				_value = _rand.Next();
				if (_value % 2 == 0)
				{
					stack.Push(i);
				}
				else
				{
					if (stack.Count > 0)
					{
						stack.Pop();
					}
				}
			}
		}
	}

}
