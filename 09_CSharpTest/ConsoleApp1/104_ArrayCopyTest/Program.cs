using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _104_ArrayCopyTest
{
	class Program
	{

		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.ArrayCopyTest();

			for(int i = 0; i < 5; i++)
				_p.ArrayCreateAndCopyTest();


			Console.ReadKey();
		}

		int loop = 1000000;
		DateTime[] dt = new DateTime[10];
		void ArrayCopyTest()
		{
			byte[] _b1 = new byte[1024];
			byte[] _b2 = new byte[1024];
			byte[] _b3 = new byte[1024];
			for (int i = 0; i < _b1.Length; i++)
				_b1[i] = (byte)i;

			//1회복사 20byte:	47	47	47	47	
			//2회복사 20byte:	94	93	93	91	
			//1회복사 1024byte:	47	47	47	47	
			//2회복사 1024byte:	94	93	93	91

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				Array.Copy(_b1, 0, _b2, 0, 20);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				Array.Copy(_b1, 0, _b2, 0, 20);
				Array.Copy(_b2, 0, _b3, 0, 20);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				Array.Copy(_b1, 0, _b2, 0, _b1.Length);
			}

			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				Array.Copy(_b1, 0, _b2, 0, _b1.Length);
				Array.Copy(_b2, 0, _b3, 0, _b1.Length);
			}

			dt[4] = DateTime.Now;

			Console.WriteLine("============");
			Display(_b1, 20, 20);
			Display(_b2, 20, 20);

			Console.WriteLine("1회복사 20byte:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("2회복사 20byte:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("1회복사 1024byte:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("2회복사 1024byte:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);

		}

		void ArrayCreateAndCopyTest()
		{
			byte[] _b1 = new byte[1024];
			byte[] _b2 = new byte[0];
			byte[] _b3 = new byte[0];
			for (int i = 0; i < _b1.Length; i++)
				_b1[i] = (byte)i;

			//1회복사 20byte:	47	47	47	47	
			//2회복사 20byte:	94	93	93	91	
			//1회복사 1024byte:	47	47	47	47	
			//2회복사 1024byte:	94	93	93	91

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_b2 = new byte[1024];
				Array.Copy(_b1, 0, _b2, 0, 20);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_b2 = new byte[1024];
				_b3 = new byte[1024];
				Array.Copy(_b1, 0, _b2, 0, 20);
				Array.Copy(_b2, 0, _b3, 0, 20);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_b2 = new byte[1024];
				Array.Copy(_b1, 0, _b2, 0, _b1.Length);
			}

			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_b2 = new byte[1024];
				_b3 = new byte[1024];
				Array.Copy(_b1, 0, _b2, 0, _b1.Length);
				Array.Copy(_b2, 0, _b3, 0, _b1.Length);
			}

			dt[4] = DateTime.Now;

			Console.WriteLine("============");
			if (_b1 != null) Display(_b1, 20, 20);
			if (_b2 != null) Display(_b2, 20, 20);

			Console.WriteLine("1회생성복사 20byte:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("2회생성복사 20byte:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("1회생성복사 1024byte:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("2회생성복사 1024byte:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);

		}
		public void Display(byte[] _src, int _start = -1, int _len = -1)
		{
			if (_start == -1)
				_start = 0;

			if (_len == -1)
				_len = _src.Length - _start;
			else
				_len = _start + _len;

			for (int i = _start; i < _len; i++)
				Console.Write("{0} ", _src[i]);
			Console.WriteLine("");
		}
	}
}
