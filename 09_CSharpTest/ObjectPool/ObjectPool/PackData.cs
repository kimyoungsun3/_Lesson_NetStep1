using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPool
{
	public class PackData
	{
		public static int indexSequence;
		public int index;
		public int code;
		public int error;
		public int callCount;
		public PackData()
		{
			index = indexSequence++;
		}

		public void PlusCount()
		{
			callCount++;
		}
	}
}
