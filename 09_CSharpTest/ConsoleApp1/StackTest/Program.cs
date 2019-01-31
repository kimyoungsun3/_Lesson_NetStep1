using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackTest
{
	class Program
	{
		public int capacity = 4;
		public Stack<int> stack;
		Random rand = new Random();
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoSometing();
		}

		void DoSometing()
		{
			stack = new Stack<int>(capacity);
			for(int i = 0; i < 2000; i++)
				stack.Push(rand.Next());

			Console.WriteLine("stack capacity:" + capacity);
			while (true) {
				string key = Console.ReadKey().Key.ToString();
				if (key.ToUpper() == "W")
				{
					stack.Push(rand.Next());
					Console.WriteLine("Push > " + stack.Count);
				}
				else
				{
					if (stack.Count > 0)
					{
						stack.Pop();
					}
					Console.WriteLine("Pop > " + stack.Count);
				}
			}
		}
	}
}
