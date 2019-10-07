using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleQueue
{
	public class RingBuffer
	{
		private byte[] buffer;
		//private byte[] buffer2;
		//private byte[] buffer4;
		int head, tail, count;
		public RingBuffer(int _length)
		{
			buffer		= new byte[_length];
			Clear();
		}

		public void Clear() {
			head = 0;
			tail = 0;
			count = 0;
		}

		public bool IsReceiveData()
		{
			return count >= 2;
		}

		//public int Length { get { return buffer.Length; } }

		public bool Push(byte[] _srcBuffer, int _srcOffset, int _srcLength)
		{
			bool _rtn = false;
			if (_srcBuffer == null || _srcLength <= 0 ) return _rtn;

			//1. 사용공간 + 넣을공간 < 남은 공간.
			if (count + _srcLength <= buffer.Length)
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

		void OffSet(int _size)
		{
			head += _size;
			tail += _size;
			head = head >= buffer.Length ? 0 : head;
			tail = tail >= buffer.Length ? 0 : tail;
			count -= _size;
		}


		public int Count		{	get { return count; }				}
		public int GetSize()	{	return Util.GetShort(buffer, tail);	}
		public void ReadData(out byte[] _buf, int _size)
		{
			_buf = null;
			if (count < 4) return;
			_buf = new byte[_size];

			int _end = tail + _size;
			if(_end >= buffer.Length)
			{
				// 한번 순환...
				int _size2 = buffer.Length - _end;
				int _size1 = _size - _size2;
				Array.Copy(buffer, tail,	_buf,      0, _size1);
				Array.Copy(buffer, 0,		_buf, _size1, _size2);
			}
			else
			{
				//통으로 읽을수 있음...
				Array.Copy(buffer, tail, _buf, 0, _size);
			}

			OffSet(_size);
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

			_p.TestReadAndParse();

			Console.ReadLine();
		}

		RingBuffer ringBuffer = new RingBuffer(10);
		byte[] _tmp = new byte[]{	4, 0, 1, 0, 4, 0, 2, 0, 4, 0 };
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

		void TestReadAndParse()
		{
			Console.WriteLine("TestReadAndParse");
			ringBuffer.Clear();
			ringBuffer.Push(_tmp, 0, 8);
			while(ringBuffer.IsReceiveData())
			{
				int _size = ringBuffer.GetSize();
				int _code = -1;
				
				if( _size >= 4 )
				{
					if (_size > ringBuffer.Count)
					{
						Console.WriteLine("Head Data Receive and Body Not Receive");
						break;
						//for (int i = 0; i < 2; i++)
						//	ringBuffer.Push(_tmp, i, 1);
					}
					else
					{
						byte[] _buf;
						ringBuffer.ReadData(out _buf, _size);
						_code = Util.GetShort(_buf, 2);
					}
				}
				Console.WriteLine("size:{0} code:{1}", _size, _code);
			}
			//ringBuffer.IsData()
		}
	}
}
