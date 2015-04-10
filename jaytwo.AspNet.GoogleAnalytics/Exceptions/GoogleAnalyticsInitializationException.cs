using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace jaytwo.AspNet.GoogleAnalytics.Exceptions
{
	[Serializable]
	public class GoogleAnalyticsInitializationException : GoogleAnalyticsException
	{
		public GoogleAnalyticsInitializationException()
			: base()
		{
		}

		public GoogleAnalyticsInitializationException(string message)
			: base(message)
		{
		}

		public GoogleAnalyticsInitializationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected GoogleAnalyticsInitializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
