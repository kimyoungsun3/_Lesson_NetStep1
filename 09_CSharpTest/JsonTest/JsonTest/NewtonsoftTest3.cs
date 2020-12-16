using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace JsonTest
{
	class NewtonsoftTest3
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
					Fun_ParseJsonTest2();
					t[2] = DateTime.Now;
					Debug.Log("NewtonsoftTest3 Fun_ParseJsonTest2:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_ParseJsonTest4();
					t[2] = DateTime.Now;
					Debug.Log("\tNewtonsoftTest3 Fun_ParseJsonTest4:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_ParseJsonTest5();
					t[2] = DateTime.Now;
					Debug.Log("\t\tNewtonsoftTest3 Fun_ParseJsonTest5:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

				}
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		private void Fun_ParseJsonTest2()
		{
			JObject o = JObject.Parse(@"{
						  'Stores': [
							'Lambton Quay',
							'Willis Street'
						  ],
						  'Manufacturers': [
							{
							  'Name': 'Acme Co',
							  'Products': [
								{
								  'Name': 'Anvil',
								  'Price': 50
								}
							  ]
							},
							{
							  'Name': 'Contoso',
							  'Products': [
								{
								  'Name': 'Elbow Grease',
								  'Price': 99.95
								},
								{
								  'Name': 'Headlight Fluid',
								  'Price': 4
								}
							  ]
							}
						  ]
						}");

			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				string name = (string)o.SelectToken("Manufacturers[0].Name");
				string _s1 = (string)o.SelectToken("Manufacturers[0].Name");
				string _s2 = (string)o.SelectToken("Manufacturers[0].Name");
				string _s3 = (string)o.SelectToken("Manufacturers[0].Name");
				string _s4 = (string)o.SelectToken("Manufacturers[0].Name");
				// Acme Co

				decimal productPrice = (decimal)o.SelectToken("Manufacturers[0].Products[0].Price");
				// 50

				string productName = (string)o.SelectToken("Manufacturers[1].Products[0].Name");
			}
		}

		private void Fun_ParseJsonTest4()
		{
			JObject o = JObject.Parse(@"{
							'position':{ 'x':1.2344, 'y':2.23456, 'z':3.2345},
							'rotation':{ 'x':1.2344, 'y':1.23456, 'z':1.2345, 'w':1.2345678},
						}");

			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				string x	= (string)o.SelectToken("position.x");
				string y	= (string)o.SelectToken("position.y");
				//string z = (string)o.SelectToken("position.z");

				//string w = (string)o.SelectToken("rotation.w");
				// Acme Co

				//decimal productPrice = (decimal)o.SelectToken("Manufacturers[0].Products[0].Price");
				// 50

				//string productName = (string)o.SelectToken("Manufacturers[1].Products[0].Name");
			}
		}

		private void Fun_ParseJsonTest5()
		{
			//var jsonObject = new JObject();
			//jsonObject.Add("Date", DateTime.Now);
			//jsonObject.Add("Album", "Me Against The World");
			//jsonObject.Add("Year", 1995);
			//jsonObject.Add("Artist", "2Pac");
			//Debug.Log(jsonObject.ToString());

			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				var jsonObject = new JObject();
				jsonObject.Add("Date", DateTime.Now);
				jsonObject.Add("Album", "Me Against The World");
				jsonObject.Add("Year", 1995);
				jsonObject.Add("Artist", "2Pac");
			}
		}
	}
}
