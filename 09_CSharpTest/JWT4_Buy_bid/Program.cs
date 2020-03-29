using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpbitAPI_CS_Wrapper;

namespace JWT4_Buy_bid
{
	public class Program
	{
		static void Main(string[] args)
		{
			Program _p = new Program();
			_p.RunUpbit();
		}

		//영선
		string akey = "72yDxrSDYpSF2HT4w02WsSoyfkLCsmT2BpM049Mr";
		string skey = "a9RoMMP6wdD49rinx6yN5MqRgR7vVxCwFYUifgDO";
		string msg = "영선";

		//엄마 
		//string akey = "wcwwTczlqYcYUWgM02sbA9lN2zRTwgp6zLbIbHQK";
		//string skey = "Af6ItyLopXFthoUoZTWkxH4D3ueHf9eSpj695z1t";
		//string msg = "엄마";

		decimal moneyUse, moneyLock;
		string sysmsg;
		string uuid;
		void RunUpbit()
		{
			UpbitAPI U = new UpbitAPI(akey, skey);
			Person person = new Person();

			#region 자산 조회
			//Console.WriteLine("\n\n\n================");
			//Console.WriteLine("개인:"+ U.GetAccount());
			//var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(U.GetAccount()));
			//foreach (var o in orders as object[])
			//{
			//	Dictionary<object, object> d = o as Dictionary<object, object>;
			//	//Console.WriteLine((string)d["currency"] + ":" + (string)d["balance"] + ":" + (string)d["locked"]);
			//	if ((string)d["currency"] == "KRW")
			//	{
			//		moneyUse	= GetDicimal(d["balance"]);
			//		moneyLock	= GetDicimal(d["locked"]);
			//		print(moneyUse);
			//	}
			//}
			//return;
			#endregion

			#region 주문 가능 정보 
			// 주문 가능 정보
			//Console.WriteLine("\n\n\n================");
			//Console.WriteLine("KRW-BTC:" + U.GetOrderChance("KRW-BTC"));

			// 개별 주문 조회
			//Console.WriteLine(U.GetOrder("주문 uuid"));

			// 주문 리스트 조회
			//Console.WriteLine(U.GetAllOrder());
			//var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(U.GetAllOrder()));
			//Console.ReadLine();
			#endregion


			#region 매수주문하기
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
			////}
			#endregion


			#region 단타칠때 랜덤 주문하기... 주문넣기... ( 연속 주문넣기 )
			//string coinCode = "KRW-CPT";
			//decimal coinPrice = 900m;
			//int max = 2050;
			//int min = 502;
			//int RAND_DOUBLE = 5;// r + 4
			//int randDouble = RAND_DOUBLE;
			//Random moneyRandom = new Random();
			//for (int i = 0; i < 10000; i++)
			//{
			//	decimal money = moneyRandom.Next(min, max);
			//	decimal coinCount = money / coinPrice;
			//	randDouble--;
			//	if (randDouble < 0)
			//	{
			//		randDouble = moneyRandom.Next(RAND_DOUBLE, RAND_DOUBLE + 4);
			//		coinCount = coinCount * 2;
			//	}
			//	Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.bid, coinCount, coinPrice));
			//	int time = 1844;
			//	//time = moneyRandom.Next(3000, 9000);
			//	Thread.Sleep(time);
			//}
			#endregion


			#region 랜덤 주문하기... 키에따라 동작
			//string coinCode = "KRW-COSM";
			//decimal coinPrice = 39.0m;
			//int max = 950;
			//int min = 502;
			//int RAND_DOUBLE = 5;// r + 4

			//int randDouble = RAND_DOUBLE;
			//Random moneyRandom = new Random();
			//ConsoleKeyInfo keys;

			//OrderSell(U, coinCode);

			//for (int i = 0; i < 110; i++)
			//{
			//	decimal money = moneyRandom.Next(min, max);
			//	decimal coinCount = money / coinPrice;
			//	randDouble--;
			//	if (randDouble < 0)
			//	{
			//		randDouble = moneyRandom.Next(RAND_DOUBLE, RAND_DOUBLE + 4);
			//		coinCount = coinCount * 2;
			//	}
			//	Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.bid, coinCount, coinPrice));

			//	//키입력을 받으면 나간다.
			//	if (Console.KeyAvailable)
			//	{
			//		keys = Console.ReadKey(true);
			//		switch (keys.Key)
			//		{
			//			case ConsoleKey.S:
			//			case ConsoleKey.UpArrow:
			//				Console.WriteLine("==> SellCancel");
			//				OrderCancel(U, coinCode, OrderKind.SellCancel);
			//				break;
			//			case ConsoleKey.D:
			//			case ConsoleKey.DownArrow:
			//				Console.WriteLine("==> BuyCancel");
			//				OrderCancel(U, coinCode, OrderKind.BuyCancel);
			//				break;
			//			//case ConsoleKey.Enter:
			//			case ConsoleKey.RightArrow:
			//			case ConsoleKey.LeftArrow:
			//				Console.WriteLine("==> AllCancel");
			//				OrderCancel(U, coinCode, OrderKind.AllCancel);
			//				break;
			//			case ConsoleKey.Enter:
			//				Console.WriteLine("==> AllSell");
			//				//OrderAllSell(U, coinCode, OrderKind.AllSell);
			//				break;
			//			default:
			//				Console.WriteLine("==> Not Change");
			//				break;
			//		}
			//		break;
			//	}

			//	int time = 1444;
			//	//time = moneyRandom.Next(3000, 9000);
			//	Thread.Sleep(time);
			//}
			#endregion


			#region 특정코인만 주문취소하기...
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
			#endregion

			#region 와리가리 (IOST,ADRDR 들어가지마라)
			//----------------------------------------------------
			////업글 > 코인이름만 넣으면 검색해서 자동으로 와리가리한다.
			//----------------------------------------------------
			//string coinCode			= "KRW-IQ";
			//decimal COIN_PRICE		= 6.12m;
			//decimal COIN_INVTERVAL	= GetPriceGap(COIN_PRICE) * 2m;
			//decimal bidPrice		= COIN_PRICE + COIN_INVTERVAL;
			//decimal askPrice		= COIN_PRICE - COIN_INVTERVAL;
			//decimal bidCount		= 501m / bidPrice;
			//decimal askCount		= 501m / askPrice;
			//for (int i = 0; i < 1000; i++)
			//{
			//	Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.ask, askCount, askPrice));	//판매
			//	Thread.Sleep(2000);
			//	Console.WriteLine(U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.bid, bidCount, bidPrice));	//구입
			//	Thread.Sleep(2000);
			//}
			#endregion


			#region 매도주문하기 (시작지점 위로) > 거미줄 걸기.
			//----------------------------------------------------
			// 매도주문하기 (시작지점 위로) > 거미줄 걸기.
			// KRW-CPT  / 9.64m 위쪽 방향으로  (+1) -> 50 개 / 호가당 4000m 원
			// KRW-IOST / 15.60m 위쪽 방향으로 (+1) -> 20 개 / 호가당 50000m 원
			// KRW-BAT  / 433m  위쪽 방향으로  (+1) -> 20 개 / 호가당 50000m 원
			// KRW-EOS  / 8275m  위쪽 방향으로  (+1) -> 5 개 / 호가당 501m 원
			//----------------------------------------------------
			//string coinCode = "KRW-CPT";
			//decimal COIN_PRICE_ASK = 9.60m + 0.3m * 0;
			//int COIN_HOGA_ASK = +1;
			//int ORDER_ASK_LOOP = 60;
			//decimal COIN_MONEY_ASK = 50000m;
			//decimal COIN_INVTERVAL_ASK = GetPriceGap(COIN_PRICE_ASK);
			//decimal askPrice = COIN_PRICE_ASK;
			//decimal askCount = COIN_MONEY_ASK / askPrice;
			//List<OrderBook> listASK = new List<OrderBook>();
			//OrderBook orderbook = new OrderBook();
			//msg += "\n==================================================\n"
			//			+ coinCode + " 매도 거미줄 걸기(" + (ORDER_ASK_LOOP * COIN_MONEY_ASK) + ") : " + COIN_PRICE_ASK + " > " + (COIN_PRICE_ASK + COIN_HOGA_ASK * ORDER_ASK_LOOP * COIN_INVTERVAL_ASK)
			//			+ "\n==================================================\n";
			//Console.WriteLine(msg);
			//for (int i = 0; i < ORDER_ASK_LOOP * COIN_HOGA_ASK; i++)
			//{
			//	if (i % Math.Abs(COIN_HOGA_ASK) == 0)
			//	{
			//		orderbook.SetASK(askCount, askPrice);
			//		listASK.Add(orderbook);
			//		Console.WriteLine(i + " > " + askPrice + ":" + askCount);
			//	}

			//	askPrice += COIN_INVTERVAL_ASK * Math.Sign(COIN_HOGA_ASK);
			//	askCount = COIN_MONEY_ASK / askPrice;
			//	COIN_INVTERVAL_ASK = GetPriceGap(askPrice);
			//}
			//Console.WriteLine(msg);
			//Console.WriteLine("Y 누르시면 반영합니다. 그외에는 적용이 안됩니다.");
			//if (Console.ReadKey().Key == ConsoleKey.Y)
			//{
			//	for (int i = 0; i < listASK.Count; i++)
			//	{
			//		//Console.WriteLine(i + " > " + listASK[i].ask_price + ":" + listASK[i].ask_size);
			//		sysmsg = U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.ask, listASK[i].ask_size, listASK[i].ask_price);
			//		Console.WriteLine(sysmsg);
			//		if (CheckSystemWaitMessage(sysmsg))
			//		{
			//			Thread.Sleep(1000);
			//		}
			//	}
			//}
			#endregion


			#region 매수주문하기 (시작지점 밑으로) > 거미줄 걸기.
			//----------------------------------------------------
			// 매수주문하기(시작지점 밑으로) > 거미줄 걸기.
			//KRW - CPT / 9.55m 아래 방향으로(-1) -> 50 개 / 호가당 4000m 원
			// KRW - CPT / 9.55m 아래 방향으로(-3) -> 50 개 / 호가당 4000m 원
			// KRW - IOST / 15.40m 아래 방향으로(-1) -> 15 개 / 호가당 20000m 원
			// KRW - BAT / 410m  아래 방향으로(-2) -> 50 개 / 호가당 10000m 원
			// KRW - EOS / 8205m  아래 방향으로(-2) ->80 개 / 호가당 2000m 원
			// KRW - COSM / 41.20m  아래 방향으로(-1) ->20 개 / 호가당 200000m 원
			////----------------------------------------------------
			string coinCode = "KRW-EOS";
			decimal COIN_PRICE_BID = 7620m - 20.0m * 0;
			int COIN_HOGA_BID = -1;
			int ORDER_BID_LOOP = 10;
			decimal COIN_MONEY_BID = 5000m;
			decimal COIN_INVTERVAL_BID = GetPriceGap(COIN_PRICE_BID);
			decimal bidPrice = COIN_PRICE_BID;
			decimal bidCount = COIN_MONEY_BID / bidPrice;
			List<OrderBook> listBID = new List<OrderBook>();
			OrderBook orderbook = new OrderBook();
			msg += "\n==================================================\n"
						+ coinCode + " 매수 거미줄 걸기(" + (ORDER_BID_LOOP * COIN_MONEY_BID) + ") : " + COIN_PRICE_BID + " > " + (COIN_PRICE_BID + COIN_HOGA_BID * ORDER_BID_LOOP * COIN_INVTERVAL_BID)
						+ "\n==================================================\n";
			Console.WriteLine(msg);
			for (int i = 0, j = 0; i < ORDER_BID_LOOP  * Math.Abs(COIN_HOGA_BID); i++)
			{
				if (i % Math.Abs(COIN_HOGA_BID) == 0)
				{
					orderbook.SetBID(bidCount, bidPrice);
					listBID.Add(orderbook);
					Console.WriteLine(j++ + " > " + bidPrice + ":" + bidCount);
				}

				bidPrice += COIN_INVTERVAL_BID * Math.Sign(COIN_HOGA_BID);
				bidCount = COIN_MONEY_BID / bidPrice;
				COIN_INVTERVAL_BID = GetPriceGap(bidPrice);
			}
			Console.WriteLine(msg);
			Console.WriteLine("Y 누르시면 반영합니다. 그외에는 적용이 안됩니다.");
			if (Console.ReadKey().Key == ConsoleKey.Y)
			{
				for (int i = 0; i < listBID.Count; i++)
				{
					Console.WriteLine(i + " > " + listBID[i].bid_price + ":" + listBID[i].bid_size);
					sysmsg = U.MakeOrder(coinCode, UpbitAPI.UpbitOrderSide.bid, listBID[i].bid_size, listBID[i].bid_price);
					Console.WriteLine(sysmsg);
					if (CheckSystemWaitMessage(sysmsg))
					{
						Thread.Sleep(1000);
					}
				}
			}
			#endregion


			#region 주문 취소
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


			Console.WriteLine(" ---> Program End");
			Console.ReadLine();

		}


