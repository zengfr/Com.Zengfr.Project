
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using System.Reflection;
using IBatisNet.DataMapper.Exceptions;
using Com.Zfrong.Common.Data.IBatisNet;
namespace Com.Zfrong.Common.Data.IBatisNet.Persistence.MapperDao
{
   
	/// <summary>
	/// Summary description for BaseSqlMapDao.
	/// </summary>
    [Serializable]
	public class BaseSqlMapDao :IBaseDao , IDao
    {
        protected virtual ISqlMapper GetLocalSqlMap()
		{
			IDaoManager daoManager = DaoManager.GetInstance(this);
			SqlMapDaoSession sqlMapDaoSession = (SqlMapDaoSession)daoManager.LocalDaoSession;
			return sqlMapDaoSession.SqlMap;
		}

        protected virtual IList ExecuteQueryForList(string statementName, object parameterObject)
		{
			ISqlMapper sqlMap = GetLocalSqlMap();
			try 
			{
				return sqlMap.QueryForList(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new IBatisNetException("Error executing query '"+statementName+"' for list.  Cause: " + e.Message, e);
			}
		}
        protected virtual IList ExecuteQueryForList(string statementName, object parameterObject, int skipResults, int maxResults) 
		{
			ISqlMapper sqlMap = GetLocalSqlMap();
			try 
			{
				return sqlMap.QueryForList(statementName, parameterObject, skipResults, maxResults);
			} 
			catch (Exception e) 
			{
				throw new IBatisNetException("Error executing query '"+statementName+"' for list.  Cause: " + e.Message, e);
			}
		}

        protected virtual IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject)
        {
           ISqlMapper sqlMap = GetLocalSqlMap();
            try
            {
                return sqlMap.QueryForList<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new IBatisNetException("Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }
        protected virtual IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            ISqlMapper sqlMap = GetLocalSqlMap();
            try
            {
                return sqlMap.QueryForList<T>(statementName, parameterObject, skipResults, maxResults);
            }
            catch (Exception e)
            {
                throw new IBatisNetException("Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }

        protected virtual IDictionary<K, V> ExecuteQueryForDictionary<K, V>(string statementName, object parameterObject, string keyProperty)
        {
            ISqlMapper sqlMap = GetLocalSqlMap();

            try
            {
                return sqlMap.QueryForDictionary<K,V>(statementName, parameterObject,keyProperty);
            }
            catch (Exception e)
            {
                throw new IBatisNetException("Error executing query '" + statementName + "' for Dictionary.  Cause: " + e.Message, e);
            }
        }
        protected virtual IDictionary<K, V> ExecuteQueryForDictionary<K, V>(string statementName, object parameterObject, string keyProperty, string valueProperty)
        {
            ISqlMapper sqlMap = GetLocalSqlMap();

            try
            {
                return sqlMap.QueryForDictionary<K, V>(statementName, parameterObject, keyProperty, valueProperty);
            }
            catch (Exception e)
            {
                throw new IBatisNetException("Error executing query '" + statementName + "' for Dictionary<K, V>.  Cause: " + e.Message, e);
            }
        }
        
        #region Update/Insert/Delete
        protected T ExecuteQueryForObject<T>(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = GetLocalSqlMap();

            try
            {
                return sqlMap.QueryForObject<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new IBatisNetException("Error executing query '" + statementName + "' for object.  Cause: " + e.Message, e);
            }
        }
        protected int ExecuteUpdate(string statementName, object parameterObject) 
		{
			ISqlMapper sqlMap = GetLocalSqlMap();

			try 
			{
				return sqlMap.Update(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new IBatisNetException("Error executing query '"+statementName+"' for update.  Cause: " + e.Message, e);
			}
		}
		protected object ExecuteInsert(string statementName, object parameterObject) 
		{
			ISqlMapper sqlMap = GetLocalSqlMap();

			try 
			{
				return sqlMap.Insert(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new IBatisNetException("Error executing query '"+statementName+"' for insert.  Cause: " + e.Message, e);
			}
		}
        protected int ExecuteDelete(string statementName, object parameterObject)
        {
            ISqlMapper sqlMap = GetLocalSqlMap();

            try
            {
                return sqlMap.Delete(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new IBatisNetException("Error executing query '" + statementName + "' for Delete.  Cause: " + e.Message, e);
            }
        }
        #endregion

        #region Paginated/∏ﬂ–ß∑÷“≥
        protected virtual IList<T> ExecutePaginatedQueryForList<T>(string statementName, object paramObject, int pageIndex, int pageSize)
        {
            ISqlMapper mapper = GetLocalSqlMap();
            return PaginatedHelper.ExecuteQueryForList<T>(mapper, statementName, paramObject, pageIndex, pageSize, null);
        }
        protected virtual IList<T> ExecutePaginatedQueryForList<T>(string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            ISqlMapper mapper = GetLocalSqlMap();
            return PaginatedHelper.ExecuteQueryForList<T>(mapper, statementName, paramObject, pageIndex, pageSize,sort);
        }
        public virtual DataSet ExecutePaginatedQueryForDataSet(string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            ISqlMapper mapper = GetLocalSqlMap();
            return PaginatedHelper.ExecuteQueryForDataSet(mapper, statementName, paramObject, pageIndex, pageSize, sort);
        }
        public virtual DataTable ExecutePaginatedQueryForDataTable(string statementName, object paramObject, int pageIndex, int pageSize, string sort)
        {
            ISqlMapper mapper = GetLocalSqlMap();
            return PaginatedHelper.ExecuteQueryForDataTable(mapper, statementName, paramObject, pageIndex, pageSize, sort);
        }
        #endregion

    }
}
