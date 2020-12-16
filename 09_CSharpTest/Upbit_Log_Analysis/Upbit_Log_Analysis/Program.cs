using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Upbit_Log_Analysis
{
	class Program
	{
		static void Main(string[] args)
		{
			string _t1 = File.ReadAllText(@"Upbit.Log");
			Console.WriteLine("=====> "  + _t1);
			//string[] _array = _t1.Split("AmountTable__cell");


			//string[] _t2 = File.ReadAllLines(@"Upbit.Log");
			//Console.WriteLine("=====> " + _t2[0]);


		}
	}
}
