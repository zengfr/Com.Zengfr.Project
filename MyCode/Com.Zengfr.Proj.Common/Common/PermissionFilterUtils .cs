using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Com.Zengfr.Proj.Common 
{
    public class PermissionFilterUtils
    {
        public static T GetValueByConfig<T>()
        {
            string key = "currentManageWebProvinceID";
            return GetValueByConfig<T>(key);
        }
        public static T GetValueByConfig<T>(string key)
        {
            string subkey = GetKey();
            var value= AppSettingsUtils.Get<T>(key, subkey);
            return value;
        }
        public static string GetKey()
        {
            string key = string.Empty;
            if (string.IsNullOrWhiteSpace(key))
            {
                key = GetKeyByURL(0);
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                key = GetKeyByCookies("pf");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                key = GetKeyByForm("pf");
            }
            return key;
        }
        public static string GetKeyByURL(int index)
        {
            string key = string.Empty;
            if (HttpContext.Current != null)
            {
                string[] HostStrings = HttpContext.Current.Request.Url.Host.Split(',','.',':','/');
                if (HostStrings.Length>=3&&index < HostStrings.Length)//w.w.com
                {
                    key = HostStrings[index];
                }
            }
            return key;
        }
        public static string GetKeyByForm(string formKey)
        {
            string key = string.Empty;
            if (HttpContext.Current != null)
            {
                key = HttpContext.Current.Request.Form[formKey];
            }
            return key;
        }
        public static string GetKeyByCookies(string cookiesKey)
        {
            string key = string.Empty;
            if (HttpContext.Current != null)
            {
                var c = HttpContext.Current.Request.Cookies[cookiesKey];
                if (c != null)
                {
                    key = c.Value;
                }
            }
            return key;
        }
    }
}
