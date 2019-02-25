using System.Collections.Generic;
using System.Threading.Tasks;
using Mx.EventHub.Model;

namespace EventHubPublisher.Managers
{
	public interface IEventHubManager
	{

		Task SendEventAsync(EventMessageModel messageModel);

		Task SendEventsAsync(IEnumerable<EventMessageModel> messages);
	}
}
