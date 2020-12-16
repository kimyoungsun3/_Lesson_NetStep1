using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ZeroFormatter;

namespace JsonTest
{
	public class ZeroFormatterTest
	{
		DateTime[] t = new DateTime[10];
		public void Start()
		{
			int _loop = 1;
			while (true)
			{
				for (int i = 0; i < _loop; i++)
				{
					t[1] = DateTime.Now;
					Fun_JsonTest();
					t[2] = DateTime.Now;
					Debug.Log("ZeroFormatterTest Fun_JsonTest:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_stringSplit();
					t[2] = DateTime.Now;
					Debug.Log("ZeroFormatterTest Fun_stringSplit:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_Class();
					t[2] = DateTime.Now;
					Debug.Log("ZeroFormatterTest Fun_Class:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

				}
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		private void Fun_JsonTest()
		{
			byte[] c = null; 
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				c = ZeroFormatterSerializer.Serialize(new StringZ {
					name		= "a",
					health		= 100,
					fff			= 1.124f,
					position1	= new Vector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position2	= new Vector3 { x = 1.123f, y = 2.123f, z = 3.123f }
				});
				//c.name += "b";
			}
			//Console.WriteLine(c);

			//var re = ZeroFormatterSerializer.Convert(c);
		}

		private void Fun_stringSplit()
		{
			string _s = "1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@1@";
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				string[] _s2 = _s.Split('@');
			}
		}

		private void Fun_Class()
		{
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				StringZ2 _s2 = new StringZ2()
				{
					name = "a",
					health = 100,
					fff = 1.124f,
					position1 = new Vector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position2 = new Vector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position3 = new Vector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position4 = new Vector3 { x = 1.123f, y = 2.123f, z = 3.123f }
				};
			}
		}
	}


	[ZeroFormattable]
	public class StringZ
	{
		[Index(0)]
		public virtual string name { get; set; }

		[Index(1)]
		public virtual int health { get; set; }

		[Index(2)]
		public virtual float fff { get; set; }

		[Index(3)]
		public virtual Vector3 position1 { get; set; }

		[Index(4)]
		public virtual Vector3 position2 { get; set; }

		[Index(5)]
		public virtual Vector3 position3 { get; set; }

		[Index(6)]
		public virtual Vector3 position4 { get; set; }

		public override string ToString()
		{
			StringBuilder _b = new StringBuilder();
			_b.Append("'name':");
			_b.Append(name);
			_b.Append(',');

			_b.Append("'health':");
			_b.Append(health);
			_b.Append(',');

			_b.Append("'fff':");
			_b.Append(fff);
			_b.Append(',');

			_b.Append("'position1':");
			_b.Append(position1.ToString());
			_b.Append(',');

			_b.Append("'position2':");
			_b.Append(position2.ToString());
			_b.Append(',');

			_b.Append("'position3':");
			_b.Append(position3.ToString());
			_b.Append(',');

			_b.Append("'position4':");
			_b.Append(position4.ToString());
			_b.Append(',');
			return _b.ToString();
		}
	}

	[ZeroFormattable]
	public class Vector3
	{
		[Index(0)]
		public virtual float x { get; set; }
		[Index(1)]
		public virtual float y { get; set; }
		[Index(2)]
		public virtual float z { get; set; }

		public override string ToString()
		{
			StringBuilder _b = new StringBuilder();
			_b.Append("'x':");
			_b.Append(x);
			_b.Append(',');

			_b.Append("'y':");
			_b.Append(y);
			_b.Append(',');

			_b.Append("'z':");
			_b.Append(z);
			return _b.ToString();
		}
	}

	public class StringZ2
	{
		public string name;
		public int health { get; set; }
		public float fff { get; set; }
		public Vector3 position1 { get; set; }
		public Vector3 position2 { get; set; }
		public Vector3 position3 { get; set; }
		public Vector3 position4 { get; set; }
	}
}
