using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace JsonTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//NewtonsoftTest _p = new NewtonsoftTest();
			//Thread _t1 = new Thread(new ThreadStart(_p.Start));
			//_t1.Start();

			//NewtonsoftTest2 _p2 = new NewtonsoftTest2();
			//Thread _t2 = new Thread(new ThreadStart(_p2.Start));
			//_t2.Start();

			//NewtonsoftTest3 _p3 = new NewtonsoftTest3();
			//Thread _t3 = new Thread(new ThreadStart(_p3.Start));
			//_t3.Start();

			//ZeroFormatterTest _zft = new ZeroFormatterTest();
			//Thread _tzft = new Thread(new ThreadStart(_zft.Start));
			//_tzft.Start();

			MessagePackTest _mst = new MessagePackTest();
			Thread _tmst = new Thread(new ThreadStart(_mst.Start));
			_tmst.Start();
		}
    }
}
