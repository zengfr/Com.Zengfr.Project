namespace Com.Zfrong.Common.Data.SQL
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Xml;

    public sealed class SqlHelper
    {
        private static string pConnectionString;
        private static string pParametersReplaceChar;

        private SqlHelper()
        {
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters != null) && (parameterValues != null))
            {
                if (commandParameters.Length != parameterValues.Length)
                {
                    throw new ArgumentException("Parameter count does not match Parameter Value count.");
                }
                int index = 0;
                int length = commandParameters.Length;
                while (index < length)
                {
                    commandParameters[index].Value = parameterValues[index];
                    index++;
                }
            }
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter parameter in commandParameters)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                command.Parameters.Add(parameter);
            }
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            command.Parameters.Clear();
            command.Dispose();
            adapter.Dispose();
            return dataSet;
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            command.Parameters.Clear();
            adapter.Dispose();
            command.Dispose();
            return dataSet;
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        public static DataTable ExecuteDataTable(string commandText)
        {
            
            return ExecuteDataset(pConnectionString, CommandType.Text, commandText).Tables[0];
        }
        public static DataTable ExecuteDataTable(string spName, params object[] parameterValues)
        {
            return ExecuteDataset(pConnectionString, spName, parameterValues).Tables[0];
        }
        public static DataSet ExecuteDataset(string spName, params object[] parameterValues)
        {
            return ExecuteDataset(pConnectionString, spName, parameterValues);//
        }
        public static DataSet ExecuteDataset(string spName, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(pConnectionString, CommandType.StoredProcedure, spName, commandParameters);//
        }
        public static object ExecuteObject(string commandText)
        {
            
            return ExecuteDataTable(commandText).Rows[0][0];
        }

        public static int ExecuteInt32(string commandText)
        {
            return Convert.ToInt32(ExecuteObject(commandText));
        }

        public static DataTable ExecuteDataTable(string commandText, bool WithParameters)
        {
            SqlConnection connection = new SqlConnection(pConnectionString);
            connection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.Connection = connection;
            string str = commandText;
            if (WithParameters)
            {
                str = "";
                int num = 0;
                for (int i = 0; i < commandText.Length; i++)
                {
                    if ((i < ((commandText.Length - 1) - pParametersReplaceChar.Length)) && (commandText.Substring(i, pParametersReplaceChar.Length) == pParametersReplaceChar))
                    {
                        i += pParametersReplaceChar.Length;
                        string str2 = "";
                        while (commandText.Substring(i, pParametersReplaceChar.Length) != pParametersReplaceChar)
                        {
                            str2 = str2 + commandText.Substring(i, 1);
                            i++;
                        }
                        i += pParametersReplaceChar.Length - 1;
                        str = str + "@" + num.ToString();
                        selectCommand.Parameters.Add("@" + num.ToString(), str2);
                        num++;
                    }
                    else
                    {
                        str = str + commandText.Substring(i, 1);
                    }
                }
            }
            selectCommand.CommandText = str;
            SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            selectCommand.Dispose();
            adapter.Dispose();
            connection.Close();
            connection.Dispose();
            return dataSet.Tables[0];
        }

        public static int ExecuteNonQuery(string commandText)
        {
            
            return ExecuteNonQuery(pConnectionString, CommandType.Text, commandText);
        }

        public static void ExecuteNonQuery(string commandText, bool WithParameters)
        {
            SqlConnection connection = new SqlConnection(pConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            string str = commandText;
            if (WithParameters)
            {
                str = "";
                int num = 0;
                for (int i = 0; i < commandText.Length; i++)
                {
                    if ((i < ((commandText.Length - 1) - pParametersReplaceChar.Length)) && (commandText.Substring(i, pParametersReplaceChar.Length) == pParametersReplaceChar))
                    {
                        i += pParametersReplaceChar.Length;
                        string str2 = "";
                        while (commandText.Substring(i, pParametersReplaceChar.Length) != pParametersReplaceChar)
                        {
                            str2 = str2 + commandText.Substring(i, 1);
                            i++;
                        }
                        i += pParametersReplaceChar.Length - 1;
                        str = str + "@" + num.ToString();
                        command.Parameters.Add("@" + num.ToString(), str2);
                        num++;
                    }
                    else
                    {
                        str = str + commandText.Substring(i, 1);
                    }
                }
            }
            command.CommandText = str;
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return num;
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            command.Dispose();
            return num;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlDataReader reader;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                reader = ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return reader;
        }

        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            SqlDataReader reader;
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters);
            if (connectionOwnership == SqlConnectionOwnership.External)
            {
                reader = command.ExecuteReader();
            }
            else
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            command.Parameters.Clear();
            return reader;
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            object obj2 = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj2;
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            object obj2 = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj2;
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(connection, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            XmlReader reader = command.ExecuteXmlReader();
            command.Parameters.Clear();
            return reader;
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            XmlReader reader = command.ExecuteXmlReader();
            command.Parameters.Clear();
            return reader;
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        public static string ConnectionString
        {
            get
            {
                return pConnectionString;
            }
            set
            {
                pConnectionString = value;
            }
        }

        public static string ParametersReplaceChar
        {
            get
            {
                return pParametersReplaceChar;
            }
            set
            {
                pParametersReplaceChar = value;
            }
        }

        private enum SqlConnectionOwnership
        {
            Internal,
            External
        }
    }
}

