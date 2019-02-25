
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Mx.EventHub.Model;
using Newtonsoft.Json;

namespace EventHubListener
{
	public class TestEventHubPartitionReceiveHandler : IPartitionReceiveHandler
	{
		private readonly string _partitionId;
		public TestEventHubPartitionReceiveHandler(string partitionId, int maxBatchSize)
		{
			_partitionId = partitionId;
			MaxBatchSize = maxBatchSize;
		}

		public Task ProcessEventsAsync(IEnumerable<EventData> events)
		{
			if (events != null)
			{
				foreach (var eventData in events)
				{
					var receivedMessage = Encoding.UTF8.GetString(eventData.Body.Array);
					var eventMessage = JsonConvert.DeserializeObject<EventMessageModel>(receivedMessage);
					Console.WriteLine($"{eventMessage.DisplayFormat} : PartitionId: {_partitionId}");
				}
			}
			return Task.CompletedTask;
		}

		public Task ProcessErrorAsync(Exception error)
		{
			Console.WriteLine($"Exception: {error.Message}");
			return Task.CompletedTask;
		}

		public int MaxBatchSize { get; set; }


	}
}
