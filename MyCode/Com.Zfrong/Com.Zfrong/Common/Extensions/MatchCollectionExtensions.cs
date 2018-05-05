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
    public static class MatchCollectionExtensions
    {
        /// <summary>
        /// 安全获取Match成功项
        /// </summary>
        /// <param name="mColl"></param>
        /// <param name="action"></param>
        public static void ForEach(this MatchCollection mColl, Action<Match> action)
        {
            foreach (Match item in mColl)
            {
                if(item.Success)
                  action(item);
            }
        }
        /// <summary>
        /// 匹配任何一项
        /// </summary>
        /// <param name="input"></param>
        /// <param name="patterns"></param>
        /// <param name="opts"></param>
        /// <param name="action"></param>
        public static void MatchAny(this string input, string[] patterns, RegexOptions opts, Action<int, MatchCollection> action)
        {
            int index = 0;
            foreach (string pattern in patterns)
            {
                MatchCollection mColl = Regex.Matches(input, pattern, opts);
                {
                    action(index, mColl);
                }
                index++;
            }
        }
    }
}
