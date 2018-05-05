using System;
using System.Collections;//
using System.Collections.Generic;
using System.Text;
using NHibernate;
using System.Data;
using System.Data.Common;
using Math = System.Math;
namespace Com.Zfrong.Common.Data.Core
{
    public abstract partial class HelperBase
    {
        #region
        //public static ISession GetSession()
        //{
        //    return null;//
        //}
        //public static ISession GetSession(Type type)
        //{
        //    return null;//
        //}
        #endregion
        #region IList ExecQuery 2
        public static IList ExecSQLQuery(ISession session, string sql)
        {
            ISQLQuery query = CreateSQLQuery(session, sql);
            return query.List();
        }
        public static IList ExecQuery(ISession session, string hql)
        {
            IQuery query = CreateQuery(session, hql);
            return query.List();
        }
        #endregion
        #region IList ExecQuery Paged 2
        public static IList ExecSQLPagedQuery(ISession session, string sql, int pageIndex, int pageSize)
        {
            ISQLQuery query = CreateSQLQuery(session, sql);
            query = CreateSQLPagedQuery(query, pageIndex, pageSize);//
            return query.List();
        }
        public static IList ExecPagedQuery(ISession session, string hql, int pageIndex, int pageSize)
        {
            IQuery query = CreateQuery(session, hql);
            query = CreatePagedQuery(query, pageIndex, pageSize);//
            return query.List();
        }
        #endregion

        #region IList<T> ExecQuery<T> 2
        public static IList<T> ExecSQLQuery<T>(ISession session, string sql)
        {
            ISQLQuery query = CreateSQLQuery(session, sql);
            return query.List<T>();
        }
        public static IList<T> ExecQuery<T>(ISession session, string hql)
        {
            IQuery query = CreateQuery(session, hql);
            return query.List<T>();
        }
        #endregion
        #region IList<T> ExecQuery<T> Paged 2
        public static IList<T> ExecSQLPagedQuery<T>(ISession session, string sql, int pageIndex, int pageSize)
        {
            ISQLQuery query = CreateSQLQuery(session, sql);
            query = CreateSQLPagedQuery(query, pageIndex, pageSize);//
            return query.List<T>();
        }
        public static IList<T> ExecPagedQuery<T>(ISession session, string hql, int pageIndex, int pageSize)
        {
            IQuery query = CreateQuery(session, hql);
            query = CreatePagedQuery(query, pageIndex, pageSize);//
            return query.List<T>();
        }
        #endregion

        #region ExecNamedQuery 2
        public static IList ExecNamedQuery(ISession session, string spname, IDictionary<string, object> idict)
        {
            IQuery query = CreateNamedQuery(session, spname, idict);
            return query.List();
        }
        public static IList<T> ExecNamedQuery<T>(ISession session, string spname, IDictionary<string, object> idict)
        {
            IQuery query = CreateNamedQuery(session, spname, idict);
            return query.List<T>();
        }
        #endregion
        #region ExecNamedQuery Paged 2
        public static IList ExecNamedPagedQuery(ISession session, string spname, IDictionary<string, object> idict, int pageIndex, int pageSize)
        {
            IQuery query = CreateNamedQuery(session, spname, idict);
            query = CreatePagedQuery(query, pageIndex, pageSize);//
            return query.List();
        }
        public static IList<T> ExecNamedPagedQuery<T>(ISession session, string spname, IDictionary<string, object> idict, int pageIndex, int pageSize)
        {
            IQuery query = CreateNamedQuery(session, spname, idict);
            query = CreatePagedQuery(query, pageIndex, pageSize);//
            return query.List<T>();
        }
        #endregion

