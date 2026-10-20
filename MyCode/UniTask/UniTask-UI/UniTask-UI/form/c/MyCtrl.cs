using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using log4net.Core;
using UniTask_UI.app1;
using UniTask_UI.Task;
using WeifenLuo.WinFormsUI.Docking;

namespace UniTask_UI.p1
{
    public class MyCtrl
    {
        public static MyCtrl Instance = new MyCtrl();
        public WebBrowser webBrowser1;
        public WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        public MySolutionExplorer solutionExplorer;
        public MyPropertiesForm propertyForm;
        public MyPoolForm poolForm;

        public MyAccountForm accountForm;
        public MyCustomForm customForm;
        public MyPriceForm priceForm;
        public MyProductForm productForm;
        public MySettingForm settingForm;
        public MyOrderForm orderForm;
        public MyProxyForm proxyForm;
        public MyHeadForm headForm;
        public MyUserAgentForm userAgentForm;
        public TaskSettingForm taskSettingForm;
        public TaskGridForm taskGridForm;
        string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "dpanel.config");
        public void SaveAsXml()
        {
            dockPanel1.SaveAsXml(configFile);
        }
        public void load()
        {
            if (File.Exists(configFile))
            {
                DeserializeDockContent m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
            }
            initForm();
        }
        private void initForm()
        {
            if (poolForm == null)
            {
                poolForm = new MyPoolForm();
                poolForm.Dock = DockStyle.Top;
                poolForm.Anchor = AnchorStyles.Top;
                poolForm.Show(this.dockPanel1, DockState.Document);
            }

            if (headForm == null)
            {

                headForm = new MyHeadForm();
                headForm.Dock = DockStyle.Top;
                headForm.Anchor = AnchorStyles.Top;
                headForm.Show(this.dockPanel1, DockState.Document);
            }
            if (proxyForm == null)
            {

                proxyForm = new MyProxyForm();
                proxyForm.Dock = DockStyle.Top;
                proxyForm.Anchor = AnchorStyles.Top;
                proxyForm.Show(this.dockPanel1, DockState.Document);
            }
            if (userAgentForm == null)
            {

                userAgentForm = new MyUserAgentForm();
                userAgentForm.Dock = DockStyle.Top;
                userAgentForm.Anchor = AnchorStyles.Top;
                userAgentForm.Show(this.dockPanel1, DockState.Document);
            }
            if (orderForm == null)
            {

                orderForm = new MyOrderForm();
                orderForm.Dock = DockStyle.Top;
                orderForm.Anchor = AnchorStyles.Top;
                orderForm.Show(this.dockPanel1, DockState.Document);
            }
            if (priceForm == null)
            {

                priceForm = new MyPriceForm();
                priceForm.Dock = DockStyle.Top;
                priceForm.Anchor = AnchorStyles.Top;
                priceForm.Show(this.dockPanel1, DockState.Document);
            }
            if (productForm == null)
            {

                productForm = new MyProductForm();
                productForm.Dock = DockStyle.Top;
                productForm.Anchor = AnchorStyles.Top;
                productForm.Show(this.dockPanel1, DockState.Document);
            }
            if (accountForm == null)
            {

                accountForm = new MyAccountForm();
                accountForm.Dock = DockStyle.Fill;
                accountForm.Anchor = AnchorStyles.Bottom;
                accountForm.Show(this.dockPanel1, DockState.Document);
            }
            if (customForm == null)
            {

                customForm = new MyCustomForm();
                customForm.Dock = DockStyle.Fill;
                customForm.Anchor = AnchorStyles.Bottom;
                customForm.Show(this.dockPanel1, DockState.Document);
            }
            if (taskGridForm == null)
            {

                taskGridForm = new TaskGridForm();
                taskGridForm.Dock = DockStyle.Fill;
                taskGridForm.Anchor = AnchorStyles.Bottom;
                taskGridForm.Show(this.dockPanel1, DockState.Document);
            }
            if (solutionExplorer == null)
            {

                solutionExplorer = new MySolutionExplorer();
                solutionExplorer.Dock = DockStyle.Top | DockStyle.Left;
                solutionExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                solutionExplorer.Show(this.dockPanel1, DockState.DockLeft);
            }
            if (settingForm == null)
            {

                settingForm = new MySettingForm();
                settingForm.Dock = DockStyle.Top | DockStyle.Left;
                settingForm.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                settingForm.Show(this.dockPanel1, DockState.DockLeft);
            }
            if (taskSettingForm == null)
            {

                taskSettingForm = new TaskSettingForm();
                taskSettingForm.Dock = DockStyle.Top | DockStyle.Left;
                taskSettingForm.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                taskSettingForm.Show(this.dockPanel1, DockState.DockLeft);
            }
            MyLogForm logForm = FindDocument("INFO日志") as MyLogForm;
            if (logForm == null)
            {
                logForm = new MyLogForm();
                //logForm.Dock = DockStyle.Bottom;
                //logForm.Anchor = AnchorStyles.Bottom;
                //logForm.Show(this.dockPanel1, DockAlignment.Bottom, 0.35);
                logForm.Show(this.dockPanel1, DockState.DockBottom);
            }

            MyLogForm logForm2 = FindDocument("WARN日志") as MyLogForm;
            if (logForm2 == null)
            {
                logForm2 = new MyLogForm();
                logForm2.LevelToMatch = Level.Warn;
                logForm2.Show(this.dockPanel1, DockState.DockBottom);
            }

            MyLogForm logForm3 = FindDocument("刷价日志") as MyLogForm;
            if (logForm3 == null)
            {
                logForm3 = new MyLogForm();
                logForm3.LevelToMatch = null;
                logForm3.StringToMatch = "刷价";
                logForm3.Show(this.dockPanel1, DockState.DockBottom);
            }

            MyLogForm logForm4 = FindDocument("刷量日志") as MyLogForm;
            if (logForm4 == null)
            {
                logForm4 = new MyLogForm();
                logForm4.LevelToMatch = null;
                logForm4.StringToMatch = "刷量";
                logForm4.Show(this.dockPanel1, DockState.DockBottom);
            }

            MyLogForm logForm5 = FindDocument("订单日志") as MyLogForm;
            if (logForm5 == null)
            {
                logForm5 = new MyLogForm();
                logForm5.LevelToMatch = null;
                logForm5.StringToMatch = "订单";
                logForm5.Show(this.dockPanel1, DockState.DockBottom);
            }
        }
        private IDockContent GetContentFromPersistString(string persistString)
        {
            IDockContent dc;
            if (persistString == typeof(MyPropertiesForm).ToString())
            {
                this.propertyForm = new MyPropertiesForm();
                dc = this.propertyForm;
                return dc;
            }
            else if (persistString == typeof(MySolutionExplorer).ToString())
            {
                this.solutionExplorer = new MySolutionExplorer();
                dc = this.solutionExplorer;
                return dc;
            }

            else if (persistString == typeof(MySettingForm).ToString())
            {
                this.settingForm = new MySettingForm();
                dc = this.settingForm;
                return dc;
            }
            else if (persistString == typeof(MyAccountForm).ToString())
            {
                this.accountForm = new MyAccountForm();
                dc = this.accountForm;
                return dc;
            }
            else if (persistString == typeof(MyCustomForm).ToString())
            {
                this.customForm = new MyCustomForm();
                dc = this.customForm;
                return dc;
            }
            else if (persistString == typeof(MyPriceForm).ToString())
            {
                this.priceForm = new MyPriceForm();
                dc = this.priceForm;
                return dc;
            }
            else if (persistString == typeof(MyProductForm).ToString())
            {
                this.productForm = new MyProductForm();
                dc = this.productForm;
                return dc;
            }
            else if (persistString == typeof(MyOrderForm).ToString())
            {
                this.orderForm = new MyOrderForm();
                dc = this.orderForm;
                return dc;
            }
            else if (persistString == typeof(MyUserAgentForm).ToString())
            {
                this.userAgentForm = new MyUserAgentForm();
                dc = this.userAgentForm;
                return dc;
            }
            else if (persistString == typeof(MyProxyForm).ToString())
            {
                this.proxyForm = new MyProxyForm();
                dc = this.proxyForm;
                return dc;
            }
            else if (persistString == typeof(MyHeadForm).ToString())
            {
                this.headForm = new MyHeadForm();
                dc = this.headForm;
                return dc;
            }
            else if (persistString == typeof(MyPoolForm).ToString())
            {
                this.poolForm = new MyPoolForm();
                dc = this.poolForm;
                return dc;
            }
            else
            {
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                else if (parsedStrings[0] != typeof(MyLogForm).ToString())
                    return null;
                else
                {
                    MyLogForm lf = new MyLogForm();
                    lf.StringToMatch = null;
                    lf.LevelToMatch = null;
                    if (parsedStrings[1] != string.Empty && !parsedStrings[1].Equals("null"))
                        lf.LevelToMatch = log4net.LogManager.GetRepository().LevelMap[parsedStrings[1]];
                    if (parsedStrings[2] != string.Empty && !parsedStrings[1].Equals("null"))
                        lf.StringToMatch = parsedStrings[2];
                    lf.init2();
                    lf.binddata();
                    return lf;
                }

            }
        }
        private IDockContent FindDocument(string text)
        {
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                //foreach (Form form in MdiChildren)
                //if (form.Text == text)
                //return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel1.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;
                foreach (IDockContent content in dockPanel1.Contents)
                    if (content.DockHandler.TabText == text)
                        return content;
                return null;
            }
        }
    }
}
