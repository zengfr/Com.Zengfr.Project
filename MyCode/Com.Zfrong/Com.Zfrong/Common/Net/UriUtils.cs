using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Com.Zfrong.Common.Net
{
    class UriUtils
    {
        public static string ToGB2312(string paramName)
        {
           return HttpUtility.UrlEncode(paramName,Encoding.GetEncoding("gb2312"));
        }
        public static string FormGB2312(string param)
        {
           return HttpUtility.UrlDecode(param, Encoding.GetEncoding("gb2312"));
        }
    }
}
