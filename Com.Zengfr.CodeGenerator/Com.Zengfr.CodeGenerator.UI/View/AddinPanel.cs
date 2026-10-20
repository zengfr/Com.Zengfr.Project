using System;
using System.Windows.Forms;
using Com.Zengfr.CodeGenerator.Common;
using Com.Zengfr.CodeGenerator.Config;
using Com.Zengfr.CodeGenerator.Config.Impl;
using Com.Zengfr.CodeGenerator.Schema;
using Com.Zengfr.CodeGenerator.UI.Common;
using WeifenLuo.WinFormsUI.Docking;
using Com.Zengfr.CodeGenerator.Builder;
using Com.Zengfr.CodeGenerator.Template;
namespace Com.Zengfr.CodeGenerator.UI.View
{
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class AddinPanel : DockContent
    {
        private DockPanel dockPanel;
        private DBTreeViewExplorer dbTreeViewExplorer;
        private TemplateTreeViewExplorer templateTreeViewExplorer;
        private PropertyGridViewDockContent propertyGridViewDockContent;
        private OutputDockContent outputDockContent;
        public AddinPanel()
        {
            Com.Zengfr.CodeGenerator.Common.CommonLogger.Logging += new Action<string>(CommonLogger_Logging);
            InitializeComponent();
            InitializeControls();
            Config_Load();
        }

        void CommonLogger_Logging(string obj)
        {
            VarUtils.ShowInfo(obj);
        }
        private void InitializeControls()
        {
            this.SuspendLayout();
            this.AllowDrop = true; this.AutoHidePortion = 0.1;
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();

            this.dockPanel.SuspendLayout();
            this.dockPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            this.dockPanel.Location = new System.Drawing.Point(44, 44);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.AutoSize = true;
            this.dockPanel.AllowDrop = true;

            this.toolStripContainer1.ContentPanel.Controls.Add(this.dockPanel);
            this.IsMdiContainer = true;
            dbTreeViewExplorer = new DBTreeViewExplorer();
            templateTreeViewExplorer = new TemplateTreeViewExplorer();
            propertyGridViewDockContent = new PropertyGridViewDockContent();
            outputDockContent = new OutputDockContent();

            dbTreeViewExplorer.HideOnClose = true;
            templateTreeViewExplorer.HideOnClose = true;
            outputDockContent.HideOnClose = true;
            propertyGridViewDockContent.HideOnClose = true;

            dbTreeViewExplorer.Show(dockPanel, DockState.DockLeft);
            templateTreeViewExplorer.Show(dockPanel, DockState.DockLeft);

            propertyGridViewDockContent.Show(dockPanel, DockState.DockLeft);
            outputDockContent.Show(dockPanel, DockState.Document);

            propertyGridViewDockContent.TabText = "配置";
            templateTreeViewExplorer.TabText = "模板";
            dbTreeViewExplorer.TabText = "数据";
            outputDockContent.TabText = "输出";

            propertyGridViewDockContent.ShowIcon = false;
            templateTreeViewExplorer.ShowIcon = false;
            dbTreeViewExplorer.ShowIcon = false;
            outputDockContent.ShowIcon = false;

            propertyGridViewDockContent.Icon = null;
            templateTreeViewExplorer.Icon = null;
            dbTreeViewExplorer.Icon = null;
            outputDockContent.Icon = null;



            outputDockContent.CloseButtonVisible = false;

            this.dockPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        private void Config_Load()
        {
            VarUtils.DockPanel = this.dockPanel;
            var config = VarUtils.Config;

            config.AddinDir = IOUtils.GetAddinDirectory();
            config.TemplatesDir = IOUtils.GetTemplatesDirectory();
            config.Templates = IOUtils.GetTemplatesFullName(VarUtils.Config.TemplatesDir);
            propertyGridViewDockContent.Bind(config);
            propertyGridViewDockContent.GetPropertyGrid().PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
            OutputDirChanged();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            VarUtils.ShowInfo(string.Format("PropertyValue->Changed:{0}", e.ChangedItem.PropertyDescriptor.Name));
            switch (e.ChangedItem.PropertyDescriptor.Name)
            {
                case "TemplatesDir":
                    VarUtils.Config.Templates = IOUtils.GetTemplatesFullName(VarUtils.Config.TemplatesDir);

                    break;
                case "OutputRootDir":
                    OutputDirChanged();
                    break;
            }
        }
        private void ChangeConfigType()
        {
            try
            {
                var type = VarUtils.ChangeConfigType();

                this.btnViewSchema.Enabled = type == 1 ? true : false;
                this.dbTreeViewExplorer.Visible = this.btnViewSchema.Enabled;
                this.propertyGridViewDockContent.Activate();
                Config_Load();
            }
            catch (Exception ex)
            {
                VarUtils.ShowError(ex);
            }
        }
        private void OutputDirChanged()
        {
            var config = VarUtils.Config;
            bool rtn = false;
            if (config != null)
            {
                if (string.IsNullOrEmpty(VarUtils.SolutionDirectory) && string.IsNullOrEmpty(config.OutputRootDir))
                {
                    config.OutputRootDir = VarUtils.SolutionDirectory;
                }
                rtn = !string.IsNullOrEmpty(config.OutputRootDir);
            }
            this.btnBuildProj.Enabled = rtn;
            this.btnBuildCode.Enabled = rtn;

        }
        #region
        private void btnViewSchema_Click(object sender, EventArgs e)
        {
            //查看DB
            this.btnViewSchema.Enabled = false;
            Action action = delegate
              { ViewSchema(); };
            AsyncInvoker.BeginInvoke(this, action, null);

            this.btnViewSchema.Enabled = true;
        }

        private void btnBuildProj_Click(object sender, EventArgs e)
        {
            //生成项目
            this.btnBuildProj.Enabled = false;
            Action action = delegate
            {
                CreateProj(VarUtils.Config);
            };
            AsyncInvoker.BeginInvoke(this, action, null);
            this.btnBuildProj.Enabled = true;
        }
        private void btnBuildCode_Click(object sender, EventArgs e)
        {
            //生成代码
            this.btnBuildCode.Enabled = false;
            Action action = delegate
             {
                 CreateCode(VarUtils.Config);
             };
            AsyncInvoker.BeginInvoke(this, action, null);

            this.btnBuildCode.Enabled = true;
        }
        private void btnViewTemplate_Click(object sender, EventArgs e)
        {
            //查看模板
            //查看DB
            this.btnViewTemplate.Enabled = false;
            Action action = delegate { ViewTemplate(); };
            AsyncInvoker.BeginInvoke(this, action, null);

            this.btnViewTemplate.Enabled = true;
        }
        private void btnChangeMode_Click(object sender, EventArgs e)
        {
            ChangeConfigType();
        }
        #endregion

        private void ViewTemplate()
        {
            try
            {
                var tmplatesDirectory = VarUtils.Config.TemplatesDir;
                var templatePanel = this.templateTreeViewExplorer;
                if (templatePanel != null)
                {
                    templatePanel.Activate();
                    ((TemplateTreeViewExplorer)templatePanel).Init(tmplatesDirectory);

                }
            }
            catch (Exception ex)
            {
                VarUtils.ShowError(ex);
            }
        }
        private void ViewSchema()
        {
            try
            {
                var cfg = VarUtils.Config as DataAbstractConfig;
                if (cfg != null)
                {
                    var dbPanel = this.dbTreeViewExplorer;
                    if (dbPanel != null)
                    {
                        dbPanel.Activate();
                        ((DBTreeViewExplorer)dbPanel).Init(cfg.DatabaseSchema);
                    }
                }
            }
            catch (Exception ex)
            {
                VarUtils.ShowError(ex);
            }
        }
        private static void CreateProj(AbstractConfig config)
        {
            try
            {
                VarUtils.ShowInfo("Start");

                AssemblySchema assemblySchema;
                if (config is DataAbstractConfig)
                {
                    var cfg = config as DataAbstractConfig;
                    assemblySchema = SchemaBuilder.BuildAssemblySchema(cfg.DatabaseSchema, cfg.TablePrefix,
                        cfg.NamespacePrefix, cfg.Tables, cfg.Views);
                }
                else
                {
                    var cfg = config as AssemblyConfig;
                    assemblySchema = SchemaBuilder.BuildAssemblySchema(cfg.Assembly, true);
                }
                Builders.BuildByTemplates(
                     config.OutputRootDir,
                     config.NamespacePrefix,
                     assemblySchema, config.GenProjFile, config.GenImplementEmptyFile,config.OutputZIP);
                VarUtils.ShowInfo("End");

            }
            catch (Exception ex)
            {
                VarUtils.ShowError(ex);
            }
        }
        private static void CreateCode(AbstractConfig config)
        {
            try
            {
                VarUtils.ShowInfo("Start");

                AssemblySchema assemblySchema;
                if (config is DataAbstractConfig)
                {
                    var cfg = config as DataAbstractConfig;
                    assemblySchema = SchemaBuilder.BuildAssemblySchema(cfg.DatabaseSchema, cfg.TablePrefix,
                        cfg.NamespacePrefix, cfg.Tables, cfg.Views);
                }
                else
                {
                    var cfg = config as AssemblyConfig;
                    assemblySchema = SchemaBuilder.BuildAssemblySchema(cfg.Assembly, true);
                }
                Builders.BuildByTemplates(
                     config.OutputRootDir,
                     config.NamespacePrefix,
                     assemblySchema, false, config.GenImplementEmptyFile, config.OutputZIP);
                VarUtils.ShowInfo("End");

            }
            catch (Exception ex)
            {
                VarUtils.ShowError(ex);
            }
        }
    }
}