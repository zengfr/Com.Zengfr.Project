using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
namespace Zfrong.Amib.Threading
{
    public class EventManager
    {
        public EventManager() { StatusInfo = new int[7]; }

        public event System.Action<TaskBase, TaskStatus, TaskStatus> OnTaskStatusChanged;
        public event System.Action<TaskBase> OnTaskCompleted;
        public event System.Action<TaskBase> OnTaskFailing;
        public event System.Action<TaskBase> OnTaskQueued;
        public event System.Action<TaskBase> OnTaskWaitting;
        public event System.Action<TaskBase> OnTaskRunning;
        public event System.Action<TaskBase> OnTaskScheduled;
        public event System.Action<IWorkItemsGroup> OnWorkItemsGroupIdle;
        public event ThreadTerminationHandler OnThreadTermination;
        public int[] StatusInfo { get; set; }
        public string Name { get; set; }
        protected object lockobj = new object();
        
        internal virtual void FireThreadTermination()
        {
            if (this.OnThreadTermination != null)
                this.OnThreadTermination.BeginInvoke(null, null);
        }

        internal virtual void FireTaskStatusChanged(TaskBase task, TaskStatus oldStatus, TaskStatus newStatus)
        {
            lock (lockobj)
            {
                StatusInfo[(int)oldStatus] -= 1;
                StatusInfo[(int)newStatus] += 1;
            }
            switch (newStatus)
            {
                case TaskStatus.Completed:
                    if (this.OnTaskCompleted != null)
                        this.OnTaskCompleted.BeginInvoke(task,  null, null);
                    break;
                case TaskStatus.Failing:
                    if (this.OnTaskFailing != null)
                        this.OnTaskFailing.BeginInvoke(task,  null, null);
                    break;
                case TaskStatus.Queued:
                    if (this.OnTaskQueued != null)
                        this.OnTaskQueued.BeginInvoke(task,  null, null);
                    break;
                case TaskStatus.Waitting:
                    if (this.OnTaskWaitting != null)
                        this.OnTaskWaitting.BeginInvoke(task, null, null);
                    break;
                case TaskStatus.Running:
                    if (this.OnTaskRunning != null)
                        this.OnTaskRunning.BeginInvoke(task,  null, null);
                    break;
                case TaskStatus.Scheduled:
                    if (this.OnTaskScheduled != null)
                        this.OnTaskScheduled.BeginInvoke(task,  null, null);
                    break;
                default: break;
            }
            if (this.OnTaskStatusChanged != null)
                this.OnTaskStatusChanged.BeginInvoke(task, oldStatus,newStatus, null, null);
        }
        internal virtual void FireWorkItemsGroupIdle(IWorkItemsGroup wig)
        {
            if (this.OnWorkItemsGroupIdle != null)
                this.OnWorkItemsGroupIdle.BeginInvoke(wig, null, null);
        }
    }
}
