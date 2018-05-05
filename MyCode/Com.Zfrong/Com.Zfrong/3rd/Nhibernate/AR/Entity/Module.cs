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
    [ActiveRecord("ut_Module")]
    public class Module : EntityBaseWithName, IHasFunctions
    {
        IList<Function> functions;
        private SubSystem subSystem;

        [BelongsTo("SubSystemID")]
        public SubSystem SubSystem { get { return subSystem; } set { subSystem = value; } }


        [HasMany(typeof(Function), Lazy = true, Table = "Function", RelationType = RelationType.Bag, ColumnKey = "ModuleID")]
        public IList<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

    }
}
