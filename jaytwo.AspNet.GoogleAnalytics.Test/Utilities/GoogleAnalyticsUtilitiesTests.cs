using jaytwo.AspNet.GoogleAnalytics.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.AspNet.GoogleAnalytics.Test.Utilities
{
	[TestFixture]
	public static class GoogleAnalyticsUtilitiesTests
	{
		[Test]
		public static void GoogleAnalyticsUtilities_GetJavaScriptValueString_anonymous_object()
		{
			var value = new { Name = "William T Riker", Rank = "First Officer" };			
			var expected = "{\"Name\":\"William T Riker\",\"Rank\":\"First Officer\"}";
			var actual = GoogleAnalyticsUtilities.GetJavaScriptValueString(value);

			Assert.AreEqual(expected, actual);
		}

		[Test]		
		[TestCase(null, ExpectedResult = "null")]
		[TestCase((string)null, ExpectedResult = "null")]		
		[TestCase("", ExpectedResult = "''")]
		[TestCase("Jake", ExpectedResult = "'Jake'")]
		[TestCase("Jake's car", ExpectedResult = @"'Jake\'s car'")]
		public static string GoogleAnalyticsUtilities_GetJavaScriptValueString_string(string value)
		{
			return GoogleAnalyticsUtilities.GetJavaScriptValueString(value);
		}
	}
}
