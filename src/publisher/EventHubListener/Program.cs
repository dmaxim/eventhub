using System;
using System.IO;
using System.Linq;
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

			//await ReceiveFromSinglePartition(connectionString).ConfigureAwait(false);

			await ReceiveFromAll(connectionString).ConfigureAwait(false);

		}


		private static async Task ReceiveFromSinglePartition(string connectionString)
		{
			Console.WriteLine("Connecting to the Event Hub...");
			var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);

			//var partitionReceiver = eventHubClient.CreateReceiver("$Default", "0", EventPosition.FromStart());
			var partitionReceiver = eventHubClient.CreateReceiver("$Default", "0", EventPosition.FromEnqueuedTime(DateTime.Now));

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

		private static async Task ReceiveFromAll(string connectionString)
		{
			Console.WriteLine("Connecting to the Event Hub...");
			var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);

			var runtimeInformation = await eventHubClient.GetRuntimeInformationAsync().ConfigureAwait(false);

			//var partitionReceivers = runtimeInformation.PartitionIds.Select(partitionId =>
			//		eventHubClient.CreateReceiver("$Default", partitionId,
			//			EventPosition.FromStart())).ToList();

			var partitionReceivers = runtimeInformation.PartitionIds.Select(partitionId =>
				eventHubClient.CreateReceiver("event_handler_console_direct", partitionId,
					EventPosition.FromEnqueuedTime(DateTime.Now))).ToList();

			Console.WriteLine("Waiting for incoming events ....");

			foreach (var partitionReceiver in partitionReceivers)
			{
				partitionReceiver.SetReceiveHandler(
					new TestEventHubPartitionReceiveHandler(partitionReceiver.PartitionId, 10));
			}

			Console.WriteLine("Press any key to shutdown");
			Console.ReadLine();
			await eventHubClient.CloseAsync();
		}
	}
}
