using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Zengfr.CodeGenerator.Web.Util
{
    public class SystemUtil
    {
        public static readonly string BaseDir= @"C:\Windows\Temp";
        public static readonly string FilleExtFilter = @"(\.vm|\.tpl|\.xml|\.model|\.db|\.cfg|\.log)$";

        public static string GetUserId() {
            var userId = "zfr";
            return userId;

        }
    }
}