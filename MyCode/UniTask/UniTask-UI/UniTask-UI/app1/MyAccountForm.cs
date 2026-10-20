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
    public partial class MyAccountForm : MyDataGridForm<Account>
    {
        public MyAccountForm() : base()
        {
            InitializeComponent();
        }


       

        public override void initBind()
        {
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.Name = "MyAccountForm";
            this.TabText = "账号管理";
            this.Text = "账号管理";
            MyCtrl.Instance.accountForm = this; 
            setDataSource(MyData.Instance.accounts.OrderByDescending(t=>t.Sort));
        }
        public override void ItemDelete(Account t1)
        {
            LoggerUtil.Info("delete:{0}", t1.Name);
            MyData.Instance.accounts.Remove(t1);
        }
        public override void ItemAdd()
        {
            MyData.Instance.accounts.Add(new Account());
        }
    }
}
