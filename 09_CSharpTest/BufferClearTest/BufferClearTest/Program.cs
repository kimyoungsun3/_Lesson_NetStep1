using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BufferClearTest
{
	class Program
	{
		public int LOOP = 10000000;
		static DateTime[] dt = new DateTime[10];


		static void Main(string[] args)
		{

			Program p = new Program();
			dt[0] = DateTime.Now;
			p.Fun1(20);
			dt[1] = DateTime.Now;
			p.Fun2(20);
			dt[2] = DateTime.Now;
			p.Fun1(200);
			dt[3] = DateTime.Now;
			p.Fun2(200);
			dt[4] = DateTime.Now;

			Console.WriteLine("Array.Clear 20:" + (int)((TimeSpan)(dt[1] - dt[0])).TotalMilliseconds);
			Console.WriteLine("offset:" + (int)((TimeSpan)(dt[2] - dt[1])).TotalMilliseconds);
			Console.WriteLine("Array.Clear 200:" + (int)((TimeSpan)(dt[3] - dt[2])).TotalMilliseconds);
			Console.WriteLine("offset:" + (int)((TimeSpan)(dt[4] - dt[3])).TotalMilliseconds);
		}

		byte[] buffer = new byte[1024];
		public void Fun1(int _size = 100)
		{
			int _offset = 0;
			int _transferred = _size;
			for (int i = 0; i < LOOP; i++)
			{
				Array.Clear(buffer, _offset, _transferred);
			}
		}

		public void Fun2(int _size = 100)
		{
			int _offset = 0;
			int _transferred;
			for (int i = 0; i < LOOP; i++)
			{
				_transferred = _size;
				//Array.Clear(buffer, _offset, _transferred);
				_transferred = 0;
			}
		}
	}
}
