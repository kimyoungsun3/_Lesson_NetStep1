using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _102_BitConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.FunByte();
			_p.CopyTest();
			_p.SizeTest();
			_p.GetBytesTest();
			_p.GetBytesTest();
			_p.GetBytesTest();
			_p.GetBytesTest();

			Console.ReadKey();
		}

		void GetBytesTest()
		{
			int _loop = 10000000;
			Int16 _int16 = 256;
			Int32 _int32 = 256;
			byte[] _b1 = new byte[2];
			byte[] _b2 = new byte[2];
			byte[] _b3 = new byte[4];
			byte[] _b4 = new byte[4];

			dt[0] = DateTime.Now;
			for (int i = 0; i < _loop; i++)
			{
				_b1 = BitConverter.GetBytes(_int16);
			}
			dt[1] = DateTime.Now;
			for (int i = 0; i < _loop; i++)
			{
				Util.SetShort(_b2, 0, _int16);
			}
			dt[2] = DateTime.Now;
			for (int i = 0; i < _loop; i++)
			{
				_b3 = BitConverter.GetBytes(_int32);
			}
			dt[3] = DateTime.Now;
			for (int i = 0; i < _loop; i++)
			{
				Util.SetInt(_b4, 0, _int32);
			}
			dt[4] = DateTime.Now;

			Console.WriteLine("============");
			Display(_b1);
			Display(_b2);
			Display(_b3);
			Display(_b4);
			Console.WriteLine("BitConverter.GetBytes:{0}",	((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("Util.SetShort:{0}",			((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("BitConverter.GetBytes:{0}",	((TimeSpan)(dt[3] - dt[2])).Milliseconds);
			Console.WriteLine("Util.SetInt:{0}",			((TimeSpan)(dt[4] - dt[3])).Milliseconds);
		}

		byte[] buffer = { 0, 1, 2, 3, 4, 5, 6, 7 };
		void FunByte()
		{

			for (int i = 0; i < buffer.Length; i++)
			{
				byte _data3 = (byte)buffer[i];
				Console.WriteLine(" >> (byte)buffer[i]:" + _data3);
			}

			for (int i = 0; i < buffer.Length - 1; i++)
			{
				byte _data = (byte)BitConverter.ToInt16(buffer, i);
				Console.WriteLine(" >> (byte)ToInt16():" + _data);
			}

			for (int i = 0; i < buffer.Length - 1; i++)
			{
				Int16 _data3 = (Int16)BitConverter.ToInt16(buffer, i);
				Console.WriteLine(" >> (Int16)ToInt16():" + _data3);
			}

			//char _data2 = (char)BitConverter.ToChar(buffer, 0);
			//Console.WriteLine(" >> ToChar():" + _data2);
		}

		DateTime[] dt = new DateTime[10];
		public void CopyTest()
		{
			byte[] _src = new byte[8];
			byte[] _tar = new byte[8];
			byte[] _tar2 = new byte[8];
			for (int i = 0; i < _src.Length; i++)
				_src[i] = (byte)i;

			int loop = 1000000;
			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				Array.Copy(_src, 0, _tar, 0, _src.Length);
			}
			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_src.CopyTo(_tar2, 0); 
			}
			dt[2] = DateTime.Now;
			Display(_tar);
			Display(_tar2);

			Console.WriteLine("Array.Copy:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("_src.CopyTo:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);


		}

		public void SizeTest()
		{
			Console.WriteLine("byte:{0} Int16:{1}, Int32:{2}, Single:{3}",
				sizeof(byte),
				sizeof(Int16),
				sizeof(Int32),
				sizeof(Single));
		}

		public void Display(byte[] _src)
		{
			for (int i = 0; i < _src.Length; i++)
				Console.Write("{0} ", _src[i]);
			Console.WriteLine("");
		}
	}
}
