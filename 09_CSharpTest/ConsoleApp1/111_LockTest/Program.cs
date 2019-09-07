using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _111_LockTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Run();
			Console.ReadKey();
		}

		object lockObj = new object();
		int counter = 100;
		public void Run()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread(new ParameterizedThreadStart(UnsafeCalc)).Start(i + 1);
			}
		}

		void UnsafeCalc(object _obj)
		{
			int _num = (int)_obj;
			int _val = 0; 
			lock (lockObj)
			{
				++counter;
			}
			for (int i = 0; i < counter; i++)
					for (int j = 0; j < counter; j++)
						_val++;
				Console.WriteLine(_num + ":" + counter +":" + _val);
		}
	}
}
