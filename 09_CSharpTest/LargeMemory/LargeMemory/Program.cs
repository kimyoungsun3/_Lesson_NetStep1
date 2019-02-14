using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeMemory
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Test();
		}

		void Test()
		{
			int maxCount = 200000000;
			ComplexNumber[] arr = null;
			try
			{
				Console.WriteLine(11);
				arr = new ComplexNumber[maxCount];
				Console.WriteLine(12);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

	public struct ComplexNumber
	{
		public double Re;
		public double Im;

		public ComplexNumber(double re, double im)
		{
			Re = re;
			Im = im;
		}
	}
}
