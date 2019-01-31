using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int16short
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(sizeof(int)   + ":" + int.MaxValue);

			Console.WriteLine(sizeof(Int16) + ":" + Int16.MaxValue);
			Console.WriteLine(sizeof(short) + ":" + short.MaxValue);


			Console.WriteLine(sizeof(float) + ":" + float.MaxValue);
			Console.WriteLine(sizeof(Single) + ":" + Single.MaxValue);
		}
	}
}
