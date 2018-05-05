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
    /// <summary>
    /// 功能子系统
    /// </summary>
    [ActiveRecord("ut_SubSystem")]
    public class SubSystem : EntityBaseWithName
    {
       IList<Module> modules;
       [HasMany(typeof(Module), Lazy = true, Table = "Module", RelationType = RelationType.Bag, ColumnKey = "SubSystemID")]
       public IList<Module> Modules
       {
           get { return modules; }
           set { modules = value; }
       }
    }
}
