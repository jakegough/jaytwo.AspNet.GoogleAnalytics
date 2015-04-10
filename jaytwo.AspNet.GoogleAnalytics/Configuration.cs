using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace jaytwo.AspNet.GoogleAnalytics
{
	public class Configuration
	{
		public virtual bool Enabled
		{
			get
			{
				return GetAppSettingValueAsBoolean("jaytwo.AspNet.GoogleAnalytics.Enabled") ?? false;
			}
		}

		public virtual string GetAppSettingValue(string key)
		{
			return GetAppSettings()[key];
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

		public virtual IDictionary<string, string> GetAppSettings()
		{
			return ConfigurationManager.AppSettings.
		}
	}
}
