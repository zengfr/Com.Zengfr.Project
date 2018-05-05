using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
namespace Com.Zfrong.Common.Log
{
    public class Log4netFactory
    {
        public static ILog Create(string projectName,string groupName)
        {
            log4net.Repository.ILoggerRepository repository = LogManager.CreateRepository("" + projectName);
            Hierarchy hierarchy = (Hierarchy)repository; 
            hierarchy.Name = "" + projectName;
            hierarchy.Root.Level = log4net.Core.Level.All;
            hierarchy.Configured = true;

            MyRollingFileAppender rollAppender = new MyRollingFileAppender();
            rollAppender.Name = projectName;
 
            log4net.Config.BasicConfigurator.Configure(repository);
            ILog log = log4net.LogManager.GetLogger(hierarchy.Name);

            ((log4net.Repository.Hierarchy.Logger)log.Logger).AddAppender(rollAppender);
            ChangeLog4netLogFileName(log,projectName+"\\"+groupName+"\\");//+Thread.CurrentThread.ManagedThreadId+"\\");
            return log;
        }
        private static void ChangeLog4netLogFileName(log4net.ILog log, string fileName)
        {
            log4net.Core.LogImpl logImpl = log as log4net.Core.LogImpl;
            if (logImpl != null)
            {
                log4net.Appender.AppenderCollection ac = ((log4net.Repository.Hierarchy.Logger)logImpl.Logger).Appenders;
                for (int i = 0; i < ac.Count; i++)
                {    //这里我只对RollingFileAppender类型做修改
                    log4net.Appender.RollingFileAppender rfa = ac[i] as log4net.Appender.RollingFileAppender;
                    if (rfa != null)
                    {
                        int s1 = rfa.File.LastIndexOf("applog\\");
                        if (s1 != -1)
                        {
                            string tmp = rfa.File.Substring(0, s1);
                            rfa.File = tmp + "applog\\" + fileName + "\\";
                            rfa.ActivateOptions(); tmp = null;
                        }
                    }
                }
            }
        }
        static Log4netFactory()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Info(ILog log, string format, params object[] args)
        {
            log.InfoFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + format, args);
        }
        public static void InfoAndShow(ILog log, string format, params object[] args)
        {
            log.InfoFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff ") + format, args);
            Show(log, format, args);//
        }
        public static void Show(ILog log, string format, params object[] args)
        {
            Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("MM-dd HH:mm:ss fff"), string.Format( format, args)));//
        }
    }
}
