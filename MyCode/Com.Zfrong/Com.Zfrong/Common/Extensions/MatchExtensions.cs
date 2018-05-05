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
    public static class MatchExtensions
    {
        /// <summary>
        /// 安全获取Match成功项
        /// </summary>
        /// <param name="m"></param>
        /// <param name="action"></param>
        public static void MatchSuccess(this Match m, Action<Match> action)
        {
            if(m.Success)
               action(m);
        }
        

    }
}
