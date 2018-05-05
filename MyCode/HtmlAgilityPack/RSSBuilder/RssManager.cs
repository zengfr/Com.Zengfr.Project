using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;

using CommonPack;
using BlackHen.Threading;//
using System.Collections;
using HtmlAgilityPack;
using NHibernate;//
using NHibernate.Expression;//
namespace RSSBuilder
{
  public  class RssManager:Manager
  {
      public static void xxx()
      {
          string file = System.AppDomain.CurrentDomain.BaseDirectory + @"\Config\rss\config\config.xml";//
          //Config = MyConfigHelper.Get(file);
          //if (Config.Dictionary.Count == 0)
          BindConfig(file);//
          DB.RSSFeed obj;
          foreach (KeyValuePair<string, IDictionary<string, string>> kv1 in Config.Dictionary)
          {
              foreach (KeyValuePair<string, string> kv in kv1.Value)
              {
                  obj = new DB.RSSFeed();//
                  obj.Link = kv.Key;//
                  obj.LinkText = kv.Value;//
                  obj.LinkHash = obj.Link.GetHashCode();//
                  obj.Save();//
              }
          }
      }
      static void BindConfig(string file)
      {
          XmlDocument xmlPort = new XmlDocument();
          XmlNodeList NodeList;
          xmlPort.Load(file);
          NodeList = xmlPort.SelectNodes("//channel");
          Dictionary<string, string> dict = new Dictionary<string, string>();//
          foreach (XmlNode Portnode in NodeList)
          {
              if (!dict.ContainsKey(Portnode["link"].InnerText))
                  dict.Add(Portnode["link"].InnerText, Portnode["name"].InnerText);
          }
          Config.Dictionary.Clear();//
          Config.Dictionary.Add("1", dict);//
      }
      #region Config
      public static void SaveBloomFilter()
      {
          BloomFilterFactory.Save("RssBloomFilter", GetBloomFilterFilePath());//
          ShowMessge("BloomFilter...Saved");//
      }
      private static string GetBloomFilterFilePath()
      {
          return AppDomain.CurrentDomain.BaseDirectory + @"\Config\rss\config\RssBloomFilter.dat";
      }
      
      #endregion
      #region Field
      public static IList<MyRssItem> RssItems = new List<MyRssItem>();//
     
      protected static MyConfig Config = new MyConfig();
      public static BloomFilter BloomFilter;
      
      public static WorkQueue RssFeedWorkQueue = new WorkQueue();
      public static WorkQueue RssItemWorkQueue = new WorkQueue();
      public static WorkQueue AnalyticsWorkQueue = new WorkQueue();
     

      static int[] Status = new int[6];//
      public static HtmlWeb HtmlWeb = new HtmlWeb();//
      #endregion
      static RssManager()
      {
          BloomFilter = BloomFilterFactory.Get("RssBloomFilter");//
          BloomFilterFactory.Load("RssBloomFilter", GetBloomFilterFilePath());//

          RssFeedWorkQueue.ConcurrentLimit = 20;//
          //RssFeedWorkQueue.AllWorkCompleted += new EventHandler(Work_AllWorkCompleted);
          //RssFeedWorkQueue.CompletedWorkItem += new WorkItemEventHandler(Work_CompletedWorkItem);
              //RssFeedWorkQueue.ChangedWorkItemState += new ChangedWorkItemStateEventHandler(Work_ChangedWorkItemState);
              //RssFeedWorkQueue.FailedWorkItem += new WorkItemEventHandler(Work_FailedWorkItem);
              //RssFeedWorkQueue.RunningWorkItem += new WorkItemEventHandler(Work_RunningWorkItem);

          RssItemWorkQueue.ConcurrentLimit = 20;//
          RssItemWorkQueue.AllWorkCompleted += new EventHandler(RssItemWorkQueue_AllWorkCompleted);
          //RssItemWorkQueue.CompletedWorkItem += new WorkItemEventHandler(Work_CompletedWorkItem);
              //RssItemWorkQueue.ChangedWorkItemState += new ChangedWorkItemStateEventHandler(Work_ChangedWorkItemState);
              //RssItemWorkQueue.FailedWorkItem += new WorkItemEventHandler(Work_FailedWorkItem);
              //RssItemWorkQueue.RunningWorkItem += new WorkItemEventHandler(Work_RunningWorkItem);
      }

      public static void AddRssItem(MyRssItem item)
      {
          lock (SysncObj)
          {
              RssItems.Add(item);//
              System.Threading.Monitor.PulseAll(SysncObj);//
          }
      }
     

