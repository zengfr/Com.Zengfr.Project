using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Gen
{
    public class ZDefaultVar
    {
        public static Boolean IsInWebEnv = true;
        public static String NamespacePrefix = "Com.Zengfr.Project";
        public static String TemplateFileExt = "vm";
        public static String TemplateRootDir = string.Format("{0}\\Resource\\template\\", System.AppDomain.CurrentDomain.BaseDirectory);
        public static String OutputRootDir = string.Format("{0}\\output\\", System.AppDomain.CurrentDomain.BaseDirectory);
    }
}
