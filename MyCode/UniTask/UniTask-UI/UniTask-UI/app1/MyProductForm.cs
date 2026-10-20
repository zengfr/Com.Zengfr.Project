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
using log4net;
using UniTask_UI.common;
using UniTask_UI.p1.c;

namespace UniTask_UI.p1
{
    public partial class MyProductForm : MyDataGridForm<ProductDetail>
    { 
        public MyProductForm():base()
        {
            InitializeComponent(); 
        }
        protected override void initCtrl()
        {
            //base.initCtrl();
        }
        public override  void initBind()
        {
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;

            this.Name = "MyProductForm";
            this.TabText = "量态数据";
            this.Text = "量态数据";

            MyCtrl.Instance.productForm = this;
            setDataSource(MyData.Instance.products.Values.OrderByDescending(t => t.lastUpate));
        }
        public override void ItemDelete(ProductDetail p)
        {
            var k = String.Format("{0},{1},{2}", p.lineCode, p.bookDate, p.beginTime);
            LoggerUtil.Info ("delete:{0}", k);
            MyData.Instance.products.Remove(k);
        }
        public override void ItemAdd()
        {
            //MyData.Instance.products.Add(new Customer());
        }
    }
}
