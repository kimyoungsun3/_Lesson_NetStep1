using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _119_ArraySegment
{
	class Program
	{
		//byte[] -> 감쌓고 있는 struct (안의내용은 레퍼런스로 참조하고 있음)
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();

			Console.ReadLine();
		}

		void DoTest()
		{
			Console.WriteLine("변경전");
			string[] _array = {
				"1", "2", "3", "4", "5",
				"6", "7", "8", "9",  "0"
			};
			Display(_array);

			ArraySegment<string> _array2 = new ArraySegment<string>(_array);
			Display(_array2);

			ArraySegment<string> _array3 = new ArraySegment<string>(_array, 2, 3);
			Display(_array3);

			Console.WriteLine("변경후");
			_array[2] = "222";
			_array[4] = "44";
			Display(_array);
			Display(_array2);
			Display(_array3);
		}

		void Display(string[] _arr)
		{
			for (int i = 0; i < _arr.Length; i++)
			{
				Console.Write ("[{0}]:{1} ", i, _arr[i]);
			}
			Console.WriteLine();
		}

		void Display(ArraySegment<string> _arr)
		{
			int _len = _arr.Offset + _arr.Count;
			for (int i = _arr.Offset; i < _len; i++)
			{
				Console.Write("[{0}]:{1} ", i, _arr.Array[i]);
			}
			Console.WriteLine();
		}
	}
}
