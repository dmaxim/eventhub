
using System;

namespace EventHubPublisher.Models
{
	public class NewEventModel
	{
		public string Message { get; set; }

		public int EventId { get; set; }
		
		public Guid CorrelationId { get; set; }

		public DateTime EventTime { get; set; }
	}
}
