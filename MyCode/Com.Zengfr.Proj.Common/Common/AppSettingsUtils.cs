using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Configuration;

namespace Com.Zengfr.Proj.Common
{
    public class AppSettingsUtils
    {
        static AppSettings appSettings = new AppSettings();
        public static T Get<T>(string key,string subKey)
        {
            var key2 = key;
            if (!string.IsNullOrWhiteSpace(subKey))
            {
                key2 = key + "-" + subKey;
            }
            return appSettings.Get<T>(key2, default(T));
        }
        public static T Get<T>(string key)
        {

            return appSettings.Get<T>(key, default(T));
        }
        public static T Get<T>(string key, T defaultValue)
        {

            return appSettings.Get<T>(key, defaultValue);
        }
        public static string Get(string key)
        {
            return appSettings.GetString(key);
        }
    }
}
