using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace Mx.EventHub.Sender.Models
{
	public static class EventMessageModelExtensions
	{

		public static IEnumerable<EventData> ToEventData(this IEnumerable<EventMessageModel> messages)
		{
			return (messages.Select(message => new EventData(message.ToBytes()))).ToList();
		}

		public static byte[] ToBytes(this EventMessageModel messageModel)
		{
			return JsonConvert.SerializeObject(messageModel).ToBytes();
		}


		internal static byte[] ToBytes(this string messageJson)
		{

			return Encoding.UTF8.GetBytes(messageJson);
		}
	}
}
