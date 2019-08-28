using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackQueue
{
	class ArrayReadCompare
	{
		public byte[] src = new byte[8];
		DateTime[] t = new DateTime[10];
		public int LOOP_MAX = 1000000;

		public void Start()
		{
			for (int i = 0; i < src.Length; i++)
				src[i] = (byte)(i + 1);

			//Update();
		}

		public void Update()
		{
			t[0] = DateTime.Now;
			FunGetInt();
			t[1] = DateTime.Now;
			FunGetIntUtil();
			t[2] = DateTime.Now;
			GetBitConverterToInt32();
			t[3] = DateTime.Now;
			t[4] = DateTime.Now;
			t[5] = DateTime.Now;
			t[6] = DateTime.Now;


			Console.WriteLine("=========================");
			Console.WriteLine("FunGetInt :" + (t[1] - t[0]).TotalMilliseconds);
			Console.WriteLine("FunGetIntUtil :" + (t[2] - t[1]).TotalMilliseconds);
			Console.WriteLine("GetBitConverterToInt32 :" + (t[3] - t[2]).TotalMilliseconds);
		}

		void FunGetInt()
		{
			int x = 0;
			for (int i = 0; i < LOOP_MAX; i++)
			{
				x = GetInt(src, 0);
			}
		}

		void FunGetIntUtil()
		{
			int x = 0;
			for (int i = 0; i < LOOP_MAX; i++)
			{
				x = CUtil.GetInt(src, 0);
			}
		}

		int GetInt(byte[] _src, int _pos)
		{
			return (int)(
				   _src[_pos + 0]
				| (_src[_pos + 1] <<  8)
				| (_src[_pos + 2] << 16)
				| (_src[_pos + 3] << 24));
		}

		void GetBitConverterToInt32()
		{
			int x = 0;
			for (int i = 0; i < LOOP_MAX; i++)
			{
				x = BitConverter.ToInt32(src, 0);
			}
		}
	}
}
