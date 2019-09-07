using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _108_InnerClass
{
	public class MasterA
	{
		public int index;
		public int reward;
		public SubA1 sub1;
		public SubA2 sub2;
		public MasterA(int _index, int _reward, SubA1 _s1, SubA2 _s2)
		{
			index = _index;
			reward = _reward;
			sub1 = _s1;
			sub2 = _s2;
		}

		public class SubA1
		{
			public int num;
			public SubA1(int _num)
			{
				num = _num;
			}
		}

		public class SubA2
		{
			public int num;
			public SubA2(int _num)
			{
				num = _num;
			}
		}
	}
}
