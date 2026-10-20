using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Com.Zengfr.CodeGenerator.Common.Meta;

namespace Com.Zengfr.CodeGenerator.Common.Gen
{
    public class ZSysConfig
    {
        #region # 系统设定
        public  bool IsInWebEnv { set; get; }
        public  String TemplateRootDir { set; get; }
        public  String TemplateFileExt { set; get; }
        public  String OutputRootDir { get; set; }
        #endregion
        public ZSysConfig() {
            this.IsInWebEnv = ZDefaultVar.IsInWebEnv;
            this.TemplateFileExt = ZDefaultVar.TemplateFileExt;
            this.TemplateRootDir = ZDefaultVar.TemplateRootDir;
            this.OutputRootDir = ZDefaultVar.OutputRootDir;
        }

    }

}