        #region BuildCommand CreateCommand 4
        public static IDbCommand BuildCommand(ISession session, string text)
        {
            IDbCommand cmd = CreateCommand(session);//
            cmd.CommandText = text;
            return cmd;//
        }
        public static IDbCommand BuildSPCommand(ISession session, string spName)
        {
            IDbCommand cmd = CreateCommand(session);
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;//
        }
        public static IDbCommand BuildSPCommand(ISession session, string spName, IDictionary<string, object> idict)
        {
            IDbCommand cmd = BuildSPCommand(session, spName);//
            // 加入参数
            if (idict != null)
            {
                foreach (KeyValuePair<string, object> kv in idict)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = kv.Key;
                    parameter.Value = kv.Value;
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;//
        }
        public static IDbCommand CreateCommand(ISession session)
        {
            return session.Connection.CreateCommand();//
        }
        #endregion
        #region BuildQuery 7
        public static ICriteria CreateCriteria(ISession session, Type targetType)
        {
            return session.CreateCriteria(targetType);
        }
        public static ISQLQuery CreateSQLQuery(ISession session, string sql)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            return query;//
        }
        public static IQuery CreateQuery(ISession session, string hql)
        {
            IQuery query = session.CreateQuery(hql);
            return query;//
        }
        public static IMultiQuery CreateMultiQuery(ISession session)
        {
            IMultiQuery query = session.CreateMultiQuery();
            return query;//
        }
        public static IQuery CreateNamedQuery(ISession session, string spname, IDictionary<string, object> param)
        {
            IQuery query = session.GetNamedQuery(spname);

            foreach (KeyValuePair<string, object> kv in param)
            {
                query = query.SetParameter(kv.Key, kv.Value);
            }
            return query;//
        }

        public static ISQLQuery CreateSQLPagedQuery(ISQLQuery query, int pageIndex, int pageSize)
        {
            int first = System.Math.Max((pageIndex - 1) * pageSize, 0);
            query.SetFirstResult(first);//
            query.SetMaxResults(pageSize);//
            return query;//
        }
        public static IQuery CreatePagedQuery(IQuery query, int pageIndex, int pageSize)
        {
            int first = System.Math.Max((pageIndex - 1) * pageSize, 0);
            query.SetFirstResult(first);//
            query.SetMaxResults(pageSize);//
            return query;//
        }
        #endregion

        #region CommandToList
        public static void CommandToList(ref IList<IList<object>> result, IDbCommand cmd, bool toClose)
        {
            try
            {
                IDataReader rs = cmd.ExecuteReader();
                ReaderToList(ref result, rs, toClose);//
            }
            catch
            {
                ;//
            }
        }
        public static void CommandToList(ref IList result, IDbCommand cmd, bool toClose)
        {
            try
            {
                IDataReader rs = cmd.ExecuteReader();
                ReaderToList(ref result, rs, toClose);//
            }
            catch
            {
                ;//
            }
        }
        public static void ReaderToList(ref IList result, IDataReader rs, bool toClose)
        {
            while (rs.Read())
            {
                int fieldCount = rs.FieldCount;
                object[] values = new Object[fieldCount];
                for (int i = 0; i < fieldCount; i++)
                    values[i] = rs.GetValue(i);
                result.Add(values);
            }
        }
        public static void ReaderToList(ref IList<IList<object>> result, IDataReader rs, bool toClose)
        {
            while (rs.Read())
            {
                int fieldCount = rs.FieldCount;
                IList<object> values = new List<object>(fieldCount);//
                for (int i = 0; i < fieldCount; i++)
                    values.Add(rs.GetValue(i));
                result.Add(values);
            }
        }
        #endregion
        #region CommandToDS
        public static void CommandToDS(ref DataSet dataSet, DbProviderFactory factory, IDbCommand cmd)
        {
            string tableName = "TableName";
            if (dataSet != null)
                tableName = "Table" + dataSet.Tables.Count;//
            CommandToDS(ref dataSet, tableName, factory.CreateDataAdapter(), cmd);//
        }
        public static void CommandToDS(ref DataSet dataSet, string tableName, DbProviderFactory factory, IDbCommand cmd)
        {
            CommandToDS(ref dataSet, tableName, factory.CreateDataAdapter(), cmd);//
        }
        public static void CommandToDS(ref DataSet dataSet, DbDataAdapter dbDataAdapter, IDbCommand cmd)
        {
            string tableName = "TableName";
            if (dataSet != null)
                tableName = "Table" + dataSet.Tables.Count;//

            CommandToDS(ref  dataSet, tableName, dbDataAdapter, cmd);//
        }
        public static void CommandToDS(ref DataSet dataSet, string tableName, DbDataAdapter dbDataAdapter, IDbCommand cmd)
        {
            if (dataSet == null)
                dataSet = new DataSet();
            dbDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            dbDataAdapter.SelectCommand = cmd as DbCommand;
            dbDataAdapter.Fill(dataSet, tableName);
            dataSet.AcceptChanges();
        }
        #endregion

