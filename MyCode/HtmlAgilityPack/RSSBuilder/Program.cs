using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;//
namespace RSSBuilder
{
    class Program
    {
       
      static IConfigurationSource GetConfigSource()
      {
          return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
      }
  public  static void Init()
      {
          log4net.Config.XmlConfigurator.Configure();
           ActiveRecordStarter.ResetInitializationFlag();
            ActiveRecordStarter.Initialize(GetConfigSource(),typeof(DB.RSSFeed), typeof(DB.RSSItem),typeof(DB.RSSPage));
            DB.SqlHelper.ConnectionString = "Data Source=.;Initial Catalog=datadb;Integrated Security=True";//
      }
        public static void Start_Analytics()
        {
            AnalyticsManager.TheadStart();//
        }
      public  static void Start_Rss()
      {
          //Init();//
                // Console.WindowWidth =Console.LargestWindowWidth-8;//
                //Console.SetWindowPosition(0, 0);//
                //Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                    RssManager.TheadStart();//
                //Console.ReadKey();//
        }
        public static void Pause()
        {
            RssManager.RssFeedWorkQueue.Pause();//
            RssManager.RssItemWorkQueue.Pause();//

            AnalyticsManager.WorkQueue.Pause();//
        }
        public static void Continue()
        {
            RssManager.RssFeedWorkQueue.Resume();//
            RssManager.RssItemWorkQueue.Resume();//

            AnalyticsManager.WorkQueue.Resume();//
        }
        public static void Quit()
        {
            if (!RssManager.IsQuit)
            {
                Manager.IsQuit = true;//
                Save();//
            }
        }
        public static void Save()
        {
            Pause();//
            RssManager.SaveItems();//
            RssManager.SaveBloomFilter();//
            AnalyticsManager.SaveItems();//
        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Quit();//
        }
    }
}
