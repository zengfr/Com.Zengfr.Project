/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/

using System;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using System.Collections;
using NHibernate.Criterion;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using System.Text;//
using Com.Zfrong.Common.Data.Core;
namespace Com.Zfrong.Common.Data.NH
{
    public partial class NHHelper
    {
        #region ExecNamedQuery 2
        public static IList ExecNamedQuery(string spname, IDictionary<string, object> idict)
        {
            return HelperBase.ExecNamedQuery(GetSession(), spname, idict);
        }
        public static IList<T> ExecNamedQuery<T>(string spname, IDictionary<string, object> idict)
        {
            return HelperBase.ExecNamedQuery<T>(GetSession(), spname, idict);
        }
        #endregion
        #region ExecNamedQuery Paged 2
        public static IList ExecNamedPagedQuery(string spname, IDictionary<string, object> idict, int pageIndex, int pageSize)
        {
            return HelperBase.ExecNamedPagedQuery(GetSession(), spname, idict, pageIndex, pageSize);//
        }
        public static IList<T> ExecNamedPagedQuery<T>(string spname, IDictionary<string, object> idict, int pageIndex, int pageSize)
        {
            return HelperBase.ExecNamedPagedQuery<T>(GetSession(), spname, idict, pageIndex, pageSize);//
        }
        #endregion

        #region IList ExecQuery 2
        public static IList ExecSQLQuery(string sql)
        {
            return HelperBase.ExecSQLQuery(GetSession(), sql);
        }
        public static IList ExecQuery(string hql)
        {
            return HelperBase.ExecQuery(GetSession(), hql);
        }
        #endregion
        #region IList ExecQuery Paged 2
        public static IList ExecSQLPagedQuery(string sql, int pageIndex, int pageSize)
        {
            ISession session = GetSession();
            return HelperBase.ExecSQLPagedQuery(session, sql, pageIndex, pageSize);
        }
        public static IList ExecPagedQuery(string queryString, int pageIndex, int pageSize)
        {
            ISession session = GetSession();
            return HelperBase.ExecPagedQuery(session, queryString, pageIndex, pageSize);
        }
        #endregion

        #region IList<T> ExecQuery<T> 2
        public static IList<T> ExecSQLQuery<T>(string sql)
        {
            ISession session = GetSession(typeof(T));
            return HelperBase.ExecSQLQuery<T>(session, sql);
        }
        public static IList<T> ExecQuery<T>(string queryString)
        {
            ISession session = GetSession(typeof(T));
            return HelperBase.ExecQuery<T>(session, queryString);
        }
        #endregion
        #region IList<T> ExecQuery<T> Paged 2
        public static IList<T> ExecSQLPagedQuery<T>(string sql, int pageIndex, int pageSize)
        {
            ISession session = GetSession(typeof(T));
            return HelperBase.ExecSQLPagedQuery<T>(session, sql, pageIndex, pageSize);
        }
        public static IList<T> ExecPagedQuery<T>(string queryString, int pageIndex, int pageSize)
        {
            ISession session = GetSession(typeof(T));
            return HelperBase.ExecPagedQuery<T>(session, queryString, pageIndex, pageSize);
        }

        #endregion

        #region Session Exec Command 7
        public static IList Execute(string text)
        {
            return HelperBase.Execute(GetSession(), text);
        }
        public static DataTable ExecuteForDT(string text)
        {
            return HelperBase.ExecuteForDT(GetSession(), text);
        }

        public static IList ExecuteSP(string spName, IDictionary<string, object> param)
        {
            return HelperBase.ExecuteSP(GetSession(), spName, param);
        }
        public static DataTable ExecuteSPForDT(string spName, IDictionary<string, object> param)
        {
            return HelperBase.ExecuteSPForDT(GetSession(), spName, param);
        }

        public static int ExecuteNonQuery(string sql)
        {
            ISession session = GetSession();//
            return HelperBase.ExecuteNonQuery(session, sql);
        }
        public static object ExecuteScalar(string sql)
        {
            ISession session = GetSession();//
            return HelperBase.ExecuteScalar(session, sql);
        }
        public static T ExecuteScalar<T>(string sql)
        {
            ISession session = GetSession();//
            return HelperBase.ExecuteScalar<T>(session, sql);
        }
        #endregion

        public static DataSet ExecuteDataSet(string text)
        {
            return HelperBase.ExecuteDataSet(GetSession(), System.Data.SqlClient.SqlClientFactory.Instance, text);
        }
        public static DataSet ExecuteDataSet(string text, DbProviderFactory factory)
        {
            return HelperBase.ExecuteDataSet(GetSession(), factory, text);
        }

