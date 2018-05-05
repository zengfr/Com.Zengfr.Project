namespace DB
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class SqlHelperParameterCache
    {
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private SqlHelperParameterCache()
        {
        }

        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            string str = connectionString + ":" + commandText;
            paramCache[str] = commandParameters;
        }

        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] parameterArray = new SqlParameter[originalParameters.Length];
            int index = 0;
            int length = originalParameters.Length;
            while (index < length)
            {
                parameterArray[index] = (SqlParameter) ((ICloneable) originalParameters[index]).Clone();
                index++;
            }
            return parameterArray;
        }

        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] parameterArray2;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(spName, connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(command);
                    if (!includeReturnValueParameter)
                    {
                        command.Parameters.RemoveAt(0);
                    }
                    SqlParameter[] parameterArray = new SqlParameter[command.Parameters.Count];
                    command.Parameters.CopyTo((Array) parameterArray, 0);
                    parameterArray2 = parameterArray;
                }
            }
            return parameterArray2;
        }

        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string str = connectionString + ":" + commandText;
            SqlParameter[] originalParameters = (SqlParameter[]) paramCache[str];
            if (originalParameters == null)
            {
                return null;
            }
            return CloneParameters(originalParameters);
        }

        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }
        public static SqlParameter[] GetSpParameterSet(string spName)
        {
            return GetSpParameterSet(SqlHelper.ConnectionString, spName, false);
        }
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string str = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            SqlParameter[] originalParameters = (SqlParameter[]) paramCache[str];
            if (originalParameters == null)
            {
                object obj2;
                paramCache[str] = obj2 = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter);
                originalParameters = (SqlParameter[]) obj2;
            }
            return CloneParameters(originalParameters);
        }
    }
}

