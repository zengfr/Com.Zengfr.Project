using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Zfrong.Amib.Threading
{
    public class WorkerFactory : DictionaryManagerBase<string, WorkerBase>
    {
        public static WorkerFactory Instance = new WorkerFactory();
         /// <summary>
        ///  添加处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workerName"></param>
        public virtual WorkerBase Create<T>(string workerName) where T : WorkerBase, new()
        {
            if (!this.Exist(workerName))
            {
                T item = new T();
                item.Name = workerName;
                this.Items[item.Name] = item;
            }
            return this.Items[workerName];
        }
       
        public virtual void Pause()
        {
            foreach (KeyValuePair<string, WorkerBase> kv in Items)
                kv.Value.Pause();
        }
        public virtual void Resume()
        {
            foreach (KeyValuePair<string, WorkerBase> kv in Items)
                kv.Value.Resume();
        }
    }
   
   

}
