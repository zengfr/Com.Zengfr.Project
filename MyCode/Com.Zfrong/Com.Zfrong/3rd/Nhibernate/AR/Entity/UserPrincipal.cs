using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using System.Security.Principal;
using System.Security;
using Com.Zfrong.Common.Data.AR.Entity;
namespace Com.Zfrong.Common.Data.AR.Base
{
    public class UserPrincipal : IPrincipal
    {
        public UserPrincipal(User user)
        {
            if (user != null && user.IsAuthenticated)
            {
                this._user = user;
            }
            else
            {
                throw new SecurityException("Cannot create a principal without u valid user");
            }
        }

        private User _user;
        public IIdentity Identity
        {
            get { return this._user; }
        }
        #region ###Role
        public bool IsInRole(string roleName)
        {
            foreach (UserRole role in this._user.UserRoles)
            {
                if (role.Name.Equals(roleName))
                    return true;
                if (IsInRole(role.Parent, roleName))
                    return true;
            }
            return false;
        }
        public static bool IsInRole(UserRole role, string roleName)
        {
            if (role == null)
                return false;
            if (role.Name.Equals(roleName))
                return true;
            if (IsInRole(role.Parent, roleName))
                return true;
            return false;
        }
        #endregion
        #region ###Group
        public bool IsInGroup(int groupID)
        {
            foreach (UserGroup userGroup in this._user.UserGroups)
            {
                if (userGroup.ID.Equals(groupID))
                    return true;
                if (IsInGroup(userGroup.Parent, groupID))
                    return true;
            }
            return false;
        }
        public static bool IsInGroup(UserGroup userGroup, int groupID)
        {
            if (userGroup == null)
                return false;
            if (userGroup.ID.Equals(groupID))
                return true;
            if (IsInGroup(userGroup.Parent, groupID))
                return true;
            return false;
        }
        #endregion
        #region ###Department
        public bool IsInDepartment(int departmentID)
        {
            foreach (Department department in this._user.Departments)
            {
                if (department.ID.Equals(departmentID))
                    return true;
                if (IsInDepartment(department.Parent, departmentID))
                    return true;
            }
            return false;
        }
        public static bool IsInDepartment(Department department, int departmentID)
        {
            if (department == null)
                return false;
            if (department.ID.Equals(departmentID))
                return true;
            if (IsInDepartment(department.Parent, departmentID))
                return true;
            return false;
        }
        #endregion
        #region ### Function
        public bool IsInFunction(string funCode)
        {
            return IsInFunction(this._user, funCode);
        }
        public static bool IsInFunction(User user, string funCode)
        {
            foreach (Function fun in user.Functions)
            {
                if (fun.Code.Equals(funCode))
                    return true;
            }
            foreach (UserGroup userGroup in user.UserGroups)
            {
                if (IsInFunction(userGroup, funCode))
                    return true;
            }
            foreach (UserRole role in user.UserRoles)
            {
                if (IsInFunction(role, funCode))
                    return true;
            }
            foreach (Department department in user.Departments)
            {
                if (IsInFunction(department, funCode))
                    return true;
            }
            return false;
        }
        public static bool IsInFunction(UserGroup userGroup, string funCode)
        {
            if (userGroup == null)
                return false;
            foreach (Function fun in userGroup.Functions)
            {
                if (fun.Code.Equals(funCode))
                    return true;
            }
            if (IsInFunction(userGroup.Parent, funCode))
                return true;
            return false;
        }
        public static bool IsInFunction(UserRole role, string funCode)
        {
            if (role == null)
                return false;
            foreach (Function fun in role.Functions)
            {
                if (fun.Code.Equals(funCode))
                    return true;
            }
            if (IsInFunction(role.Parent, funCode))
                return true;
            return false;
        }
        public static bool IsInFunction(Department department, string funCode)
        {
            if (department == null)
                return false;
            foreach (Function fun in department.Functions)
            {
                if (fun.Code.Equals(funCode))
                    return true;
            }
            if (IsInFunction(department.Parent, funCode))
                return true;
            return false;
        }
        #endregion

    }
}