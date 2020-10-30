using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace PokerServer.Interface
{
	interface IClient
	{
		 string id { get;  }
		 string name { get; set; }


		 double many { get; set; }

		 List<string> cartInHand { get; set; }
		 NetworkStream networkStream { get;   }

		  string GetMessage();
		  void SendMessage(string message);
	}
}
