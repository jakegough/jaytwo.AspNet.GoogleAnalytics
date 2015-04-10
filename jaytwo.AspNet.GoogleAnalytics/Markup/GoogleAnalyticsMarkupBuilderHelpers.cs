using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web;
using jaytwo.AspNet.GoogleAnalytics.Commands;
using System.Collections.Specialized;
using jaytwo.AspNet.GoogleAnalytics.Utilities;

namespace jaytwo.AspNet.GoogleAnalytics.Markup
{
	public static class GoogleAnalyticsMarkupBuilderHelpers
	{
		public static string GetTrackerJavascript(string trackerObjectName, params object[] parameters)
		{
			var quotedEscapedParams = parameters
				.Select(x => GoogleAnalyticsUtilities.GetJavaScriptValueString(x))
				.ToArray();

			var joinedParams = string.Join(",", quotedEscapedParams);

			return string.Format("{0}({1});", trackerObjectName, joinedParams);
		}

		// it's the async model
		// https://developers.google.com/analytics/devguides/collection/gajs/methods/gaJSApiBasicConfiguration#_gat.GA_Tracker_._setVar
		//public static string GetTrackerJavascript(string trackerObjectName, params object[] parameters)
		//{
		//    var quotedEscapedParams = parameters
		//        .Select(x => (x is string) ? "'" + HttpUtility.JavaScriptStringEncode((string)x) + "'" : serializer.Serialize(x))
		//        .ToArray();

		//    var joinedParams = string.Join(",", quotedEscapedParams);

		//    return string.Format("{0}.push([{1}]);", trackerObjectName, joinedParams);
		//}

		public static string GetTrackerJavascript(string trackerObjectName, IGoogleAnalyticsCommand command)
		{
			var parameters = command.GetCommandParameters();
			return GetTrackerJavascript(trackerObjectName, parameters);
		}

		public static string GetTrackerJavascript(string trackerObjectName, IList<IGoogleAnalyticsCommand> commands)
		{
			var result = new StringBuilder();

			foreach (var command in commands)
			{
				result.AppendLine(GetTrackerJavascript(trackerObjectName, command));
			}

			return result.ToString();
		}

		public static SendCommand GetTrackerSendCommand(string eventType)
		{
			var result = new SendCommand()
			{
				EventType = eventType,
			};

			return result;
		}

		public static SendCommand GetTrackerSendCommand(string eventType, params object[] options)
		{
			var result = new SendCommand()
			{
				EventType = eventType,
				Options = options,
			};

			return result;
		}

		public static SetCommand GetTrackerSetCommand(string key, object value)
		{
			var result = new SetCommand()
			{
				Key = key,
				Value = value,
			};

			return result;
		}

		public static CreateCommand GetTrackerCreateCommand(string trackerId, object options)
		{
			var result = new CreateCommand()
			{
				TrackerId = trackerId,
				Options = options,
			};

			return result;
		}

		public static CreateCommand GetTrackerCreateCommand(string trackerId)
		{
			var result = new CreateCommand()
			{
				TrackerId = trackerId,
				Options = "auto",
			};

			return result;
		}

		public static CreateCommand GetTrackerCreateCommand(string trackerId, string userId)
		{
			var result = new CreateCommand()
			{
				TrackerId = trackerId,
				Options = new { userId = userId },
			};

			return result;
		}

		public static SendCommand GetTrackerSendExceptionCommand(Exception exception, bool fatal)
		{
			var description = string.Format("{0}::{1}", exception.GetType().Name, exception.Message);
			return GetTrackerSendExceptionCommand(description, fatal);
		}

		public static SendCommand GetTrackerSendExceptionCommand(string description, bool fatal)
		{
			var options = new { exDescription = description, exFatal = fatal };
			return GetTrackerSendCommand("exception", options);
		}

		public static SetCommand GetTrackerSetDocumentTitleCommand(string value)
		{
			return GetTrackerSetCommand("title", value);
		}

		public static SetCommand GetTrackerSetScreenNameCommand(string value)
		{
			return GetTrackerSetCommand("screenName", value);
		}

		public static SetCommand GetTrackerSetApplicationNameCommand(string value)
		{
			return GetTrackerSetCommand("appName", value);
		}

		public static SetCommand GetTrackerSetApplicationIdCommand(string value)
		{
			return GetTrackerSetCommand("appId", value);
		}

		public static SetCommand GetTrackerSetApplicationVersionCommand(string value)
		{
			return GetTrackerSetCommand("appVersion", value);
		}

		public static SetCommand GetTrackerSetCustomDimensionCommand(int dimensionId, string value)
		{
			var dimension = string.Format("dimension{0}", dimensionId);
			return GetTrackerSetCommand(dimension, value);
		}

