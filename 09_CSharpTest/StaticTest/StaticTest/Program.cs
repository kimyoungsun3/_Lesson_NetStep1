using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Util.score);
			Console.WriteLine(Util.Add(1, 2));
		}
	}

	public static class Util
	{
		public static int score;
		public static int Add(int _x, int _y)
		{
			return _x + _y;
		}
	}
}
