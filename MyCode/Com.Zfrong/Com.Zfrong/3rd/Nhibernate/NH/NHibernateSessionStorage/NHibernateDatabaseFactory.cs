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
    /// ��������ISessionʵ���Ĺ���
    /// </summary>
    public static class NHibernateDatabaseFactory
    {
        #region ˽�о�̬����

        private static object m_Locker = new object();
        private static NHibernate.Cfg.Configuration m_Configuration = null;
        private static ISessionFactory m_SessionFactory = null;
        private static ISessionStorage m_Sessionsource;

        #endregion

        #region ��̬���캯��

        static NHibernateDatabaseFactory()
        {
            m_Sessionsource = SessionStorageFactory.GetSessionStorage();
        }

        #endregion

        #region �ڲ���̬����

        /// <summary>
        /// NHibernate���ö���
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
        /// NHibernate�Ķ��󹤳�
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

        #region ��������

        /// <summary>
        /// ����ISessionFactory��ʵ��
        /// </summary>
        /// <returns></returns>
        public static SessionObject CreateSession()
        {
            if (Config.UserSessionSource &&(m_Sessionsource is ThreadSessionSource ||(System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items != null))) //���ʹ�ñ����ISession
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
            else //���ʹ����ISession
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

        #region ˽�з���

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