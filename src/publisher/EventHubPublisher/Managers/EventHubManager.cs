

using System.Collections.Generic;
using System.Threading.Tasks;
using Mx.EventHub.Sender;
using Mx.EventHub.Sender.Models;

namespace EventHubPublisher.Managers
{
	public class EventHubManager : IEventHubManager
	{
		private readonly IEventHubMessageSender _sender;
		public EventHubManager(IEventHubMessageSender sender)
		{
			_sender = sender;
		}

		public async Task SendEventAsync(EventMessageModel messageModel)
		{
			await _sender.SendAsync(messageModel).ConfigureAwait(false);
		}

		public async Task SendEventsAsync(IEnumerable<EventMessageModel> messages)
		{
			await _sender.SendAsync(messages).ConfigureAwait(false);
		}
	}
}
