using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Diagnostics;

namespace _104_ArrayCopyTest2
{
	class Program
	{
		int LOOM_MAX		= 1_000_000;
		const int SIZE		= 1024 * 8;
		byte[] src = new byte[SIZE];
		byte[] dst = new byte[SIZE];
		const int copySize = 500;
		DateTime[] dt = new DateTime[10];
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			Console.ReadKey();
		}

		void DoTest()
		{
			Ready();
			for(int i = 0; i < 10; i++)
			{
				DoCopyAll();
				DoCopyPart();
			}
		}

		void Ready()
		{
			Console.WriteLine("=====[Data Ready]=======");
			byte[] _b = {
				1, 0, 0, 0,
				2, 0, 0, 0,
				3, 0, 0, 0
			};
			int _len = _b.Length;
			for (int i = 0, imax = src.Length / _len; i < imax; i++)
			{
				Buffer.BlockCopy(_b, 0, src, i* _len, _len);
			}
		}

		void DoCopyAll()
		{
			//Console.WriteLine("==========");
			dt[0] = DateTime.Now;
			for (int i = 0; i< LOOM_MAX; i++)
			{
				Buffer.BlockCopy(src, 0, dst, 0, src.Length);
			}
			dt[1] = DateTime.Now;
			TimeSpan _ts = (dt[1] - dt[0]);
			Console.WriteLine("ALL : \t{0} / \t{1}", _ts.Milliseconds, _ts.Ticks);
		}
		void DoCopyPart()
		{
			//Console.WriteLine("==========");
			dt[0] = DateTime.Now;
			int _pos = 0;
			int j, jmax;
			for (int i = 0; i < LOOM_MAX; i++)
			{
				_pos = 0;
				for (j = 0, jmax = 30; j < jmax; j++)
				{
					_pos = j * (4 * 3);
					Buffer.BlockCopy(src, _pos, dst, _pos, 4 * 3);
				}
			}
			dt[1] = DateTime.Now;
			TimeSpan _ts = (dt[1] - dt[0]);
			Console.WriteLine("Part : \t{0} / \t{1}", _ts.Milliseconds, _ts.Ticks);
		}
	}
}
