using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;
using log4net;
using UniTask_UI.common;

namespace UniTask_UI.p1
{
    public class MyThreadPool
    {
        public static Dictionary<String, MyThreadPool> Instances = new Dictionary<String, MyThreadPool>();

       
        public static MyThreadPool getInstance(string key)
        {
            if (!Instances.ContainsKey(key))
            {
                lock (Instances)
                {
                    if (!Instances.ContainsKey(key))
                    {
                        var p = new MyThreadPool();
                        p.Name = key;
                        Instances[key] = p;
                    }
                }
            }
            return Instances[key];
        }
        private object lockObj = new object();
        public SmartThreadPool smart = null;
        public string Name { get; set; }
        STPStartInfo stp;
        public void init(int thread)
        {
            if (thread < 1)
                thread = 1;
            init(1, thread);
        }
        public void init(int minThread, int maxThread)
        {
            lock (lockObj)
            {
                try
                {
                    if (smart == null)
                    {

                        //stp状态设置
                        if (stp == null)
                        {
                            stp = new STPStartInfo(); 
                        }
                        stp.AreThreadsBackground = true;
                        stp.ApartmentState = ApartmentState.MTA;
                        stp.ThreadPoolName = Name;
                        stp.DisposeOfStateObjects = true;

                        stp.CallToPostExecute = CallToPostExecute.Always;

                        stp.ThreadPriority = ThreadPriority.AboveNormal;

                        stp.UseCallerCallContext = false;

                        stp.MaxWorkerThreads = maxThread;
                        stp.MinWorkerThreads = minThread;
                        smart = new SmartThreadPool(stp);// new SmartThreadPool(60, maxThread, minThread);
                        LoggerUtil.Info("smart engine initialized...");

                    }
                    stp.MaxWorkerThreads = maxThread;
                    stp.MinWorkerThreads = minThread;
                    smart.MinThreads = minThread;
                    smart.MaxThreads = maxThread;
                }
                catch (Exception ex)
                {
                    LoggerUtil.Error("init", ex);
                }
            }



        }
        public void cancel()
        {
            lock (lockObj)
            {
                try
                {
                    LoggerUtil.Info("cancel");
                    smart.Cancel(true);
                }
                catch (Exception ex)
                {
                    LoggerUtil.Error("cancel", ex);
                }
            }

        }
        public void stop()
        {
            lock (lockObj)
            {
                try
                {
                    LoggerUtil.Info("stop");
                    smart.WaitForIdle();
                    smart.Shutdown();

                }
                catch (Exception ex)
                {
                    LoggerUtil.Error("stop", ex);
                }
            }

        }

        public void addTask<T, T2, T3, R>(WorkItemPriority p,Func<T, T2, T3, R> action, T arg, T2 arg2, T3 arg3)
        {
            smart.QueueWorkItem<T, T2, T3, R>((a1, a2, a3) =>
            {
                var msg = String.Format("Thread:{0:d04};Priority:{1}",
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Priority.ToString());
                LoggerUtil.Debug("{0} {1}", "Work Start", msg);
                var rtn = action.Invoke(a1, a2, a3);
                LoggerUtil.Debug("{0} {1}", "Work   End", msg);
                return rtn;
            }, arg, arg2, arg3, p);
        }
        public void addTask<T, T2, T3>(WorkItemPriority p, Action<T, T2, T3> action, T arg, T2 arg2, T3 arg3)
        {
            smart.QueueWorkItem<T, T2, T3>((a1, a2, a3) =>
            {
                var msg = String.Format("Thread:{0:d04};Priority:{1}",
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Priority.ToString());
                LoggerUtil.Info("{0} {1}", "Work Start", msg);
                action.Invoke(a1, a2, a3);
                LoggerUtil.Debug("{0} {1}", "Work   End", msg);
            }, arg, arg2, arg3, p);
        }
        public void addTask<T, T2, R>(WorkItemPriority p, Func<T, T2, R> action, T arg, T2 arg2)
        {
            smart.QueueWorkItem<T, T2, R>((a1, a2) =>
             {
                 var msg = String.Format("Thread:{0:d04};Priority:{1}",
                 Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Priority.ToString());
                 LoggerUtil.Debug("{0} {1}", "Work Start", msg);
                 var rtn = action.Invoke(a1, a2);
                 LoggerUtil.Debug("{0} {1}", "Work   End", msg);
                 return rtn;
             }, arg, arg2, p);
        }
        public void addTask<T, T2>(WorkItemPriority p, Action<T, T2> action, T arg, T2 arg2)
        {
            smart.QueueWorkItem<T, T2>((a1, a2) =>
             {
                 var msg = String.Format("Thread:{0:d04};Priority:{1}",
                 Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Priority.ToString());
                 LoggerUtil.Debug("{0} {1}", "Work Start", msg);
                 action.Invoke(a1, a2);
                 LoggerUtil.Debug("{0} {1}", "Work   End", msg);
             }, arg, arg2, p);
        }
        public void start()
        {
            lock (lockObj)
            {
                try
                {
                    LoggerUtil.Info("start");
                    smart.Start();
                }
                catch (Exception ex)
                {
                    LoggerUtil.Error("start", ex);
                }
            }
        }
    }
}

