using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Com.Zfrong.Common.Extensions
{
    /// <summary>
    /// zfr
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 安全获取子字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="s">开始偏移量</param>
        /// <param name="end"></param>
        /// <param name="d">结束偏移量</param>
        /// /// <param name="include">是否包含收尾 默认不包含</param>
        /// <returns></returns>
        public static string Substring(this string source, string start,int s,string end,int d,bool include)
        {
                int i=0;
                int j=0;
                if (source.IndexOf(start) != -1 && source.IndexOf(end) != -1)
                {
                    i = source.IndexOf(start) + s;
                    j = source.IndexOf(end) + d;
                    if (!include)
                    {
                        i = i + start.Length;
                        j = j - end.Length;
                    }
                    if (j > i)
                    {
                        return source.Substring(i, j - i);
                    }
                }
               return "";
        }
        /// <summary>
        /// 匹配任何一项
        /// </summary>
        /// <param name="input"></param>
        /// <param name="patterns"></param>
        /// <param name="opts"></param>
        /// <param name="action"></param>
        public static void MatchAny(this string input, string[] patterns, RegexOptions opts, Action<int,MatchCollection> action)
        {
            int index = 0;
            foreach (string pattern in patterns)
            {
                MatchCollection mColl = Regex.Matches(input, pattern, opts);
                {
                    action(index,mColl);
                }
                index++;
            }
        }
        /// <summary>
        /// source.IndexOf(sub) != -1
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static bool IndexOfTrue(this string source, string sub)
        {
            return source.IndexOf(sub) != -1;
        }
        public static int ToInt(this string source)
        {
            int rtn = 0;
            int.TryParse(source,out rtn);
            return rtn;
        }
        public static DateTime ToDateTime(this string source)
        {
            DateTime rtn = DateTime.Now;
            DateTime.TryParse(source,out rtn);
            return rtn;
        }
    }
}
