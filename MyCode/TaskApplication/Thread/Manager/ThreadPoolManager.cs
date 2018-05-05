using System;
using System.Collections.Generic;
using Amib.Threading;
using System.Text;
namespace Zfrong.Amib.Threading
{
    /// <summary>
    /// Facade 外观模式类(支持任务、分组、处理器、事件、优先级、暂停恢复、参数动态设置、日志)
    /// </summary>
    /// <remarks>
    ///t = SmartThreadPoolFacade.Create(name, 40); 
    ///t.AddWorker ("worker"); 
    ///t.AddGroup("group", 32); 
    ///t.EventFacade.OnThreadInitialization += new ThreadInitializationHandler(EventFacade_OnThreadInitialization); 
    ///t.EventFacade.OnThreadTermination += new ThreadTerminationHandler(EventFacade_OnThreadTermination); 
    ///t.EventFacade.OnTaskAdd += new Action <br/>
    ///t.EventFacade.OnTaskProcessing += new Action  
    ///t.EventFacade.OnTaskCompleted += new Action  
    ///t.EventFacade.OnWorkItemsGroupIdle += new Action  
    ///t.AddTask(new Task() { GroupName ="group", WorkerName = "worker", ID = i }); 
    ///t.Start();<br/>
    ///System.Threading.Thread.Sleep(5000); 
    ///t.Pause();<br/>
    ///System.Threading.Thread.Sleep(6000); 
    ///t.Resume(); 
    /// </remarks>
    public partial class ThreadPoolManager
    {
       
        internal ThreadPoolManager(string poolName, int maxWorkerThreads) 
        {
            InitThreadPool(poolName, maxWorkerThreads);
        }
        

        public SmartThreadPool SmartThreadPool { get; protected set; }
        public WorkItemsGroupFactory WorkItemsGroupFactory { get; protected set; }

        public WorkerFactory WorkerFactory { get; protected set; }
        public EventManager EventManager { get; protected set; }
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="maxWorkerThreads"></param>
        protected virtual void InitThreadPool(string poolName, int maxWorkerThreads)
        {
            this.SmartThreadPool = SmartThreadPoolFactory.Instance.Create(poolName, maxWorkerThreads);
            this.WorkItemsGroupFactory = new WorkItemsGroupFactory(SmartThreadPool);

            this.WorkerFactory = WorkerFactory.Instance;
            this.EventManager = EventManagerFactory.Instance.Create(this.SmartThreadPool.Name);
        }
        /// <summary>
        /// 添加队列分组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="concurrency"></param>
        public virtual void CreateWorkItemsGroup(string groupName, int concurrency)
        {
            this.WorkItemsGroupFactory.Create(groupName, concurrency);
        }
        /// <summary>
        /// 添加处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workerName">名称、标识</param>
        /// <param name="concurrency">并发数</param>
        public virtual void CreateWorker<T>(string workerName) where T : WorkerBase, new()
        {
            this.WorkerFactory.Create<T>(workerName);
        }
        /// <summary>
        /// 添加任务,添加前必须添加Group，Worker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        public virtual void AddTask<T>(T task) where T : TaskBase
        {
            
            this.WorkItemsGroupFactory.AddTask<T>(task);
        }
        #region
        public virtual void Start()
        {
            this.SmartThreadPool.Start();
            this.WorkItemsGroupFactory.Start();
        }
        public virtual void Cancel()
        {
            this.SmartThreadPool.Cancel();
            this.WorkItemsGroupFactory.Cancel();
        }
        public virtual void Resume()
        {
            this.WorkerFactory.Resume();
        }
        public virtual void Pause()
        {
            this.WorkerFactory.Pause();
        }
        public virtual string GetStatisticsInfo()
        {
            return GetStatisticsInfo(this.SmartThreadPool);
        }
        static string GetStatisticsInfo(SmartThreadPool pool)
        {
            return string.Format("ActiveThreads:{0,3},InUseThreads:{1,3},Queued:{3},Processed:{2}",
              pool.PerformanceCountersReader.ActiveThreads,
              pool.PerformanceCountersReader.InUseThreads,
              pool.PerformanceCountersReader.WorkItemsQueued,
              pool.PerformanceCountersReader.WorkItemsProcessed
              );
        }
        #endregion
    }
}
