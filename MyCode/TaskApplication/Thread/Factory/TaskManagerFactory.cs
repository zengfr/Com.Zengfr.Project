using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zfrong.Amib.Threading
{
    public class TaskManagerFactory : DictionaryManagerBase<string, TaskManager>
    {
        public static TaskManagerFactory Instance = new TaskManagerFactory();
        public virtual TaskManager Create(string keyName) 
        {
            return Create<TaskManager>(keyName);
        }
          public virtual TaskManager Create<T>(string keyName) where T : TaskManager, new()
       {
           if (!this.Exist(keyName))
           {
               T item = new T();
               item.Name = keyName;
               this.Items[item.Name] =item;
           }
           return this.Items[keyName];
       }
       
    }
}
