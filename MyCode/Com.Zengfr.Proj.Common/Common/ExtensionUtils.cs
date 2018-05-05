using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common
{
    public static class ExtensionUtils
    {
        public static string CutString(this string str, string split)
        {
            if (!string.IsNullOrWhiteSpace(str) && !string.IsNullOrWhiteSpace(split))
            {
                str = str.Trim(',');
                var index = str.IndexOf(split);
                if (index > 0)
                {
                    str=str.Substring(0, index);
                }
            }
            return str;
        }
        public static T Action<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> objs, Action<T> action)
        {
            foreach (var item in objs)
            {
                action(item);
            }
            return objs;
        }
    }
}
