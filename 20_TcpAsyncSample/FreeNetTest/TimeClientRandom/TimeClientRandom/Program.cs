using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeClientRandom
{
	class Program
	{
		private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		static void Main(string[] args)
		{
			Console.Title = "Time Echo Client";
			Program _p = new Program();
			_p.LoopConnect();
			_p.SendLoop();
			//Console.ReadKey();
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
				}catch(SocketException _e)
				{
					Console.Clear();
					Console.WriteLine("Connection attempts:" + _attempts);
				}
			}
			Console.Clear();
			Console.WriteLine("Connect");
		}

		void SendLoop()
		{
			DateTime _start;
			TimeSpan _time;
			Random _rand = new Random();
			byte[] _receiveBuffer = new byte[1024];
			byte[] _buffer = Encoding.ASCII.GetBytes("get time");
			int _loopCount = 0;
			while (true)
			{
				_start = DateTime.Now;
				_loopCount++;

				clientSocket.Send(_buffer);
				//Console.WriteLine("[C -> S] : {0} ", Encoding.ASCII.GetString(_buffer));

				int _rec = clientSocket.Receive(_receiveBuffer);
				byte[] _data = new byte[_rec];
				Array.Copy(_receiveBuffer, _data, _rec);

				_time = DateTime.Now - _start;
				if(_time.TotalMilliseconds > 0)
				{
					Console.WriteLine(" start:{0} end:{0}", _start, DateTime.Now);
				}
				if(_loopCount % 100 == 0)
					Console.WriteLine("[C <- S] ({0}/ms):{1}", _time.TotalMilliseconds, Encoding.ASCII.GetString(_data));

				int _sleep = 10 + _rand.Next() % 50;
				System.Threading.Thread.Sleep(_sleep);
			}
		}
	}
}
