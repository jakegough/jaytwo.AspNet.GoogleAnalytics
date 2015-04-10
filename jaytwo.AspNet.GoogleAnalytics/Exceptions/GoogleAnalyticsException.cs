using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace jaytwo.AspNet.GoogleAnalytics.Exceptions
{
	[Serializable]
	public class GoogleAnalyticsException : Exception
	{
		public GoogleAnalyticsException()
			: base()
		{
		}

		public GoogleAnalyticsException(string message)
			: base(message)
		{
		}

		public GoogleAnalyticsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected GoogleAnalyticsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
