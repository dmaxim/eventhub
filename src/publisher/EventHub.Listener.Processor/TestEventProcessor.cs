
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Mx.EventHub.Model;
using Newtonsoft.Json;

namespace EventHub.Listener.Processor
{
	public class TestEventProcessor : IEventProcessor
	{
		public Task OpenAsync(PartitionContext context)
		{
			Console.WriteLine($"Initialized processor for partition {context.PartitionId}");
			return Task.CompletedTask;
		}

		public Task CloseAsync(PartitionContext context, CloseReason reason)
		{
			Console.WriteLine($"Shutting down processor for partition {context.PartitionId}. Reason: {reason}");
			return Task.CompletedTask;
		}

		public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
		{

			if (messages != null)
			{
				foreach (var eventData in messages)
				{
					var receivedMessage = Encoding.UTF8.GetString(eventData.Body.Array);
					var eventMessage = JsonConvert.DeserializeObject<EventMessageModel>(receivedMessage);
					Console.WriteLine(
						$"{eventMessage.DisplayFormat} : PartitionId: {context.PartitionId} | OffSet: {eventData.SystemProperties.Offset}");

					//eventData.SystemProperties.PartitionKey;
				}
			}

			return context.CheckpointAsync(); // Writes the offset in Azure Blob Storage
			

		}

		public Task ProcessErrorAsync(PartitionContext context, Exception error)
		{
			Console.WriteLine($"Error for partition {context.PartitionId} : {error.Message}");	
			return Task.CompletedTask;
		}
	}
}
