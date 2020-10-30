using PokerServer.Core;

using System;
using System.Collections.Generic;
using System.Linq;
namespace PokerServer
{
	class Program
	{
		List<int> ls = new List<int> { 1, 2, 3, 4 };
		List<int> ls1=new List<int>();
		static void Main(string[] args)
		{
			ServerObject serverObject = new ServerObject();
			try
			{
				serverObject.Listener();


			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				serverObject.ClouseServer();
			}
			Console.ReadLine();
		}
	

	}
}
