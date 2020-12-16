using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadHeater
{
    class Program
    {
        static object obj = new object();
        static void Main(string[] args)
        {
            long powCnt = 0;
            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                new Thread(() =>
                {
                    //double x = 1.234234;
                    while (true)
                    {
                        for (int u = 1; u < 500_000; u++)
                        {
                            //x = x / 2.1234;
                            //x = x * 4.13245;
                            //x = x - 1.453457839;
                        }
                        lock (obj)
                        {
                            powCnt += 1;
                        }
                    }
                }).Start();
            }
            var Ccnt = 0;
            Thread.Sleep(1000);
            powCnt = 0;
            while (true)
            {
                Thread.Sleep(1000);
                Ccnt += 1;
                lock (obj)
                {
                    //Console.Clear();
                    Console.WriteLine($"PcPower:{powCnt / Ccnt}");
                    //powCnt = 0;
                }
            }
        }
    }
}
