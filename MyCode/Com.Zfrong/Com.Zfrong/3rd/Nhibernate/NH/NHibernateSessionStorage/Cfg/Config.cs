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
    /// ������Ϣ������
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
        /// Session��ԴԴ����;http,threadStatic
        /// </summary>
        public static string SessionSourceType
        {
            get
            {
                return NHIBERNATE_SESSION_SOURCE_TYPE;
            }
        }

        /// <summary>
        /// HttpSessionSource���HttpContext.Current.Items�ļ�ֵ��
        /// </summary>
        public static string HttpSessionSourceItemName
        {
            get
            {
                return NHIBERNATE_HTTP_SESSION_SOURCE_ITEM_NAME;
            }

        }

        /// <summary>
        /// �Ƿ�ʹ��Session��ԴԴ
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