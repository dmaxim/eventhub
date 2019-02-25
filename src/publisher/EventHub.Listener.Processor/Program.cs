using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;

namespace EventHub.Listener.Processor
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
			var storageConnectionString = configuration["EventHub:StorageConnectionString"];
			var consumerGroup = configuration["EventHub:ConsumerGroup"];
			var leaseContainerName = configuration["EventHub:StorageContainerName"];
			var eventHubName = configuration["EventHub:Name"];


			Console.WriteLine($"Register the {nameof(TestEventProcessor)}");

			var eventProcessorHost = new EventProcessorHost(eventHubName, consumerGroup, connectionString,
				storageConnectionString, leaseContainerName);

			await eventProcessorHost.RegisterEventProcessorAsync<TestEventProcessor>().ConfigureAwait(false);

			Console.WriteLine("Waiting for incoming events ...");
			Console.WriteLine("Press any key to shutdown");
			Console.ReadLine();

			await eventProcessorHost.UnregisterEventProcessorAsync().ConfigureAwait(false);

		}
	}
}
