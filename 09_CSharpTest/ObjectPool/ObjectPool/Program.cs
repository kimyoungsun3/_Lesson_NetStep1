using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ObjectPool
{
	class Program
	{
		static void Main(string[] args)
		{
			ClassPoolT_Test _cpt = new ClassPoolT_Test();
			Thread _t1 = new Thread(new ThreadStart(_cpt.Start));
			_t1.Start();

			ClassPoolD_Test _cpd = new ClassPoolD_Test();
			Thread _t2 = new Thread(new ThreadStart(_cpd.Start));
			_t2.Start();

			ClassPoolNew_Test _cpn = new ClassPoolNew_Test();
			Thread _t3 = new Thread(new ThreadStart(_cpn.Start));
			_t3.Start();

			ClassPoolEmpty_Test _cpe = new ClassPoolEmpty_Test();
			Thread _t4 = new Thread(new ThreadStart(_cpe.Start));
			_t4.Start();
		}



	}
}
