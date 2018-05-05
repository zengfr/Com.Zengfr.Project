using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
namespace Zfrong.Amib.Threading
{
    public class WorkItemsGroupFactory : DictionaryManagerBase<string, IWorkItemsGroup>
    {
        #region Instance

        public WorkItemsGroupFactory(SmartThreadPool smartThreadPool)
        {
            this.SmartThreadPool = smartThreadPool;
        }
        protected SmartThreadPool SmartThreadPool { get; private set; }
        #endregion

          /// <summary>
        ///   添加队列分组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="concurrency"></param>
        public virtual IWorkItemsGroup Create(string groupName, int concurrency)
        {
            IWorkItemsGroup wig;
            if (!this.Exist(groupName))
            {
                wig = this.SmartThreadPool.CreateWorkItemsGroup(concurrency);
                wig.Name = groupName;
                EventManager ev = EventManagerFactory.Instance.Create(this.SmartThreadPool.Name);
                wig.OnIdle += ev.FireWorkItemsGroupIdle;
                this.Items[wig.Name] = wig;
            }
            return this.Items[groupName];
        }
         
        /// <summary>
        /// 添加任务,添加前必须添加Group，Worker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        public void AddTask<T>(T task) where T : TaskBase
        {
            task.TaskInfo.PoolName = this.SmartThreadPool.Name;
            WorkerBase worker = WorkerFactory.Instance.Get(task.WorkerName);
            if (worker != null)
            {
                IWorkItemsGroup wig = Get(task.GroupName);
                if (wig != null)
                {
                    task.TaskStatus = TaskStatus.Scheduled;
                    wig.QueueWorkItem<T>(worker.DoWorker, task, task.Priority);
                    task.TaskStatus = TaskStatus.Queued;
                }
                else
                {
                    throw new Exception("AddTask<T>:wig null");
                }
            }
            else
            {
                throw new Exception("AddTask<T>:worker null");
            }
        }
        #region
        public virtual void Start()
        {
            foreach (KeyValuePair<string, IWorkItemsGroup> kv in this.Items)
                kv.Value.Start();
        }
        public virtual void Cancel()
        {
            foreach (KeyValuePair<string, IWorkItemsGroup> kv in this.Items)
                kv.Value.Cancel();
        }
        #endregion
    }
}
