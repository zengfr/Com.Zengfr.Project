/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/



using System;
using NHibernate;
using Com.Zfrong.Common.Data.NH.SessionStorage.Cfg;
using NHibernate.Cfg;
/// <summary>
/// NHibernateDatabaseFactory 
/// </summary>
namespace Com.Zfrong.Common.Data.NH.SessionStorage
{

    /// <summary>
    /// 用来生成ISession实例的工厂
    /// </summary>
    public static class NHibernateDatabaseFactory
    {
        #region 私有静态变量

        private static object m_Locker = new object();
        private static NHibernate.Cfg.Configuration m_Configuration = null;
        private static ISessionFactory m_SessionFactory = null;
        private static ISessionStorage m_Sessionsource;

        #endregion

        #region 静态构造函数

        static NHibernateDatabaseFactory()
        {
            m_Sessionsource = SessionStorageFactory.GetSessionStorage();
        }

        #endregion

        #region 内部静态变量

        /// <summary>
        /// NHibernate配置对象
        /// </summary>
        public static NHibernate.Cfg.Configuration Configuration
        {
            get
            {
                lock (m_Locker)
                {
                    if (m_Configuration == null)
                    {
                        CreateConfiguration();
                    }
                    return m_Configuration;
                }
            }
            set { m_Configuration = value; }
        }

        /// <summary>
        /// NHibernate的对象工厂
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (null == m_SessionFactory)
                {
                    if (m_Configuration == null)
                    {
                        CreateConfiguration();
                    }
                    lock (m_Locker)
                    {
                        m_SessionFactory = Configuration.BuildSessionFactory();
                    }
                }

                return m_SessionFactory;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 建立ISessionFactory的实例
        /// </summary>
        /// <returns></returns>
        public static SessionObject CreateSession()
        {
            if (Config.UserSessionSource &&(m_Sessionsource is ThreadSessionSource ||(System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items != null))) //如果使用保存的ISession
            {//zfr
                SessionObject sessionObj = m_Sessionsource.Get();

                if (sessionObj == null || sessionObj.Session == null || !sessionObj.Session.IsOpen)
                {
                    sessionObj = CreateNewSessionObject();
                    sessionObj.IsNeedClose = false;

                    m_Sessionsource.Set(sessionObj);
                }

                if (!sessionObj.Session.IsConnected)
                    sessionObj.Session.Reconnect();

                return sessionObj;


            }
            else //如果使用新ISession
            {
                return CreateNewSessionObject();
            }
        }

        private static SessionObject CreateNewSessionObject()
        {
            ISession s = SessionFactory.OpenSession();
            return new SessionObject(s, true);
        }

        #endregion

        #region 私有方法

        private static void CreateConfiguration()
        {
            string filename;
            if (m_Sessionsource is HttpSessionSource)
                filename = System.Web.HttpContext.Current.Request.MapPath("~") + @"\hibernate.cfg.xml";
            else
                filename = System.AppDomain.CurrentDomain.BaseDirectory + @"\hibernate.cfg.xml";//zfr
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure(filename);

            m_Configuration = cfg;
            // Add interceptor, if you need to.
            // _config.Interceptor = new Interceptor();
        }

        #endregion
    }
}