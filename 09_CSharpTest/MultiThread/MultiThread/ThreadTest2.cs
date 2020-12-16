using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThread
{
	public class ThreadTest2
	{
		public Queue<PackData> sendQueue	= new Queue<PackData>();
		public Queue<PackData> receiveQueue = new Queue<PackData>();
		public void Start()
		{
			ClassPoolD.Initialize(10000);
			Thread _t1 = new Thread(new ThreadStart(Work_Main));
			Thread _t2 = new Thread(new ThreadStart(Work_Sender));
			Thread _t3 = new Thread(new ThreadStart(Work_Receiver));
			_t1.Start();
			_t2.Start();
			_t3.Start();
		}

		void Work_Main()
		{
			while (true)
			{
				Console.WriteLine("\tMain:");
				PackData _packSender = null;

				lock (receiveQueue)
				{
					if (receiveQueue.Count > 0)
					{
						PackData _packReceive = receiveQueue.Dequeue();
						Console.WriteLine("\tMain: receive/process/copy >> " + _packReceive.ToString());

						_packSender = ClassPoolD.Dequeue();
						_packSender.Copy(_packReceive);
						ClassPoolD.Enqueue(_packReceive);
					}
				}

				if (_packSender != null)
				{
					lock (sendQueue)
					{
						Console.WriteLine("\tMain: sender queue input " + _packSender.ToString());
						sendQueue.Enqueue(_packSender);
					}
				}
				
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		void Work_Sender()
		{
			while (true)
			{
				lock (sendQueue)
				{
					if(sendQueue.Count > 0)
					{
						Console.WriteLine("\t\tWork_Sender:");
						PackData _packSender = sendQueue.Dequeue();

						Console.WriteLine("\t\t:" + _packSender.ToString());
						ClassPoolD.Enqueue(_packSender);
					}
				}
				
				Thread.Sleep(Constant.SLEEP_TIME);
			}
		}

		void Work_Receiver()
		{
			int _count = 0;
			while (true)
			{
				if(_count++ % 10 == 0)
				{
					Console.WriteLine("Work_Receiver >> Data Receive");
					PackData _pack = ClassPoolD.Dequeue();
					_pack.code = 1;
					_pack.position = new Vector3(1.1f, 2.2f, 3.3f);

					lock (receiveQueue)
					{
						receiveQueue.Enqueue(_pack);
					}
					Console.WriteLine("" + _pack.ToString());
					Console.WriteLine("pool free count:" + ClassPoolD.queue.Count);
				}


				Thread.Sleep(Constant.SLEEP_TIME);
			}

		}
	}
}
