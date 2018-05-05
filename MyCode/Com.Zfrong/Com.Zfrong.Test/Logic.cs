using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using NHibernate;
namespace Test
{
   public class Logic
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(QQGroup));
        public static void ExecuteNonQuery<T>(string sql)
        {
            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.GetSessionFactory(typeof(T)).OpenSession();

            try
            {
                System.Data.IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery(); command.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }

            sessionHolder.ReleaseSession(session);
        }
        public static object ExecuteScalar<T>(string sql)
        {
            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(T));

            object v = 0;
            try
            {
                System.Data.IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = sql;
                log.InfoFormat("执行SQL:{0}", sql);
                v = command.ExecuteScalar();
                command.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }

            sessionHolder.ReleaseSession(session);
            return v;
        }
    }
}
