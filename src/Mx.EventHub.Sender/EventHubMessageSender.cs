using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Mx.EventHub.Model;
using Mx.EventHub.Sender.Models;

namespace Mx.EventHub.Sender
{
	public class EventHubMessageSender : IEventHubMessageSender
	{
		
		private readonly EventHubClient _eventHubClient;
		public EventHubMessageSender(EventHubConfiguration configuration)
		{
			_eventHubClient = EventHubClient.CreateFromConnectionString(configuration.ConnectionString);
		}

		public async Task SendAsync(EventMessageModel message)
		{
			await _eventHubClient.SendAsync(new EventData(message.ToBytes())).ConfigureAwait(false);
		}

		public async Task SendAsync(IEnumerable<EventMessageModel> messages)
		{
			var eventDataBatch = _eventHubClient.CreateBatch();
			var eventData = messages.ToEventData();

			foreach (var data in eventData)
			{
				if(!eventDataBatch.TryAdd(data))
				{
					await SendBatchAsync(eventDataBatch);
					eventDataBatch = _eventHubClient.CreateBatch();
					eventDataBatch.TryAdd(data);
				}
			}

			if (eventDataBatch.Count > 0)
			{
				await SendBatchAsync(eventDataBatch);
			}
			
		}

		private async Task SendBatchAsync(EventDataBatch batch)
		{
			await _eventHubClient.SendAsync(batch).ConfigureAwait(false);
		}
	}



}
