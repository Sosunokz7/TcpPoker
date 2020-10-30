using PokerServer.Abstrcat;
using PokerServer.Client;
using PokerServer.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PokerServer.Interface;

namespace PokerServer.Server
{
	class GameMaster
	{
		private List<AbstractClient> clientObjects;
		private int quantityRound = 0;
		public GameMaster(List<AbstractClient> clientObjects)
		{
			this.clientObjects =clientObjects;
			
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
			quantityRound++;
			if(quantityRound<5)
				return randomCart.TakeCard();
			return null;
		}

		private async Task GivingCardsForUser()
		{
			await Task.Factory.StartNew(() => {
				for (int i = 0; i != clientObjects.Count; i++)
				{
					for (int i1 = 0; i != 2; i++)
					{
						clientObjects[i].cartInHand.Add(randomCart.TakeCard());
						clientObjects[i].networkStream.Write(Encoding.UTF8.GetBytes(clientObjects[i].cartInHand[i1]));
					}
				}
			});
		}
		


	}
}
