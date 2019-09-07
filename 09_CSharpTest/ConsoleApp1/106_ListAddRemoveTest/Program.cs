using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _106_ListAddRemoveTest
{
	public class AAA
	{
		public int num;
		public AAA(int _num)
		{
			num = _num;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.TestList();
			Console.ReadKey();
		}

		void TestList()
		{
			List<AAA> list = new List<AAA>();
			for(int i = 0; i < 8; i++)
			{
				list.Add(new AAA(i));
			}
			
			Display(list);

			//-------------------------------------------
			//for(int i = 0; i < list.Count; i++) 논리적인 오류가 있다.
			// i	:   0 1 2 3 4 5 6 7 8
			//		:   x   x   x   
			//count	: 8 7 7 6 6 5 5 (뒤로 계산을안하고 이탈)
			//                    * 이자리 뒤부터 검사를 안한다...
			//for (int i = 0; i < list.Count - 1; i++)
			//{
			//	if (i % 2 == 0)
			//		list.RemoveAt(i);
			//}
			//-------------------------------------------
			//
			//-------------------------------------------
			// 7 6 5 4 3 2 1 0
			//   x   x   x   x
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if(i % 2 == 0)
					list.RemoveAt(i);
			}
			Display(list);
		}

		void Display(List<AAA> _list)
		{
			Console.WriteLine("==============");
			for (int i = 0; i < _list.Count; i++)
				Console.Write(" " + _list[i].num);
			Console.WriteLine("");
		}
	}
}
