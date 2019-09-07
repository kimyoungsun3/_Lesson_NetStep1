using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _107_ListForEach
{
	class Player : IEquatable<Player>
	{
		public int idx;
		public Player (int _idx)
		{
			idx = _idx;
		}

		public bool Equals(Player _other)
		{
			if (_other == null) return false;
			return idx.Equals(_other.idx);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.DoTest2();

			Console.ReadKey();
		}

		List<Player> player = new List<Player>();
		void DoTest()
		{
			Console.WriteLine("======[DoTest 1]========");
			//1. list add
			for (int i = 0; i < 10; i++)
				player.Add(new Player(i));

			//2. list 순회공영.
			//player.ForEach(p => Console.WriteLine(p.idx));
			//player.ForEach(p => Console.WriteLine(p.idx));
			//player.ForEach(p => Console.WriteLine(p.idx));
			//player.ForEach(p => Console.WriteLine(p.idx));
			//player.ForEach(p => Console.WriteLine(p.idx));
			//player.ForEach(p => Console.WriteLine(p.idx));
			//player.ForEach(p => {
			//	Console.WriteLine(p.idx);
			//});
			//player.ForEach(p =>
			//{
			//	Console.WriteLine(p.idx);
			//});
			//player.ForEach(_p => Console.WriteLine(_p.idx));
			//player.ForEach(_p =>
			//{
			//	Console.WriteLine(_p.idx);
			//});
			//player.ForEach(_p =>
			//{
			//	Console.WriteLine(_p.idx);
			//});
		}

		List<Player> listPlayer2 = new List<Player>();
		void DoTest2()
		{
			Console.WriteLine("======[DoTest 2]========");
			//1. list add
			for (int i = 0; i < 10; i++)
			{
				listPlayer2.Add(new Player(i));
			}

			Console.WriteLine("5 is Contain:{0}", listPlayer2.Contains(new Player(5)));
			Console.WriteLine("15 is Contain:{0}", listPlayer2.Contains(new Player(15)));

			Console.WriteLine("5 is Find:{0}", listPlayer2.Find(x => x.idx.Equals(5)));
			Console.WriteLine("15 is Find:{0}", listPlayer2.Find(x => x.idx.Equals(5)));


			Console.WriteLine("5 is Exists:{0}", 
				listPlayer2.Exists(x => x.idx == 5));

			//public bool Exists(Predicate<T> match);
			//public delegate bool Predicate<T>(T obj);
			//player.Exists(p => )
		}
	}
}
