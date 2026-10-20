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
    public partial class MyOrderForm : MyDataGridForm<Order>
    {
        public MyOrderForm() : base()
        {
            InitializeComponent();
        }

        public override void initBind()
        {
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;

            this.Name = "MyProductForm";
            this.TabText = "订单数据";
            this.Text = "订单数据";

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = "Modify";
            btn.HeaderText = "功能操作";
            btn.DefaultCellStyle.NullValue = "取消订单";
            dataGridView1.Columns.Add(btn);

            MyCtrl.Instance.orderForm = this;
            setDataSource(MyData.Instance.orders.OrderByDescending(t => t.LastUpdate));
            this.dataGridView1.ReadOnly = true;

        }
        protected override void initCtrl()
        {
            //base.initCtrl();
        }
        protected override void CellContentClick(DataGridViewCellEventArgs e, EntityBase item)
        {
            if (e.ColumnIndex == 4)
            {
                Order order = item as Order;
                LoggerUtil.Info ("订单取消:{0}", order.OrderNum);
                // this.dataGridView1.Rows.RemoveAt(e.RowIndex);//删除当前行
            }
        }

        public override void ItemDelete(Order p)
        {
            //log.InfoFormat("delete:{0}", p.OrderNum);
            //MyData.Instance.orders.Remove(p);
        }
        public override void ItemAdd()
        {
            //MyData.Instance.products.Add(new Customer());
        }
    }
}
