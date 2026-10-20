using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.CodeGenerator.Common.Meta;

namespace Com.Zengfr.CodeGenerator.Common.Gen
{
    

        public class ZOutputConfig
        {
            public String OutputDir { get; set; }
            public String OutputFileName { get; set; }

            public IDictionary<string,object> OutputParams { get; set; }

            public ZOutputConfig() {
                OutputDir = string.Empty;
                OutputFileName = string.Empty;
                OutputParams = new Dictionary<string, object>();
            }
        }

}
