using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Util
{
   public class CSharpUtil
    {
        private static CSharpUtil instance;
        public static CSharpUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CSharpUtil();
                }
                return instance;
            }
        }
    }
}
