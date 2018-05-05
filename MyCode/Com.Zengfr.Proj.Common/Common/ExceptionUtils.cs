using System;
using System.Diagnostics;
using System.IO;
using ServiceStack.ServiceClient.Web;

namespace Com.Zengfr.Proj.Common
{
    public class ExceptionUtils
    {
        public static void ErrorFormat(log4net.ILog logger,Exception exception,string msg)
        {
            string result = string.Empty;
            result = string.Format("\r\nTime->{0},Error->{1},Msg->{2}",DateTime.Now.ToString("yyyyMMddHHmmssfff"),ExceptionUtils.GetDetails(exception), msg);
            if (exception is WebServiceException)
            {
                var ex = (exception as WebServiceException);
                result += string.Format("\r\nErrorMessage:{0}", ex.ErrorMessage);
                result += string.Format("\r\nResponseBody:{0}", ex.ResponseBody);
            }
            logger.ErrorFormat(string.Format("{0}",result));

            
            var path = AppDomain.CurrentDomain.BaseDirectory;
            File.AppendAllText(string.Format("{0}\\{1}.log", path,DateTime.Today.ToString("yyyyMM")), result);
        }
        public static string GetDetails(Exception exception)
        {
            string result = string.Empty;
            result = string.Format("Messages:{0},StackTraces:{1}",
                ExceptionUtils.GetDetailMessages(exception),
                ExceptionUtils.GetDetailStackTraces(exception));
            return result;
        }
        public static string GetDetailStackTraces(Exception exception)
        {
            StackFrame[] stacks = new StackTrace(exception, 1, true).GetFrames();
            string result = string.Empty;
            if (stacks != null)
            {
                for (int index = 0; index < stacks.Length - 1; index++)
                {
                    StackFrame stack = stacks[index];
                    var type = stack.GetMethod().ReflectedType;
                    result += string.Format("->{0}.{1}({2}.{3}){4}\r\n",
                        type == null ? string.Empty : type.Name,
                        stack.GetMethod().Name,
                        stack.GetFileLineNumber(),
                        stack.GetFileColumnNumber(),
                        stack.GetFileName()
                        );
                }
            }
            return result;
        }
        public static string GetDetailMessages(Exception exception)
        {
            string result = string.Empty;
            if (exception != null)
            {
                result += string.Format("{0}\r\n ", exception.Message);
                if (exception.InnerException != null)
                {
                    result += GetDetailMessages(exception.InnerException);
                }
            }
            return result;
        }
    }
}
