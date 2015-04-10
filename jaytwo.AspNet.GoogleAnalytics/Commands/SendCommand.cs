using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics.Commands
{
	public class SendCommand : IGoogleAnalyticsCommand
	{
		public string EventType { get; set; }
		public object[] Options { get; set; }

		public object[] GetCommandParameters()
		{
			var joinedValuesList = new List<object>();
			joinedValuesList.Add("send");
			joinedValuesList.Add(EventType);

			if (Options != null)
			{
				joinedValuesList.AddRange(Options);
			}

			return joinedValuesList.ToArray();
		}
	}
}
