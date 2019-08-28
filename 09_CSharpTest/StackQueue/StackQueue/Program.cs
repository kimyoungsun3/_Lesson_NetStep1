using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackQueue
{

	class Program
	{
		static void Main(string[] args)
		{
			Program p = new Program();

			ArrayReadCompare _a = new ArrayReadCompare();
			_a.Start();
			//_a.Update();

			SpeedTest1 _b = new SpeedTest1();
			_b.Start();
			//_b.Update();

			for(int i = 0; i < 10; i++)
			{
				_a.Update();
				_b.Update();
			}

			Console.ReadKey();



		}

	}

}
