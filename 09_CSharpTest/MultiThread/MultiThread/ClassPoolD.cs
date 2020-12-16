using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiThread
{
	public class PackData
	{
		public int code;
		public Vector3 position;

		public void Copy(PackData _src)
		{
			code		= _src.code;
			position	= _src.position;
		}

		public override string ToString()
		{
			StringBuilder _sb = new StringBuilder();
			_sb.Append("code:");
			_sb.Append(code);
			_sb.Append(",");

			_sb.Append("position:{x:");
			_sb.Append(position.x);
			_sb.Append(", y:");
			_sb.Append(position.y);
			_sb.Append(", z:");
			_sb.Append(position.z);
			_sb.Append("}");

			return _sb.ToString();
		}
	}

	public struct Vector3
	{
		public float x, y, z;
		public Vector3(float _x, float _y, float _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}
	}

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
