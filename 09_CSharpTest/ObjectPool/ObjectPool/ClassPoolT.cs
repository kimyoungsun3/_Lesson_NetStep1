using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPool
{
	public static class ObjectPoolT<T> where T : class, new()
	{
		public static Queue<T> queue = new Queue<T>();
		static int maxCount;
		static int useCount;
		static int percentCount;

		public static void Initialize(int _count)
		{
			maxCount = 0;
			useCount = 0;
			percentCount = 10;
			CreateObject(_count);
		}

		static void CreateObject(int _count)
		{
			for (int i = 0; i < _count; i++)
			{
				//해결법1...
				queue.Enqueue(new T());
			}
			maxCount += _count;
		}

		public static void Enqueue(T _t)
		{
			lock (queue)
			{
				queue.Enqueue(_t);
				useCount--;
			}
		}

		public static T Dequeue()
		{
			lock (queue)
			{
				if (useCount >= maxCount)
				{
					CreateObject(percentCount);
				}
				useCount++;
				return queue.Dequeue();
			}
		}
	}
}
