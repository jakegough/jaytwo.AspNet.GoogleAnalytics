using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics.Commands
{
	public class GenericCommand : IGoogleAnalyticsCommand
	{
		private object[] _parameters;

		public GenericCommand(object[] parameters)
		{
			_parameters = parameters;
		}

		public object[] GetCommandParameters()
		{
			return _parameters;
		}
	}
}
