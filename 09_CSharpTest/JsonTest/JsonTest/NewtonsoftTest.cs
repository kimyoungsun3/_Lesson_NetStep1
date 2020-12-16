using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace JsonTest
{
	public class NewtonsoftTest
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
					FunToJson();
					t[2] = DateTime.Now;
					Debug.Log("NewtonsoftTest FunToJson:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

					t[1] = DateTime.Now;
					FunFromJson();
					t[2] = DateTime.Now;
					Debug.Log("\tNewtonsoftTest FunFromJson:" + ((TimeSpan)(t[2] - t[1])).Milliseconds);

				}
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		void FunToJson()
		{
			//1. class -> json string
			PackData _packData = new PackData()
			{
				code		= 1,
				error		= 2,
				callCount	= 3
			};
			//Debug.Log(_jsonStr);
			//string _jsonStr = JsonConvert.SerializeObject(_packData, Formatting.Indented);

			string _jsonStr = "";
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{	
				_jsonStr = JsonConvert.SerializeObject(_packData);
			}
			Debug.Log(_jsonStr);
		}

		void FunFromJson()
		{
			//1. class -> json string
			PackData _packData = new PackData()
			{
				code		= 1,
				error		= 2,
				callCount	= 3
			};
			string _jsonStr		= JsonConvert.SerializeObject(_packData);

			PackData _packData2 = new PackData();
			for (int i = 0; i < Constant.LOOP_MAX; i++)
			{
				_packData2 = JsonConvert.DeserializeObject<PackData>(_jsonStr);
			}
			Debug.Log("\t"+_packData2.ToString());
		}
	}
}
