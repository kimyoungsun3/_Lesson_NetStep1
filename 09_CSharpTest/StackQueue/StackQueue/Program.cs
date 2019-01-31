﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackQueue
{
	class Program
	{
		public int LOOP = 1000000;
		public Stack<int> stack = new Stack<int>();
		public Queue<int> queue = new Queue<int>();
		static DateTime[] dt = new DateTime[10];


		static void Main(string[] args)
		{
			Program p = new Program();
			dt[0] = DateTime.Now;
			p.StackTest();
			dt[1] = DateTime.Now;
			p.QueueTest();
			dt[2] = DateTime.Now;
			p.StackTest();
			dt[3] = DateTime.Now;
			p.QueueTest();
			dt[4] = DateTime.Now;

			Console.WriteLine((int)((TimeSpan)(dt[1] - dt[0])).TotalMilliseconds);
			Console.WriteLine((int)((TimeSpan)(dt[2] - dt[1])).TotalMilliseconds);
			Console.WriteLine((int)((TimeSpan)(dt[3] - dt[2])).TotalMilliseconds);
			Console.WriteLine((int)((TimeSpan)(dt[4] - dt[3])).TotalMilliseconds);
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
