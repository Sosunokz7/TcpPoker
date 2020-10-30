using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using PokerServer.Client;
using System.Threading;
using PokerServer.Core;
using System.Linq;
using System.Threading.Tasks;
using PokerServer.Server;
using PokerServer.Abstrcat;
using PokerServer.Interface;

namespace PokerServer
{
	class ServerObject
	{

		private TcpListener tcpListener;
		private  string ip="127.0.0.1";
		private  int port=8080;
		private int readinessPlayers = 0;
		private List<ClientObject> clientsList = new List<ClientObject>();
		private GameMaster roundControler;
		public InterpreterMessage interpreter;
		public ServerObject()
		{
			interpreter = new InterpreterMessage();
			ChangeSettings();
			roundControler = new GameMaster(clientsList.ConvertAll((obj)=>obj as AbstractClient));
				
		}

		internal void Listener()
		{
			try
			{
				
				tcpListener = new TcpListener(IPAddress.Parse(ip),port);
				tcpListener.Start();
				while(true)
				{
					TcpClient tcpClient = tcpListener.AcceptTcpClient();
					ClientObject clientObject = new ClientObject(tcpClient,this);
					clientObject.eventGetMessage += EveInterpretate;
					Thread thread = new Thread(() => { clientObject.GetMessage(); });
					
					thread.Start();
				}

			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		private string EveInterpretate(string message, ClientObject client)
		{
			try
			{
				string teg = message.Substring(0, message.IndexOf(';'));
				message = message.Remove(0, message.IndexOf(';') + 1);
				switch (teg)
				{

					case "Message"://Send message for user
						{
							return message;

						}
					case "Readiness"://Readiness player 
						{
							client.readiness = !client.readiness;
							if (client.readiness)
								++readinessPlayers;
							else
								--readinessPlayers;
							Play();
							break;
						}
					case "Rate"://Get rate
						{
							
							double rat = Math.Round(double.Parse(message), 2);
							client.rate += rat;
							client.many -= rat;
							if (clientsList.Last().id == client.id)
								NextRound();
							break;
						}

				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return null;
		}

		internal void NotifyAllUser(string message)
		{
			clientsList.ForEach(i=>i.SendMessage(message));
		}
		
		
		internal void RemoveClientFromList(string id)
		{
			Task.Factory.StartNew(() => { clientsList.RemoveAll((obj) => obj.id == id); });
			
		}
		internal void AddClient(ClientObject clientObject)
		{
			clientsList.Add(clientObject);
		}
		private void ChangeSettings()
		{
			try
			{
				string message;
				Console.WriteLine("Do you want chenge ip ?");
				message = Console.ReadLine();
				if (message != string.Empty)
					ip = message;
				Console.WriteLine("Do you want chenge port ?");
				message = Console.ReadLine();
				if (message != string.Empty)
					port = int.Parse(message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		internal void ClouseServer()
		{
			try
			{
				clientsList.ForEach(i => i.ClouseClient());
				tcpListener.Stop();
				Console.WriteLine("Сервер успешно завершился");
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			
		}

		internal void Play()
		{
			if (readinessPlayers == clientsList.Count)
			{
				roundControler.NewRaund();
				for (int i = 0; i != 3; i++)
				{
					NotifyAllUser(roundControler.TakeOneCard());
				}
			}

		}

		internal void NextRound()
		{
			NotifyAllUser(roundControler.TakeOneCard());
		}

	}
}
