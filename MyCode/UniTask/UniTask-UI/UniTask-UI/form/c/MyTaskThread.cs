using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using TaobaoTask;
using UniTask_UI.p1;
using UniTask_UI.common;
using System.Windows.Forms;

namespace UniTask_UI.form.c
{
    public abstract class MyTaskThread4Pool : MyTaskThread
    {
        MyThreadPool pool;
        protected override void runInit()
        {
            pool = MyThreadPool.getInstance(Name);
            pool.init(goalSetting.initThread);
            pool.start();
        }
        public override void runTask()
        {
            pool.init(goalSetting.initThread);
            runTask(pool);
        }
        protected abstract void runTask(MyThreadPool pool);
    }
    public abstract class MyBase
    {
        protected void DebugFormat(string format, params object[] args)
        {
            LoggerUtil.Debug("{0} {1}", Name, String.Format(format, args));
        }
        protected void InfoFormat(string format, params object[] args)
        {
            LoggerUtil.Info("{0} {1}", Name, String.Format(format, args));
        }
        protected void WarnFormat(string format, params object[] args)
        {
            LoggerUtil.Warn("{0} {1}", Name, String.Format(format, args));
        }
        public String Name { get; set; }
    }
    public abstract class MyTaskThread: MyBase
    { 
        private System.Threading.Thread thread;

        private object lockObj = new object();
        private bool isInit = false;
        //ManualResetEvent mre = new ManualResetEvent(false);
        protected GoalSetting goalSetting = MyData.Instance.goalSetting;

        public MyTaskThread()
        {
           
        }
        public virtual void init()
        {
            if (isInit) 
                return;
            runInit();
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(run));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Normal;
            thread.Start();
            isInit = true;
        }
        public virtual void cancel()
        {
            try
            {
                thread.Abort();
            }
            catch (Exception ex)
            {
                LoggerUtil.Error("cancel", ex);
            }
        }
        protected virtual void runInit()
        { }
        protected virtual void run()
        {
            MyData d = MyData.Instance;
            DateTime last = DateTime.Today;
            var gs = d.goalSetting;
           
            while (gs.init)
            {
                Application.DoEvents();
                if (gs.isStart)
                {
                    try
                    {
                        var enable = enabled();
                        if (enable)
                        {
                            runTask();
                        }
                        else if ((DateTime.Now - last).TotalSeconds >= 60 * 10)
                        {
                            InfoFormat(String.Format("enable:{0}", enable));
                            last = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerUtil.Error("env", ex);
                    }
                }
                Thread.Sleep(60);
                //mre.WaitOne(gs.wait);
            }
        }

        protected abstract bool enabled();
        public abstract void runTask();
    }

}
