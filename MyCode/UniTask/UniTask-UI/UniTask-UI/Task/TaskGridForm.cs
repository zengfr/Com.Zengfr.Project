using DockSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTask_UI.p1.c;
using UniTask_UI.p1;
using UniTask_UI.common;

namespace UniTask_UI.Task
{
    public partial class TaskGridForm : MyDataGridForm<TaskItem>
    {
        public override void initBind()
        {
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.Name = "taskGridForm";
            this.TabText = "任务管理";
            this.Text = "任务管理";
            MyCtrl.Instance.taskGridForm = this;
            setDataSource(TaskManager.Instance.tasks.OrderBy(t => t.id));
        }
        public override void ItemDelete(TaskItem t1)
        {
            LoggerUtil.Info("delete:{0}", t1.name);
            TaskManager.Instance.tasks.Remove(t1);
        }
        public override void ItemAdd()
        {
            TaskManager.Instance.tasks.Add(new TaskItem());
        }
    }

}
