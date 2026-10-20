using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using log4net;
using log4net.Core;
using UniTask_UI.app1;
using UniTask_UI.common;
using UniTask_UI.p1;
using UniTask_UI.p1.c;
using WeifenLuo.WinFormsUI.Docking;
namespace UniTask_UI.p1
{
    public partial class MyMainForm : Form

    {
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender1 = new VisualStudioToolStripExtender();
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1 = new VS2015LightTheme();
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme1 = new VS2015BlueTheme();
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1 = new VS2015DarkTheme();
    
        public MyMainForm()
        {
            InitializeComponent();
             
        }
        private void initMyData()
        {
            

            MyData d = MyStoreUtil.load<MyData>("db");
            if (d != null)
            {
                d.init();
                MyData.Instance = d;
            }
        }

        private void initCtrl()
        {
            menuItemSchemaVS2015Blue.Checked = true;
            var theme = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.dockPanel1.Theme = theme;

            MyCtrl.Instance.dockPanel1 = this.dockPanel1;
            MyCtrl.Instance.load();

            var d = MyData.Instance;
            if (d != null)
            {
                foreach (Order order in d.orders)
                {
                    LoggerUtil.Info("订单:{0} 客户:{1} 账户:{2} 备注:{3}", order.OrderNum, order.CustomerName, order.AccountName, order.Comment);
                }
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            initMyData();

            initCtrl();
        }





        private void MyMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                MyData d = MyData.Instance;
                d.goalSetting.init = false;
                d.goalSetting.isStart = false;
                d.goalSetting.wait = 40;

                if (d.goalSetting.regDate < DateTime.Today.AddYears(-99))
                    d.goalSetting.regDate = DateTime.Now;
                foreach (var a in d.accounts)
                    a.Status = "未执行";
                MyStoreUtil.save<MyData>("db", d);

                MyCtrl.Instance.SaveAsXml();
                MyTaskUtil.Instance.stop();
            }
            catch (Exception ex)
            {

            }

        }
        private void SetSchema(object sender, System.EventArgs e)
        {
            MyCtrl.Instance.SaveAsXml();

            CloseAllContents();

            if (sender == this.menuItemSchemaVS2015Blue)
            {
                this.dockPanel1.Theme = this.vS2015BlueTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015BlueTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Light)
            {
                this.dockPanel1.Theme = this.vS2015LightTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015LightTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Dark)
            {
                this.dockPanel1.Theme = this.vS2015DarkTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015DarkTheme1);
            }


            menuItemSchemaVS2015Light.Checked = (sender == menuItemSchemaVS2015Light);
            menuItemSchemaVS2015Blue.Checked = (sender == menuItemSchemaVS2015Blue);
            menuItemSchemaVS2015Dark.Checked = (sender == menuItemSchemaVS2015Dark);
            //if (dockPanel.Theme.ColorPalette != null)
            //{
            //    statusBar.BackColor = dockPanel.Theme.ColorPalette.MainWindowStatusBarDefault.Background;
            //}
            MyCtrl.Instance.load();
        }
        private void SetDocumentStyle(object sender, System.EventArgs e)
        {
            DocumentStyle oldStyle = dockPanel1.DocumentStyle;
            DocumentStyle newStyle;
            if (sender == menuItemDockingMdi)
                newStyle = DocumentStyle.DockingMdi;
            else if (sender == menuItemDockingWindow)
                newStyle = DocumentStyle.DockingWindow;
            else if (sender == menuItemDockingSdi)
                newStyle = DocumentStyle.DockingSdi;
            else
                newStyle = DocumentStyle.SystemMdi;

            if (oldStyle == newStyle)
                return;

            if (oldStyle == DocumentStyle.SystemMdi || newStyle == DocumentStyle.SystemMdi)
                CloseAllDocuments();

            dockPanel1.DocumentStyle = newStyle;
            menuItemDockingMdi.Checked = (newStyle == DocumentStyle.DockingMdi);
            menuItemDockingWindow.Checked = (newStyle == DocumentStyle.DockingWindow);
            menuItemDockingSdi.Checked = (newStyle == DocumentStyle.DockingSdi);
            //menuItemSystemMdi.Checked = (newStyle == DocumentStyle.SystemMdi); 
        }
        private void CloseAllDocuments()
        {
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel1.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
        }
        private void CloseAllContents()
        {

            // we don't want to create another instance of tool window, set DockPanel to null
            MyCtrl.Instance.solutionExplorer.DockPanel = null;
            MyCtrl.Instance.accountForm.DockPanel = null;
            MyCtrl.Instance.customForm.DockPanel = null;
            MyCtrl.Instance.headForm.DockPanel = null;
            MyCtrl.Instance.orderForm.DockPanel = null;
            MyCtrl.Instance.productForm.DockPanel = null;
            MyCtrl.Instance.proxyForm.DockPanel = null;
            MyCtrl.Instance.propertyForm.DockPanel = null;
            MyCtrl.Instance.settingForm.DockPanel = null;
            MyCtrl.Instance.userAgentForm.DockPanel = null;

            // Close all other document windows
            CloseAllDocuments();

            // IMPORTANT: dispose all float windows.
            foreach (var window in dockPanel1.FloatWindows.ToList())
                if (window != null)
                {
                    window.Dispose(); 
                }
            foreach (var p in dockPanel1.Panes.ToList())
            {
                if (p != null && p.DockPanel != null)
                {
                    p.CloseActiveContent();
                }
            }
            foreach (var p in dockPanel1.DockWindows.ToList())
            {
                if (p != null && p.DockPanel != null)
                {
                    p.Dispose();
                }
            }
            System.Diagnostics.Debug.Assert(dockPanel1.Panes.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel1.Contents.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel1.FloatWindows.Count == 0);
        }
        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            vsToolStripExtender1.SetStyle(mainMenu, version, theme);
            //vsToolStripExtender1.SetStyle(toolBar, version, theme);
            vsToolStripExtender1.SetStyle(statusBar, version, theme);
        }
        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItemSolutionExplorer_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.solutionExplorer.Show(this.dockPanel1);
        }

        private void menuItemToolbox_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.accountForm.Show(this.dockPanel1);
        }

        private void menuItemTaskList_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.customForm.Show(this.dockPanel1);
        }

        private void 浏览器管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.userAgentForm.Show(this.dockPanel1);
        }

        private void 头标识管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.headForm.Show(this.dockPanel1);
        }

        private void 网络代理管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.proxyForm.Show(this.dockPanel1);
        }

        private void 线程池管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           MyCtrl.Instance.poolForm.Show(this.dockPanel1);
        }

        private void 订单管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.orderForm.Show(this.dockPanel1);
        }

        private void 价量态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.productForm.Show(this.dockPanel1);
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.settingForm.Show(this.dockPanel1);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            MyCtrl.Instance.priceForm.Show(this.dockPanel1);
        }

       
    }
}
