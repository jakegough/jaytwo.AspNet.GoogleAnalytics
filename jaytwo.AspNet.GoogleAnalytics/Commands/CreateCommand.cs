using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics.Commands
{
	public class CreateCommand : IGoogleAnalyticsCommand
	{
		public string TrackerId { get; set; }
		public object Options { get; set; }

		public object[] GetCommandParameters()
		{
			var joinedValuesList = new List<object>();
			joinedValuesList.Add("create");
			joinedValuesList.Add(TrackerId);

			if (Options != null)
			{
				joinedValuesList.Add(Options);
			}

			return joinedValuesList.ToArray();
		}
	}
}
