﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data;

namespace JsonTest
{
	public class PackData
	{
		public static int indexSequence;
		public int index;
		public int code;
		public int error;
		public int callCount;
		public string name;
		public DateTime expire;
		public string[] items;
		public PackData()
		{
			index = indexSequence++;
		}

		public void PlusCount()
		{
			callCount++;
		}

		public override string ToString()
		{
			return "index:" +index 
				+ " code:" + code
				+ " error:" + error
				+ " callCount:" + callCount
				+ " name:" + name
				+ " expire:" + expire
				;
		}
	}
}
