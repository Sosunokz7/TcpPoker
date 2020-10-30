
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerServer.Core
{
	class RandomCard
	{
		public Random random = new Random();
		protected Dictionary<int, List<int>> aLlCarts;
		protected List<int> mastForCarts = new List<int>(4) { 1, 2, 3, 4 };//all key for dictonary

		protected List<string> cardOnTable;

		public RandomCard()
		{
			aLlCarts = new Dictionary<int, List<int>>();
			cardOnTable = new List<string>();
			for (int i = 1; i != 5; i++)
			{
				aLlCarts.Add(i, Enumerable.Range(1, 13).ToList());
			}
		}

		public  string TakeCard()
		{
			int nummMast = mastForCarts[random.Next(0, mastForCarts.Count())];
			
			int cart = aLlCarts[nummMast][random.Next(0, aLlCarts[nummMast].Count)];
			string result = $"{nummMast}_{cart}";

			aLlCarts[nummMast].Remove(cart);
			if (aLlCarts[nummMast].Count == 0)
				mastForCarts.Remove(nummMast);
			
			return result;
		}

		public  string TakeCard(int userId)
		{
			return null;
		}

		public  string TakeCardForTable()
		{
			string cart = TakeCard();
			cardOnTable.Add(cart);
			return cart;

		}


	}
}
