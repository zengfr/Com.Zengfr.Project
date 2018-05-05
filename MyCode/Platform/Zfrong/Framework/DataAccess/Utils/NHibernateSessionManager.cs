using System;
using System.Collections.Generic;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Runtime.Remoting.Messaging;
namespace Zfrong.Framework.Repository.Utils
{
   public sealed class NHibernateSessionManager
    {
        #region Thread-safe唯一实例
        private NHibernateSessionManager() {}
        private static object locker = new object();
        static NHibernateSessionManager instance;
        public static NHibernateSessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new NHibernateSessionManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
       /// <summary>
       /// 
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
       public ISessionFactory GetSessionFactory(string key) {
            
            ISessionFactory sessionFactory = 
              (ISessionFactory)HttpRuntime.Cache.Get(key);

             if (sessionFactory == null) {
                Configuration cfg =NHibernateConfiguration.Instance.GetConfiguration(key);

                sessionFactory = cfg.BuildSessionFactory();
                if (sessionFactory == null) {
                    throw new InvalidOperationException(
                      "cfg.BuildSessionFactory() returned null.");
                }

                HttpRuntime.Cache.Add(key, 
                            sessionFactory, null, DateTime.Now.AddHours(16),
                    TimeSpan.Zero, CacheItemPriority.High, null);
            }

            return sessionFactory;
        }
       public void RegisterInterceptor(string key, 
                                          IInterceptor interceptor) {
            ISession session = (ISession)contextSessions[key];
            if (session != null && session.IsOpen) {
                throw new Exception("You cannot register an interceptor once a session has already been opened");
            }
            GetSession(key,interceptor);
        }
        public ISession GetSession(string key) {
            return GetSession(key, null);
        }

        private ISession GetSession(string key,IInterceptor interceptor) {
            ISession session = (ISession)contextSessions[key];
            if (session == null) {
                ISessionFactory  sessionFactory = GetSessionFactory(key);
                if (interceptor== null){
                  session = sessionFactory.OpenSession();
                }
                else {
                    session = sessionFactory.OpenSession(interceptor);
                 }
                contextSessions[key] = session;
            }
            
            if (session == null) 
                throw new ApplicationException("session was null");
            
            return session;
        }

        public void CloseSession(string key) {
            ISession session = (ISession)contextSessions[key];
            contextSessions.Remove(key);
            if (session != null && session.IsOpen) {
                session.Close();
            }
        }

        private Hashtable contextTransactions {
            get {
                if (CallContext.GetData("CONTEXT_TRANSACTIONS") == null) {
                    CallContext.SetData("CONTEXT_TRANSACTIONS", new Hashtable());
                }

                return (Hashtable)CallContext.GetData("CONTEXT_TRANSACTIONS");
            }
        }
        private Hashtable contextSessions {
            get {
                if (CallContext.GetData("CONTEXT_SESSIONS") == null) {
                    CallContext.SetData("CONTEXT_SESSIONS", new Hashtable());
                }
                
                return (Hashtable)CallContext.GetData("CONTEXT_SESSIONS");
            }
        }
    }
}
