using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeClient
{
	class Program
	{
        bool DEBUG = false;
		private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		static void Main(string[] args)
		{
			Console.Title = "Time Echo Client";
			Program _p = new Program();
			_p.LoopConnect();
			_p.SendLoop();
			//Console.ReadKey();
		}

		void SendLoop()
		{
			DateTime _start;
			TimeSpan _time;
			while (true)
			{
				//Console.WriteLine("Enter a request:");
				//string _reg = Console.ReadLine();

				_start = DateTime.Now;

				//byte[] _buffer = Encoding.ASCII.GetBytes("get time");
                byte[] _buffer = Encoding.ASCII.GetBytes("goto x,y,z");
                clientSocket.Send(_buffer);
				if(DEBUG) Console.WriteLine("[C -> S] : {0}", Encoding.ASCII.GetString(_buffer));

				byte[] _receiveBuffer = new byte[1024];
				int _rec = clientSocket.Receive(_receiveBuffer);
				byte[] _data = new byte[_rec];
				Array.Copy(_receiveBuffer, _data, _rec);

				_time = DateTime.Now - _start;
				if(_time.TotalMilliseconds > 2)
				{
                    Console.WriteLine(" DelayTime:{0} ", (DateTime.Now - _start).TotalMilliseconds);
				}
                else
                {
                    Console.WriteLine(" >> fast");
                }
                if (DEBUG) Console.WriteLine("[C <- S] ({0}/ms):{1}", _time.TotalMilliseconds, Encoding.ASCII.GetString(_data));



				System.Threading.Thread.Sleep(5);

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
                    //clientSocket.Connect(IPAddress.Loopback, 100);
                    clientSocket.Connect(IPAddress.Parse("192.168.0.74"), 100);
				}catch(SocketException _e)
				{
					Console.Clear();
					Console.WriteLine("Connection attempts:" + _attempts);
				}
			}

			Console.Clear();
			Console.WriteLine("Connected");
		}
	}
}
