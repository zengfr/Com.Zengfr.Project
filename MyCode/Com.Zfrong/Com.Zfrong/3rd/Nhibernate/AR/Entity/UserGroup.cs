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
    [ActiveRecord("ut_UserGroup")]
    public class UserGroup : EntityBaseWithName, IHasFunctions
    {
        private UserGroup parent;
       private IList<User> users;
       private IList<Function> functions;
        [BelongsTo("ParentID")]
       public virtual UserGroup Parent
        {
            get { return parent; }
            set { parent = value; }
        }
       [HasAndBelongsToMany(typeof(User), Index = "User_UserGroup_UserGroupID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_UserGroup", ColumnRef = "UserID", ColumnKey = "UserGroupID")]
       public virtual IList<User> Users
       {
           get { return users; }
           set { users = value; }
       }
       [HasAndBelongsToMany(typeof(Function), Index = "UserGroup_Function_UserGroupID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_UserGroup_Function", ColumnRef = "FunctionID", ColumnKey = "UserGroupID")]
       public virtual IList<Function> Functions
       {
           get { return functions; }
           set { functions = value; }
       }
    }
}
