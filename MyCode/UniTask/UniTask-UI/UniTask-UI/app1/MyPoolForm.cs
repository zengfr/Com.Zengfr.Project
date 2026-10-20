using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amib.Threading;
using DockSample;
using UniTask_UI.p1.c;

namespace UniTask_UI.p1
{
    public partial class MyPoolForm : MyDataGridForm<SmartThreadPool>
    {
        public MyPoolForm()
        {
            InitializeComponent();
        }
        protected override void initCtrl()
        {
            //base.initCtrl();
        }
        public override void initBind()
        {
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;

            this.Name = "MyPoolForm";
            this.TabText = "线程池";
            this.Text = "线程池";
            //MyCtrl.Instance.proxyForm = this;
            setDataSource(MyThreadPool.Instances.Values.Select(t=>t.smart));
            this.dataGridView1.ReadOnly = true;
        } 

        public override void ItemDelete(SmartThreadPool t1)
        {
            //MyData.Instance.proxys.Remove(t1);
        }
        public override void ItemAdd()
        {
            //MyData.Instance.proxys.Add(new Proxy());
        }

    }
}
