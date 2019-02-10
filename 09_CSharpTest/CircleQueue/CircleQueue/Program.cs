using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleQueue
{
	public class RingBuffer<T>
	{
		private T[] buffer;
		int head, tail, count;
		public RingBuffer(int _length)
		{
			buffer		= new T[_length];
			Clear();
		}

		public void Clear() {
			head = 0;
			tail = 0;
			count = 0;
		}

		//public int Length { get { return buffer.Length; } }

		public bool Push(T[] _srcBuffer, int _srcOffset, int _srcLength)
		{
			bool _rtn = false;
			if (_srcBuffer == null) return _rtn;

			//1. 사용공간 + 넣을공간 < 남은 공간
			if(count + _srcLength <= buffer.Length)
			{
				if (head + _srcLength <= buffer.Length)
				{
					//공간 충분해서 카피...
					Array.Copy(_srcBuffer, _srcOffset, buffer, head, _srcLength);
					head += _srcLength;
					if(head >= buffer.Length)
					{
						head = 0;
					}
					count += _srcLength;
					Console.WriteLine(" > step1 tail:{0} head:{1} count:{2}", tail, head, count);
				}
				else
				{
					//경계라인 부분에서는 나눠서 카피...
					int _part2 = (head + _srcLength) - buffer.Length;
					int _part1 = _srcLength - _part2;
					Array.Copy(_srcBuffer, _srcOffset,          buffer, head, _part1);
					Array.Copy(_srcBuffer, _srcOffset + _part1, buffer,    0, _part2);
					head = _part2;
					count += _srcLength;
					Console.WriteLine(" > step2 tail:{0} head:{1} count:{2}", tail, head, count);
				}
				_rtn = true;
			}

			return _rtn;
		}

		public bool Pop(ref T[] _target)
		{
			bool _rtn = false;
			

			return _rtn;
		}
	}

	class Program
	{

		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Test1();
			_p.Test2();
			_p.Test4();
			_p.Test8();
		}

		RingBuffer<byte> ringBuffer = new RingBuffer<byte>(6);
		byte[] _tmp = new byte[]{	1, 2, 3, 4, 5, 6, 7, 8 };
		void Test1()
		{
			Console.WriteLine("test1");
			ringBuffer.Clear();
			for (int i = 0; i < _tmp.Length; i++)
				ringBuffer.Push(_tmp, i, 1);
		}
		void Test2()
		{
			Console.WriteLine("test2");
			ringBuffer.Clear();
			for (int i = 0; i < _tmp.Length; i+=2)
				ringBuffer.Push(_tmp, i, 2);
		}
		void Test4()
		{
			Console.WriteLine("test4");
			ringBuffer.Clear();
			for (int i = 0; i < _tmp.Length; i+=4)
				ringBuffer.Push(_tmp, i, 4);
		}
		void Test8()
		{
			Console.WriteLine("Test8");
			ringBuffer.Clear();
			for (int i = 0; i < _tmp.Length; i += 8)
				ringBuffer.Push(_tmp, i, 8);
		}
	}
}
