using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _114_TextEncoding
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			Console.ReadLine();
		}

		void DoTest()
		{
			Console.WriteLine("=======[1. string -> byte[] ]========");
			Encoding _encoding = Encoding.UTF8;
			string _str = "ABC123 abc!";
			byte[] _buffer = _encoding.GetBytes(_str);
			foreach(byte _b in _buffer)
			{
				Console.WriteLine(_b + " ");
			}
		}
	}
}
