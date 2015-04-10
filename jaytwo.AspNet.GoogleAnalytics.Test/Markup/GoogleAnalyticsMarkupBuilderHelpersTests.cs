using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using jaytwo.AspNet.GoogleAnalytics.Markup;
using System.Collections.Specialized;
using jaytwo.AspNet.GoogleAnalytics.Commands;
using NUnit.Framework;

namespace jaytwo.AspNet.GoogleAnalytics.Test.Markup
{
	[TestFixture]
	public static class GoogleAnalyticsMarkupBuilderHelpersTests
	{
		[Test]
		public static void GetTrackerCommandsFromAppSettings_multiple_set_commands_and_ignore_TrackerId()
		{
			var appSettings = new NameValueCollection();
			appSettings.Set("WhateverPrefix.TrackerId", "TrackerId"); // should not be added to the result at all
			appSettings.Set("WhateverPrefix.set.key1", "value1"); // 0
			appSettings.Set("WhateverPrefix.set.key2", "value2"); // 1

			var commands = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerCommandsFromAppSettings(
				appSettings,
				"WhateverPrefix.");

			Assert.AreEqual(2, commands.Count);

			var setKey1Command = (commands[0] as SetCommand);
			Assert.IsNotNull(setKey1Command);
			Assert.AreEqual("key1", setKey1Command.Key);
			Assert.AreEqual("value1", setKey1Command.Value);

			var setKey2Command = (commands[1] as SetCommand);
			Assert.IsNotNull(setKey2Command);
			Assert.AreEqual("key2", setKey2Command.Key);
			Assert.AreEqual("value2", setKey2Command.Value);
		}

		[Test]
		public static void GetTrackerCommandsFromAppSettings_SetDocumentTitle()
		{
			var appSettings = new NameValueCollection();
			appSettings.Set("WhateverPrefix.SetDocumentTitle", "DocumentTitle");  // 0
			appSettings.Set("WhateverPrefix.SetScreenName", "ScreenName"); // 1
			appSettings.Set("WhateverPrefix.SetApplicationName", "ApplicationName"); // 2
			appSettings.Set("WhateverPrefix.SetApplicationId", "ApplicationId"); // 3
			appSettings.Set("WhateverPrefix.SetApplicationVersion", "ApplicationVersion"); // 4
			appSettings.Set("WhateverPrefix.SetContentExperimentId", "ContentExperimentId"); // 5
			appSettings.Set("WhateverPrefix.SetContentExperimentVariant", "ContentExperimentVariant"); // 6
			appSettings.Set("WhateverPrefix.SetCustomDimension.0", "CustomDimension0"); // 7
			appSettings.Set("WhateverPrefix.SetCustomDimension.1", "CustomDimension1"); // 8
			appSettings.Set("WhateverPrefix.SetCustomDimension.abc", "invalid"); // should not be added to the result at all
			appSettings.Set("WhateverPrefix.SetCustomMetric.0", "CustomMetric0"); // 9
			appSettings.Set("WhateverPrefix.SetCustomMetric.1", "CustomMetric1"); // 10
			appSettings.Set("WhateverPrefix.SetCustomMetric.abc", "invalid");  // should not be added to the result at all

			var commands = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerCommandsFromAppSettings(
				appSettings,
				"WhateverPrefix.");

			Assert.AreEqual(11, commands.Count);

			var setDocumentTitleCommand = (commands[0] as SetCommand);
			Assert.IsNotNull(setDocumentTitleCommand);
			Assert.AreEqual("title", setDocumentTitleCommand.Key);
			Assert.AreEqual("DocumentTitle", setDocumentTitleCommand.Value);

			var setScreenNameCommand = (commands[1] as SetCommand);
			Assert.IsNotNull(setScreenNameCommand);
			Assert.AreEqual("screenName", setScreenNameCommand.Key);
			Assert.AreEqual("ScreenName", setScreenNameCommand.Value);

			var setApplicationNameCommand = (commands[2] as SetCommand);
			Assert.IsNotNull(setApplicationNameCommand);
			Assert.AreEqual("appName", setApplicationNameCommand.Key);
			Assert.AreEqual("ApplicationName", setApplicationNameCommand.Value);

			var setApplicationIdCommand = (commands[3] as SetCommand);
			Assert.IsNotNull(setApplicationIdCommand);
			Assert.AreEqual("appId", setApplicationIdCommand.Key);
			Assert.AreEqual("ApplicationId", setApplicationIdCommand.Value);

			var setApplicationVersionCommand = (commands[4] as SetCommand);
			Assert.IsNotNull(setApplicationVersionCommand);
			Assert.AreEqual("appVersion", setApplicationVersionCommand.Key);
			Assert.AreEqual("ApplicationVersion", setApplicationVersionCommand.Value);

			var setContentExperimentIdCommand = (commands[5] as SetCommand);
			Assert.IsNotNull(setContentExperimentIdCommand);
			Assert.AreEqual("expId", setContentExperimentIdCommand.Key);
			Assert.AreEqual("ContentExperimentId", setContentExperimentIdCommand.Value);

			var setContentExperimentVariantCommand = (commands[6] as SetCommand);
			Assert.IsNotNull(setContentExperimentVariantCommand);
			Assert.AreEqual("expVar", setContentExperimentVariantCommand.Key);
			Assert.AreEqual("ContentExperimentVariant", setContentExperimentVariantCommand.Value);

			var setCustomDimension0Command = (commands[7] as SetCommand);
			Assert.IsNotNull(setCustomDimension0Command);
			Assert.AreEqual("dimension0", setCustomDimension0Command.Key);
			Assert.AreEqual("CustomDimension0", setCustomDimension0Command.Value);

			var setCustomDimension1Command = (commands[8] as SetCommand);
			Assert.IsNotNull(setCustomDimension1Command);
			Assert.AreEqual("dimension1", setCustomDimension1Command.Key);
			Assert.AreEqual("CustomDimension1", setCustomDimension1Command.Value);

			var setCustomMetric0Command = (commands[9] as SetCommand);
			Assert.IsNotNull(setCustomMetric0Command);
			Assert.AreEqual("metric0", setCustomMetric0Command.Key);
			Assert.AreEqual("CustomMetric0", setCustomMetric0Command.Value);

			var setCustomMetric1Command = (commands[10] as SetCommand);
			Assert.IsNotNull(setCustomMetric1Command);
			Assert.AreEqual("metric1", setCustomMetric1Command.Key);
			Assert.AreEqual("CustomMetric1", setCustomMetric1Command.Value);
		}
	}
}
