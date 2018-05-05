using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate;
using Castle.ActiveRecord.Framework;
using NHibernate.Cfg;
using Castle.ActiveRecord.Queries;
using System.Data;
using NHibernate.Dialect;//
using NHibernate.Connection;//
using System.Data.Common;
using Com.Zfrong.Common.Data.Core;
namespace Com.Zfrong.Common.Data.AR.Base
{
   public partial class ARHelper
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
    public partial class ARHelper
    {
        public static ISession GetSession()
        {
            return GetSession(typeof(ActiveRecordBase));//
        }
        public static ISession GetSession(Type type)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(type);//
            return session;//
        }
        public static IList<T> ExecMapingNamedQuery<T>(string spname, IDictionary<String, Object> idict)
        {
            Type type = typeof(T);
            string map = BuildMappingXML(type,spname, idict);//
            CreateQueryMapping(type, map);//创建一个SQL-Query

            ISession session = GetSession(type);
            return HelperBase.ExecNamedQuery<T>(session, spname, idict);
        }
        protected static string BuildMappingXML(Type type,string spname, IDictionary<String, Object> idict)
        {
            StringBuilder sp = new StringBuilder();
            foreach (string key in idict.Keys)
            {
                sp.AppendFormat(":{0},", key);
            }
            if (sp.Length != 0)
                sp.Length--;
            string map = String.Format(@"<sql-query name='{0}'><return class='{1}'/>exec {0} {2}</sql-query>",
           spname, type.Name, sp.ToString());
            return map;//
        }
       
        protected static void CreateQueryMapping(Type type, string xml)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            NHibernate.Cfg.Configuration config = holder.GetConfiguration(holder.GetRootType(type));

            //    xml = ;
            config.AddXmlString(
                 string.Format("<hibernate-mapping xmlns='{0}' assembly='{1}' namespace='{2}'>{3}</hibernate-mapping>",
                           "urn:nhibernate-mapping-2.0",
                             type.Assembly.FullName,
                              type.Namespace,//命名空间
                             xml//内容即<Sql-Query />
                         )
                    );
            //return config.NamedSQLQueries.Count.ToString();
        }
    }
}
