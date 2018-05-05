using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Amib.Threading;
namespace Zfrong.Amib.Threading
{
    public enum TaskStatus:sbyte
    { 
        Created,
       Scheduled,
       Queued,
       Waitting,
       Running,
        Failing,
        Completed
    }
    public class TaskInfo  
    {
        static long IDCreator = 0;
        static object lockobj = new object();
        public TaskInfo()
        {
            Status = TaskStatus.Created;
            lock (lockobj)
            {
                System.Threading.Interlocked.Increment(ref IDCreator);
                this.ID = IDCreator;
            }
        }
        /// <summary>
        /// 任务状态
        /// </summary>
        internal virtual TaskStatus Status { get;  set; }
        public virtual Exception Exception { get; internal set; }
        
        /// <summary>
        /// 任务标识
        /// </summary>
        public long ID { get; protected set; }

        /// <summary>
        /// PoolName
        /// </summary>
        public string PoolName { get; internal set; }
        /// <summary>
        /// 处理开始时间
        /// </summary>
        public DateTime StartTime { get; internal set; }
        /// <summary>
        /// 处理结束时间
        /// </summary>
        public DateTime EndTime { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public double TimeSpan
        {
            get { return (EndTime - StartTime).TotalMilliseconds; }
        }
    }
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract class TaskBase
    {
       
        public TaskBase() {
            this.TaskInfo = new TaskInfo();
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string TaskType { get; set; }
        
        /// <summary>
        /// 队列分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 工作处理器名称
        /// </summary>
        public string WorkerName { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public WorkItemPriority Priority { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual TaskInfo TaskInfo { get;protected set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual TaskStatus TaskStatus
        {
           internal set
            {
                if (!this.TaskStatus.Equals(value))
                {
                    TaskStatus oldStatus = this.TaskStatus;
                    this.TaskInfo.Status = value;
                    EventManager ev = EventManagerFactory.Instance.Create(this.TaskInfo.PoolName);
                    ev.FireTaskStatusChanged(this, oldStatus, this.TaskStatus);
                }
            }
            get
            {
                return this.TaskInfo.Status;
            }

        }
        
       
        
    }
}
