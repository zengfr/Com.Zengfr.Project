using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using log4net;
using UniTask_UI.common;

namespace UniTask_UI.p1.c
{
    public class MyDataGridForm<T> : ToolWindow where T : class
    { 
        public MyDataGridForm()
        {
            InitializeComponent2();
            InitializeComponent3();
            init();
        }
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent3()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();

            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // 
            // dataGridView1
            // 
            this.dataGridView1.ScrollBars = ScrollBars.Both;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(284, 261);
            this.dataGridView1.TabIndex = 0;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.RowHeaderMouseClick += DataGridView1_RowHeaderMouseClick;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.CellContentClick += DataGridView1_CellContentClick;
           
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem,
             this.新增ToolStripMenuItem,
            this.删除ToolStripMenuItem,this.清空ToolStripMenuItem  });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);

            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);

            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);

            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MyCustomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));

            this.Load += new System.EventHandler(this.MyAccountForm_Load);


            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void DataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            var p = MyCtrl.Instance.propertyForm;
            if (p == null)
            {
                p = new app1.MyPropertiesForm();
                MyCtrl.Instance.propertyForm = p;
            }
            p.propertyGrid.SelectedObject = this.DataSource.ElementAt(index);
            p.Show(this.DockPanel);
        }


        #endregion

        protected System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        IEnumerable<T> DataSource;
        private void init()
        {
            initCtrl(); 
            initBind();
           
        }
        protected virtual void initCtrl()
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = "UP";
            btn.HeaderText = "UP";
            btn.DefaultCellStyle.NullValue = "+";
            dataGridView1.Columns.Add(btn);

            DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
            btn2.Name = "DN";
            btn2.HeaderText = "DN";
            btn2.DefaultCellStyle.NullValue = "-";
            dataGridView1.Columns.Add(btn2);

            DataGridViewButtonColumn btn3 = new DataGridViewButtonColumn();
            btn3.Name = "TOP";
            btn3.HeaderText = "TP";
            btn3.DefaultCellStyle.NullValue = "++";
            dataGridView1.Columns.Add(btn3);

            DataGridViewButtonColumn btn4 = new DataGridViewButtonColumn();
            btn4.Name = "BN";
            btn4.HeaderText = "BN";
            btn4.DefaultCellStyle.NullValue = "--";
            dataGridView1.Columns.Add(btn4);

           
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
            if (column is DataGridViewButtonColumn)
            {
                EntityBase item = dataGridView1.Rows[e.RowIndex].DataBoundItem as EntityBase;
                switch (e.ColumnIndex)
                {

                    case 0:
                        item.Sort += 1;
                        if (item.Sort > 111) item.Sort = 111;
                        LoggerUtil.Info ("顺序:{0}", item.Sort);
                        break;
                    case 1:
                        item.Sort -= 1;
                        if (item.Sort < 0|| item.Sort > 111) item.Sort = 0;
                        LoggerUtil.Info ("顺序:{0}", item.Sort);
                        break;
                    case 2:
                        item.Sort += 100;
                        if (item.Sort > 111) item.Sort = 111;
                        LoggerUtil.Info ("顺序:{0}", item.Sort);
                        break;
                    case 3:
                        item.Sort -= 100;
                        if (item.Sort < 0 || item.Sort > 111) item.Sort = 0;
                        LoggerUtil.Info ("顺序:{0}", item.Sort);
                        break;
                    default:
                        CellContentClick( e,item);
                        break;
                }
                this.dataBind();
            }
        }
        protected virtual void CellContentClick(DataGridViewCellEventArgs e, EntityBase item)
        {

        }

        public void setDataSource(ICollection<T> rs)
        {
            this.DataSource = rs; dataBind();
        }
        public void setDataSource(IEnumerable<T> rs)
        {
            this.DataSource = rs;
            dataBind();
        }
        private void MyAccountForm_Load(object sender, EventArgs e)
        {
            // init();
        }
        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggerUtil.Info("clear");
            dataGridView1.SelectAll();
            ItemDeleteSelected();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggerUtil.Info("refresh");
            this.dataBind();
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggerUtil.Info("add");
            this.ItemAdd();
            this.dataBind();
        }


        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggerUtil.Info("delete");
            ItemDeleteSelected();
        }
        protected virtual void ItemDeleteSelected()
        {
            var srs = dataGridView1.SelectedRows;
            foreach (DataGridViewRow r in srs)
            {
                var t = r.DataBoundItem as T;
                this.ItemDelete(t);
            }
            this.dataBind();
        }
        public virtual void ItemDelete(T t1)
        {

        }
        public virtual void initBind()
        {

        }
        public virtual void ItemAdd()
        {

        }
      
        public void dataBind()
        {
            UpdaterUI.Update(dataGridView1, new UpdaterUI.UpdateInvoke(dataBind_));
        }
        private void dataBind_()
        {
            if (this.DataSource != null)
            {
                LoggerUtil.Info("bind");
                dataGridView1.DataSource = new BindingList<T>(this.DataSource.ToList());
                int cc = this.dataGridView1.Columns.Count;
                foreach (System.Windows.Forms.DataGridViewColumn c in dataGridView1.Columns)
                {
                    if (c.HeaderText.Equals("启用"))
                    {
                        c.DisplayIndex = 0;
                    }
                    else if (c.HeaderText.Equals("顺序"))
                    {
                        c.DisplayIndex = 6;
                    }
                    else if (c.HeaderText.Equals("最后使用"))
                    {
                        c.DisplayIndex = cc-2;
                    }
                    else if (c.HeaderText.Equals("最后更新"))
                    {
                        c.DisplayIndex = cc-1;
                    }
                    else if (c.HeaderText.Equals("UP"))
                    {
                        c.DisplayIndex = 2;
                    }
                    else if (c.HeaderText.Equals("DN"))
                    {
                        c.DisplayIndex = 3;
                    }
                    else if (c.HeaderText.Equals("TP"))
                    {
                        c.DisplayIndex = 4;
                    }
                    else if (c.HeaderText.Equals("BN"))
                    {
                        c.DisplayIndex = 5;
                    }
                    else 
                    {
                        c.DisplayIndex = cc-3;
                    }
                }
                dataGridView1.Refresh();
            }
        }
        public void RefreshGrid()
        {
            dataGridView1.Refresh();
        }
            private void InitializeComponent2()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(97, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(292, 266);
            this.splitContainer1.SplitterDistance = 1;
            this.splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // MyDataGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MyDataGridForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private ToolStrip toolStrip1;
        private SplitContainer splitContainer1;
    }
}
