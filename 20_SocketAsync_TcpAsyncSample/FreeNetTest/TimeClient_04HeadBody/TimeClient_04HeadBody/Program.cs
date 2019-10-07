using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TimeClient_04HeadBody
{
	class Protocol
	{
		public const bool DEBUG = true;
		public const bool DEBUG_PACKET_SHOW = false;
	}

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
				}
				catch (SocketException _e)
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
			byte[] _sendBuffer = new byte[1024];
			byte[] _buffer = Encoding.ASCII.GetBytes("get time");
			byte[] _header = new byte[2];
			int _loopCount = 0;
			int _multiTime = 10;//10, 5, 1, 0
			int MULTI_LOOP = 1;// 100000;
			int _sendSize;
			try {
				while (true)
				{
					_start = DateTime.Now;
					_loopCount++;

					//---------------------------------
					// 횟수만큼만 전송해버리기...
					//---------------------------------
					for (int i = 0; i < MULTI_LOOP; i++)
					{
						Console.WriteLine("[{0}] delay:{1} data:[{2}]", i, _multiTime, Encoding.ASCII.GetString(_buffer));
						_buffer = Encoding.ASCII.GetBytes("get time[" + i + "]");
						Util.SetShort(_header, 0, (short)_buffer.Length);

						_sendSize = _header.Length + _buffer.Length;
						Array.Copy(_header, 0, _sendBuffer, 0, _header.Length);
						Array.Copy(_buffer, 0, _sendBuffer, 2, _buffer.Length);

						if (Protocol.DEBUG_PACKET_SHOW)
							for (int j = 0; j < _sendSize; j++)
								Console.WriteLine("[{0}]={1}", j, _sendBuffer[j]);

						clientSocket.Send(_sendBuffer, 0, _sendSize, SocketFlags.None);
						if (_multiTime > 0)
							System.Threading.Thread.Sleep(_multiTime);
					}
					Console.WriteLine(" >>> ");
					if (Protocol.DEBUG) Console.WriteLine("[C -> S] : {0} ", Encoding.ASCII.GetString(_buffer));

					int _rec = clientSocket.Receive(_receiveBuffer);
					byte[] _data = new byte[_rec];
					Array.Copy(_receiveBuffer, _data, _rec);

					_time = DateTime.Now - _start;
					if (Protocol.DEBUG)
					{
						if (_time.TotalMilliseconds > 0)
						{
							Console.WriteLine(" start:{0} end:{1}", _start, DateTime.Now);
						}
						Console.WriteLine("[C <- S] ({0}/ms):{1}", _time.TotalMilliseconds, Encoding.ASCII.GetString(_data));
					}
					if (_time.TotalMilliseconds > 1)
					{
						Console.WriteLine(" start:{0} end:{1} ({2}/ms)", _start, DateTime.Now, _time.TotalMilliseconds);
					}

					int _sleep = 10 + _rand.Next() % 20;
					System.Threading.Thread.Sleep(_sleep);
				}
			}
			catch (Exception _e)
			{
				Console.WriteLine("error:{0}", _e);
			}
		}
	}
}
