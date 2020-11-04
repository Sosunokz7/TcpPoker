using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerServer.Server.Game
{
	class FindCombinationCard
	{

		private enum allCOmbinadtion : int
		{
			bigCard = 1,
			onePar,
			twoPar,
			threeKidn,
			straight,
			flush,
			fullHouse,
			fourKind,
			straightFlush,
			royalFlush,
		}

		private Dictionary<int, int> maxCombination = new Dictionary<int, int>();

		private List<string> allCardz = new List<string>();


		public List<int> Start(List<string> allCard)
		{
			this.allCardz = allCard;
			allCardz = allCardz.OrderByDescending(or => or.Substring(2)).ToList();
			maxCombination.Add((int)allCOmbinadtion.bigCard, int.Parse(allCardz.First().Substring(2)));
			findColorCard();
			int key = maxCombination.Keys.Max();
			return new List<int>() { key, maxCombination[key] };
			Console.WriteLine(string.Join(Environment.NewLine, maxCombination));

		}

		private void findStric(List<string> cards = null)
		{
			int strick = 0;

			if (cards == null)
				cards = allCardz;

			for (int i = 0; i != cards.Count() - 1; i++)
			{
				if (int.Parse(cards[i].Substring(2)) <= int.Parse(cards[i + 1].Substring(2)) + 1)
				{
					++strick;
					if (strick == 4)
					{
						if (cards.Count() > 4)
						{
							if (cards.First().Substring(2) == "13")
							{
								maxCombination.Add((int)allCOmbinadtion.royalFlush, 13);
								return;
							}
							maxCombination.Add((int)allCOmbinadtion.straightFlush, int.Parse(cards.First().Substring(2)));
							return;
						}
						maxCombination.Add((int)allCOmbinadtion.straight, int.Parse(cards.First().Substring(2)));
						break;
					}
				}
				else
				{
					strick = 0;
					cards[0] = cards[i + 1];
				}
			}

			findDoublicate();
		}

		private void findDoublicate()
		{
			var arr = allCardz
				.GroupBy(gr => gr.Substring(2)).Where(g => g.Count() > 1)
				.OrderByDescending(or => or.Count())
				.ThenByDescending(or => int.Parse(or.Key))
				.ToDictionary(x => x.Key, y => y.Count());
			if (arr.Count == 0)
				return;

			int firstElement = int.Parse(arr.Keys.First());

			switch (arr.Values.First())
			{
				case 4:
					maxCombination.Add((int)allCOmbinadtion.fourKind, firstElement);
					return;
				case 3:
					maxCombination.Add((int)allCOmbinadtion.threeKidn, firstElement);
					break;
				case 2:
					maxCombination.Add((int)allCOmbinadtion.onePar, firstElement);
					break;
			}

			if (arr.Count > 1)
				switch (arr.Values.First() + arr.Values.Take(1).First())
				{
					case 4:
						maxCombination.Add((int)allCOmbinadtion.twoPar, firstElement);
						break;

					case int x when (x > 4):
						maxCombination.Add((int)allCOmbinadtion.fullHouse, firstElement);
						break;
				}

		}
		private void findColorCard()
		{

			var colorCard = allCardz.Where(x => allCardz.Count(el => el[0] == x[0]) > 4).OrderByDescending(ord => (int.Parse(ord.Substring(2)))).ToList();
			if (colorCard.Count > 4)
			{
				maxCombination.Add((int)allCOmbinadtion.flush, int.Parse(colorCard.First().Substring(2)));
				findStric(colorCard.GetRange(0, 5));
				return;
			}
			findStric();

		}

		
	}

}
