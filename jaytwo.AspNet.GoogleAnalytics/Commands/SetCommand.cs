using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics.Commands
{
	public class SetCommand : IGoogleAnalyticsCommand
	{
		public string Key { get; set; }
		public object Value { get; set; }

		public object[] GetCommandParameters()
		{
			return new object[] { "set", Key, Value };
		}
	}
}
