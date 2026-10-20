using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace UniTask_UI.common
{
    public class LoggerUtil
    {
        static IDictionary<string, ILog> loggers = new Dictionary<string, ILog>();
        public static ILog GetLogger(string key)
        {
            ILog mre = null;
            key = key == null ? "-" : key;
            if (!loggers.ContainsKey(key))
            {
                lock (loggers)
                {
                    if (!loggers.ContainsKey(key))
                    {
                        mre = log4net.LogManager.GetLogger(key);
                        loggers[key] = mre;
                        return mre;
                    }
                }
            }
            mre = loggers[key];
            return mre;
        }
        public static void Debug(string format, params object[] args)
        {
            GetLogger(Thread.CurrentThread.Name).DebugFormat(format, args);
        }
        public static void Error(string msg,Exception ex)
        {
            GetLogger(Thread.CurrentThread.Name).Error(msg, ex);
        }
        public static void Error(string format, params object[] args)
        {
            GetLogger(Thread.CurrentThread.Name).ErrorFormat(format, args);
        }
        public static void Info(string format, params object[] args)
        {
            GetLogger(Thread.CurrentThread.Name).InfoFormat(format, args);
        }
        public static void Warn(string format, params object[] args)
        {
            GetLogger(Thread.CurrentThread.Name).WarnFormat(format, args);
        }
    }
}
