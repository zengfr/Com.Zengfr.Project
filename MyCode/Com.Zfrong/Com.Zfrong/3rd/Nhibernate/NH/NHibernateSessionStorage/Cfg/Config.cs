/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/



using System;
using System.Configuration;

namespace Com.Zfrong.Common.Data.NH.SessionStorage.Cfg
{

    /// <summary>
    /// 配置信息帮助类
    /// </summary>
    public class Config
    {
        #region Fields

        private static string NHIBERNATE_SESSION_SOURCE_TYPE = Com.Zfrong.Common.Data.NH.AppSettings.NHibernateSessionSourceType;
        private static string NHIBERNATE_HTTP_SESSION_SOURCE_ITEM_NAME = Com.Zfrong.Common.Data.NH.AppSettings.NHibernateHttpSessionSourceItemName;
        private static string NHIBERNATE_USER_SESSION_SOURCE = Com.Zfrong.Common.Data.NH.AppSettings.NHibernateUserSessionSource;

        #endregion

        #region Properties

        /// <summary>
        /// Session资源源类型;http,threadStatic
        /// </summary>
        public static string SessionSourceType
        {
            get
            {
                return NHIBERNATE_SESSION_SOURCE_TYPE;
            }
        }

        /// <summary>
        /// HttpSessionSource存放HttpContext.Current.Items的键值名
        /// </summary>
        public static string HttpSessionSourceItemName
        {
            get
            {
                return NHIBERNATE_HTTP_SESSION_SOURCE_ITEM_NAME;
            }

        }

        /// <summary>
        /// 是否使用Session资源源
        /// </summary>
        public static bool UserSessionSource
        {
            get
            {
                return Convert.ToBoolean(NHIBERNATE_USER_SESSION_SOURCE);
            }
        }

        #endregion

    }
}