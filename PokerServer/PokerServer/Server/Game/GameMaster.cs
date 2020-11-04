using PokerServer.Abstrcat;
using PokerServer.Client;
using PokerServer.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PokerServer.Interface;
using PokerServer.Server.Game;

namespace PokerServer.Server
{
	class GameMaster
	{
		private List<AbstractClient> clientObjects;
		private FindCombinationCard findCombinationCard;
	
		public GameMaster(List<AbstractClient> clientObjects)
		{
			this.clientObjects =clientObjects;
			findCombinationCard = new FindCombinationCard();
		}

		private RandomCard randomCart;
		public async Task NewRaund()
		{
			randomCart = new RandomCard();

			clientObjects.ForEach(i => i.cartInHand = new List<string>());
			GivingCardsForUser();
			
		}

		public string TakeOneCard()
		{

			return randomCart.TakeCard();

		}

		public List<AbstractClient> EndRaunt()
		{
			for (int i = 0; i != clientObjects.Count(); i++)
			{
				List<int> infoAboutHandClien = findCombinationCard.Start(clientObjects[i].cartInHand);
				clientObjects[i].numCombination = infoAboutHandClien[0];
				clientObjects[i].maxCardInCombination = infoAboutHandClien[1];

			}
			return getViners();
		}

		private List<AbstractClient> getViners()
		{
			List<AbstractClient> idVinUsers = new List<AbstractClient>();
			int maxCombination = 0; ;
			
			for (int i = 0; i != clientObjects.Count(); i++)
			{
				
				if(maxCombination<clientObjects[i].numCombination)
				{
					idVinUsers.Clear();
					maxCombination = clientObjects[i].numCombination;
					idVinUsers.Add(clientObjects[i]);

				}
				else if (maxCombination == clientObjects[i].numCombination)
				{
					if(clientObjects[i].maxCardInCombination>idVinUsers.First().maxCardInCombination)
					{
						idVinUsers.Clear();
						idVinUsers.Add(clientObjects[i]);
					}
					else if (clientObjects[i].maxCardInCombination == idVinUsers.First().maxCardInCombination)
					{
						idVinUsers.Add(clientObjects[i]);
					}
				}
			}
			return idVinUsers;
		}

		private async Task GivingCardsForUser()
		{
			await Task.Factory.StartNew(() => {
				for (int i = 0; i != clientObjects.Count; i++)
				{
					for (int i1 = 0; i != 2; i++)
					{
						clientObjects[i].cartInHand.Add(randomCart.TakeCard());
						clientObjects[i].networkStream.Write(Encoding.UTF8.GetBytes(clientObjects[i].cartInHand[i1] + Environment.NewLine)) ;
					}
				}
			});
		}
		


	}
}
