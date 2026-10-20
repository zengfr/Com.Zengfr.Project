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
using UniTask_UI.p1.c;

namespace UniTask_UI.p1
{
    public partial class MyHeadForm : MyDataGridForm<Head>
    {
        public MyHeadForm()
        {
            InitializeComponent();
        }
        public override void initBind()
        {
            this.Name = "MyHeadForm";
            this.TabText = "头标识管理";
            this.Text = "头标识管理";
            MyCtrl.Instance.headForm = this;
            setDataSource(MyData.Instance.heads);
        }

        public override void ItemDelete(Head t1)
        {
            MyData.Instance.heads.Remove(t1);
        }
        public override void ItemAdd()
        {
            MyData.Instance.heads.Add(new Head());
        }

    }
}
