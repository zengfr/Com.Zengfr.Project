using System;
using System.Collections.Generic;
using System.Text;

using ThreadWorker;
using System.Threading;//
using CommonPack;
using BlackHen.Threading;//
using System.Collections;
using HtmlAgilityPack;
using NHibernate;//
using NHibernate.Expression;//
namespace RSSBuilder
{
    public class AnalyticsManager:Manager
    {
        public static IList<DB.RSSItem> RssItems = new List<DB.RSSItem>();//
        public static WorkQueue WorkQueue = new WorkQueue();
        static AnalyticsManager()
        {
            WorkQueue.WorkerPool = new WorkThreadPool(10, 40);//
            WorkQueue.ConcurrentLimit = 30;//
            WorkQueue.AllWorkCompleted += new EventHandler(Work_AllWorkCompleted);
            //WorkQueue.CompletedWorkItem += new WorkItemEventHandler(Work_CompletedWorkItem);

        }
        //static void Work_CompletedWorkItem(object sender, WorkItemEventArgs e)
        //{

        //}
        static void Work_AllWorkCompleted(object sender, EventArgs e)
        {
            if (!IsQuit)
            {
                SaveItems();
                GetDBToQueue();//
            }
        }

        public static void AddRssItem(DB.RSSItem item)
        {
            lock (SysncObj)
            {
                RssItems.Add(item);//
                System.Threading.Monitor.PulseAll(SysncObj);//
            }
        }

        static public void TheadStart()
        {
            ThreadStart ts1 = new ThreadStart(GetDBToQueue);
            Thread t1 = new Thread(ts1);//
            t1.Start();//

            ThreadStart ts2 = new ThreadStart(TheadSaveItems);
            Thread t2 = new Thread(ts2);//
            t2.Start();
        }
        private static void GetDBToQueue()
        {
            DB.RSSItem[] objs ;
            do
            {
                objs = DB.RSSItem.SlicedFindAll(0, 100, Expression.Eq("State", byte.Parse("0")));//
                if (objs.Length < 10)
                    Thread.Sleep(10 * 1000);//解决无数据重复查询问题
            } while (objs.Length < 10);
            WorkQueue.Pause();
            foreach (DB.RSSItem obj in objs)
            {
                WorkQueue.Add(new AnalyticsWorkItem(obj));//
            }
            WorkQueue.Resume();
        }
        static void TheadSaveItems()
        {
            while (!IsQuit)
            {
                Thread.Sleep(20 * 60 * 1000);
                if (RssItems.Count!=0)
                    SaveItems();//
            }
        }
        public static void SaveItems()
        {
            IList<DB.RSSItem> objs = new List<DB.RSSItem>();
            lock (SysncObj)
            {
                foreach (DB.RSSItem obj in RssItems)
                {
                    objs.Add(obj);//
                }
                RssItems.Clear();//
                System.Threading.Monitor.PulseAll(SysncObj);//
            }
            foreach (DB.RSSItem rss in objs)
            {
                if (rss.Content == null || rss.Content.Length < 200)
                {
                    //DB.SqlHelper.ExecuteNonQuery("delete from rssitem where id=" + rss.ID);//
                    DB.SqlHelper.ExecuteNonQuery("update rssitem set [state]=-1 where id=" + rss.ID);//
                    ShowMessge("Item Deleted:" + rss.Link);//
                }
                else
                {
                    rss.ContentHash = rss.Content.GetHashCode();
                    rss.State = 1;
                    rss.UpdateTime = DateTime.Now;//
                    rss.Update();//
                    //DB.SqlHelper.ExecuteNonQuery("update rssItem set State=1,Content='" + rss.Content + "',ContentHash=" + rss.ContentHash + ",UpdateTime ='" + rss.UpdateTime + "' where ID=" + rss.ID);//
                    ShowMessge("Item Updated:" + rss.Link);//
                }
                Thread.Sleep(500);//
            }
        }
    }
    public class AnalyticsWorkItem : WorkItem
    {
        private DB.RSSItem Item = null;//
        public AnalyticsWorkItem(DB.RSSItem item)
        {
            Item = item;//
        }
        public override void Perform()
        {
            if (!AnalyticsManager.IsQuit)
                GetItemContent(Item);//
        }
        private void GetItemContent(DB.RSSItem item)
        {
                DateTime t1 = DateTime.Now;
                item.Content = CommonPack.HtmlAgilityPack.HtmlToSubject.Analytics2(item.Content);//
                AnalyticsManager.ShowMessge(t1, DateTime.Now, " Analytics:" + item.Link.ToString());//

                //if(item.Content.IndexOf(item.Description)==-1)
                //item.Content = GetMainContentHelper.GetMainContent(doc.Text);
                AnalyticsManager.AddRssItem(item);//
        }
    }
}
