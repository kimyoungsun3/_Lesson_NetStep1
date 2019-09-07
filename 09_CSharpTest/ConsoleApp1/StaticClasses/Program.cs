using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _109_StaticClasses
{
	public class CPacketBufferManager
	{
		static Stack<object> pool1;
		static Stack<object> pool2;
		static List<object> list1;
		static List<object> list2;
		static Dictionary<object, int> dic1;
		static int poolCapacity;
		public static int GetCount() { return pool1.Count; }
		public CPacketBufferManager()
		{
			Console.WriteLine("Constructor");
		}

		public static void Initialize(int _capacity)
		{
			Console.WriteLine("Initialize");
			poolCapacity = _capacity;
			pool1	= new Stack<object>(_capacity);
			pool2	= new Stack<object>(_capacity);
			list1	= new List<object>(_capacity);
			list2	= new List<object>(_capacity);
			dic1 = new Dictionary<object, int>();
			Allocate();
		}

		private static void Allocate()
		{
			Console.WriteLine("Allocate");

			//stack -> 다른obj, 동일obj들어감
			//list -> 
			object _obj2 = new object();
			for (int i = 0; i < poolCapacity; i++)
			{
				pool1.Push(new object());
				pool2.Push(_obj2);

				list1.Add(new object());
				list2.Add(_obj2);

				//동일한 키 사용에대 대한 에러..
				//dic1.Add(_obj2, i);
			}
			Console.WriteLine(pool1.Count + ":" + pool2.Count + ":" + list1.Count + ":" + list2.Count);

			object _p1, _p2;
			for (int i = 0; i < poolCapacity; i++)
			{
				_p1 = pool1.Pop();
				_p2 = pool2.Pop();
				Console.WriteLine("_obj2 == _p1:" + (_obj2 == _p1)
								+ " _obj2 == _p2:" + (_obj2 == _p2)
								+ " _p1 == _p2:" + (_p1 == _p2));
			}
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Test();
			Console.ReadKey();
		}

		void Test()
		{
			CPacketBufferManager.Initialize(10);
			Console.WriteLine( CPacketBufferManager.GetCount());
		}
	}
}
