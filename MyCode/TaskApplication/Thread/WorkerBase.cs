using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Zfrong.Amib.Threading
{
    /// <summary>
    /// 工作基类
    /// </summary>
    public abstract class WorkerBase
    {
        private ManualResetEvent _pauseFlag = new ManualResetEvent(true);
        /// <summary>
        /// 名称、标识
        /// </summary>
        public string Name { get; set; }
        public virtual void Pause()
        {
            _pauseFlag.Reset();
        }
        public virtual void Resume()
        {
            _pauseFlag.Set();
        }
        /// <summary>
        /// 入口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        internal virtual void DoWorker<T>(T task) where T : TaskBase
        {
            if (task != null)
            {
                task.TaskStatus = TaskStatus.Waitting;
                _pauseFlag.WaitOne();
                task.TaskStatus= TaskStatus.Running;
                task.TaskInfo.StartTime = DateTime.Now;
                try
                {
                    DoTask<T>(task);
                }
                catch (Exception ex)
                {
                    task.TaskStatus = TaskStatus.Failing;
                    task.TaskInfo.Exception = ex;
                }
                task.TaskInfo.EndTime = DateTime.Now;
                task.TaskStatus = TaskStatus.Completed;
            }
        }
        protected abstract void DoTask<T>(T t) where T : TaskBase;
    }
}
