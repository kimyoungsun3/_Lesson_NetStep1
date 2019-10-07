using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeServer20
{
	public sealed class Debug
	{
		public Debug() { }

		public static void Log(byte _b) => Log(_b.ToString());
		public static void Log(short _b) => Log(_b.ToString());
		public static void Log(int _b) => Log(_b.ToString());
		public static void Log(float _b) => Log(_b.ToString());
		public static void Log(long _b) => Log(_b.ToString());

		public static void Log(object _obj)
		{
			string _str = (string)_obj;
			_str =
				"[" + System.Threading.Thread.CurrentThread.ManagedThreadId + "]"
				+ _str;
			Console.WriteLine(_str);
		}
	}
}
