using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using netduino;

namespace _105_FloatToByteTest
{
	class Program
	{
		static void Main(string[] args)
		{
			byte[] msbFirst = new byte[] { 0x42, 0xF6, 0xE9, 0xE0 };
			byte[] lsbFirst = new byte[] { 0xE0, 0xE9, 0xF6, 0x42 };
			const float f = 123.456789F;

			byte[] _b = netduino.BitConverter.GetBytes(f);
			float _f2 = netduino.BitConverter.ToSingle(_b, 0);
			Console.WriteLine(_b.Length + ":" + _f2 + ":" + f);

			Program _p = new Program();
			_p.Test();
			_p.Test();
			_p.Test();
			_p.Test();

			//byte[] b = netduino.BitConverter.GetBytes(f, netduino.BitConverter.ByteOrder.LittleEndian);
			//for (int i = 0; i < b.Length; i++)
			//{
			//	Console.WriteLine("BitConverter.GetBytes(float, BigEndian) i=" + i);
			//}

			//Debug.Assert(f == netduino.BitConverter.ToSingle(msbFirst, 0, netduino.BitConverter.ByteOrder.LittleEndian));

			Console.WriteLine("All tests passed");
			Console.ReadKey();
		}


		int loop = 1000000;
		DateTime[] dt = new DateTime[10];
		void Test()
		{
			float _f1 = 123.456789F;

			byte[] _b1 = new byte[1024];
			byte[] _b2 = new byte[0];
			netduino.BitConverter.SetFloat(_b1, 20, _f1);
			float _f2 = netduino.BitConverter.GetFloat(_b1, 20);
			float _f3 = 0, _f4 = 0;

			Console.WriteLine(" => " + _f1 + ":" + _f2);
			
			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				netduino.BitConverter.SetFloat(_b1, 20, _f1);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_f3 = netduino.BitConverter.GetFloat(_b1, 20);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_b2 = System.BitConverter.GetBytes(_f1);
				Array.Copy(_b2, 0, _b1, 20, 4);
			}

			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_f4 = System.BitConverter.ToSingle(_b1, 20);
			}

			dt[4] = DateTime.Now;

			Console.WriteLine("============");
			Console.WriteLine(" => " + _f3 + ":" + _f4);

			Console.WriteLine("*넣고:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("*빼기:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("B넣고:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("B빼기:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);

		}
	}
}
