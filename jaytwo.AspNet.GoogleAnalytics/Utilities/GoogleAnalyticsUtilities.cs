using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace jaytwo.AspNet.GoogleAnalytics.Utilities
{
	internal static class GoogleAnalyticsUtilities
	{
		public static string EscapeJavaScriptString(string value)
		{
			// https://briancaos.wordpress.com/2012/05/16/javascript-string-encoding-in-c/

			return Uri.EscapeDataString(value)
				.Replace("'", @"\'")
				.Replace(@"""", @"\""");
		}

		public static string GetJavaScriptValueString(object value)
		{
			var valueAsString = value as string;
			if (value is string && valueAsString != null)
			{
				return "'" + GoogleAnalyticsUtilities.EscapeJavaScriptString(valueAsString) + "'";
			}
			else
			{
				return new JavaScriptSerializer().Serialize(value);
			}
		}
	}
}
