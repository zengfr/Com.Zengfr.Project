using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using Common.Logging;
namespace Zfrong.Framework.Utils
{
    public class LogData
    {
        public string Type { get; set; }
        public int ThreadID { get; set; }
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public string IP { get; set; }

        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }

        public string RequestType { get; set; }
        public string RequestMethod { get; set; }

        public byte   ResponseCode { get; set; }
        public string Message{ get; set; }

        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        }
    public class MyMethodInterceptor : IMethodInterceptor
    {
        public ILog Log { get; set; }
        public object Invoke(IMethodInvocation invocation)
        {
            DateTime start = DateTime.Now;
            Exception exception=null;
            object rtn = null;
            try
            {
               rtn=invocation.Proceed();
            }
            catch(Exception ex) {
                exception = ex;
            }
            finally
            {
                DateTime end = DateTime.Now;
                LogData logData =GetLogData(invocation, start, end);
                if(exception!=null)
                    logData.ResponseCode=5;
                string log = MySerializer.DataSerialize(logData);
                SaveLog(exception, "{0}", log);
                logData = null; log = null;  
            }
            return rtn;
        }
         LogData GetLogData(IMethodInvocation invocation, DateTime start, DateTime end)
        {
            LogData log = new LogData();
            try{
            log.ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            log.Type = "MyMethodInterceptor";
            log.RequestTime=start;
            log.ResponseTime=end;
            log.RequestType = invocation.TargetType.FullName;
            log.RequestMethod = invocation.Method.Name;
            log.RequestData = MySerializer.DataSerialize(invocation.Arguments as IEnumerable<object>);
            }catch(Exception ex)
            {
                log.ResponseCode = 5;
                log.Message=ex.Message;
            }
            return log;
        }
        void SaveLog(Exception ex,string format,params object[] args)
        {
            if (Log != null)
            {
                Log.Info(String.Format(format,args),ex);
            }
        }
    }
}
