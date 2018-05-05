using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Layout;
namespace Com.Zfrong.Common.Log
{
   public class MyRollingFileAppender:RollingFileAppender
    {
      public  MyRollingFileAppender():base()
      {
          this.AppendToFile = true;
          this.DatePattern = "yyyyMMdd-HHmm\".log\"";
          this.MaxSizeRollBackups = 10;
          this.StaticLogFileName = false;
          this.RollingStyle = RollingMode.Date;
          this.Threshold = log4net.Core.Level.All;//
          this.File = "Log\\";
          PatternLayout layout=new PatternLayout();
          layout.ConversionPattern=@"%d [%t] %-5p %c [%x] - %m%n%n";
          layout.ActivateOptions();
          this.Layout = layout;//
      }
       protected override void Append(log4net.Core.LoggingEvent log)
       {
           base.Append(log);
       }
       protected override void Append(log4net.Core.LoggingEvent[] log)
       {
           base.Append(log);
       }
    }
}
