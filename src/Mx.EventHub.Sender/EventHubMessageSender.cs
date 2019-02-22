
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
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
			await _eventHubClient.SendAsync(messages.ToEventData()).ConfigureAwait(false);
		}
	}
}
