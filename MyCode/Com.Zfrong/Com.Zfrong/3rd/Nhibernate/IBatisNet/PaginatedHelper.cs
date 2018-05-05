using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Pagination;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.DaoSessionHandlers;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataMapper.MappedStatements;

using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Scope;
using System.Data;
using IBatisNet.DataMapper.MappedStatements.ResultStrategy;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements.PostSelectStrategy;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Commands;
using IBatisNet.Common.Logging;
using System.Collections.Specialized;
using System.Reflection;
using IBatisNet.DataMapper.Exceptions;
namespace Com.Zfrong.Common.Data.IBatisNet
{
    public class PaginatedHelper
    {
        public static IList<T> ExecuteQueryForList<T>(ISqlMapper mapper, string statementName, object paramObject, int pageIndex, int pageSize)
        {
            return ExecuteQueryForList<T>(mapper, statementName, paramObject, pageIndex, pageSize, null);
        }
        public static IList<T> ExecuteQueryForList<T>(ISqlMapper mapper, string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            IMappedStatement mappedsStatement = mapper.GetMappedStatement(statementName);
            RequestScope request = mappedsStatement.Statement.Sql.GetRequestScope(mappedsStatement, paramObject, mapper.LocalSession);
            mappedsStatement.PreparedCommand.Create(request, mapper.LocalSession, mappedsStatement.Statement, paramObject);
            return ExecuteQueryForList<T>(request, mapper.LocalSession, mappedsStatement.Statement, paramObject, pageIndex, pageSize, sort);
        }
        #region DataTable/DataSet
        public static DataTable ExecuteQueryForDataTable(ISqlMapper mapper, string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            DataTable obj = null;//
            IDbCommand command = ExecuteQueryForDbCommand(mapper, statementName, paramObject, pageIndex, pageSize, sort);
            obj = IBatisNetHelper.ConvertToDataTable(command);//
            return obj;//
        }
        public static DataSet ExecuteQueryForDataSet(ISqlMapper mapper, string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            DataSet obj = null;//
            IDbCommand command = ExecuteQueryForDbCommand(mapper, statementName, paramObject, pageIndex, pageSize, sort);
            obj = IBatisNetHelper.ConvertToDataSet(mapper, command);//
            return obj;//
        }
        internal static IDbCommand ExecuteQueryForDbCommand(ISqlMapper mapper, string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            IMappedStatement mappedsStatement = mapper.GetMappedStatement(statementName);
            RequestScope request = mappedsStatement.Statement.Sql.GetRequestScope(mappedsStatement, paramObject, mapper.LocalSession);
            mappedsStatement.PreparedCommand.Create(request, mapper.LocalSession, mappedsStatement.Statement, paramObject);
            string sql;
            IDbCommand command2;
            using (IDbCommand command = request.IDbCommand)
            {
                sql = command.CommandText;
                sql = BuildPaginatedSql(sql, pageIndex, pageSize, sort);//
                command.CommandText = sql;

                command2 = mapper.LocalSession.CreateCommand(CommandType.Text);
                command2.CommandText = command.CommandText;
                IDbDataParameter para;
                foreach (IDataParameter pa in command.Parameters)
                {
                    para = mapper.LocalSession.CreateDataParameter();
                    para.ParameterName = pa.ParameterName;
                    para.Value = pa.Value;
                    command2.Parameters.Add(para);
                }

            }
            return command2;//
        }
        #endregion

