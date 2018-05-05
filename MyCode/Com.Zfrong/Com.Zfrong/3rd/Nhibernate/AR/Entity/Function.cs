using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using System.Security.Principal;
using System.Security;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    public interface IHasFunctions
    {
        IList<Function> Functions { get;set; }
    }
    [ActiveRecord("ut_Function")]
    public class Function :EntityBaseWithName
    {
        private Module module;
        string pagePath;
        string code;

        [Property]
        public string PagePath { get { return pagePath; } set { pagePath = value; } }
        [Property]
        public string Code { get { return code; } set { code = value; } }
        [BelongsTo("ModuleID")]
        public Module Module { get { return module; } set { module = value; } }

    }
}
