using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ObjectPool
{
	public class ClassPoolD_Test
	{
		List<PackData> list		= new List<PackData>();
		DateTime[] t			= new DateTime[10];
		public void Start()
		{
			int _loop	= 5;
			t[0]		= DateTime.Now;
			ClassPoolD.Initialize(Constant.BUFFER_SIZE);
			t[1]		= DateTime.Now;
			Debug.Log("ClassPoolD_Test Loop:" + Constant.LOOP_MAX);
			Debug.Log("OPD create:" + ((TimeSpan)(t[1] - t[0])).Milliseconds);
			while (true)
			{
				for (int i = 0; i < _loop; i++)
				{
					t[1] = DateTime.Now;
					switch (Constant.type)
					{
						case eTestType.ListFirst:	Fun_Random_First(); break;
						case eTestType.ListEnd:		Fun_Random_End();	break;
						case eTestType.ToggleFirst: Fun_Toggle_First(); break;
					}
					
					t[2] = DateTime.Now;
					Debug.Log("\tClassPoolD_Test test:" + ((TimeSpan)(t[2] - t[1])).Milliseconds 
						+ ":" + list.Count 
						+ ":" + ClassPoolD.queue.Count);
				}
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		void Fun_Random_First()
		{
			PackData _packet;
			Random _random = new Random();
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				int _count = list.Count;
				int _rand = _random.Next() % 2;
				if (_rand == 0)
				{
					_packet = ClassPoolD.Dequeue();
					_packet.PlusCount();
					list.Add(_packet);
				}
				else if (_count > 0)
				{
					ClassPoolD.Enqueue(list[0]);
					list.RemoveAt(0);
				}
			}
		}

		void Fun_Random_End()
		{
			PackData _packet;
			Random _random = new Random();
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				int _count = list.Count;
				int _rand = _random.Next() % 2;
				if (_rand == 0)
				{
					_packet = ClassPoolD.Dequeue();
					_packet.PlusCount();
					list.Add(_packet);
				}
				else if (_count > 0)
				{
					ClassPoolD.Enqueue(list[_count - 1]);
					list.RemoveAt(_count - 1);
				}
			}
		}


		void Fun_Toggle_First()
		{
			PackData _packet;
			int _rr = 0;
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				int _count = list.Count;
				int _rand = _rr++ % 2;
				if (_rand == 0)
				{
					_packet = ClassPoolD.Dequeue();
					_packet.PlusCount();
					list.Add(_packet);
				}
				else if (_count > 0)
				{
					ClassPoolD.Enqueue(list[0]);
					list.RemoveAt(0);
				}


				//if (list.Count > 800)
				//{
				//	while (list.Count > 0)
				//	{
				//		ClassPoolD.Enqueue(list[0]);
				//		list.RemoveAt(0);
				//	}
				//}
			}
		}

	}
}
