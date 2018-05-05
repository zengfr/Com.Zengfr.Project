using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using BlackHen.Threading;//
using ParserEngine;//
using DB;
using NHibernate.Expression;
using System.Threading;//
using CommonPack;//
using CommonPack.Common;
using Com.Zfrong.Xml;
namespace WordBuilder
{
   public class AdvDataManager:Manager
   {
       public static WorkQueue KeyWorkQueue = new WorkQueue();

       static int[] Status = new int[6];//
       public static WorkQueue WorkQueue = new WorkQueue();
       
       static AdvDataManager()
       {
           WorkQueue.ConcurrentLimit = 15;//
           WorkQueue.ChangedWorkItemState += new ChangedWorkItemStateEventHandler(Work_ChangedWorkItemState);
           WorkQueue.RunningWorkItem += new WorkItemEventHandler(Work_RunningWorkItem);
           WorkQueue.CompletedWorkItem += new WorkItemEventHandler(WorkQueue_CompletedWorkItem);

           KeyWorkQueue.ConcurrentLimit = 2;//
           KeyWorkQueue.ChangedWorkItemState += new ChangedWorkItemStateEventHandler(KeyWorkQueue_ChangedWorkItemState);
           //KeyWorkQueue.RunningWorkItem += new WorkItemEventHandler(KeyWorkQueue_RunningWorkItem);
           KeyWorkQueue.CompletedWorkItem += new WorkItemEventHandler(KeyWorkQueue_CompletedWorkItem);
           StartTheadSaveBloomFilter();//
       }

       static void KeyWorkQueue_CompletedWorkItem(object sender, WorkItemEventArgs e)
       {
           ThreadStart();
       }
       static void KeyWorkQueue_RunningWorkItem(object sender, WorkItemEventArgs e)
       {

       }
       static void KeyWorkQueue_ChangedWorkItemState(object sender, ChangedWorkItemStateEventArgs e)
       {

       }

       static void WorkQueue_CompletedWorkItem(object sender, WorkItemEventArgs e)
       {
           ThreadStart();//
       }
       static void StartTheadSaveBloomFilter()
       {
           ThreadStart ts = new ThreadStart(TheadSaveBloomFilter);
           Thread t = new Thread(ts);//
           t.Start();//
       }
       static void TheadSaveBloomFilter()
       {
           while (!IsQuit)
           {
               Thread.Sleep(20 * 60 * 1000);
                   SaveBloomFilter();//
           }
       }
     
       static void Work_RunningWorkItem(object sender, WorkItemEventArgs e)
       {
           Manager.ShowMessge(ConsoleColour.ForeGroundColour.Red, "\t\t\t\tCreated:{0} Scheduled:{1} Queued:{2} Running:{3} Failing:{4} Completed:{5} WorkCount:{6}"
               , Status[0], Status[1], Status[2], Status[3], Status[4], Status[5],WorkQueue.Count);
       }

       static void Work_ChangedWorkItemState(object sender, ChangedWorkItemStateEventArgs e)
       {
           lock (SysncObj)
           {
               Status[(int)e.PreviousState] -= 1;
               Status[(int)e.WorkItem.State] += 1;
               Monitor.PulseAll(SysncObj);//
           }
           if(WorkQueue.Count%50==0)
               GC.Collect();
       }
       
       static void ShowWorkItem(WorkItem item)
       {
           ShowMessge("ItemState:"+item.State+" StartedTime:"+item.StartedTime+" ProcessingTime:"+item.ProcessingTime+" ExpendTime:"+item.ExpendTime+" CreatedTime:"+item.CreatedTime+" CompletedTime:"+item.CompletedTime);
       }

       public static void ThreadStart()
       {
         ThreadStart ts=new ThreadStart (AdvKeyWorkItemAdd);
         Thread t = new Thread(ts);//
         t.Start();//
       }
       private static void AdvKeyWorkItemAdd()
       {
           if (WorkQueue.Count < 5&&KeyWorkQueue.Count<5)
           {
               Thread.Sleep(2000);//
               KeyWorkQueue.Add(new AdvKeyWorkItem());//
           }
       }
       public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
       {
          // key = "新闻";
          // return YahooSearcher_Blog.Search(key, pn, ref totalCount);//
          // return YahooSearcher_News.Search(key, pn, ref totalCount);//

          // return SinaSearcher_Blog.Search(key, pn, ref totalCount);//

          // return GoogleSearcher_News.Search(key, pn, ref totalCount);//
           //return GoogleSearcher_Blog.Search(key, pn, ref totalCount);//

             return BaiduSearcher_News.Search(key, pn, ref totalCount);//
           // return BaiduSearcher_Blog.Search(key, pn, ref totalCount);//
       }
    }
   
       
 
}