        protected static IList<T> ExecuteQueryForList<T>(RequestScope request, ISqlMapSession session, IStatement statement, object parameterObject, int pageIndex, int pageSize, string sort)
        {
            IList<T> list;
            if (statement.ListClass == null)
                list = new List<T>();
            else
                list = statement.CreateInstanceOfGenericListClass<T>();

            IResultStrategy resultStrategy = ResultStrategyFactory.Get(statement);
            string sql; IDataReader reader; object obj;
            using (IDbCommand command = request.IDbCommand)
            {
                sql = command.CommandText;
                //ÐÞ¸Äexec
                //sql=" [Paging] @sql = N'" +sql +"',@sort = N'IDID',@PageIndex = " + pageIndex + ",@PageSize = " + pageSize;
                sql = BuildPaginatedSql(sql, pageIndex, pageSize, sort);//
                command.CommandText = sql;
                reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        obj = resultStrategy.Process(request, ref reader, null);
                        if (obj != BaseStrategy.SKIP)
                            list.Add((T)obj);
                    }
                }
                catch { throw; }
                finally
                {
                    reader.Close(); reader.Dispose();
                }
                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, command, parameterObject);
            }
            return list;
        }

        internal static string BuildPaginatedSql(string sql, int pageIndex, int pageSize)
        {
            return BuildPaginatedSql(sql, pageIndex, pageSize, null);
        }
        internal static string BuildPaginatedSql(string sql, int pageIndex, int pageSize, string sort)
        {
            int m = pageIndex * pageSize;
            int n = m + pageSize - 1;
            if (sort == null) sort = Sort_RAND;//
            string strSql = "select * from (select ZFR_T.*, ROW_NUMBER() OVER(ORDER BY " + sort + ") AS ZFR_rownum from (" + sql + ") as ZFR_T) as ZFR_TT " +
                "where ZFR_rownum BETWEEN " + m + " and " + n + " order by " + sort;
            return strSql;//
        }

        public const string Sort_RAND = "RAND()";
        public const string Sort_NEWID = "NEWID()";
        #region ##123456##
        private static void ExecutePostSelect(RequestScope request)
        {
            while (request.QueueSelect.Count > 0)
            {
                PostBindind postSelect = request.QueueSelect.Dequeue() as PostBindind;

                PostSelectStrategyFactory.Get(postSelect.Method).Execute(postSelect, request);
            }
        }
        private static void RetrieveOutputParameters(RequestScope request, ISqlMapSession session, IDbCommand command, object result)
        {
            if (request.ParameterMap != null)
            {
                int count = request.ParameterMap.PropertiesList.Count;
                for (int i = 0; i < count; i++)
                {
                    ParameterProperty mapping = request.ParameterMap.GetProperty(i);
                    if (mapping.Direction == ParameterDirection.Output ||
                        mapping.Direction == ParameterDirection.InputOutput)
                    {
                        string parameterName = string.Empty;
                        if (session.DataSource.DbProvider.UseParameterPrefixInParameter == false)
                        {
                            parameterName = mapping.ColumnName;
                        }
                        else
                        {
                            parameterName = session.DataSource.DbProvider.ParameterPrefix +
                                mapping.ColumnName;
                        }

                        if (mapping.TypeHandler == null) // Find the TypeHandler
                        {
                            lock (mapping)
                            {
                                if (mapping.TypeHandler == null)
                                {
                                    Type propertyType = ObjectProbe.GetMemberTypeForGetter(result, mapping.PropertyName);

                                    mapping.TypeHandler = request.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(propertyType);
                                }
                            }
                        }

                        // Fix IBATISNET-239
                        //"Normalize" System.DBNull parameters
                        IDataParameter dataParameter = (IDataParameter)command.Parameters[parameterName];
                        object dbValue = dataParameter.Value;

                        object value = null;

                        bool wasNull = (dbValue == DBNull.Value);
                        if (wasNull)
                        {
                            if (mapping.HasNullValue)
                            {
                                value = mapping.TypeHandler.ValueOf(mapping.GetAccessor.MemberType, mapping.NullValue);
                            }
                            else
                            {
                                value = mapping.TypeHandler.NullValue;
                            }
                        }
                        else
                        {
                            value = mapping.TypeHandler.GetDataBaseValue(dataParameter.Value, result.GetType());
                        }

                        request.IsRowDataFound = request.IsRowDataFound || (value != null);

                        request.ParameterMap.SetOutputParameter(ref result, mapping, value);
                    }
                }
            }
        }
        #endregion
    }
}
