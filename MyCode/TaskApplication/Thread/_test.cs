using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Amib.Threading;
using Zfrong.Amib.Threading;
namespace TaskApplication
{
    public partial class SmartThreadPoolTest
    {
        static ThreadPoolManager t;
        public static void Test()
        {
            string name = "pool";
            t = ThreadPoolManagerFactory.Instance.Create(name, 40);
            t.CreateWorker<Work>("worker");
            t.CreateWorkItemsGroup("group", 32);
            t.EventManager.OnThreadTermination += new ThreadTerminationHandler(EventFacade_OnThreadTermination);
            t.EventManager.OnTaskScheduled += new System.Action<TaskBase>(OnTaskAdd);
            t.EventManager.OnTaskCompleted += new System.Action<TaskBase>(OnTaskCompleted);
            t.EventManager.OnWorkItemsGroupIdle += new Action<IWorkItemsGroup>(EventFacade_OnWorkItemsGroupIdle);
            for (ulong i = 0; i < 2666;i++ )
            {
                t.AddTask(new Task() { GroupName = "group", WorkerName = "worker" });
               
            }
            t.Start();
            System.Threading.Thread.Sleep(2000); 
            t.Pause();
            System.Threading.Thread.Sleep(3000);
            t.Resume();
          //  System.Threading.Thread.Sleep(1000*66); 
            for (ulong i = 6666; i < 59999; i++)
            {
                t.AddTask(new Task() { GroupName = "group", WorkerName = "worker" });
            }
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
            Console.WriteLine("Idle:{0}" , obj.Name);
        }

        static void OnTaskCompleted(TaskBase obj)
        {
            TaskManager tm = TaskManagerFactory.Instance.Create(obj.WorkerName);
            tm.Remove(obj.TaskInfo.ID);
           // Console.WriteLine("{0}|{1}|{2} {3,6} {4,6} ID:{5}",
              //   DateTime.Now.ToString("mm:ss fff"), t.GetStatisticsInfo(),
              //  "Completed->", obj.GroupName, obj.WorkerName, obj.TaskInfo.ID);
            EventManager ev = EventManagerFactory.Instance.Create(obj.TaskInfo.PoolName);
            Console.WriteLine("{0}|{1} {2} {3} {4} {5} {6} {7}",
                 DateTime.Now.ToString("mm:ss fff"), 
                 ev.StatusInfo[0],ev.StatusInfo[1], ev.StatusInfo[2],
                 ev.StatusInfo[3],ev.StatusInfo[4],ev.StatusInfo[5],ev.StatusInfo[6]);
        }

        static void OnTaskAdd(TaskBase obj)
        {
            TaskManager tm = TaskManagerFactory.Instance.Create(obj.WorkerName);
            tm.Add(obj);
        }
        public class Work : WorkerBase
        {
            Random r = new Random();
           protected override void DoTask<T>(T t)
            {
                System.Threading.Thread.Sleep(10*r.Next(1,111));
            }
        }
        public class Task : TaskBase
        {

        }
    }
}
