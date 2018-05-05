using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.DataMapper;
using IBatisNet.Common;
using IBatisNet.Common.Pagination;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.MappedStatements;
using System.Data;
using IBatisNet.DataAccess;//
namespace Com.Zfrong.Common.Data.IBatisNet
{
   public class IBatisNetHelper
    {
       protected const int PAGE_SIZE = 10;
       public static IPaginatedList ToPaginatedList<T>(IEnumerable<T> objs)
        {
            return ToPaginatedList<T>(objs, PAGE_SIZE);
        }
       public static IPaginatedList ToPaginatedList<T>(IEnumerable<T> objs, int pageSize)
        {
            IPaginatedList list = new PaginatedArrayList(pageSize);
            foreach (T obj in objs)
                list.Add(obj);//
            return list;//
        }
       public static string GetSql(ISqlMapper mapper, string statementName, object paramObject)
        {
            //SqlMapper mapper = GetLocalSqlMap();// Mapper.Instance();
            IMappedStatement statement = mapper.GetMappedStatement(statementName);
            if (!mapper.IsSessionStarted)
            {
                mapper.OpenConnection();
            }
            RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
            string sql = scope.PreparedStatement.PreparedSql;
            mapper.CloseConnection();//
            return sql;//
        }
       public static IDbCommand GetDbCommand(ISqlMapper mapper, string statementName, object paramObject)
       {
           IMappedStatement mappedsStatement = mapper.GetMappedStatement(statementName);
           RequestScope request = mappedsStatement.Statement.Sql.GetRequestScope(mappedsStatement, paramObject, mapper.LocalSession);
           mappedsStatement.PreparedCommand.Create(request, mapper.LocalSession, mappedsStatement.Statement, paramObject);
           IDbCommand command2;
           using (IDbCommand command = request.IDbCommand)
           {
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
       public static DataTable ConvertToDataTable(IDbCommand command)
       {
           DataTable obj = new DataTable();
           using (IDbConnection conn= command.Connection)
           {
               conn.Open();
               IDataReader reader = command.ExecuteReader();
               obj.Load(reader);
               conn.Close();
           }
           return obj;
       }
       public static DataSet ConvertToDataSet(ISqlMapper mapper, IDbCommand command)
       {
           DataSet obj = new DataSet();
           mapper.LocalSession.CreateDataAdapter(command).Fill(obj);
           return obj;
       }
       public static void InitScript(IDaoManager daoManager, string script)
       { 
          InitScript(daoManager.LocalDataSource, script);
       }
       protected static void InitScript(IDataSource datasource, string script)
       {
           ScriptRunner runner = new ScriptRunner();

           runner.RunScript(datasource, script);
       }
    }
}
