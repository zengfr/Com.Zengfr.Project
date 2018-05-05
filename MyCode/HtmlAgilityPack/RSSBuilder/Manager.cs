using System;
using System.Collections.Generic;
using System.Text;

using CommonPack;

namespace RSSBuilder
{
    public class MyRssItem : Rss.RssItem
    {
        public string Content;
        public string Key;//
        public string Category;
        public int CategoryHash; 
    }
  public   class Manager
    {
      
        #region ShowMessge
        public static void ShowMessge(DateTime t1, DateTime t2, string msg)
        {
            TimeSpan ts = (TimeSpan)(t2 - t1);
            ShowMessge(ts, msg);
        }
        public static void ShowMessge(TimeSpan ts, string msg)
        {
            ShowMessge("TimeSpan:"+ts.Minutes+":"+ ts.Seconds + " " + msg);//
        }
        public static void ShowMessge(string msg)
        {
            Console.WriteLine(msg);//
            Log.Info(msg);//
        }
        #endregion
        public static bool IsQuit = false;//
        public static object SysncObj = new object();//
    }
}
