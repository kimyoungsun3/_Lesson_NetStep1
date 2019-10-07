using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Timers;

namespace _115_01_Timer
{
	class Program
	{
		static void Main(string[] args)
		{
			int threadCount = 10000;
			//Parallel.For(0, threadCount, s =>
			//{
			//	Program _p = new Program();
			//	_p.DoTest(s);
			//});

			//Parallel.For(0, threadCount, s =>
			//{
			//	Program _p = new Program();
			//	_p.DoTest2(s);
			//});

			Parallel.For(0, threadCount, s =>
			{
				Program _p = new Program();
				_p.DoTest4(s);
			});

			Console.ReadKey();
		}

		int identity;
		int count;
		System.Timers.Timer timer;
		void DoTest(int _idx)
		{
			identity	= _idx;
			count		= 0;
			if (identity == 9999)
				Console.WriteLine($"======[DoTest System.Timers.Timer {_idx}]========");
			timer			= new System.Timers.Timer();
			timer.Interval	= 20;//ms
			timer.Elapsed	+= new ElapsedEventHandler(OnTimer);
			timer.Start();
		}

		// 쓰레드풀의 작업쓰레드가 지정된 시간 간격으로 아래 이벤트 핸들러 실행
		
		void OnTimer(object _obj, ElapsedEventArgs _args)
		{
			count++;
			if (identity == 9999 && count % 50 == 0)
			{
				string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
				Console.WriteLine($"OnTimer {identity}/{count} >> {_time}  {System.Threading.Thread.CurrentThread.ManagedThreadId}");
				if (count > 200)
				{
					Console.WriteLine(" >> 중지...");
					timer.Stop();
				}
			} 

			// 다운로드 내용을 파일에 저장
			//WebClient _webClient = new WebClient();
			//string _page = _webClient.DownloadString("http://mssql.tools");
			//string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss");
			//string outputFile	= string.Format("page_{0}.html", _time);
			//File.WriteAllText(outputFile, webpage);
			//Console.WriteLine(_page);
			//Console.WriteLine("Web save:" + outputFile);
		}

		System.Timers.Timer timer2;
		int identity2, count2;
		void DoTest2(int _idx)
		{
			identity2	= _idx;
			count2		= 0;
			if (identity2 == 9999)
				Console.WriteLine($"======[DoTest2 System.Timers.Timer {_idx}]========");
			timer2 = new System.Timers.Timer(20);
			timer2.Elapsed				+= new ElapsedEventHandler(OnTimer2);
			//public event ElapsedEventHandler Elapsed;
			//public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);
			timer2.Start();
		}
		void OnTimer2(object _sender, ElapsedEventArgs _args)
		{
			count2++;
			if (identity2 == 9999 && count2 % 50 == 0)
			{
				string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
				Console.WriteLine($"OnTimer2 {identity2}/{count2} >> {_time}  {System.Threading.Thread.CurrentThread.ManagedThreadId}");
				if (count2 > 200)
				{
					Console.WriteLine(" >> 중지...");
					timer2.Stop();
				}
			}

			//Console.WriteLine("web access");
			//WebClient _webClient	= new WebClient();
			//string _webPage			= _webClient.DownloadString("http://naver.com");
			//string _time			= DateTime.Now.ToString("yyyyMMdd_hhmmss");
			//Console.WriteLine(_webPage);

			//string _outputFile = string.Format("page_{0}.html", _time);
			//File.WriteAllText(_outputFile, _webPage);
		}

		System.Threading.Timer timer4;
		int count4;
		void DoTest4(int _idx)
		{
			identity		= _idx;
			count4			= 0;
			string _time	= DateTime.Now.ToString("yyyyMMdd_hhmmss");

			if (identity == 9999)
				Console.WriteLine($"======[Timer4 System.Threading.Timer {_idx}]========{_time}");
			timer4 = new System.Threading.Timer(OnTimer4, _idx, 0, 20);
			//public Timer(TimerCallback callback, object state, uint dueTime, uint period);
			//public delegate void TimerCallback(object state);			
		}

		void OnTimer4(object _obj)
		{
			count4++;
			if (identity == 9999 && count4 % 50 == 0)
			{
				string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss_fff");
				Console.WriteLine($"OnTimer4 {(int)_obj}/{identity}/{count4} >> {_time} {System.Threading.Thread.CurrentThread.ManagedThreadId}");
				if (count4 > 200)
				{
					Console.WriteLine(" >> 중지...");
					timer4.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
				}
			}
		}
	}
}
