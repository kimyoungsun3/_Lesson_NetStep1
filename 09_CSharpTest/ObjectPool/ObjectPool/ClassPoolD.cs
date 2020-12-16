using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPool
{
	public static class ClassPoolD
	{
		public static Queue<PackData> queue = new Queue<PackData>();
		static int maxCount = 0;
		static int percentCount = 10;

		public static void Initialize(int _count)
		{
			CreateObject(_count);
		}

		public static void CreateObject(int _count)
		{
			for (int i = 0; i < _count; i++)
			{
				//해결법1...
				queue.Enqueue(new PackData());
			}
			maxCount += _count;
		}

		public static void Enqueue(PackData _t)
		{
			lock (queue)
			{
				queue.Enqueue(_t);
			}
		}

		public static PackData Dequeue()
		{
			lock (queue)
			{
				if (queue.Count <= percentCount)
				{
					CreateObject(percentCount);
				}
				return queue.Dequeue();
			}
		}
	}
}
