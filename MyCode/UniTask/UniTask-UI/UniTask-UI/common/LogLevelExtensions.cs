using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace UniTask_UI.common
{
    public static class LogLevelExtensions
    {
        public static readonly Level INF1Level = new Level(41000, "INF1");
        public static readonly Level INF2Level = new Level(42000, "INF2");
        public static readonly Level INF3Level = new Level(43000, "INF3");
        public static readonly Level INF4Level = new Level(44000, "INF4");
        public static void init()
        {
            log4net.Repository.Hierarchy.Hierarchy hierarchy = ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository());

            hierarchy.LevelMap.Add(INF1Level);
            hierarchy.LevelMap.Add(INF2Level);
            hierarchy.LevelMap.Add(INF3Level);
            hierarchy.LevelMap.Add(INF4Level);
        }
        public static void INF1(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF1Level, message, null);
        }

        public static void INF1Format(this ILog log, string message, params object[] args)
        {
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF1Level, formattedMessage, null);
        }
        public static void INF2(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF2Level, message, null);
        }

        public static void INF2Format(this ILog log, string message, params object[] args)
        {
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF2Level, formattedMessage, null);
        }

        public static void INF3(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF3Level, message, null);
        }

        public static void INF3Format(this ILog log, string message, params object[] args)
        {
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF3Level, formattedMessage, null);
        }
        public static void INF4(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF4Level, message, null);
        }

        public static void INF4Format(this ILog log, string message, params object[] args)
        {
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                INF4Level, formattedMessage, null);
        }
    }
}
