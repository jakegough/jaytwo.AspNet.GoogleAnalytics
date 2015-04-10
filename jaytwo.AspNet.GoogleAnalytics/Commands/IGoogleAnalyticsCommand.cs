using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics.Commands
{
	public interface IGoogleAnalyticsCommand
	{
		object[] GetCommandParameters();
	}
}
