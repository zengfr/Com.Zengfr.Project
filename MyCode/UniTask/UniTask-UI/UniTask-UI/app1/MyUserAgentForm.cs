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
    public partial class MyUserAgentForm : MyDataGridForm<UserAgent>
    {
        public MyUserAgentForm()
        {
            InitializeComponent();
        }
        public override void initBind()
        {
            this.Name = "MyUserAgentForm";
            this.TabText = "浏览器管理";
            this.Text = "浏览器管理";
            MyCtrl.Instance.userAgentForm = this;
            setDataSource(MyData.Instance.userAgents);
        }

        public override void ItemDelete(UserAgent t1)
        {
            MyData.Instance.userAgents.Remove(t1);
        }
        public override void ItemAdd()
        {
            MyData.Instance.userAgents.Add(new  UserAgent());
        }

    }
}
