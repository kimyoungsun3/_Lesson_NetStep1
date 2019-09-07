using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _114_Parallel
{
	class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.DoTest();
			_p.DoTest2();

			Console.ReadKey();
		}

		void DoTest()
		{
			//하나의 Thread 처리..
			for(int i = 0; i < 1000; i++)
			{
				Console.Write("{0}:{1}\t", Thread.CurrentThread.ManagedThreadId, i);
			}
			Console.WriteLine();

			//병렬처리 다중 쓰레드 병렬처리...
			Parallel.For(0, 1000, (i) =>
			{
				Console.Write("{0}:{1}\t", Thread.CurrentThread.ManagedThreadId, i);
			});
		}

		DateTime[] t = new DateTime[10];
		void DoTest2()
		{
			Console.WriteLine("Single or Multi ...");
			t[0] = DateTime.Now;
			SequentialEncryt();
			t[1] = DateTime.Now;
			ParallelEncryt();
			t[2] = DateTime.Now;

			Console.WriteLine("SequentialEncryt:{0}", (t[1] - t[0]).Milliseconds);
			Console.WriteLine("SequentialEncryt:{0}", (t[2] - t[1]).Milliseconds);

		}

		const int MAX = 10000000;
		const int SHIFT = 3;
		void SequentialEncryt()
		{
			// 테스트 데이타 셋업
			// 1000 만개의 스트링
			string text = "I am a boy. My name is Tom.";
			List<string> textList = new List<string>(MAX);
			for (int i = 0; i < MAX; i++)
			{
				textList.Add(text);
			}

			// 순차 처리 (Test run: 8.7 초)
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			for (int i = 0; i < MAX; i++)
			{
				char[] chArr = textList[i].ToCharArray();
				// 모든 문자를 시저 암호화
				for (int x = 0; x < chArr.Length; x++)
				{
					// 시저 암호
					if (chArr[x] >= 'a' && chArr[x] <= 'z')
					{
						chArr[x] = (char)('a' + ((chArr[x] - 'a' + SHIFT) % 26));
					}
					else if (chArr[x] >= 'A' && chArr[x] <= 'Z')
					{
						chArr[x] = (char)('A' + ((chArr[x] - 'A' + SHIFT) % 26));
					}
				}

				// 변경된 암호로 치환
				textList[i] = new String(chArr);
			};
			watch.Stop();
			Console.WriteLine(watch.Elapsed.ToString());
		}

		void ParallelEncryt()
		{
			// 테스트 데이타 셋업
			// 1000 만개의 스트링
			string text = "I am a boy. My name is Tom.";
			List<string> textList = new List<string>(MAX);
			for (int i = 0; i < MAX; i++)
			{
				textList.Add(text);
			}

			// 병렬 처리 (Test run: 6.1 초)
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			Parallel.For(0, MAX, i =>
			{
				char[] chArr = textList[i].ToCharArray();

				// 모든 문자를 시저 암호화
				for (int x = 0; x < chArr.Length; x++)
				{
					// 시저 암호
					if (chArr[x] >= 'a' && chArr[x] <= 'z')
					{
						chArr[x] = (char)('a' + ((chArr[x] - 'a' + SHIFT) % 26));
					}
					else if (chArr[x] >= 'A' && chArr[x] <= 'Z')
					{
						chArr[x] = (char)('A' + ((chArr[x] - 'A' + SHIFT) % 26));
					}
				}

				// 변경된 암호로 치환
				textList[i] = new String(chArr);
			});
			watch.Stop();
			Console.WriteLine(watch.Elapsed.ToString());
		}
	}
}
