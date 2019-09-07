using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _109_Static
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Test();
			_p.Test2();
			Console.ReadKey();
		}

		List<SubClass> list = new List<SubClass>();
		void Test()
		{
			for(int i = 0; i < 10; i++)
			{
				list.Add(new SubClass());
			}

			list.ForEach(_subClass =>
			{
				_subClass.Display();
			});
		}

		void Test2()
		{
			SubClass2 _sub2 = null;
			for (int i = 0; i< 10; i++)
			{
				_sub2 = new SubClass2(i);
			}

			if(_sub2 != null)
				_sub2.Display();
		}
	}

	public class SubClass
	{
		static int count = 0;
		int myNum = 0;
		public SubClass()
		{
			count++;
			myNum = count;
		}

		public void Display()
		{
			Console.WriteLine("myNum:{0} count:{1}", myNum, count);
		}
	}

	public class SubClass2
	{
		static List<int> list2 = new List<int>();
		static List<int> list;
		static SubClass2()
		{
			Console.WriteLine("static Constructor =>");
			SubClass2.list = new List<int>();
		}

		public SubClass2(int _val)
		{
			Console.WriteLine("public Constructor =>" + _val);
			list.Add(_val);
		}

		public void Display()
		{
			list.ForEach(_val => {
				Console.WriteLine(_val);
			});
		}
	}
}