        #region Convert
        public static IList<T> ListToGList<T>(IList list)
        {
            IList<T> lists = new List<T>();
            foreach (Object obj in list)
            {
                lists.Add((T)obj);
            }
            return lists;
        }
        public static DataTable ListToDT(IList list)
        {
            if (list.Count == 0)
                return null;
            Type t = list[0].GetType();//
            switch (t.Name)
            {
                case "IDictionary":
                    return ListToDT(ListToGList<IDictionary>(list)); break;//
                case "IList":
                    return ListToDT(ListToGList<IList>(list)); break;//
                case "ArrayList":
                //return ListToDT(ListToGList<ArrayList>(list));break;//
                case "Array":
                //return ListToDT(ListToGList<Array>(list));break;//
                case "Object[]":
                    return ListToDT(ListToGList<object[]>(list)); break;//
                default: return null;
            }

        }
        public static DataTable ListToDT(IList<IDictionary> list)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            if (list.Count != 0)
                foreach (object b in list[0].Keys)
                {
                    dt.Columns.Add(new DataColumn(b.ToString(), b.GetType()));
                }

            foreach (Hashtable obj in list)
            {
                dr = dt.NewRow();
                foreach (String key in obj.Keys)
                {
                    dr[key] = obj[key];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static DataTable ListToDT(IList<IList> list)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            if (list.Count != 0)
            {
                for (int i = 0; i < list[0].Count;i++ )
                {
                    dt.Columns.Add(i.ToString(), list[0][i].GetType());
                }
                dr = dt.NewRow();
                dr.ItemArray = list[0] as object[];//
                dt.Rows.Add(dr);
            }
            for (int i = 1; i < list.Count; i++)
            {
                dr = dt.NewRow();
                dr.ItemArray = list[i] as object[];//
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static DataTable ListToDT(IList<object[]> list)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            if (list.Count != 0)
            {
                for (int i = 0; i < list[0].Length; i++)
                {
                    dt.Columns.Add(i.ToString(), list[0][i].GetType());
                }
                dr = dt.NewRow();
                dr.ItemArray = list[0];//
                dt.Rows.Add(dr);
            }
            for (int i = 1; i < list.Count; i++)
            {
                dr = dt.NewRow();
                dr.ItemArray = list[i];//
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #region ListToDS<T>
        /// <summary>
        /// Ilist<T> 转换成 DataSet
        /// http://mhx1982.cnblogs.com/archive/2006/06/14/425686.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet ListToDS<T>(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            string name;
            System.Reflection.PropertyInfo pi;
            foreach (T t in list)
            {
                if (t != null)
                {
                    row = dt.NewRow();
                    for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                    {
                        pi = myPropertyInfo[i]; name = pi.Name;
                        if (dt.Columns[name] == null)
                        {
                            column = new DataColumn(name, pi.PropertyType);
                            dt.Columns.Add(column);
                        }
                        row[name] = pi.GetValue(t, null);
                    }

                    dt.Rows.Add(row);
                }
            }

            ds.Tables.Add(dt);

            return ds;
        }
        #endregion
        #endregion

        #region Execute Command 7
        /// <summary>
        /// 执行SQL语句
        /// from http://www.narchitecture.net/Topics/TopicDetails.aspx?id=p4ti5f
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IList Execute(ISession session, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            IList result = new ArrayList();
            try
            {
                CommandToList(ref result, cmd, false);//
            }
            finally
            {
                cmd.Connection.Close();//
            }

            return result;
        }
        public static DataTable ExecuteForDT(ISession session, string text)
        {
            return ListToDT(Execute(session, text));//
        }

        /// <summary>
        /// 执行存储过程
        /// from http://www.narchitecture.net/Topics/TopicDetails.aspx?id=p4ti5f
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="paramInfos">参数</param>
        /// <returns></returns>
        public static IList ExecuteSP(ISession session, string spName, IDictionary<string, object> param)
        {
            IDbCommand cmd = BuildSPCommand(session, spName, param);//
            IList result = new ArrayList();
            try
            {
                CommandToList(ref result, cmd, false);//
            }
            finally
            {
                cmd.Connection.Close();//
            }

            return result;
        }
        public static DataTable ExecuteSPForDT(ISession session, string spName, IDictionary<string, object> param)
        {
            return ListToDT(ExecuteSP(session, spName, param));//
        }

        public static int ExecuteNonQuery(ISession session, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            return cmd.ExecuteNonQuery();//
        }
        public static object ExecuteScalar(ISession session, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            return cmd.ExecuteScalar();//
        }
        public static T ExecuteScalar<T>(ISession session, string text)
        {
            return (T)ExecuteScalar(session, text);//
        }
        #endregion
        #region ExecuteDataSet 8
        public static DataSet ExecuteDataSet(ISession session, DbProviderFactory factory, string tableName, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            DataSet ds = new DataSet();//
            try
            {
                CommandToDS(ref ds, tableName, factory, cmd);//
            }
            catch
            {

            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }
        public static DataSet ExecuteDataSet(ISession session, DbDataAdapter adapter, string tableName, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            DataSet ds = new DataSet();//
            try
            {
                CommandToDS(ref ds, tableName, adapter, cmd);//
            }
            catch
            {

            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(ISession session, DbProviderFactory factory, string tableName, string spName, IDictionary<string, object> paramInfos)
        {
            IDbCommand cmd = BuildSPCommand(session, spName, paramInfos);//
            DataSet ds = null;//
            try
            {
                CommandToDS(ref ds, tableName, factory, cmd);//
            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }
        public static DataSet ExecuteDataSet(ISession session, DbDataAdapter adapter, string tableName, string spName, IDictionary<string, object> paramInfos)
        {
            IDbCommand cmd = BuildSPCommand(session, spName, paramInfos);//
            DataSet ds = null;//
            try
            {
                CommandToDS(ref ds, tableName, adapter, cmd);//
            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(ISession session, DbProviderFactory factory, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            DataSet ds = new DataSet();//
            try
            {
                CommandToDS(ref ds, factory, cmd);//
            }
            catch
            {

            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }
        public static DataSet ExecuteDataSet(ISession session, DbDataAdapter adapter, string text)
        {
            IDbCommand cmd = BuildCommand(session, text);//
            DataSet ds = new DataSet();//
            try
            {
                CommandToDS(ref ds, adapter, cmd);//
            }
            catch
            {

            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(ISession session, DbProviderFactory factory, string spName, IDictionary<string, object> paramInfos)
        {
            IDbCommand cmd = BuildSPCommand(session, spName, paramInfos);//
            DataSet ds = null;//
            try
            {
                CommandToDS(ref ds, factory, cmd);//
            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }
        public static DataSet ExecuteDataSet(ISession session, DbDataAdapter adapter, string spName, IDictionary<string, object> paramInfos)
        {
            IDbCommand cmd = BuildSPCommand(session, spName, paramInfos);//
            DataSet ds = null;//
            try
            {
                CommandToDS(ref ds, adapter, cmd);//
            }
            finally
            {
                cmd.Connection.Close();//
            }

            return ds;
        }

        #endregion
    }
    
    
    public  abstract partial class HelperBase
   {
      
   }
}
