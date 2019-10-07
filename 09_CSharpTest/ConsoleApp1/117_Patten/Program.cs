using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _117_Patten
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest("010-1234-5555");
			_p.DoTest("01a-1234-5555");
			_p.DoTest("010-a234-5555");
			_p.DoTest("012-1234-a555");
			_p.DoTest("012-1234-555");

			_p.DoTest2("홍길동");
			_p.DoTest2("ㅁ길동");
			_p.DoTest2("a길동");
			_p.DoTest2("홍길동1");

			_p.DoTest3();
			_p.DoTest4();

			Console.ReadLine();
		}

		void DoTest(string _phone)
		{
			Console.WriteLine("=======[번호검사]==========");
			Regex _regex = new Regex(@"^01[01678]-[0-9]{4}-[0-9]{4}$");
			if (_regex.IsMatch(_phone))
			{
				Console.WriteLine(_phone + " >> Match");
			}
			else
			{
				Console.WriteLine(_phone + " >> not Match");
			}
		}

		void DoTest2(string _name)
		{
			Console.WriteLine("=======[한글검사]==========");
			Regex _regex = new Regex(@"^[가-힝]{3}$");
			if (_regex.IsMatch(_name))
			{
				Console.WriteLine(_name + " >> Match");
			}
			else
			{
				Console.WriteLine(_name + " >> not Match");
			}
		}

		void DoTest3()
		{
			Console.WriteLine("=======[문자열검사]==========");
			string _str = "abcd def";
			Console.WriteLine(_str + " : " + _str.Length);

			string[] _arr = _str.Split(' ');
			for (int i = 0; i < _arr.Length; i++)
			{
				Console.WriteLine("[{0}]:{1} ", i, _arr[i]);
			}

			Console.WriteLine("[ ] => " + _str.IndexOf(' '));

			Console.WriteLine("[1] => " + _str[1]);

			string[] _arr2 = new string[3];
			_arr2[0] = _str.Substring(0, 4);
			_arr2[1] = _str.Substring(4, 4);
			_arr2[2] = _str.Substring(6);
			for (int i = 0; i < _arr2.Length; i++)
			{
				Console.WriteLine("[{0}] => [{1}]", i, _arr2[i]);
			}

			//StartWith 특정 문자로 시작하는지 검사.
			Console.WriteLine("StartWith ab:" + _str.StartsWith("ab"));
			Console.WriteLine("StartWith ab:" + _str.StartsWith("cd"));

			//Contains
			Console.WriteLine("Contains def:" + _str.Contains("def"));
			Console.WriteLine("Contains dc:" + _str.Contains("dc"));

			//Trim
			Console.WriteLine("Trim :[{0}]", _str.Trim());
		}

		DateTime[] t = new DateTime[10];
		void DoTest4()
		{
			Console.WriteLine("========[string IndexOf, Contains]===========");
			string _strBase = "abcdefghijkklovemopqrstuvwxyz";
			string _strIn = "love";

			t[0] = DateTime.Now;
			CheckIndexOf(_strBase, _strIn);

			t[1] = DateTime.Now;
			CheckContains(_strBase, _strIn);

			t[2] = DateTime.Now;
			CheckIndexOf(_strBase, _strIn);

			t[3] = DateTime.Now;
			CheckContains(_strBase, _strIn);

			t[4] = DateTime.Now;

			Console.WriteLine(".IndexOf => " + (t[1] - t[0]).TotalMilliseconds);
			Console.WriteLine(".Contains => " + (t[2] - t[1]).TotalMilliseconds);
			Console.WriteLine(".IndexOf => " + (t[3] - t[2]).TotalMilliseconds);
			Console.WriteLine(".Contains => " + (t[4] - t[3]).TotalMilliseconds);
		}

		int LOOP_MAX = 1000_000;
		void CheckIndexOf(string _strBase, string _strIn)
		{
			for(int i = 0; i < LOOP_MAX; i++)
			{
				_strBase.IndexOf(_strIn);
			}
		}

		void CheckContains(string _strBase, string _strIn)
		{
			for (int i = 0; i < LOOP_MAX; i++)
			{
				_strBase.Contains(_strIn);
			}

		}
	}
}
