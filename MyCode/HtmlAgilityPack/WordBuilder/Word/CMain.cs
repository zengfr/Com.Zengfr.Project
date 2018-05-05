using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DockSample;
using CommonPack.HtmlAgilityPack;
using CommonPack.LCS;
namespace WordBuilder
{
    public partial class CMain : Form
    {
        TemplateBuilder templateBuilder = new TemplateBuilder();
        ItemBuilder itemBuilder = new ItemBuilder();
        public CMain()
        {
            InitializeComponent();

            this.listBox1.Items.Insert(0, "http://zhidao.baidu.com/question/40886025.html");//
            this.listBox1.Items.Insert(0, "http://zhidao.baidu.com/question/40476071.html");//
            templateBuilder.TemplateFile = "tmp.html";
        }
        /// <summary>
        /// //初始化
        /// </summary>
        private void InitC()
        {

        }
        #region 菜单
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();//
          if(f.IsTrue)
             this.listBox1.Items.Insert(0, f.textBox1.Text);
        }
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.textBox1.Text = this.listBox1.SelectedItem.ToString();
            f.ShowDialog();//
            if (f.IsTrue)
                this.listBox1.SelectedItem = f.textBox1.Text;
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
        }

        private void 生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            templateBuilder.Urls.Clear();//
            foreach (string str in this.listBox1.Items)
                templateBuilder.AddUrl(str);//
            
            templateBuilder.Start();//
            //
            MessageBox.Show("生成成功！");//
        }

        
        
        private void 预览结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Rows.Clear();//

            itemBuilder.SetTemplate(templateBuilder.TemplateFile);//
            itemBuilder.SetText(this.listBox1.SelectedItem.ToString());//
            itemBuilder.GetAllItemValue();//
            
            foreach (KeyValuePair<int, string> v in itemBuilder.Diff)
                this.dataGridView2.Rows.Add(v.Key,v.Value);//
           
        }

      

        
        #endregion
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0) return;//
            DataGridViewRow row = this.dataGridView1.SelectedRows[0];
            IsExist(this.dataGridView2,0,row.Cells[0].Value,true);
               
        }
        private void dataGridView2_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dataGridView2.SelectedRows.Count == 0) return;//
            DataGridViewRow row = this.dataGridView2.SelectedRows[0];
            if (!IsExist(this.dataGridView1, 0, row.Cells[0].Value, false))
                this.dataGridView1.Rows.Add(new object[] { row.Cells[0].Value });//
        }
        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.dataGridView1.Rows.RemoveAt(e.RowIndex);//
        }
        private static bool IsExist(DataGridView dgv, int colIndex, object value, bool selected)
        {
            if (value == null) return false;//
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[colIndex].Value == null) continue;//
                    if (row.Cells[colIndex].Value.ToString() == value.ToString())
                    {
                        if (selected)
                        {
                            row.Selected = true;//
                            dgv.FirstDisplayedScrollingRowIndex = row.Index;//
                        }
                        return true;
                    }
                }
            return false;//
        }
    }
}