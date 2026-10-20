using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTask_UI.p1;

namespace UniTask_UI.Task
{
    internal class TaskManager

    {
        public static TaskManager Instance = new TaskManager();
        public List<TaskItem> tasks { get; set; }

        public TaskManager() {
            tasks = new List<TaskItem>();
        }
    }
}
