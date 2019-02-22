using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;

namespace EventHubListener
{
	public class Program
	{
		public static async Task Main(string[] args)
		{

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();


			var configuration = builder.Build();

			var connectionString = configuration["EventHub:ListenConnectionString"];

			Console.WriteLine("Connecting to the Event Hub...");
			var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);


			var partitionReceiver = eventHubClient.CreateReceiver("$Default", "0", EventPosition.FromStart());

			Console.WriteLine("Waiting for incoming events ....");
			while (true)
			{
				var messages = await partitionReceiver.ReceiveAsync(5).ConfigureAwait(false);

				if (messages != null)
				{
					foreach (var eventData in messages)
					{
						var receivedMessage = Encoding.UTF8.GetString(eventData.Body.Array);
						Console.WriteLine($"{receivedMessage} : PartitionId: {partitionReceiver.PartitionId}");
					}
				}
			}

		}

	
	}
}
