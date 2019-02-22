
using System.Threading.Tasks;
using Mx.EventHub.Sender.Models;

namespace Mx.EventHub.Sender
{
	public class EventHubMessageSender : IEventHubMessageSender
	{
		private readonly EventHubConfiguration _configuration;
		public EventHubMessageSender(EventHubConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task SendAsync(EventMessageModel message)
		{
			throw new System.NotImplementedException();
		}
	}
}
