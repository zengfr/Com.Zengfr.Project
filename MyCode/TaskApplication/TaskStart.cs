using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using System.Collections;
using System.Text.RegularExpressions;
using TaskApplication.pro;
namespace TaskApplication
{
    public class MyJobListener: IJobListener
    {
        public void JobExecutionVetoed(JobExecutionContext context)
        {
            Show("JobExecutionVetoed->Type:{0} Group:{1} Name:{2}", context.JobDetail.JobType, context.JobDetail.Group, context.JobDetail.Name);
        }

        public void JobToBeExecuted(JobExecutionContext context)
        {
            Show("JobToBeExecuted->Type:{0} Group:{1} Name:{2}", context.JobDetail.JobType, context.JobDetail.Group, context.JobDetail.Name);
        }

        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
        {
            Show("JobWasExecuted->Type:{0} Group:{1} Name:{2}", context.JobDetail.JobType, context.JobDetail.Group, context.JobDetail.Name);
        }

        public string Name
        {
            get { return "MyJobListener"; }
        }
        protected static void Show(string f, params object[] args)
        {
            try
            {
                Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("dd HH:mm:ss fff"), string.Format(f, args)));//
            }
            catch { };
        }
    }
    public class TaskStart
    {
        static IScheduler sched;
        public static void Start()
        {
            if (sched != null)
                Shutdown();
            ISchedulerFactory sf = AdoSchedulerFactory.GetFormAdoConfig();// new StdSchedulerFactory();
            sched = sf.GetScheduler();
            sched.AddGlobalJobListener(new MyJobListener());
            DateTime t1;
            t1 = TriggerUtils.GetEvenSecondDate(DateTime.UtcNow);

            //AddJob<oooappJob>(sched, t1, 11, "oooapp", null);
            sched.Start();

        }
        static void AddJob<T>(IScheduler sched, DateTime t, int addsec, string name, IDictionary map)
        {
            JobDetail j1; SimpleTrigger tg1;
            j1 = new JobDetail("jd-" + name, "jdg-" + name, typeof(T)); j1.RequestsRecovery = true;
            if (map != null)
                j1.JobDataMap.PutAll(map);
            tg1 = new SimpleTrigger("st-" + name, "stg-" + name, t.AddSeconds(addsec));
            sched.ScheduleJob(j1, tg1);
            Show("ScheduleJob->JobDetail:{0} {1} Trigger:{2} {3}", j1.Name, j1.JobType, tg1.Name, tg1.StartTimeUtc);
        }
        protected static void Show(string f, params object[] args)
        {
            try
            {
                Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("dd HH:mm:ss fff"), string.Format(f, args)));//
            }
            catch { };
        }
        public static void Shutdown()
        {
            if (sched != null)
            {
                sched.Shutdown(false);
                sched = null;
            }
        }
    }
    public class ModelBase
    {
        [PrimaryKey]
        public virtual int ID { get; set; }
        [Property]
        public virtual int hash { get; set; }
    }
    public abstract class JobBase : IJob
    {
        protected RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;
        public abstract void Execute(JobExecutionContext context);
        protected static bool SaveItems<T>(IList<T> items) where T : ModelBase
        {
            bool exist = false;
            if (items == null||items.Count==0) return true;
            int existCount = 0;
            foreach (T item in items)
            {
                try
                {
                    if (!ActiveRecordMediator<T>.Exists(Expression.Eq("hash", item.hash)))
                    {
                        ActiveRecordMediator<T>.Create(item);
                        Show("Save->Hash:{0}", item.hash);
                    }
                    else
                    {
                        //Show("Exists->Hash:{0}", item.hash);
                        exist = true; existCount++;
                    }
                }
                catch (Exception ex) { ShowException(ex); }
            }
            Show("Exists->Count:{0}", existCount);
            return exist;
        }
       protected static void Show(string f, params object[] args)
        {
            try
            {
                Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("dd HH:mm:ss fff"), string.Format(f, args)));//
            }
            catch { };
        }
       protected static void ShowException(Exception ex)
       {
           if (ex != null)
           {
               Show("Exception:{0}", ex.Message);
               Show("StackTrace:{0}", ex.StackTrace);
               ShowException(ex.InnerException);
           }
       }
    }
}