        #region BuildCommand 3
        public static IDbCommand BuildCommand(string text)
        {
            return HelperBase.BuildCommand(GetSession(), text);//
        }
        public static IDbCommand BuildSPCommand(string spName)
        {
            return HelperBase.BuildSPCommand(GetSession(), spName);//
        }
        public static IDbCommand BuildSPCommand(string spName, IDictionary<string, object> param)
        {
            return HelperBase.BuildSPCommand(GetSession(), spName, param);//
        }
        #endregion
        #region BuildQuery 5
        public static ICriteria CreateCriteria(Type targetType)
        {
            return HelperBase.CreateCriteria(GetSession(), targetType);
        }
        public static ISQLQuery CreateSQLQuery(string sql)
        {
            return HelperBase.CreateSQLQuery(GetSession(), sql);
        }
        public static IQuery CreateQuery(string hql)
        {
            return HelperBase.CreateQuery(GetSession(), hql);
        }
        public static IMultiQuery CreateMultiQuery()
        {
            return HelperBase.CreateMultiQuery(GetSession());
        }
        public static IQuery CreateNamedQuery(string spname, IDictionary<string, object> param)
        {
            return HelperBase.CreateNamedQuery(GetSession(), spname, param);
        }
        #endregion
    }
    public partial class NHHelper
    {
        public NHHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #region CommandToDS
        //private static void CommandToDS(DataSet dataSet, IDbCommand cmd)
        //{
        //    CommandToDS(ref dataSet, NHHelper.DbProviderFactory, cmd);
        //}
        //private static void CommandToDS(DataSet dataSet, string tableName, IDbCommand cmd)
        //{
        //    CommandToDS(ref dataSet, tableName, NHHelper.DbProviderFactory, cmd);
        //}  
        #endregion

        #region TransInsert(object item)
        /// <summary>
        /// 将指定的实例插入到数据库中。（事务性操作，适用于可能包含及联处理的情况。）
        /// </summary>
        /// <param name="item">待插入的实例。</param>
        public static void TransInsert(object item)
        {
            ITransaction transaction =GetSession().BeginTransaction();

            try
            {
                GetSession().Save(item);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion
        #region TransUpdate(object item)
        /// <summary>
        /// 将指定的实例更新到数据库中。（事务性操作，适用于可能包含及联处理的情况。）
        /// </summary>
        /// <param name="item">待更新的实例。</param>
        public static void TransUpdate(object item)
        {
            ITransaction transaction = GetSession().BeginTransaction();

            try
            {
                GetSession().Update(item);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion
        #region TransSave(object item)
        /// <summary>
        /// 将指定的实例更新到数据库中。（事务性操作，适用于可能包含及联处理的情况。）
        /// </summary>
        /// <param name="item">待更新的实例。</param>
        public static void TransSave(object item)
        {
            ITransaction transaction = GetSession().BeginTransaction();

            try
            {
                GetSession().Save(item);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
          #endregion
        #region TransSaveOrUpdate(object item)
        /// <summary>
        /// 将指定的实例更新到数据库中。（事务性操作，适用于可能包含及联处理的情况。）
        /// </summary>
        /// <param name="item">待更新的实例。</param>
        public static void TransSaveOrUpdate(object item)
        {
            ITransaction transaction = GetSession().BeginTransaction();

            try
            {
                GetSession().SaveOrUpdate(item);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion
        #region TransDelete(object item)
        /// <summary>
        /// 在数据库中删除指定的实例。（事务性操作，适用于可能包含及联处理的情况。）
        /// </summary>
        /// <param name="item">待删除的实例。</param>
        public static void TransDelete(object item)
        {
            ITransaction transaction = GetSession().BeginTransaction();

            try
            {
                GetSession().Delete(item);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region 属性
        private static ISessionFactory sessionFactory;
        private static DbProviderFactory dbProviderFactory;//


        private static ISession GetSession()
        {
           // SessionFactory.GetCurrentSession();
          return  SessionStorage.NHibernateDatabaseFactory.CreateSession().Session;//
            return  SessionStorage.NHibernateDatabaseFactory.SessionFactory.OpenSession();//
            return SessionStorage.SessionStorageFactory.GetSessionStorage().Get().Session;//
        }
        public static ISession GetSession(Type type)
        {
            return GetSession();//
        }
        public static ISessionFactoryImplementor SessionFactoryImplementor
        {
            get
            {
                return (ISessionFactoryImplementor)SessionFactory;//
            }
        }
        public static ISessionFactory SessionFactory
        {
            get
            {
                if(sessionFactory==null)
                    sessionFactory = GetSession().SessionFactory;// Storage.NHibernateDatabaseFactory.SessionFactory;//
                return sessionFactory;//
            }
            //set {
            //    sessionFactory = value;//
            //}
        }
        public static DbProviderFactory DbProviderFactory
        {
            get
            {
                if (dbProviderFactory == null)
                    dbProviderFactory = System.Data.SqlClient.SqlClientFactory.Instance;//
                return dbProviderFactory;//
            }
            set
            {
                dbProviderFactory = value;//
            }
        }
        
        #endregion
    }
}
