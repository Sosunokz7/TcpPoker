using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using PokerServer.Abstrcat;
using System.Threading.Tasks;

namespace PokerServer.Client
{
	class ClientObject : AbstractClient
	{

		public Func<string,ClientObject,string> eventGetMessage;
	
		public bool readiness { get; set; } = false;
		internal NetworkStream networkStream { get; private set; }

		private TcpClient tcpClient;
		private ServerObject serverObject;
		public ClientObject(TcpClient client,ServerObject serverObject)
		{
			this.tcpClient = client;
			this.serverObject = serverObject;
			this.networkStream = client.GetStream();
			this.name = GetMessage();
		}

		

		//Получение сообщений от клиентов
		internal override string GetMessage()
		{
			
			try
			{
				Task.Factory.StartNew(()=> {
					
					int bytes;
					byte[] data = new byte[256];
					StringBuilder stringBuilder = new StringBuilder();
					do
					{
						bytes = networkStream.Read(data, 0, data.Length);
						stringBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));
					} while (networkStream.DataAvailable);


					return eventGetMessage?.Invoke(stringBuilder.ToString(), this);
					
				});
				
				
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				ClouseClient();
				
			}
			return null;
			

		}

		internal override void SendMessage(string message)
		{
			try
			{
				byte[] data = new byte[256];
				data = Encoding.UTF8.GetBytes(message);
				networkStream.Write(data);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		internal void ClouseClient()
		{
			try
			{
				//serverObject.RemoveClientFromList(this.id);
				networkStream.Close();
				tcpClient.Close();

			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		

	}
}
