using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _108_InnerClass
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.Test2();
			_p.Test();

			Console.ReadKey();
		}

		Dictionary<int, MasterA> dic_QuestList = new Dictionary<int, MasterA>();
		void Test2()
		{
			MasterA _master = null;
			for (int i = 0; i < 3; i++)
			{
				_master = new MasterA(i, i * 10, new MasterA.SubA1(i * 100), new MasterA.SubA2(i * 1000));
				dic_QuestList.Add(i, _master);
			}

			foreach(KeyValuePair<int, MasterA> _data in dic_QuestList){
				Console.WriteLine(_data.Key
					+ ":" + _data.Value.index
					+ ":" + _data.Value.reward
					+ ":" + _data.Value.sub1.num
					+ ":" + _data.Value.sub2.num
					);
			}

			Console.WriteLine( 1 + " >> " + dic_QuestList.ContainsKey(1));
			Console.WriteLine(99 + " >> " + dic_QuestList.ContainsKey(99));
			Console.WriteLine("_master >> " + dic_QuestList.ContainsValue(_master));
			_master = null;
			Console.WriteLine("null >> " + dic_QuestList.ContainsValue(_master));
			_master = new MasterA(1, 1 * 10, new MasterA.SubA1(1 * 100), new MasterA.SubA2(1 * 1000));
			Console.WriteLine("_master(1) >> " + dic_QuestList.ContainsValue(_master));
			//기본형은 데이타...
			//클래스는 레퍼런스..
		}

		List<MasterA.SubA1> list = new List<MasterA.SubA1>();
		void Test()
		{
			for (int i = 0; i < 5; i++)
				list.Add(new MasterA.SubA1(i));

			list.ForEach(_p =>
			{
				Console.WriteLine(_p.num);
			});
		}
	}
}
