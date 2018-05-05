using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using System.Security.Principal;
using System.Security;
using Com.Zfrong.Common.Data.AR.Base;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    //[ActiveRecord]
    public class UserBase : EntityBaseWithTime
    {
        private string _userName;
        private string _password;
        private string _email;
        private string _nickName;
        [Property]
        public virtual string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }
        [Property]
        public virtual string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }
        [Property]
        public virtual string Email
        {
            get { return this._email; }
            set { this._email = value; }
        }
        [Property]
        public virtual string NickName
        {
            get { return this._nickName; }
            set { this._nickName = value; }
        }
    }
    [ActiveRecord("[ut_User]",Lazy=true,DynamicUpdate=true,BatchSize=20)]
    public partial class User : UserBase, IIdentity, IHasFunctions
    {
        #region
        
        private DateTime? _lastLogin;
        private string _lastIp;
        private bool _isAuthenticated;

        private IList<UserRole> userRoles;
        private IList<Function> functions;
        private IList<UserGroup> userGroups;
        private IList<Department> departments;

        private IList<User> friends;
        private UserInfo userInfo;
       
        #endregion
        #region
        [Property(SqlType = "smallDateTime")]
        public virtual DateTime? LastLogin
        {
            get { return this._lastLogin; }
            set { this._lastLogin = value; }
        }
        [Property]
        public virtual string LastIP
        {
            get { return this._lastIp; }
            set { this._lastIp = value; }
        }
        //[Nested]
        [Display][OneToOne]
        public virtual UserInfo UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; }
        }
        [HasAndBelongsToMany(typeof(UserRole),BatchSize=20, Index = "User_UserRole_UserID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_UserRole", ColumnRef = "UserRoleID", ColumnKey = "UserID")]
        public virtual IList<UserRole> UserRoles
        {
            get { return this.userRoles; }
            set { this.userRoles = value; }
        }
        [HasAndBelongsToMany(typeof(Function), BatchSize = 20, Index = "User_Function_UserID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_Function", ColumnRef = "FunctionID", ColumnKey = "UserID")]
        public virtual IList<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }
        [HasAndBelongsToMany(typeof(UserGroup), BatchSize = 20, Index = "User_UserGroup_UserID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_UserGroup", ColumnRef = "UserGroupID", ColumnKey = "UserID")]
        public virtual IList<UserGroup> UserGroups
        {
            get { return userGroups; }
            set { userGroups = value; }
        }
        [HasAndBelongsToMany(typeof(Department), BatchSize = 20, Index = "User_Department_UserID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_Department", ColumnRef = "DepartmentID", ColumnKey = "UserID")]
        public virtual IList<Department> Departments
        {
            get { return departments; }
            set { departments = value; }
        }
        [HasAndBelongsToMany(typeof(User), BatchSize = 20, Index = "User_Friend_UserID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_Friend", ColumnRef = "FriendID", ColumnKey = "UserID")]
        public virtual IList<User> Friends
        {
            get { return friends; }
            set { friends = value; }
        }
        #endregion

        #region
        public virtual bool IsAuthenticated
        {
            get { return this._isAuthenticated; }
            set { this._isAuthenticated = value; }
        }

        public virtual string Name
        {
            get
            {
                if (this._isAuthenticated)
                    return this.ID.ToString();
                else
                    return "";
            }
        }

        /// <summary>
        /// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
        /// </summary>
        public virtual string AuthenticationType
        {
            get { return "UserAuthentication"; }
        }
        #endregion
    }
}
