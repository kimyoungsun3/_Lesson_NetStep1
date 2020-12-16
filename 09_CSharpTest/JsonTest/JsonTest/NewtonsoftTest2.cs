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
	public class NewtonsoftTest2
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
					Fun_ToJson();
					t[2] = DateTime.Now;
					Debug.Log("NewtonsoftTest2 FunToJson:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_FromJson();
					t[2] = DateTime.Now;
					Debug.Log("\tNewtonsoftTest2 FunFromJson:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_Linq();
					t[2] = DateTime.Now;
					Debug.Log("\t\t\tNewtonsoftTest2 Fun_Linq:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_ParseJson();
					t[2] = DateTime.Now;
					Debug.Log("\t\t\t\tNewtonsoftTest2 Fun_ParseJson:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					Fun_ParseJsonTest();
					t[2] = DateTime.Now;
					Debug.Log("\t\t\t\tNewtonsoftTest2 Fun_ParseJsonTest:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

				}
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		void Fun_ToJson()
		{
			//1. class -> json string
			string _json = "";
			PackData _packData = new PackData()
			{
				code = 1,
				error = 2,
				callCount = 3,
				name = "Apple",
				expire = DateTime.Now,
				items = new string[] { "One", "Two", "Three" }
			};
			//_jsonStr = JsonConvert.SerializeObject(_packData, Formatting.Indented);
			//Debug.Log(_jsonStr);

			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				_json = JsonConvert.SerializeObject(_packData);
			}
			Debug.Log(_json);
		}

		void Fun_FromJson()
		{
			string _json = @"{
								'index': 0,
								'code': 1,
								'error': 2,
								'callCount': 3,
								'name': 'Apple',
								'expire': '2020-10-14T21:33:46.5538426+09:00',
								'items': [
											'One',
											'Two',
											'Three'
										]
								}";

			PackData _packData = null;
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				_packData = JsonConvert.DeserializeObject<PackData>(_json);
			}
			Debug.Log("\t" + _packData.ToString());
		}

		void Fun_Linq()
		{
			JArray _array;
			JObject _o = null;
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				_array = new JArray();
				_array.Add(1);
				_array.Add(2);
				_array.Add(3);
				_array.Add("Manual text");
				_array.Add(DateTime.Now);
				_array.Add("One");
				_array.Add("Two");
				_array.Add("Three");

				_o = new JObject();
				_o["MyArray"] = _array;
			}

			string _json = _o.ToString();
			Debug.Log(_json);

			//JObject
		}

		private void Fun_ParseJson()
		{
			string _json = @"{
							'class'				: 'go.GraphLinksModel',
							'linkKeyProperty'	: 'stepId',
							'position'			: {'x':1.1, 'y':2.2, 'z':3.3},
							'nodeDataArray'	: [
								{
									'key':11, 
									'category':'start', 
									'text':'Start', 
									'loc':{'x':288, 'y':109}
								},
								{
									'key':101, 
									'category':'message', 
									'text':'Message', 
									'items':[ 
										{'name':'Name', 'value':'Message'},
										{'name':'Type', 'value':'Step'},
										{'name':'Category', 'value':'대분류'},
										{'name':'Size', 'value':'120'},{'name':'Max Length', 'value':'20'} 
									], 
									'loc':{'x':288, 'y':109}
								}
							],
							}";

			//string _json = @"{
			//					'index': 0,
			//					'code': 1,
			//					'error': 2,
			//					'callCount': 3,
			//					'name': 'Apple',
			//					'expire': '2020-10-14T21:33:46.5538426+09:00',
			//					'items': [
			//								'One',
			//								'Two',
			//								'Three'
			//							]
			//					}";

			PackData _packdata = new PackData();
			JObject _jo = JObject.Parse(_json);
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{

				//Debug.Log(_jo.SelectToken("class").ToString());
				//Debug.Log(_jo.SelectToken("linkKeyProperty").ToString());
				//Debug.Log(_jo.SelectToken("position").ToString());
				//Debug.Log(_jo.SelectToken("position").SelectToken("x").ToString());
				//Debug.Log(_jo.SelectToken("nodeDataArray").ToString());
				//Debug.Log(_jo.SelectToken("nodeDataArray").SelectToken("items").ToString());

				JToken _jt = _jo.SelectToken("nodeDataArray");
				foreach (JToken item in _jt)
				{
					var key = item.SelectToken("key").ToString();
					var category = item.SelectToken("category").ToString();
					var text = item.SelectToken("text").ToString();
					var items = item.SelectToken("items");
					if (items != null)
					{
						foreach (var token in items)
						{
							var name = String.Format("{0}", token.SelectToken("name"));
							var value = String.Format("{0}", token.SelectToken("value"));
						}
					}
				}
			}
		}

		private void Fun_ParseJsonTest()
		{
			string _json = @"{
							'class'				: 'go.GraphLinksModel',
							'linkKeyProperty'	: 'stepId',
							'position'			: {'x':1.1, 'y':2.2, 'z':3.3},
							'nodeDataArray'	: [
								{
									'key':11, 
									'category':'start', 
									'text':'Start', 
									'loc':{'x':288, 'y':109}
								},
								{
									'key':101, 
									'category':'message', 
									'text':'Message', 
									'items':[ 
										{'name':'Name', 'value':'Message'},
										{'name':'Type', 'value':'Step'},
										{'name':'Category', 'value':'대분류'},
										{'name':'Size', 'value':'120'},{'name':'Max Length', 'value':'20'} 
									], 
									'loc':{'x':288, 'y':109}
								}
							],
							}";


			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				JObject _jo = JObject.Parse(_json);                         //146 ~200
				JToken _jt = _jo.SelectToken("position");                   //200 ~ 300
				float _x = float.Parse(_jt.SelectToken("x").ToString());    //468 ~ 452
				float _y = float.Parse(_jt.SelectToken("y").ToString());    //451 ~ 539
				float _z = float.Parse(_jt.SelectToken("z").ToString());    //518 ~ 
			}
		}
	}
}
