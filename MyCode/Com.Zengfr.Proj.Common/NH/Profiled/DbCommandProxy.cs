using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using log4net;
using NHibernate.Stat;
using System.Web;

namespace Com.Zengfr.Proj.Common.NH
{
    internal class DbCommandProxy : RealProxy, IRemotingTypeInfo
    {
        private static readonly string[] MethodNamesToProxy = new[] { ExecuteReader, ExecuteNonQuery, ExecuteScalar };
        private static readonly IStatisticsImplementor _statisticsImplementor = null;

        private const string FieldGetter = "FieldGetter";
        private const string ExecuteReader = "ExecuteReader";
        private const string ExecuteNonQuery = "ExecuteNonQuery";
        private const string ExecuteScalar = "ExecuteScalar";

        private readonly DbCommand _instance;
        static DbCommandProxy()
        {
            _statisticsImplementor = GetStatisticsImplementor();
        }
        private static IStatisticsImplementor GetStatisticsImplementor()
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Application["NHStatistics"] as IStatisticsImplementor;
            return null;
        }
        private static void QueryExecuted(string sql, int rows, DateTime start, DateTime end)
        {
            if (_statisticsImplementor != null)
            {
                _statisticsImplementor.QueryExecuted(sql, rows, (end - start));
            }
        }
        public DbCommandProxy(DbCommand instance)
            : base(instance.GetType())
        {
            _instance = instance;

        }


        public override IMessage Invoke(IMessage msg)
        {
            var methodMessage = (IMethodCallMessage)msg;

            object returnValue = null;

            if (MethodNamesToProxy.Contains(methodMessage.MethodName))
            {
                DateTime start = DateTime.Now;
                int rows = 0;
                int fieldCount = 1;
                try
                {
                    returnValue = methodMessage.MethodBase.Invoke(_instance, methodMessage.Args);
                }
                finally
                {
                    if (methodMessage.MethodName == ExecuteNonQuery)
                        rows = int.Parse("" + returnValue);
                    if (methodMessage.MethodName == ExecuteReader)
                        fieldCount = ((DbDataReader)returnValue).FieldCount;
                    QueryExecuted(string.Format("/*F:{0:000},T:{1}*/{2}", fieldCount, methodMessage.MethodName.Replace("Execute", string.Empty), _instance.CommandText), rows, start, DateTime.Now);
                }
            }
            else if (methodMessage.MethodName == FieldGetter)
            {
                var field = (string)methodMessage.Args[1];
                var fi = _instance.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                returnValue = fi.GetValue(_instance);
            }
            if (returnValue == null)
            {
                returnValue = methodMessage.MethodBase.Invoke(_instance, methodMessage.InArgs);
            }
            return new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
        }

        public bool CanCastTo(Type fromType, object o)
        {
            if (fromType == typeof(SqlCommand))
            {
                return true;
            }

            if (fromType == typeof(DbCommand))
            {
                return true;
            }

            if (fromType == typeof(IDbCommand))
            {
                return true;
            }

            return false;
        }

        public string TypeName
        {
            get { return _instance.GetType().AssemblyQualifiedName; }
            set { }
        }
    }
}
