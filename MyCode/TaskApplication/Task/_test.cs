using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Amib.Threading;
using Zfrong.Amib.Threading;
namespace TaskApplication.Task
{
    public partial class SpiderTest
    {
        static ThreadPoolManager t;
        public static void Test()
        {
            string name = "pool";
            t = ThreadPoolManagerFactory.Instance.Create(name, 40);
            t.CreateWorker<SpiderWorker>("worker");
            t.CreateWorkItemsGroup("group", 32);
            t.EventManager.OnThreadTermination += new ThreadTerminationHandler(EventFacade_OnThreadTermination);
            t.EventManager.OnTaskScheduled += new System.Action<TaskBase>(OnTaskAdd);
            t.EventManager.OnTaskCompleted += new System.Action<TaskBase>(OnTaskCompleted);
            t.EventManager.OnWorkItemsGroupIdle += new Action<IWorkItemsGroup>(EventFacade_OnWorkItemsGroupIdle);
            string url = SpiderQueue.Instance.Dequeue();
            if (url == null){
                for (ulong i = 0; i < 90; i++)
                {
                    //addTask("http://www.icxo.com/");
                    //addTask("http://www.hao123.com/");
                    //addTask("http://www.265.com/");
                    addTask("http://news.baidu.com/");
                    //addTask("http://news.sina.com/");
                }
            }
            t.Start();
            System.Threading.Thread.Sleep(2000); 
            t.Pause();
            System.Threading.Thread.Sleep(3000);
            t.Resume();
            for (ulong i = 0; i < 222; i++)
            {
                addTask(url);
                url = SpiderQueue.Instance.Dequeue();
            }
            System.Threading.Thread.Sleep(1000*66); 
             
        }
        static void addTask(string url)
        {
            if (url == null) return;
            t.AddTask(new SpiderTask()
            {
                GroupName = "group",
                WorkerName = "worker",
                URL = url
            });
        }
        static void EventFacade_OnThreadInitialization()
        {
            Console.WriteLine("ThreadInitialization");
        }

        static void EventFacade_OnThreadTermination()
        {
            Console.WriteLine("ThreadTermination");
        }
        
       
        static void EventFacade_OnWorkItemsGroupIdle(IWorkItemsGroup obj)
        {

            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Idle:{0} {1}" , obj.Name,"");
            
        }

        static void  OnTaskCompleted(TaskBase obj)
        {
            TaskManager tm = TaskManagerFactory.Instance.Create(obj.WorkerName);
            tm.Remove(obj.TaskInfo.ID);
            Console.WriteLine("{0}|{1}|{2} {3,6} {4,6} ID:{5} {6} {7}",
                 DateTime.Now.ToString("mm:ss fff"), t.GetStatisticsInfo(),
                "Completed->", obj.GroupName, obj.WorkerName, obj.TaskInfo.ID, obj.TaskInfo.TimeSpan, ((SpiderTask)obj).URL);
            string url = SpiderQueue.Instance.Dequeue();
            for (int i = 0; i < 2; i++)
            {
                if (url == null)
                {
                    SpiderQueue.Instance.Flush();
                    url = SpiderQueue.Instance.Dequeue();
                }
                addTask(url);
            } url = null;
        }

        static void  OnTaskAdd(TaskBase obj)
        {
            TaskManager tm = TaskManagerFactory.Instance.Create(obj.WorkerName);
            tm.Add(obj);
            
        }
        

    }
}
