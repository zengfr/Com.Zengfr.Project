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
    public partial class MyProxyForm : MyDataGridForm<Proxy>
    {
        public MyProxyForm()
        {
            InitializeComponent();
        }
        public override void initBind()
        {
            this.Name = "MyProxyForm";
            this.TabText = "网络代理管理";
            this.Text = "网络代理管理";
            MyCtrl.Instance.proxyForm = this;
            setDataSource(MyData.Instance.proxys);
        }

        public override void ItemDelete(Proxy t1)
        {
            MyData.Instance.proxys.Remove(t1);
        }
        public override void ItemAdd()
        {
            MyData.Instance.proxys.Add(new Proxy());
        }

    }
}
