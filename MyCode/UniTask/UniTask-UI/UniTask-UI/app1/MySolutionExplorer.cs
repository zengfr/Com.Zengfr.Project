using System;
using System.Windows.Forms;
using DockSample;
using UniTask_UI.common;
using UniTask_UI.p1;
using WeifenLuo.WinFormsUI.Docking;
namespace UniTask_UI
{
    public partial class MySolutionExplorer : ToolWindow
    {
        string xmlFileName = "projects.xml";
        public MySolutionExplorer()
        { 
            InitializeComponent();
            this.Name = "MySolutionExplorer";
            this.TabText = "项目浏览器";
            this.Text = "项目浏览器";

        }

        //protected override void OnRightToLeftLayoutChanged(EventArgs e)
        //{
        //    treeView1.RightToLeftLayout = RightToLeftLayout;
        //}

        private void MySolutionExplorer_Load(object sender, EventArgs e)
        {
            MyCtrl.Instance.solutionExplorer = this;
            XmlTreeViewState treeState = new XmlTreeViewState();
            if (System.IO.File.Exists(xmlFileName))
            {
                treeState.LoadTreeViewState(treeView1, xmlFileName);
                
            }
            else
            {
                treeState.SaveTreeViewState(treeView1, xmlFileName);
            }
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].ExpandAll();
        }

        private void MySolutionExplorer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            XmlTreeViewState treeState = new XmlTreeViewState();
            treeState.SaveTreeViewState(treeView1, xmlFileName);
          
        }

        private void toolBar_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolBarButtonToolbox)
                menuItemToolbox_Click(null, null);
        }

        private void menuItemToolbox_Click(object p1, object p2)
        {
            var p = MyCtrl.Instance.settingForm;
            if (p == null)
            {
                p = new MySettingForm();
                MyCtrl.Instance.settingForm = p;
            }
            p.Show(this.DockPanel);
        }
    }
}