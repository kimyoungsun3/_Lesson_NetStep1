using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			Program _p = new Program();
			//_p.DoTest();
			//_p.DoTest2();
			//_p.DoTest3();
			_p.DoTest4();

			Console.ReadKey();
		}

		void DoTest()
		{
			Console.WriteLine("======[Timer 1]========");
			Timer _timer = new System.Timers.Timer();
			_timer.Interval = 5 * 1000; //ms
			//1시간 -> 60분 
			//1분   -> 60초 
			//1초   -> 1000ms
			_timer.Elapsed += new ElapsedEventHandler(TimerRun);
			_timer.Start();
		}
		// 쓰레드풀의 작업쓰레드가 지정된 시간 간격으로
		// 아래 이벤트 핸들러 실행
		void TimerRun(object _obj, ElapsedEventArgs _args)
		{
			Console.WriteLine("Web access");			
			WebClient _webClient	= new WebClient();      // 웹페이지 html문을 다운로드
			string _webPage			= _webClient.DownloadString("http://mssql.tools");

			// 다운로드 내용을 파일에 저장
			string _time		= DateTime.Now.ToString("yyyyMMdd_hhmmss");
			//string outputFile	= string.Format("page_{0}.html", _time);
			//File.WriteAllText(outputFile, webpage);
			Console.WriteLine(_webPage);
			//Console.WriteLine("Web save:" + outputFile);
		}

		void DoTest2()
		{
			Console.WriteLine("======[Timer 2]========");
			Timer _timer = new System.Timers.Timer(5000);
			_timer.Elapsed += new ElapsedEventHandler(TimerRun2);
			//public event ElapsedEventHandler Elapsed;
			//public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);
			_timer.Start();
		}

		void TimerRun2(object _sender, ElapsedEventArgs _args)
		{
			Console.WriteLine("web access");
			WebClient _webClient	= new WebClient();
			string _webPage			= _webClient.DownloadString("http://naver.com");
			string _time			= DateTime.Now.ToString("yyyyMMdd_hhmmss");
			Console.WriteLine(_webPage);

			//string _outputFile = string.Format("page_{0}.html", _time);
			//File.WriteAllText(_outputFile, _webPage);
		}

		void DoTest3()
		{
			Console.WriteLine("======[Timer 3]========");
			Timer _timer = new System.Timers.Timer(5000);
			_timer.Elapsed += new ElapsedEventHandler(TimerRun3);
			_timer.Start();
		}

		void TimerRun3(object _sender, ElapsedEventArgs _args)
		{
			WebClient _webClient = new WebClient();
			string _webPage = _webClient.DownloadString("http://daum.net");
			string _time = DateTime.Now.ToString("yyyyMMdd_hhmmss");
			Console.WriteLine(_webPage);
		}

		
		void DoTest4()
		{
			Console.WriteLine("======[Timer 4]========");
			System.Threading.Timer _timer = new System.Threading.Timer(TimerRun4, 1, 0, 1000);
			//public Timer(TimerCallback callback, object state, uint dueTime, uint period);
			//public delegate void TimerCallback(object state);			
		}


		void TimerRun4(object _state)
		{
			Console.WriteLine(" >> TimerRun4 >> {0}", (int)_state);
		}
	}
}
