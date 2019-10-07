using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;


namespace TimeClient_13_Simple
{
	class Program
	{
		//public static Stack<int> jobData = new Stack<int>();
		bool DEBUG = false;
		private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		static void Main(string[] args)
		{
			Console.Title = "Time Echo Client 13";
			int threadCount = 2;
			Parallel.For(0, threadCount, s =>
			{
				Program _p = new Program();
				_p.LoopConnect();
				_p.SendLoop();
			});
			//Console.ReadKey();
		}

		void SendLoop()
		{
			DateTime _start;
			TimeSpan _time;
			int count = 0;
			try
			{
				while (true)
				{
					_start = DateTime.Now;

					//byte[] _buffer = Encoding.ASCII.GetBytes("get time");
					byte[] _buffer = Encoding.ASCII.GetBytes("goto x,y,z");
					clientSocket.Send(_buffer);
					if (DEBUG) Console.WriteLine("[C -> S] : {0}", Encoding.ASCII.GetString(_buffer));

					byte[] _receiveBuffer = new byte[1024];
					int _rec = clientSocket.Receive(_receiveBuffer);

					_time = DateTime.Now - _start;
					if (_time.TotalMilliseconds > 5)
					{
						Console.WriteLine("[" + Thread.CurrentThread.ManagedThreadId + "] DelayTime:{0} ", (DateTime.Now - _start).TotalMilliseconds);
					}
					else
					{
						if(count % 100000 == 1)
							Console.WriteLine("[" + Thread.CurrentThread.ManagedThreadId + "] >> fast : " + count);
					}
					if (DEBUG) Console.WriteLine("[C <- S] ({0}/ms):{1}", _time.TotalMilliseconds, Encoding.ASCII.GetString(_receiveBuffer, 0, _rec));

					count++;

					//int _id = Thread.CurrentThread.ManagedThreadId;
					//if(_id % 2 == 0)
					//{
					//	jobData.Push(1);
					//}
					//else
					//{
					//	lock (jobData)
					//	{
					//		if (jobData.Count > 0)
					//			jobData.Pop();
					//	}
					//}

					System.Threading.Thread.Sleep(1);		//1000전송/1초당
					//System.Threading.Thread.Sleep(5);		//200전송/1초당
					//System.Threading.Thread.Sleep(20);	//50전송/1초당
					//System.Threading.Thread.Sleep(50);	//20전송/1초당
					//System.Threading.Thread.Sleep(1000*60);
				}
			}
			catch (Exception _e)
			{
				Console.WriteLine("error:" + _e);
			}
		}

		void LoopConnect()
		{
			int _attempts = 0;
			while (!clientSocket.Connected)
			{
				try
				{
					_attempts++;
					clientSocket.Connect(IPAddress.Loopback, 100);
					//clientSocket.Connect(IPAddress.Parse("192.168.5.37"), 100);
				}
				catch (SocketException _e)
				{
					Console.Clear();
					Console.WriteLine("Connection attempts:" + _attempts);
				}
			}

			//Console.Clear();
			Console.WriteLine("Connected");
		}
	}
}
