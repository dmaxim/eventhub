using System.Collections.Generic;
using System.Threading.Tasks;
using Mx.EventHub.Sender.Models;

namespace Mx.EventHub.Sender
{
	public interface IEventHubMessageSender
	{
		Task SendAsync(EventMessageModel message);

		Task SendAsync(IEnumerable<EventMessageModel> messages);
	}
}