		//내가 보유한 코인중에서 (한화, 코인) 수량확보하기...
		//Console.WriteLine("KRW:"			+ GetMyInfo(_U, "KRW"));
		//Console.WriteLine(_coinCode+ ":"	+ GetMyInfo(_U, _coinCode));
		public void OrderSell(UpbitAPI _U, string _coinCode)
		{
			//내가 가지고 있는 수량을 먼저 확인...
			decimal _coinCount = GetMyInfo(_U, _coinCode);
			//Console.WriteLine(_coinCode + ":" + _coinCount);

			//현재 코인의 가격을 확인...
			GetOrderbook(_U, _coinCode);

			//판매하기...
			//Console.WriteLine(_U.MakeOrder(_coinCode, UpbitAPI.UpbitOrderSide.ask, coinCountDown, coinPriceDown));//판매
		}

		public decimal GetPriceGap(decimal _coinPrice)
		{
			decimal _gap = 0m;
			if (_coinPrice >= 2000000m)
				_gap = 1000m;
			else if (_coinPrice >= 1000000m)
				_gap = 500m;
			else if (_coinPrice >= 500000m)
				_gap = 100m;
			else if (_coinPrice >= 100000m)
				_gap = 50m;
			else if (_coinPrice >= 10000m)
				_gap = 10m;
			else if (_coinPrice >= 1000m)
				_gap = 5m;
			else if (_coinPrice >= 100m)
				_gap = 1m;
			else if (_coinPrice >= 10m)
				_gap = 0.1m;
			else if (_coinPrice >= 0m)
				_gap = 0.01m;

			//Console.WriteLine(_coinPrice + " > " + _gap);
			return _gap;
		}