      static public void TheadStart()
      {
          ThreadStart ts = new ThreadStart(TheadGetDBRssFeed);
          Thread t = new Thread(ts);//
          t.Start();//

          ThreadStart ts2 = new ThreadStart(TheadSaveItems);
          Thread t2 = new Thread(ts2);//
          t2.Start();//
      }
      #region WorkQueue Event
      //static void Work_RunningWorkItem(object sender, WorkItemEventArgs e)
      //{
      //    Console.WriteLine("Created:{0} Scheduled:{1} Queued:{2} Running:{3} Failing:{4} Completed:{5}"
      //        , Status[0], Status[1], Status[2], Status[3], Status[4], Status[5]);
      //}
      //static void Work_CompletedWorkItem(object sender, WorkItemEventArgs e)
      //{

      //}
      //static void Work_ChangedWorkItemState(object sender, ChangedWorkItemStateEventArgs e)
      //{
      //    lock (SysncObj)
      //    {
      //        Status[(int)e.PreviousState] -= 1;
      //        Status[(int)e.WorkItem.State] += 1;
      //        Monitor.PulseAll(SysncObj);//
      //    }
      //}
      //static void Work_FailedWorkItem(object sender, WorkItemEventArgs e)
      //{
      //    Console.WriteLine("Failed...");//
      //}
      static void RssItemWorkQueue_AllWorkCompleted(object sender, EventArgs e)
      {
          if (!IsQuit)
              GetDBRssFeedToQueue();//
      }
      
      #endregion
      
      #region GetDBRssFeedToQueue
      static void TheadGetDBRssFeed()
      {
          GetDBRssFeedToQueue();//
      }
    
      static private void GetDBRssFeedToQueue()
      {
          DB.RSSFeed[] objs = DB.RSSFeed.SlicedFindAll(0, 1000, new Order[] { Order.Asc("DoCount") }, Expression.Eq("State", byte.Parse("1")));//

          foreach (DB.RSSFeed obj in objs)
          {
              RssFeedWorkQueue.Add(new RssFeedWorkItem(obj.Title, obj.Link));//
              DB.SqlHelper.ExecuteNonQuery("update rssfeed set doCount=doCount+1 where id="+obj.ID);//
              ShowMessge("DBToQueue:" + obj.LinkText + " " + obj.Link);//
          }
          if (RssItemWorkQueue.Count <= 10)
          {
              Thread.Sleep(5 * 1000);//防止第一次加载太快得问题
          }
      }
      #endregion
      #region SaveRssItem
      static void TheadSaveItems()
      {
          while (!IsQuit)
          {
              Thread.Sleep(20 * 60 * 1000);
              if (RssItems.Count > 100)
                  SaveItems();//
          }
      }
      public static void SaveItems()
      {
          IList<MyRssItem> objs = new List<MyRssItem>();
          lock (SysncObj)
          {
              foreach (MyRssItem obj in RssItems)
              {
                  objs.Add(obj);//
              }
              RssItems.Clear();//
              System.Threading.Monitor.PulseAll(SysncObj);//
          }
          int id = 0; 
          DB.RSSItem rss;
          foreach (MyRssItem obj in objs)
          {
              id = DB.SqlHelper.ExecuteInt32("select count(id) from RssItem where [LinkHash]=" + obj.Link.ToString().GetHashCode());//
               if (id==0)
              {
                  rss = new DB.RSSItem();//

                  rss.Category = obj.Category;//
                  rss.Key = obj.Key;
                  rss.Author = obj.Author;
                  rss.Content = obj.Content;
                  rss.Description = obj.Description;
                  rss.Link = obj.Link.ToString();
                  rss.Title= obj.Title;
                  try
                  {
                      rss.CategoryHash = rss.Category.GetHashCode();//
                      rss.KeyHash = rss.Key.GetHashCode();
                      rss.AuthorHash = rss.Author.GetHashCode();
                      rss.ContentHash = rss.Content.GetHashCode();
                      rss.DescriptionHash = rss.Description.GetHashCode();
                      rss.LinkHash = rss.Link.GetHashCode();
                      rss.TitleHash = rss.Title.GetHashCode();
                  }
                  catch { }
                  rss.State = 1;
                  rss.PubDate = DateTime.Now;
                  rss.UpdateTime = DateTime.Now;//

                  rss.Save();//
                  ShowMessge("Item Saved:" + rss.Link);//
              }
          }
      }
      #endregion
    }
}
