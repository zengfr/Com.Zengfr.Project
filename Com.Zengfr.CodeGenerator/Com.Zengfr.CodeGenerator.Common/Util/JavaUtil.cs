using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Util
{
    public class JavaUtil
    {
        private static JavaUtil instance;
        public static JavaUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JavaUtil(); 
                }
                return instance;
            }
        }
        public string getPackageName(String prefix, String name)
        {
            return string.Format("{0}.{1}", prefix.Trim('.'),   name.Trim('.')).ToLower();
        }
        public string getPackagePath(String  projectName,String packageName)
        {
            var path = string.Format("{0}\\src\\main\\{1}\\", projectName.Trim('\\'), packageName.Trim('.')).Replace('.', '\\').ToLower();
            return path;
        }
        public string getTypeName(String fullName) {
            var rtn = fullName;
            var tName = fullName;
            if (tName.LastIndexOf(".") !=-1) {
                tName = tName.Substring(tName.LastIndexOf(".")+1);
            }
            switch (tName) {
                case "Int32":
                    rtn = "Integer";
                    break;
                case "Int64":
                    rtn = "Long";
                    break;
                case "Decimal":
                    rtn = "BigDecimal";
                    break;
                case "DateTime":
                    rtn = "Date";
                    break;
                case "Boolean":
                    rtn = "Boolean";
                    break;
            }
            return rtn;
        }
    }
}