		//U
		//KRW		> KRW
		//KRW-COSM	> COSM
		public decimal GetMyInfo(UpbitAPI _U, string _coinCode)
		{
			string _currency = _coinCode;
			string _headerWord = "KRW-";
			int _headLenght = _headerWord.Length;
			//Console.WriteLine(_currency + " > " + _currency.IndexOf("KRW-") + ":" + _currency.Length);
			if (_currency.IndexOf("KRW-") >= 0)
			{
				_currency = _currency.Substring(_headLenght, _currency.Length - _headLenght);
			}
			//Console.WriteLine(_currency + " > " + _currency.IndexOf("KRW-"));

			//Console.WriteLine("\n\n\n================");
			//Console.WriteLine("개인:"+ _U.GetAccount());
			var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(_U.GetAccount()));
			foreach (var o in orders as object[])
			{
				Dictionary<object, object> d = o as Dictionary<object, object>;
				//Console.WriteLine((string)d["currency"] + ":" + (string)d["balance"] + ":" + (string)d["locked"]);
				if ((string)d["currency"] == _currency)
				{
					return GetDicimal(d["balance"]);
				}
			}
			return 0m;
		}

		public void GetOrderbook(UpbitAPI _U, string _coinCode)
		{
			// 시세 호가 정보(Orderbook) 조회
			//Console.WriteLine(_U.GetOrderbook("KRW-COSM"));
			var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(_U.GetOrderbook(_coinCode)));
			foreach (var o in orders as object[])
			{
				Dictionary<object, object> d = o as Dictionary<object, object>;
				Console.WriteLine(d["market"]);
				Console.WriteLine(d["orderbook_units"]);
				foreach (var o2 in d["orderbook_units"] as object[])
				{
					Dictionary<object, object> d2 = o2 as Dictionary<object, object>;
					Console.WriteLine(d2["ask_price"]);
					Console.WriteLine(d2["bid_price"]);
					Console.WriteLine(d2["ask_size"]);
					Console.WriteLine(d2["bid_size"]);

				}
				//if ((string)d["ask_price"] == _coinCode && (string)d["side"] == "bid")
				//{
				//	Console.WriteLine(_U.CancelOrder((string)d["uuid"]));
				//}
			}


		}

		public void OrderCancel(UpbitAPI _U, string _coinCode, OrderKind _orderKind = OrderKind.BuyCancel)
		{
			//특정코인만 주문취소하기...
			//string coinCode = "KRW-TT";
			//string orderkind = "bid";//bid : 매수, ask : 매도

			var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(_U.GetAllOrder()));
			switch (_orderKind)
			{
				case OrderKind.BuyCancel:
					foreach (var o in orders as object[])
					{
						Dictionary<object, object> d = o as Dictionary<object, object>;
						if ((string)d["market"] == _coinCode && (string)d["side"] == "bid")
						{
							Console.WriteLine(_U.CancelOrder((string)d["uuid"]));
						}
					}
					break;
				case OrderKind.SellCancel:
					foreach (var o in orders as object[])
					{
						Dictionary<object, object> d = o as Dictionary<object, object>;
						if ((string)d["market"] == _coinCode && (string)d["side"] == "ask")
						{
							Console.WriteLine(_U.CancelOrder((string)d["uuid"]));
						}
					}
					break;
				case OrderKind.AllCancel:
					foreach (var o in orders as object[])
					{
						Dictionary<object, object> d = o as Dictionary<object, object>;
						if ((string)d["market"] == _coinCode)
						{
							Console.WriteLine(_U.CancelOrder((string)d["uuid"]));
						}
					}
					break;
			}
		}

		#region Utility
		//시스템에 요청을 하면 일정량  이상이되면 과부하 메세지가 날아온다...
		//too_many_requests > 이메세지를 받으면 그때 시간을 약간 홀딩 해주면 된다.
		public bool CheckSystemWaitMessage(string _sysmsg)
		{
			bool _rtn = false;
			//Console.WriteLine(_sysmsg);
			if ((_sysmsg.IndexOf("error") >= 0 && _sysmsg.IndexOf("too_many_requests") >= 0)
				|| (_sysmsg.IndexOf("error") >= 0 && _sysmsg.IndexOf("too_many_request_order") >= 0))
			{
				Console.WriteLine(" > wait");
				_rtn = true;
			}
			//{ "error":{ "message":"Too many API requests.","name":"too_many_requests"} }
			//var orders = MessagePackSerializer.Typeless.Deserialize(MessagePackSerializer.FromJson(_sysmsg));
			//foreach (var o in orders as object[])
			//{
			//	Dictionary<object, object> d = o as Dictionary<object, object>;
			//	if ((string)d["currency"] == "KRW")
			//	{
			//		_rtn = true;
			//	}
			//}
			return _rtn;
		}

		public decimal GetDicimal(object _o)
		{
			return Decimal.Parse((string)_o);
		}

		public void print(decimal _str)
		{
			Console.WriteLine(_str);
		}

		public void print(string _str)
		{
			Console.WriteLine(_str);
		}
		#endregion
	}
}
