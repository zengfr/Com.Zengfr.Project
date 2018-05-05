using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zfrong.Amib.Threading
{
    public class TaskManager : DictionaryManagerBase<long, TaskBase>
    {
      public string Name { get; set; }
      public virtual void Add(TaskBase task)
      {
          Add(task.TaskInfo.ID, task);
       }
      
    }
}
