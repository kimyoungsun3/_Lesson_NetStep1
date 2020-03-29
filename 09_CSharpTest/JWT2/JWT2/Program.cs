using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpbitAPI_CS_Wrapper;

namespace JWT2
{
	class Program
	{
		static void Main(string[] args)
		{
			//영선
			string akey = "72yDxrSDYpSF2HT4w02WsSoyfkLCsmT2BpM049Mr";
			string skey = "a9RoMMP6wdD49rinx6yN5MqRgR7vVxCwFYUifgDO";
			string msg = "영선";

			//엄마 
			//string akey = "wcwwTczlqYcYUWgM02sbA9lN2zRTwgp6zLbIbHQK";
			//string skey = "Af6ItyLopXFthoUoZTWkxH4D3ueHf9eSpj695z1t";
			//sstring msg = "엄마";
			string uuid;

			UpbitAPI U = new UpbitAPI(akey, skey);

			#region 자산
			// 자산 조회
			//Console.WriteLine("\n\n\n================");
			//Console.WriteLine("개인:"+ U.GetAccount());
			#endregion

			#region 주문
			// 주문 가능 정보
			//Console.WriteLine("\n\n\n================");
			//Console.WriteLine("KRW-BTC:" + U.GetOrderChance("KRW-BTC"));

			// 개별 주문 조회
			//Console.WriteLine(U.GetOrder("주문 uuid"));

			// 주문 리스트 조회
			//Console.WriteLine(U.GetAllOrder());
			//var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(U.GetAllOrder()));
			//Console.ReadLine();


			// 주문하기
			//Console.WriteLine(U.MakeOrder("KRW-BTC", UpbitAPI.UpbitOrderSide.bid, 0.002m, 956000));
			//for (int i = 0; i < 2; i++)
			//{
			//	Console.WriteLine(U.MakeOrder(coin, UpbitAPI.UpbitOrderSide.bid, 2.0m, 300 - i));
			//	Thread.Sleep(333);
			//}

			//과하게 주문하는 것인데 주의
			//for (int i = 0; i < 1; i++)
			//{
			//	Console.WriteLine(U.MakeOrder("KRW-TT", UpbitAPI.UpbitOrderSide.bid, 0.00008350m, 6000000));
			//	Thread.Sleep(1333);
			//}

			//랜덤 주문하기... 주문넣기...
			string coinCode = "KRW-SOLVE";
			decimal coinPrice = 440*3;
			int max = 950;
			int min = 502;
			int RAND_DOUBLE = 5;// r + 4

			Console.WriteLine("랜덤 주문넣어 > " + msg + ":" + coinCode);
			int randDouble = RAND_DOUBLE;
			Random moneyRandom = new Random();
			for (int i = 0; i < 10000; i++)
			{
				decimal money = moneyRandom.Next(min, max);
				decimal coinCount = money / coinPrice;
				randDouble--;
				if (randDouble < 0)
				{
					randDouble = moneyRandom.Next(RAND_DOUBLE, RAND_DOUBLE + 4);
					coinCount = coinCount * 2;
				}
				Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.bid, coinCount, coinPrice));

				int time = 1844;
				//time = moneyRandom.Next(2000, 5000);
				Thread.Sleep(time);
			}



			////특정코인만 주문취소하기...
			//string coinCode = "KRW-TT";
			//string orderkind = "bid";//bid : 매수, ask : 매도
			////decimal coinPrice = 24.8m;
			//var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(U.GetAllOrder()));
			//foreach (var o in orders as object[])
			//{
			//	Dictionary<object, object> d = o as Dictionary<object, object>;
			//	if ((string)d["market"] == coinCode && (string)d["side"] == orderkind)
			//	{
			//		Console.WriteLine(U.CancelOrder((string)d["uuid"]));
			//	}
			//}

			////////와리가리 (IOST,ADRDR 들어가지마라)
			//string coinCode = "KRW-IOST";
			//decimal COIN_PRICE = 15.2m;
			//decimal COIN_INVTERVAL = 0.2m;
			//string orderkind = "bid";//bid : 매수, ask : 매도
			//decimal coinPriceUp = COIN_PRICE + COIN_INVTERVAL;
			//decimal coinPriceDown = COIN_PRICE - COIN_INVTERVAL;
			//decimal coinCountUp = 501m / coinPriceUp;
			//decimal coinCountDown = 501m / coinPriceDown;

			//for (int i = 0; i < 1000; i++)
			//{
			//	Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.ask, coinCountDown, coinPriceDown));//판매
			//	Thread.Sleep(10000);
			//	Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.bid, coinCountUp, coinPriceUp));//구입
			//	Thread.Sleep(10000);
			//}


			// 주문 취소
			//Console.WriteLine(U.CancelOrder("주문 uuid"));
			#endregion

			#region 시세 정보
			// 마켓 코드 조회
			//Console.WriteLine(U.GetMarkets());

			// 캔들(분, 일, 주, 월) 조회
			//Console.WriteLine(U.GetCandles_Minute("KRW-BTC", UpbitAPI.UpbitMinuteCandleType._1, to: DateTime.Now.AddMinutes(-2), count: 2));
			//Console.WriteLine(U.GetCandles_Day("KRW-BTC", to: DateTime.Now.AddDays(-2), count: 2));
			//Console.WriteLine(U.GetCandles_Week("KRW-BTC", to: DateTime.Now.AddDays(-14), count: 2));
			//Console.WriteLine(U.GetCandles_Month("KRW-BTC", to: DateTime.Now.AddMonths(-2), count: 2));

			// 당일 체결 내역 조회
			//Console.WriteLine(U.GetTicks("KRW-BTC", count: 2));

			// 현재가 정보 조회
			//Console.WriteLine(U.GetTicker("KRW-BTC,KRW-ETH"));

			// 시세 호가 정보(Orderbook) 조회
			//Console.WriteLine(U.GetOrderbook("KRW-BTC,KRW-ETH"));
			#endregion

			Console.ReadLine();

		}
	}
}
