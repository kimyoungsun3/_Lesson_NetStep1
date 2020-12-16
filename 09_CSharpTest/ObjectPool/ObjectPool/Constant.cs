using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPool
{
	public enum eTestType { ListFirst, ListEnd, ToggleFirst};
	public class Constant
	{
		public const int LOOP_MAX		= 100_000;
		public const int BUFFER_SIZE	= 100_000;
		public const int SLEEP_TIME		= 50;
		public static eTestType type	= eTestType.ListEnd;
	}
}
