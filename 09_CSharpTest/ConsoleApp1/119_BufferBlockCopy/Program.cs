using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace _119_BufferBlockCopy
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.DoTest2();

			Console.ReadLine();
		}

		int loop = 1000_000;
		const int SIZE = 1024*8;
		byte[] src = new byte[SIZE];
		byte[] dst = new byte[SIZE];
		const int copySize = 500;
		void DoTest()
		{
			Console.WriteLine("==== Stopwatch ======");
			Stopwatch[] _watch	= new Stopwatch[8];
			for(int i = 0; i< _watch.Length; i++)
			{
				_watch[i] = new Stopwatch();
				
				if (i % 2 == 0)
				{
					_watch[i].Start();
					Buffer_BlockCopy();
					_watch[i].Stop();
					Console.WriteLine("Buffer_BlockCopy:{0} / {1}", _watch[i].Elapsed.Milliseconds, _watch[i].Elapsed.Ticks);
				}
				else
				{
					_watch[i].Start();
					Array_Copy();
					_watch[i].Stop();
					Console.WriteLine("Array_Copy:{0} / {1}", _watch[i].Elapsed.Milliseconds, _watch[i].Elapsed.Ticks);
				}
			}
		}

		void Buffer_BlockCopy()
		{
			for (int i = 0; i < loop; i++)
			{
				Buffer.BlockCopy(src, 0, dst, 0, copySize);
			}
		}

		void Array_Copy()
		{
			for (int i = 0; i < loop; i++)
			{
				Array.Copy(src, 0, dst, 0, copySize);
			}
		}

		DateTime[] t = new DateTime[8];
		TimeSpan[] ts = new TimeSpan[8];
		void DoTest2()
		{
			Console.WriteLine("==== DateTime ======");
			for (int i = 0; i < t.Length - 2; i++)
			{
				t[i    ] = DateTime.Now;
				Buffer_BlockCopy();

				t[i + 1] = DateTime.Now;
				Array_Copy();

				t[i + 2] = DateTime.Now;

				ts[i] = t[i + 1] - t[i];
				ts[i+1] = t[i + 2] - t[i+1];
				Console.WriteLine("Buffer_BlockCopy:{0} / {1}", ts[i].Milliseconds, ts[i].Ticks);
				Console.WriteLine("Array_Copy:{0} / {1}", ts[i+1].Milliseconds, ts[i+1].Ticks);
			}
		}
	}
}
