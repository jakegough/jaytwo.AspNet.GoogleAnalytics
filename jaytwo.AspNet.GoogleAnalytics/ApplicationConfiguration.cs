using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics
{
	internal class ApplicationConfiguration
	{
		public virtual NameValueCollection AppSettings
		{
			get
			{
				return ConfigurationManager.AppSettings;
			}
		}

		public virtual bool Enabled
		{
			get
			{
				return GetAppSettingValueAsBoolean("jaytwo.AspNet.GoogleAnalytics.Enabled") ?? false;
			}
		}

		public virtual string TrackerId
		{
			get
			{
				return GetAppSettingValue("jaytwo.AspNet.GoogleAnalytics.TrackerId");
			}
		}

		public virtual string GetAppSettingValue(string key)
		{
			return AppSettings[key];
		}

		public virtual bool? GetAppSettingValueAsBoolean(string key)
		{
			var stringValue = GetAppSettingValue(key);

			bool result;
			if (bool.TryParse(stringValue, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}
	}
}
