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
			_p.DoTest3();
			_p.DoTest4();

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

		void Display(List<Player> _list)
		{
			_list.ForEach(p => Console.Write(p.idx + " "));
			Console.WriteLine();
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

			Console.WriteLine("5 is Contains:{0}",
				listPlayer2.Contains(new Player(5)));


			Display(listPlayer2);
			listPlayer2.RemoveAll(p2 => p2.idx < 5);
			Display(listPlayer2);
			//public bool Exists(Predicate<T> match);
			//public delegate bool Predicate<T>(T obj);
			//player.Exists(p => )
		}

		List<Player> listPlayer3 = new List<Player>();
		void DoTest3()
		{
			Console.WriteLine("======[DoTest 3]========");
			for(int i = 0; i < 10; i++)
			{
				listPlayer3.Add(new Player(i));
			}

			Display(listPlayer3);
			listPlayer3.Remove(new Player(1));
			Display(listPlayer3);
			listPlayer3.RemoveAt(2);
			Display(listPlayer3);
			listPlayer3.RemoveAll(_p => _p.idx % 2 == 0);
			Display(listPlayer3);
			listPlayer3.Reverse();
			Display(listPlayer3);
			listPlayer3.Add(new Player(1));
			listPlayer3.Add(new Player(1));
			Display(listPlayer3);
			listPlayer3.Insert(2, new Player(1));
			Display(listPlayer3);

		}

		List<Player> listPlayer4 = new List<Player>();
		void DoTest4()
		{
			Console.WriteLine("======[DoTest 4]========");
			for (int i = 0; i < 10; i++)
			{
				listPlayer4.Add(new Player(i));
			}

			Display(listPlayer4);
			listPlayer4.Remove(new Player(5));
			Display(listPlayer4);
			listPlayer4.RemoveAt(2);
			Display(listPlayer4);
			listPlayer4.RemoveAll(_p => _p.idx % 3 == 0);
			Display(listPlayer4);
			listPlayer4.Reverse();
			Display(listPlayer4);
			listPlayer4.Add(new Player(99));
			Display(listPlayer4);
			listPlayer4.Insert(2, new Player(88));
			Display(listPlayer4);
			listPlayer4.Insert(0, new Player(00));
			Display(listPlayer4);
		}
	}
}
