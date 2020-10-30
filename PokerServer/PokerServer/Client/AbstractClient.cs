using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace PokerServer.Abstrcat
{
	abstract class AbstractClient
	{
		internal string id { get;  private set; }=Guid.NewGuid().ToString();
		internal string name { get; set; }

		private double _many = 500d;
		internal double many { get { return Math.Round(_many, 2); }  set { _many += value; } }

		private double _rate = 0d;
		internal double rate { get { return Math.Round(_rate, 2); } set { _rate += value; } }



		internal List<string> cartInHand;
		internal NetworkStream networkStream { get; private set; }

		private ServerObject serverObject;

		internal abstract string GetMessage();
		internal abstract void SendMessage(string message);
		
	}
}
