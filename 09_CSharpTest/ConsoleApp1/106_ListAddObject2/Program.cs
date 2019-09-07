using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _106_ListAddObject2
{

	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Test();
		}

		List<object> list = new List<object>();
		void Test()
		{
			list.Add(new A1(1));
			list.Add(new A2(2));
			list.Add(new A3(3));
			foreach (var _v in list)
			{
				A _a1 = (A)_v;
				Console.WriteLine(_v + ":" + _a1);

				if ( _a1 != null)
					Console.WriteLine(_a1.val);
			}
		}
	}

	class A
	{
		public int val;
	}
	class A1 : A
	{
		public A1(int _val) { val = _val; }
	}

	class A2 : A
	{
		public A2(int _val) { val = _val; }
	}

	class A3 : A
	{
		public A3(int _val) { val = _val; }
	}
}
