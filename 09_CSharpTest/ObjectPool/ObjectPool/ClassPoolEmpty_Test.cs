using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ObjectPool
{
	class ClassPoolEmpty_Test
	{
		List<PackData> list = new List<PackData>();
		DateTime[] t = new DateTime[10];
		public void Start()
		{
			int _loop = 5;
			Debug.Log("ClassPoolEmpty_Test Loop:" + Constant.LOOP_MAX);
			while (true)
			{
				
				for (int i = 0; i < _loop; i++)
				{
					t[1] = DateTime.Now;
					switch (Constant.type)
					{
						case eTestType.ListFirst:	Fun_Random_First(); break;
						case eTestType.ListEnd:		Fun_Random_End(); break;
						case eTestType.ToggleFirst: Fun_Toggle_First(); break;
					}
					t[2] = DateTime.Now;
					Debug.Log("ClassPoolEmpty_Test test:" + ((TimeSpan)(t[2] - t[1])).Milliseconds + ":" + list.Count + ":" + 0);
				}
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}


		void Fun_Random_First()
		{
			//PackData _packet;
			Random _random = new Random();
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				int _rand = _random.Next() % 2;
				if (_rand == 0)
				{
					//_packet = new PackData();
					//_packet.PlusCount();
					//list.Add(_packet);
				}
				else if (list.Count > 0)
				{
					//list.RemoveAt(0);
				}
			}
		}

		void Fun_Random_End()
		{
			//PackData _packet;
			Random _random = new Random();
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				int _rand = _random.Next() % 2;
				if (_rand == 0)
				{
					//_packet = new PackData();
					//_packet.PlusCount();
					//list.Add(_packet);
				}
				else if (list.Count > 0)
				{
					//list.RemoveAt(0);
				}
			}
		}

		void Fun_Toggle_First()
		{
			//PackData _packet;
			Random _random = new Random();
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				int _rand = _random.Next() % 2;
				if (_rand == 0)
				{
					//_packet = new PackData();
					//_packet.PlusCount();
					//list.Add(_packet);
				}
				else if (list.Count > 0)
				{
					//list.RemoveAt(0);
				}
			}
		}
	}
}
