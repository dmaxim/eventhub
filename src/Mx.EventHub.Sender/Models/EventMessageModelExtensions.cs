﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.EventHubs;
using Mx.EventHub.Model;
using Newtonsoft.Json;

namespace Mx.EventHub.Sender.Models
{
	public static class EventMessageModelExtensions
	{

		public static IEnumerable<EventData> ToEventData(this IEnumerable<EventMessageModel> messages)
		{
			return (messages.Select(message => new EventData(ToBytes((EventMessageModel) message)))).ToList();
		}

		public static byte[] ToBytes(this EventMessageModel messageModel)
		{
			return ToBytes((string) JsonConvert.SerializeObject(messageModel));
		}


		internal static byte[] ToBytes(this string messageJson)
		{

			return Encoding.UTF8.GetBytes(messageJson);
		}
	}
}
