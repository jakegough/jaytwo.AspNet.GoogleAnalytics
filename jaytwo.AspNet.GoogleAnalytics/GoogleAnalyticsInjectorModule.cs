using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Configuration;
using jaytwo.AspNet.GoogleAnalytics;

namespace jaytwo.AspNet.GoogleAnalytics
{
	public class GoogleAnalyticsInjectorModule : IHttpModule, IDisposable
	{
		internal ApplicationConfiguration Configuration { get; private set; }

		internal GoogleAnalyticsInjectorModule(ApplicationConfiguration configuration)
		{
			Configuration = configuration;
		}

		public GoogleAnalyticsInjectorModule()
			: this(new ApplicationConfiguration())
		{
		}

		public void Init(HttpApplication httpApplication)
		{
			httpApplication.PostReleaseRequestState += (sender, e) => ApplyGoogleAnalyticsInjectorFilter(WithTestable(sender as HttpApplication));

			// i noticed glimpse also uses these events
			//httpApplication.PostAcquireRequestState += (sender, e) => ApplyGoogleAnalyticsInjectorFilter(WithTestable(sender as HttpApplication));
			//httpApplication.PostRequestHandlerExecute += (sender, e) => ApplyGoogleAnalyticsInjectorFilter(WithTestable(sender as HttpApplication));
			//httpApplication.PreSendRequestHeaders += (sender, e) => ApplyGoogleAnalyticsInjectorFilter(WithTestable(sender as HttpApplication));
		}

		private static HttpContextBase WithTestable(HttpApplication httpApplication)
		{
			return new HttpContextWrapper(httpApplication.Context);
		}

		public void ApplyGoogleAnalyticsInjectorFilter(HttpContextBase context)
		{
			var appHost = GoogleAnalyticsAppHost.Instance;

			if (Configuration.Enabled && appHost != null)
			{
				appHost.ApplyGoogleAnalyticsInjectorFilter(context);
			}			
		}

		public void Dispose()
		{
		}
	}
}
