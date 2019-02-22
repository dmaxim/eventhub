using System;


namespace Mx.EventHub.Sender.Models
{
	public class EventMessageModel
	{
		public EventMessageModel(string message)
		{
			Message = message;
			EventId = DateTime.Now.Day + DateTime.Now.Minute;
			CorrelationId = Guid.NewGuid();
			EventTime = DateTime.Now;
			
		}
		public string Message { get; set; }

		public int EventId { get; set; }

		public Guid CorrelationId { get; set; }

		public DateTime EventTime { get; set; }
	}
}
