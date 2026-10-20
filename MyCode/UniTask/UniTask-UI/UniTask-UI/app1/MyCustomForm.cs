using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using UniTask_UI.common;
using UniTask_UI.p1.c;

namespace UniTask_UI.p1
{
    public partial class MyCustomForm :MyDataGridForm<Customer>
    {
        public MyCustomForm()
        {
            InitializeComponent();
        }
        public override void initBind()
        {
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.Name = "MyCustomForm";
            this.TabText = "客户管理";
            this.Text = "客户管理";
            MyCtrl.Instance.customForm = this;
            setDataSource(MyData.Instance.customers.OrderByDescending(t => t.Sort));

            
            
        }

        public override void ItemDelete(Customer t1)
        {
            LoggerUtil.Info ("delete:{0}", t1.Name);
            MyData.Instance.customers.Remove(t1);
        }
        public override void ItemAdd()
        {
            MyData.Instance.customers.Add(new Customer());
        }

    }
}
