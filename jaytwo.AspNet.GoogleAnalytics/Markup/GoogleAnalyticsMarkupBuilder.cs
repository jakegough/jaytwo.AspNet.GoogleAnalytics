using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jaytwo.AspNet.GoogleAnalytics.Commands;
using System.Configuration;

namespace jaytwo.AspNet.GoogleAnalytics.Markup
{
	public class GoogleAnalyticsMarkupBuilder
	{
		public static readonly string TrackerObjectName = "__ga";

		protected GoogleAnalyticsAppHost AppHost { get; private set; }

		public GoogleAnalyticsMarkupBuilder(GoogleAnalyticsAppHost appHost)
		{
			AppHost = appHost;
		}

		public string GetTrackerJavascript()
		{
			var output = new StringBuilder();
			output.AppendLine();
			output.AppendLine("<!-- Google Analytics -->");
			output.AppendLine("<script>");
			output.AppendLine("(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)})(window,document,'script','//www.google-analytics.com/analytics.js','" + TrackerObjectName + "');");
			output.AppendLine(GetTrackerCreateJavascript());
			output.AppendLine(GetTrackerSendPageviewJavascript());

			var inferredCommands = AppHost.GetInferredTrackerCommandsFromAppSettings();
			var inferredCommandsJavascript = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerJavascript(TrackerObjectName, inferredCommands);
			output.Append(inferredCommandsJavascript);

			var queuedCommands = AppHost.GetCommandQueue();
			var queuedCommandsJavascript = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerJavascript(TrackerObjectName, queuedCommands);
			output.Append(queuedCommandsJavascript);

			output.AppendLine("</script>");
			output.AppendLine("<!-- End Google Analytics -->");

			return output.ToString();
		}

		public static string GetTrackerCreateJavascript(IGoogleAnalyticsCommand command)
		{
			return GoogleAnalyticsMarkupBuilderHelpers.GetTrackerJavascript(TrackerObjectName, command);
		}

		private string GetTrackerCreateJavascript()
		{
			var trackerId = AppHost.GetTrackerId();
			var userId = AppHost.GetCurrentUserId();

			var command = (!string.IsNullOrEmpty(userId))
				? GoogleAnalyticsMarkupBuilderHelpers.GetTrackerCreateCommand(trackerId, userId)
				: GoogleAnalyticsMarkupBuilderHelpers.GetTrackerCreateCommand(trackerId);

			return GoogleAnalyticsMarkupBuilderHelpers.GetTrackerJavascript(TrackerObjectName, command);
		}

		private string GetTrackerSendPageviewJavascript()
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendCommand("pageview");
			return GoogleAnalyticsMarkupBuilderHelpers.GetTrackerJavascript(TrackerObjectName, command);
		}
	}
}
