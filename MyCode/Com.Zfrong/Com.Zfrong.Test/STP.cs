using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.IO;
using Amib.Threading;
using Amib.Threading.Internal;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using NetServ.Net.Json;
using NHibernate;
using NHibernate.Criterion;
using Com.Zfrong.Common.Data.AR.Base;
using Com.Zfrong.Common.Data.AR.Base.Entity;
using Com.Zfrong.Common.Http;
using Com.Zfrong.Common.Common;
namespace Test
{
   public class STP
   {
       protected static IConfigurationSource GetConfigSource()
       {
           return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
       }
       static STP() 
       {
              }
       public static void ShowLog(string str)
       {
           Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("mm:ss fff"), str));
       }
       public static SmartThreadPool pool = new SmartThreadPool();
       public  static IWorkItemsGroup wig1, wig2,wig3;
        public static void DoWork()
        {

            WIGStartInfo wigStartInfo = new WIGStartInfo();
            wigStartInfo.StartSuspended = true;
            wigStartInfo.CallToPostExecute = CallToPostExecute.Always;
            wigStartInfo.DisposeOfStateObjects = true;
            //wigStartInfo.PostExecuteWorkItemCallback = new PostExecuteWorkItemCallback(DoPostExecuteWork1);
          
            wig1 = pool.CreateWorkItemsGroup(10,wigStartInfo);
            wig2 = pool.CreateWorkItemsGroup(10);
            wig3 = pool.CreateWorkItemsGroup(10);
            wig1.OnIdle += new WorkItemsGroupIdleHandler(wig1_OnIdle);
            for(int i=0;i<10000;i++)
            {
                 wig1.QueueWorkItem(new WorkItemCallback(Run1),null,new PostExecuteWorkItemCallback(DoPostExecuteWork1));
            }
            wig1.Start(); //STPStartInfo s;s.
            //wig1.WaitForIdle();
            //wig2.WaitForIdle();
            //wig3.WaitForIdle();
            Console.ReadKey();//
            pool.WaitForIdle();
            pool.Shutdown();
        }
        static void wig1_OnIdle(IWorkItemsGroup workItemsGroup)
        {
            
        }
        private static void DoPostExecuteWork1(IWorkItemResult wir)
        {
            wig2.QueueWorkItem(new WorkItemCallback(Run2),wir.Result, new PostExecuteWorkItemCallback(DoPostExecuteWork2));
        }
        private static void DoPostExecuteWork2(IWorkItemResult wir)
        {
            wig3.QueueWorkItem(new WorkItemCallback(Run3),null,new PostExecuteWorkItemCallback(DoPostExecuteWork3));
        }
        private static void DoPostExecuteWork3(IWorkItemResult wir)
        {
            ShowLog("" + ShowLog(wig1) + "\t" + ShowLog(wig2) + "\t" + ShowLog(wig3));//
        }
        static object ShowLog(IWorkItemsGroup g)
        {
            return g.Name+":"+g.Concurrency+" "+g.WaitingCallbacks;//
        }
        static object Run1(object state)
        {
            return null;//
        }
        static object Run2(object state)
        {
            return null;//
        }
        static object Run3(object state)
        {
            return null;//
        }
    }
}
