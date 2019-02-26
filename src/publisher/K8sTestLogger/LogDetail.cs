
using System;

namespace K8sTestLogger
{
	public class LogDetail
	{
		public LogDetail() { }

		public LogDetail(string currentUser, string action, string message)
		{
			CurrentUser = currentUser;
			Action = action;
			Message = message;
			
		}


		public string CurrentUser { get; set; }

		public string Action { get; set; }

		public string Message { get; set; }

		

	}
}
