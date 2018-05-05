using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using System.Security.Principal;
using System.Security;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    [ActiveRecord("[ut_UserRole]")]
    public class UserRole : EntityBaseWithName, IHasFunctions
    {
       
        private UserRole parent;
        private IList<Function> functions;
        private IList<User> users;
        [BelongsTo("ParentID")]
        public UserRole Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        [HasAndBelongsToMany(typeof(Function), Index = "UserRole_Function_UserRoleID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_UserRole_Function", ColumnRef = "FunctionID", ColumnKey = "UserRoleID")]
        public virtual IList<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        [HasAndBelongsToMany(typeof(User), Index = "User_UserRole_UserRoleID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_UserRole", ColumnRef = "UserID", ColumnKey = "UserRoleID")]
        public virtual IList<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }
       
        public UserRole()
        {
            this.ID = -1;
        }

        /// <summary>
        /// Check if the role has the requested access rights.
        /// </summary>
        /// <param name="accessLevel"></param>
        /// <returns></returns>
        //public virtual bool HasPermission(AccessLevel permission)
        //{
        //    return Array.IndexOf(this.Permissions, permission) > -1;
        //}

        //private void TranslatePermissionLevelToAccessLevels()
        //{
        //    ArrayList permissions = new ArrayList();
        //    AccessLevel[] accessLevels = (AccessLevel[])Enum.GetValues(typeof(AccessLevel));

        //    foreach (AccessLevel accesLevel in accessLevels)
        //    {
        //        if ((this.PermissionLevel & (int)accesLevel) == (int)accesLevel)
        //        {
        //            permissions.Add(accesLevel);
        //        }
        //    }
        //    this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
        //}

        //private string GetPermissionsAsString()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    for (int i = 0; i < this._permissions.Length; i++)
        //    {
        //        AccessLevel accessLevel = this._permissions[i];
        //        sb.Append(accessLevel.ToString());
        //        if (i < this._permissions.Length - 1)
        //        {
        //            sb.Append(", ");
        //        }
        //    }

        //    return sb.ToString();
        //}
    }
}