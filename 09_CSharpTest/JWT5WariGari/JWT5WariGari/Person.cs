using System;
using System.Collections.Generic;
using System.Text;

namespace JWT5WariGari
{
	public enum OrderKind { AllCancel, BuyCancel, SellCancel, AllSell }

	public struct OrderBook
	{
		public decimal ask_price;
		public decimal ask_size;
		public decimal bid_price;
		public decimal bid_size;

		//매도...
		public void SetASK(decimal _ask_size, decimal _ask_price)
		{
			ask_price	= _ask_price;
			ask_size	= _ask_size;
		}
		//매수
		public void SetBID(decimal _bid_size, decimal _bid_price)
		{
			bid_price	= _bid_price;
			bid_size	= _bid_size;
		}

		public override string ToString()
		{
			return ask_price + ":" + ask_size + " === " + bid_price + ":" + bid_size;
		}
	}

	public class Person
	{
	}
}
