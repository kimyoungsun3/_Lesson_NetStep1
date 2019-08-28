using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteToDataSpeedTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.StringTest();
			_p.StringTest();
			_p.StringTest();
			_p.StringTest();
			_p.ShortTest();
			_p.IntTest();
			_p.FloatTest();
			_p.DoubleTest();

			Console.ReadKey();
		}

		int loop = 10000000;
		DateTime[] dt = new DateTime[10];
		void ShortTest()
		{
			//byte[] <-> byte
			//직접 쓴다.

			//byte[] <- short, Int16
			short _data			= 256;
			byte[] _b1			= new byte[8];
			byte[] _b2			= new byte[8];
			short _data1 = 0;
			short _data2 = 0;

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetShort(_b1, 0, _data);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetShort2(_b2, 0, _data);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data1 = CUtil.GetShort(_b1, 0);
			}
			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data2 = CUtil.GetShort2(_b2, 0);
			}
			dt[4] = DateTime.Now;

			Console.WriteLine("============");
			Display(_b1);
			Display(_b2);
			Console.WriteLine("_data:{0} _data1:{1} _data2:{2}", _data, _data1, _data2);

			Console.WriteLine("SetShort:{0}",	((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("SetShort2:{0}",	((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("GetShort:{0}",	((TimeSpan)(dt[3] - dt[2])).Milliseconds);
			Console.WriteLine("GetShort2:{0}",	((TimeSpan)(dt[4] - dt[3])).Milliseconds);

		}

		void IntTest()
		{
			//byte[] <-> byte
			//직접 쓴다.

			//byte[] <- short, Int16
			int _data = 256;
			byte[] _b1 = new byte[8];
			byte[] _b2 = new byte[8];
			int _data1 = 0;
			int _data2 = 0;

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetInt(_b1, 0, _data);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetInt2(_b2, 0, _data);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data1 = CUtil.GetInt(_b1, 0);
			}
			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data2 = CUtil.GetInt2(_b2, 0);
			}
			dt[4] = DateTime.Now;

			Console.WriteLine("============");
			Display(_b1);
			Display(_b2);
			Console.WriteLine("_data:{0} _data1:{1} _data2:{2}", _data, _data1, _data2);

			Console.WriteLine("SetInt:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("SetInt2:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("GetInt:{0}", ((TimeSpan)(dt[3] - dt[2])).Milliseconds);
			Console.WriteLine("GetInt2:{0}", ((TimeSpan)(dt[4] - dt[3])).Milliseconds);

		}

		void FloatTest()
		{
			//byte[] <-> byte
			//직접 쓴다.

			//byte[] <- short, Int16
			float _data = 12.345f;
			byte[] _b1 = new byte[8];
			byte[] _b2 = new byte[8];
			float _data1 = 0f;
			float _data2 = 0f;

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetFloat(_b1, 0, _data);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetFloat(_b2, 0, _data);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data1 = CUtil.GetFloat(_b1, 0);
			}
			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data2 = CUtil.GetFloat(_b2, 0);
			}
			dt[4] = DateTime.Now;


			Console.WriteLine("============");
			Display(_b1);
			Display(_b2);
			Console.WriteLine("_data:{0} _data1:{1} _data2:{2}", _data, _data1, _data2);

			Console.WriteLine("SetFloat:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("SetFloat:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("GetFloat:{0}", ((TimeSpan)(dt[3] - dt[2])).Milliseconds);
			Console.WriteLine("GetFloat:{0}", ((TimeSpan)(dt[4] - dt[3])).Milliseconds);

			byte[] _arr = BitConverter.GetBytes(_data);
			Display(_arr);
		}

		void DoubleTest()
		{
			//byte[] <-> byte
			//직접 쓴다.

			//byte[] <- short, Int16
			double _data = 12.345f;
			byte[] _b1 = new byte[8];
			byte[] _b2 = new byte[8];
			double _data1 = 0f;
			double _data2 = 0f;

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetDouble(_b1, 0, _data);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetDouble(_b2, 0, _data);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data1 = CUtil.GetDouble(_b1, 0);
			}
			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data2 = CUtil.GetDouble(_b2, 0);
			}
			dt[4] = DateTime.Now;


			Console.WriteLine("============");
			Display(_b1);
			Display(_b2);
			Console.WriteLine("_data:{0} _data1:{1} _data2:{2}", _data, _data1, _data2);

			Console.WriteLine("SetDouble:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("SetDouble:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("GetDouble:{0}", ((TimeSpan)(dt[3] - dt[2])).Milliseconds);
			Console.WriteLine("GetDouble:{0}", ((TimeSpan)(dt[4] - dt[3])).Milliseconds);

			byte[] _arr = BitConverter.GetBytes(_data);
			Display(_arr);
		}

		void StringTest()
		{
			//byte[] <-> byte
			//직접 쓴다.

			//byte[] <- short, Int16
			string _data = "12.345f";
			byte[] _b1 = new byte[10];
			byte[] _b2 = new byte[10];
			string _data1 = string.Empty;
			string _data2 = string.Empty;

			dt[0] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetString(_b1, 0, _data);
			}

			dt[1] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.SetString2(_b2, 0, ref _data);
			}

			dt[2] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				_data1 = CUtil.GetString(_b1, 0);
			}
			dt[3] = DateTime.Now;
			for (int i = 0; i < loop; i++)
			{
				CUtil.GetString2(_b2, 0, out _data2);
			}
			dt[4] = DateTime.Now;


			Console.WriteLine("============");
			Display(_b1);
			Display(_b2);
			Console.WriteLine("_data:{0} _data1:{1} _data2:{2}", _data, _data1, _data2);

			Console.WriteLine("SetString:{0}", ((TimeSpan)(dt[1] - dt[0])).Milliseconds);
			Console.WriteLine("SetString2:{0}", ((TimeSpan)(dt[2] - dt[1])).Milliseconds);
			Console.WriteLine("GetString:{0}", ((TimeSpan)(dt[3] - dt[2])).Milliseconds);
			Console.WriteLine("GetString2:{0}", ((TimeSpan)(dt[4] - dt[3])).Milliseconds);

			
		}


		public void Display(byte[] _src)
		{
			for (int i = 0; i < _src.Length; i++)
				Console.Write("{0} ", _src[i]);
			Console.WriteLine("");
		}
	}
}
