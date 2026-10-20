using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Com.Zengfr.CodeGenerator.Common.Meta;

namespace Com.Zengfr.CodeGenerator.Common.Gen
{
    public class ZInputConfig
    {
        #region
        public  List<String> TemplateDirs { set; get; }
        public  List<String> MetaModelNames { set; get; }

        public String NamespacePrefix { get; set; }
        public IDictionary<String, String> InputParams { get; set; }
        #endregion
        public ZInputConfig() {
            this.NamespacePrefix = ZDefaultVar.NamespacePrefix;
            this.TemplateDirs = new List<string>();
            this.MetaModelNames = new List<string>();
            this.InputParams = new Dictionary<String, String>();
        }

    }

}
