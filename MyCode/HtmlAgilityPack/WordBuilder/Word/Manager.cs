using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;//
using CommonPack;
using CommonPack.Common;
namespace WordBuilder
{
   public class Manager
    {  
        #region ShowMessge
       public static void ShowMessge(DateTime t1, string msg)
       {
           ShowMessge(t1, DateTime.Now, msg);
       }
        public static void ShowMessge(DateTime t1, DateTime t2, string msg)
        {
            TimeSpan ts = (TimeSpan)(t2 - t1);
            ShowMessge(ts, msg);
        }
        public static void ShowMessge(TimeSpan ts, string msg)
        {
            string tmp = "耗:" + ts.Minutes + ":" + ts.Seconds;
            ShowMessge(tmp.PadRight(10,' ') + " \t " + msg);//
        }
       public static void ShowMessge(string msg)
        {
            ShowMessge(msg,null);
        }
       public static void ShowMessge(ConsoleColour.ForeGroundColour color, string msg, params object[] arg)
       {
           ConsoleColour.SetForeGroundColour(color);//
           ShowMessge(msg,arg);
           ConsoleColour.SetForeGroundColour();
       }
        public static void ShowMessge(string msg, params object[] arg)
        {
            if (arg != null)
            {
                Console.WriteLine(msg, arg);//
                Log.Info(string.Format(msg, arg));//
            }
            else
            {
                Console.WriteLine(msg);//
                Log.Info(msg);//
            }
        }
        #endregion
        public static bool IsQuit = false;//
        public static object SysncObj = new object();//
       public static BloomFilter BloomFilter;//
       private static string GetBloomFilterFilePath()
       {
           return AppDomain.CurrentDomain.BaseDirectory + @"\Config\rss\config\HotBloomFilter.dat";
       }
       public static void Init()
       {
           log4net.Config.XmlConfigurator.Configure();
           ActiveRecordStarter.ResetInitializationFlag();
           ActiveRecordStarter.Initialize(GetConfigSource(), typeof(DB.CnHot), typeof(DB.EnHot), typeof(DB.AdvData));
           //ActiveRecordStarter.CreateSchema();//
           DB.SqlHelper.ConnectionString = "Data Source=.;Initial Catalog=datadb;Integrated Security=True";//

           BloomFilter = BloomFilterFactory.Get("HotBloomFilter");//
           BloomFilterFactory.Load("HotBloomFilter", GetBloomFilterFilePath());//
       }
       public static void SaveBloomFilter()
       {
           ShowMessge("Filter...Saving");//
           BloomFilterFactory.Save("HotBloomFilter", GetBloomFilterFilePath());//
           ShowMessge("Filter...Saved");//
       }
       static IConfigurationSource GetConfigSource()
       {
           return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
       }
      
    }
}