		public static SetCommand GetTrackerSetContentExperimentIdCommand(string value)
		{
			return GetTrackerSetCommand("expId", value);
		}

		public static SetCommand GetTrackerSetContentExperimentVariantCommand(string value)
		{
			return GetTrackerSetCommand("expVar", value);
		}

		public static SetCommand GetTrackerSetCustomMetricCommand(int metricId, string value)
		{
			var metric = string.Format("metric{0}", metricId);
			return GetTrackerSetCommand(metric, value);
		}

		public static SendCommand GetTrackerSendEventCommand(string category, string action)
		{
			return GetTrackerSendCommand("event", category, action);
		}

		public static SendCommand GetTrackerSendEventCommand(string category, string action, string label)
		{
			return GetTrackerSendCommand("event", category, action, label);
		}

		public static SendCommand GetTrackerSendEventCommand(string category, string action, string label, int value)
		{
			return GetTrackerSendCommand("event", category, action, label, value);
		}

		public static IList<IGoogleAnalyticsCommand> GetTrackerCommandsFromAppSettings(NameValueCollection appSettings, string prefix)
		{
			var inferredGoogleAnalyticsKeys = appSettings.AllKeys
				.Where(x => x.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
				.ToList();

			var result = new List<IGoogleAnalyticsCommand>();

			foreach (var key in inferredGoogleAnalyticsKeys)
			{
				var commandName = key.Substring(prefix.Length);
				var commandParameter = appSettings[key];

				var command = TryGetTrackerCommandFromAppSettings(commandName, commandParameter);

				if (command != null)
				{
					result.Add(command);
				}
			}

			return result;
		}

		private static IDictionary<string, Func<string, IGoogleAnalyticsCommand>> GetKnownTrackerCommands()
		{
			var result = new Dictionary<string, Func<string, IGoogleAnalyticsCommand>>(StringComparer.InvariantCultureIgnoreCase);
			result.Add("SetDocumentTitle", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetDocumentTitleCommand);
			result.Add("SetScreenName", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetScreenNameCommand);
			result.Add("SetApplicationName", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetApplicationNameCommand);
			result.Add("SetApplicationId", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetApplicationIdCommand);
			result.Add("SetApplicationVersion", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetApplicationVersionCommand);
			result.Add("SetContentExperimentId", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetContentExperimentIdCommand);
			result.Add("SetContentExperimentVariant", GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetContentExperimentVariantCommand);

			return result;
		}

		public static IGoogleAnalyticsCommand TryGetTrackerCommandFromAppSettings(string commandName, string commandParameter)
		{
			try
			{
				return GetTrackerCommandFromAppSettings(commandName, commandParameter);
			}
			catch
			{
				return null;
			}
		}

		public static IGoogleAnalyticsCommand GetTrackerCommandFromAppSettings(string commandName, string commandParameter)
		{
			const string setCustomDimensionPrefix = "SetCustomDimension.";
			const string setCustomMetricPrefix = "SetCustomMetric.";
			const string setPrefix = "set.";

			var knownCommands = GetKnownTrackerCommands();

			if (knownCommands.ContainsKey(commandName))
			{
				if (!string.IsNullOrEmpty(commandParameter))
				{
					var getCommandMethod = knownCommands[commandName];
					return getCommandMethod.Invoke(commandParameter);
				}
			}
			else if (commandName.StartsWith(setCustomDimensionPrefix, StringComparison.InvariantCultureIgnoreCase))
			{
				var dimensionId = int.Parse(commandName.Substring(setCustomDimensionPrefix.Length));
				var value = commandParameter;

				if (!string.IsNullOrEmpty(commandParameter))
				{
					return GetTrackerSetCustomDimensionCommand(dimensionId, value);
				}
			}
			else if (commandName.StartsWith(setCustomMetricPrefix, StringComparison.InvariantCultureIgnoreCase))
			{
				var metricId = int.Parse(commandName.Substring(setCustomMetricPrefix.Length));
				var value = commandParameter;

				if (!string.IsNullOrEmpty(commandParameter))
				{
					return GetTrackerSetCustomMetricCommand(metricId, value);
				}
			}
			else if (commandName.StartsWith(setPrefix, StringComparison.InvariantCultureIgnoreCase))
			{
				var setKey = commandName.Substring(setPrefix.Length);
				var value = commandParameter;

				if (!string.IsNullOrEmpty(setKey) && !string.IsNullOrEmpty(commandParameter))
				{
					return GetTrackerSetCommand(setKey, value);
				}
			}

			return null;
		}
	}
}
