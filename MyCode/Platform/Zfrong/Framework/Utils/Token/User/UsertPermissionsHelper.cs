using System;
using System.Collections.Generic;
using Zfrong.Framework.Core.DataContract;
using NHibernate;
namespace Zfrong.Framework.Utils.Token.UserService
{

    public class UserPermissionsHelper : IPermissionsHelper
    {
        public static ISessionFactory SessionFactory { get; set; }
        private UserPermissionsHelper() { }
         static  IPermissionsHelper current;
         public static IPermissionsHelper Current
        {
            get { 
                if(current==null)
                    current = new UserPermissionsHelper();
                return current;
            }
        }
        #region  IPermissionsHelper
        public virtual long Login(string user, string pwd)
       {
            long userID = 0;
            if (string.IsNullOrEmpty(user) || user.Length <8)
                return userID;
            if (string.IsNullOrEmpty(pwd) || pwd.Length <10)
                return userID;
            ISession session=SessionFactory.GetCurrentSession();
            string sql = "select ID from ut_user where password=:pwd and (phone=:user or email=:user or tel=:user)";
            IQuery query=session.CreateSQLQuery(sql);
            query.SetString("pwd", pwd);
            query.SetString("user", user);
            query.SetMaxResults(1);
            object data= query.UniqueResult();
            if(data!=null)
                long.TryParse(data.ToString(),out userID);
            return userID;
       }
        public virtual string GetPermissions(long agentID)    
       {    
           string permissions=string.Empty;
           return permissions;
       }
        #endregion
    }
}
