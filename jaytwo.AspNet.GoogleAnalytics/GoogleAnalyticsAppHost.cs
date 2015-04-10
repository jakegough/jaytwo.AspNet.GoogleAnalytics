using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using jaytwo.AspNet.GoogleAnalytics.Commands;
using jaytwo.AspNet.GoogleAnalytics.Exceptions;
using System.Configuration;
using jaytwo.AspNet.GoogleAnalytics.Markup;

namespace jaytwo.AspNet.GoogleAnalytics
{
	public class GoogleAnalyticsAppHost
	{
		internal ApplicationConfiguration Configuration { get; private set; }

		internal GoogleAnalyticsAppHost(ApplicationConfiguration configuration)
		{
			Configuration = configuration;
		}

		public GoogleAnalyticsAppHost()
			: this(new ApplicationConfiguration())
		{
		}

		public virtual HttpContextBase GetCurrentHttpContext()
		{
			return new HttpContextWrapper(HttpContext.Current);
		}

		public virtual string GetTrackerId()
		{
			return Configuration.TrackerId;
		}

		public virtual string GetCurrentUserId()
		{
			var context = GetCurrentHttpContext();

			if (context != null && context.User != null && context.User.Identity != null)
			{
				return context.User.Identity.Name;
			}
			else
			{
				return null;
			}
		}

		internal static void InitializeDefault()
		{
			Initialize(new GoogleAnalyticsAppHost());
		}

		internal static void Initialize(GoogleAnalyticsAppHost appHost)
		{
			if (_Instance == null)
			{
				_Instance = appHost;
			}
			else
			{
				throw new GoogleAnalyticsInitializationException("GoogleAnalyticsAppHost.Instance has already been set");
			}
		}

		public void Initialize()
		{
			Initialize(this);
		}

		private static GoogleAnalyticsAppHost _Instance;
		public static GoogleAnalyticsAppHost Instance
		{
			get
			{				
				return _Instance ?? new GoogleAnalyticsAppHost();
			}
		}

		private const string ContextKey = "jaytwo.AspNet.GoogleAnalytics.GoogleAnalyticsContext";

		private static IList<IGoogleAnalyticsCommand> GetQueue(HttpContextBase context)
		{
			if (context != null)
			{
				var queue = (context.Items[ContextKey] as IList<IGoogleAnalyticsCommand>);
				if (queue == null)
				{
					queue = new List<IGoogleAnalyticsCommand>();
				}

				return queue;
			}
			else
			{
				return null;
			}
		}

		private static void SetQueue(IList<IGoogleAnalyticsCommand> queue, HttpContextBase context)
		{
			if (context != null)
			{
				context.Items[ContextKey] = queue;
			}
		}

		public void QueueCommand(IGoogleAnalyticsCommand command, HttpContextBase context)
		{
			var queue = GetQueue(context);
			queue.Add(command);
			SetQueue(queue, context);
		}

		public void QueueCommand(IGoogleAnalyticsCommand command)
		{
			var context = GetCurrentHttpContext();
			QueueCommand(command, context);
		}

		public IList<IGoogleAnalyticsCommand> GetCommandQueue()
		{
			var context = GetCurrentHttpContext();
			return GetQueue(context);
		}

		public void Send(string eventType, params object[] options)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendCommand(eventType, options);
			QueueCommand(command);
		}

		public void Set(string key, object value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetCommand(key, value);
			QueueCommand(command);
		}

		public void SendException(Exception exception, bool fatal)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendExceptionCommand(exception, fatal);
			QueueCommand(command);
		}

		public void SendException(string description, bool fatal)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendExceptionCommand(description, fatal);
			QueueCommand(command);
		}

		public void SetDocumentTitle(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetDocumentTitleCommand(value);
			QueueCommand(command);
		}

		public void SetScreenName(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetScreenNameCommand(value);
			QueueCommand(command);
		}

		public void SetApplicationName(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetApplicationNameCommand(value);
			QueueCommand(command);
		}

		public void SetApplicationId(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetApplicationIdCommand(value);
			QueueCommand(command);
		}

		public void SetApplicationVersion(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetApplicationVersionCommand(value);
			QueueCommand(command);
		}

		public void SetCustomDimension(int dimensionId, string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetCustomDimensionCommand(dimensionId, value);
			QueueCommand(command);
		}

		public void SetContentExperimentId(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetContentExperimentIdCommand(value);
			QueueCommand(command);
		}

		public void SetContentExperimentVariant(string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetContentExperimentVariantCommand(value);
			QueueCommand(command);
		}

		public void SetCustomMetric(int metricId, string value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSetCustomMetricCommand(metricId, value);
			QueueCommand(command);
		}

		public void SendEvent(string category, string action)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendEventCommand(category, action);
			QueueCommand(command);
		}

		public void SendEvent(string category, string action, string label)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendEventCommand(category, action, label);
			QueueCommand(command);
		}

		public void SendEvent(string category, string action, string label, int value)
		{
			var command = GoogleAnalyticsMarkupBuilderHelpers.GetTrackerSendEventCommand(category, action, label, value);
			QueueCommand(command);
		}

		public virtual IList<IGoogleAnalyticsCommand> GetInferredTrackerCommandsFromAppSettings()
		{
			return GoogleAnalyticsMarkupBuilderHelpers.GetTrackerCommandsFromAppSettings(
				Configuration.AppSettings,
				"jaytwo.AspNet.GoogleAnalytics.");
		}

		public void ApplyGoogleAnalyticsInjectorFilter(HttpContextBase context)
		{
			const string isFilterAppliedKey = "jaytwo.AspNet.GoogleAnalytics.IsFilterApplied";

			var response = context.Response;
			
			if (response.ContentType == "text/html")
			{
				var isFilterApplied = ((bool?)context.Items[isFilterAppliedKey]) ?? false;

				if (!isFilterApplied)
				{
					Func<string> htmlSnippetDelegate = () =>
					{
						return new GoogleAnalyticsMarkupBuilder(this).GetTrackerJavascript();
					};

					response.Filter = new PreBodyTagFilter(htmlSnippetDelegate, response.Filter, response.ContentEncoding);

					context.Items[isFilterAppliedKey] = true;
				}
			}
		}
	}
}
