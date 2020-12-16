using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MessagePack;

namespace JsonTest
{
    public class MessagePackTest
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
                    Fun_Serialize();
                    t[2] = DateTime.Now;
                    Debug.Log("MessagePack Fun_Serialize:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_Serialize2();
					t[2] = DateTime.Now;
					Debug.Log("MessagePack Fun_Serialize2:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_Deserialize();
					t[2] = DateTime.Now;
					Debug.Log("MessagePack Fun_Deserialize:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_DeserializeDynamic();
					t[2] = DateTime.Now;
					Debug.Log("MessagePack Fun_DeserializeDynamic:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);


					t[1] = DateTime.Now;
                    Fun_stringSplit();
                    t[2] = DateTime.Now;
                    Debug.Log("MessagePack Fun_stringSplit:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

                    t[1] = DateTime.Now;
                    Fun_Class();
                    t[2] = DateTime.Now;
                    Debug.Log("MessagePack Fun_Class:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

                }
                Thread.Sleep(Constant.SLEEP_TIME);
            }
        }

        private void Fun_Serialize()
        {
            //StringZ c = null;
            byte[] c = null;
            for (int i = 0; i < Constant.LOOP_MAX; i++)
            {
                c = MessagePackSerializer.Serialize(new mStringZ2
                {
                    name	= "a",
                    health	= 100,
                    fff		= 1.124f,
					position1 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position2 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					//position3 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					//position4 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f }
				});
                //c.name += "b";
            }
			Console.WriteLine( " >> "  + c.Length);
			//var re = ZeroFormatterSerializer.Convert(c);
		}

		private void Fun_Serialize2()
		{
			//StringZ c = null;
			byte[] c = null;
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				c = MessagePackSerializer.Serialize(new mStringZ2
				{
					name = "a",
					health = 100,
					fff = 1.124f,
					position1 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position2 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position3 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
					position4 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f }
				});
				//c.name += "b";
			}
			Console.WriteLine(" >> " + c.Length);
			//var re = ZeroFormatterSerializer.Convert(c);
		}

		private void Fun_Deserialize()
		{
			//StringZ c = null;
			byte[] c = MessagePackSerializer.Serialize(new mStringZ2
			{
				name	= "a",
				health	= 100,
				fff		= 1.124f,
				position1 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
				position2 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
				position3 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
				position4 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f }
			});

			mStringZ2 _m2 = MessagePackSerializer.Deserialize<mStringZ2>(c);
			mStringZ3 _m3 = MessagePackSerializer.Deserialize<mStringZ3>(c);
			//mStringZ4 _m4 = MessagePackSerializer.Deserialize<mStringZ4>(c);

			mStringZ2 _m = null;
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				_m = MessagePackSerializer.Deserialize<mStringZ2>(c);
			}
			Console.WriteLine(_m);
			//var re = ZeroFormatterSerializer.Convert(c);
		}


		private void Fun_DeserializeDynamic()
		{
			//StringZ c = null;
			byte[] c = MessagePackSerializer.Serialize(new mStringZ2
			{
				name = "a",
				health = 100,
				fff = 1.124f,
				position1 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
				position2 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
				position3 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
				position4 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f }
			});

			dynamic _m = null;
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				_m = MessagePackSerializer.Deserialize<dynamic>(c);
			}
			Console.WriteLine(_m);
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
                mStringZ2 _s2 = new mStringZ2()
                {
                    name = "a",
                    health = 100,
                    fff = 1.124f,
                    position1 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
                    position2 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
                    position3 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f },
                    position4 = new mVector3 { x = 1.123f, y = 2.123f, z = 3.123f }
                };
            }
        }
    }


    [MessagePackObject]
    public class mStringZ2
    {
        [Key(0)]
        public virtual string name { get; set; }

        [Key(1)]
        public virtual int health { get; set; }

        [Key(2)]
        public virtual float fff { get; set; }

        [Key(3)]
        public virtual mVector3 position1 { get; set; }

        [Key(4)]
        public virtual mVector3 position2 { get; set; }

        [Key(5)]
        public virtual mVector3 position3 { get; set; }

        [Key(6)]
        public virtual mVector3 position4 { get; set; }

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


	[MessagePackObject]
	public class mStringZ3
	{
		[Key(0)]
		public virtual string name { get; set; }

		[Key(1)]
		public virtual int health { get; set; }

		//[Key(2)]
		//public virtual float fff { get; set; }

		[Key(3)]
		public virtual mVector3 position1 { get; set; }

		[Key(4)]
		public virtual mVector3 position2 { get; set; }

		[Key(5)]
		public virtual mVector3 position3 { get; set; }

		[Key(6)]
		public virtual mVector3 position4 { get; set; }

		public override string ToString()
		{
			StringBuilder _b = new StringBuilder();
			_b.Append("'name':");
			_b.Append(name);
			_b.Append(',');

			_b.Append("'health':");
			_b.Append(health);
			_b.Append(',');

			//_b.Append("'fff':");
			//_b.Append(fff);
			//_b.Append(',');

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


	[MessagePackObject]
	public class mStringZ4
	{
		[Key(0)]
		public virtual string name { get; set; }

		[Key(1)]
		public virtual int health { get; set; }

		[Key(2)]
		public virtual float fff { get; set; }

		[Key(3)]
		public virtual mVector3 position1 { get; set; }

		[Key(4)]
		public virtual mVector3 position2 { get; set; }

		[Key(5)]
		public virtual mVector3 position3 { get; set; }

		[Key(6)]
		public virtual mVector3 position4 { get; set; }

		//[Key(7)]
		//public virtual mVector3 position7 { get; set; }

		//[Key(8)]
		//public virtual mVector3 position8 { get; set; }

		public override string ToString()
		{
			StringBuilder _b = new StringBuilder();
			_b.Append("'name':");
			_b.Append(name);
			_b.Append(',');

			_b.Append("'health':");
			_b.Append(health);
			_b.Append(',');

			//_b.Append("'fff':");
			//_b.Append(fff);
			//_b.Append(',');

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

	[MessagePackObject]
    public class mVector3
    {
        [Key(0)]
        public virtual float x { get; set; }
        [Key(1)]
        public virtual float y { get; set; }
        [Key(2)]
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

    public class mStringZ
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
