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
    [ActiveRecord("ut_Department")]
    public class Department : EntityBaseWithName, IHasFunctions
    {
        private Department parent;
       private IList<User> users;
       private IList<Function> functions;
        [BelongsTo("ParentID")]
        public Department Parent
        {
            get { return parent; }
            set { parent = value; }
        }
       [HasAndBelongsToMany(typeof(User), Index = "User_Department_DepartmentID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_Department", ColumnRef = "UserID", ColumnKey = "DepartmentID")]
       public virtual IList<User> Users
       {
           get { return users; }
           set { users = value; }
       }
       [HasAndBelongsToMany(typeof(Function), Index = "Department_Function_DepartmentID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_Department_Function", ColumnRef = "FunctionID", ColumnKey = "DepartmentID")]
       public virtual IList<Function> Functions
       {
           get { return functions; }
           set { functions = value; }
       }
    }
}
