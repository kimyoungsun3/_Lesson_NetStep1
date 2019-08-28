using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackQueue
{
	public class CUserToken
	{
		public byte[] buffer = new byte[1024];
	}

	public class SpeedTest1
	{
		public int LOOP_MAX = 1000000;
		public Stack<int> stack = new Stack<int>();
		public Queue<int> queue = new Queue<int>();
		static DateTime[] dt = new DateTime[10];
		byte[] buffer = new byte[1024];
		byte[] buffer2 = new byte[1024];
		public Queue<CUserToken> freeUser = new Queue<CUserToken>();

		public void Start()
		{
			CUserToken _token;
			for (int i = 0; i < 2000; i++)
			{
				_token = new CUserToken();
				freeUser.Enqueue(_token);
			}
		}

		public void Update()
		{
			dt[0] = DateTime.Now;
			StackTest();          //StackTest		:  43
			dt[1] = DateTime.Now;
			QueueTest();          //QueueTest		:  44
			dt[2] = DateTime.Now;
			ClearBuffer();        //ClearBuffer	: 747
			dt[3] = DateTime.Now;
			CopyBuffer();         //CopyBuffer	:  97
			dt[4] = DateTime.Now;
			CreateClass();        //CreateClass	: 135
			dt[5] = DateTime.Now;
			UseQueueClass();      //UseQueueClass	:  26
			dt[6] = DateTime.Now;
			CreateBuffer();       //CreateBuffer	:110
			dt[7] = DateTime.Now;
			dt[8] = DateTime.Now;
			dt[9] = DateTime.Now;

			Console.WriteLine("=========================");
			Console.WriteLine("StackTest:" + (int)((TimeSpan)(dt[1] - dt[0])).TotalMilliseconds);
			Console.WriteLine("QueueTest:" + (int)((TimeSpan)(dt[2] - dt[1])).TotalMilliseconds);
			Console.WriteLine("ClearBuffer:" + (int)((TimeSpan)(dt[3] - dt[2])).TotalMilliseconds);
			Console.WriteLine("CopyBuffer:" + (int)((TimeSpan)(dt[4] - dt[3])).TotalMilliseconds);
			Console.WriteLine("CreateClass:" + (int)((TimeSpan)(dt[5] - dt[4])).TotalMilliseconds);
			Console.WriteLine("UseQueueClass:" + (int)((TimeSpan)(dt[6] - dt[5])).TotalMilliseconds);
			Console.WriteLine("CreateBuffer:" + (int)((TimeSpan)(dt[7] - dt[6])).TotalMilliseconds);
		}


		public void UseQueueClass()
		{
			CUserToken _token;
			for (int i = 0; i < LOOP_MAX; i++)
			{
				_token = freeUser.Dequeue();
				freeUser.Enqueue(_token);
			}
		}

		public void CreateClass()
		{
			CUserToken _token;
			for (int i = 0; i < LOOP_MAX; i++)
			{
				_token = new CUserToken();
			}
		}

		public void CreateBuffer()
		{
			byte[] _buffer;
			for (int i = 0; i < LOOP_MAX; i++)
			{
				_buffer = new byte[1024];
			}
		}

		public void ClearBuffer()
		{
			for (int i = 0; i < LOOP_MAX; i++)
				Array.Clear(buffer, 0, buffer.Length);
		}

		public void CopyBuffer()
		{
			int _len = buffer.Length;
			for (int i = 0; i < _len; i++)
				buffer2[i] = (byte)(i % 20);

			for (int i = 0; i < LOOP_MAX; i++)
				Array.Copy(buffer2, 0, buffer, 0, _len);
		}

		public void QueueTest()
		{
			Random _rand = new Random();
			int _value;
			for (int i = 0; i < LOOP_MAX; i++)
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
			for (int i = 0; i < LOOP_MAX; i++)
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
