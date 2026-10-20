namespace Com.Zengfr.CodeGenerator.UI.View
{
    using System.Windows.Forms;

    public partial class AddinPanel
    {
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
        private void InitializeComponent()
        {
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnChangeMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnViewSchema = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnViewTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBuildProj = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBuildCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.ContentPanel.Visible = true;
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(400, 111);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(400, 28);
            this.toolStripContainer1.TabIndex = 19;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnChangeMode,
            this.toolStripSeparator5,
            this.btnViewSchema,
            this.toolStripSeparator1,
            this.btnViewTemplate,
            this.toolStripSeparator2,
            this.btnBuildProj,
            this.toolStripSeparator3,
            this.btnBuildCode,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(366, 3);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnChangeMode
            // 
            this.btnChangeMode.Image = global::Com.Zengfr.CodeGenerator.UI.Properties.Resources.cubes;
            this.btnChangeMode.Name = "btnChangeMode";
            this.btnChangeMode.Size = new System.Drawing.Size(49, 0);
            this.btnChangeMode.Text = "模式";
            this.btnChangeMode.ToolTipText = "切换模式";
            this.btnChangeMode.Click += new System.EventHandler(this.btnChangeMode_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 3);
            // 
            // btnViewSchema
            // 
            this.btnViewSchema.Image = global::Com.Zengfr.CodeGenerator.UI.Properties.Resources.db;
            this.btnViewSchema.Name = "btnViewSchema";
            this.btnViewSchema.Size = new System.Drawing.Size(49, 0);
            this.btnViewSchema.Text = "数据";
            this.btnViewSchema.Click += new System.EventHandler(this.btnViewSchema_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 3);
            // 
            // btnViewTemplate
            // 
            this.btnViewTemplate.Image = global::Com.Zengfr.CodeGenerator.UI.Properties.Resources.folder;
            this.btnViewTemplate.Name = "btnViewTemplate";
            this.btnViewTemplate.Size = new System.Drawing.Size(49, 0);
            this.btnViewTemplate.Text = "模板";
            this.btnViewTemplate.Click += new System.EventHandler(this.btnViewTemplate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 3);
            // 
            // btnBuildProj
            // 
            this.btnBuildProj.Image = global::Com.Zengfr.CodeGenerator.UI.Properties.Resources.add;
            this.btnBuildProj.Name = "btnBuildProj";
            this.btnBuildProj.Size = new System.Drawing.Size(49, 0);
            this.btnBuildProj.Text = "项目";
            this.btnBuildProj.ToolTipText = "生成项目";
            this.btnBuildProj.Click += new System.EventHandler(this.btnBuildProj_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 3);
            // 
            // btnBuildCode
            // 
            this.btnBuildCode.Image = global::Com.Zengfr.CodeGenerator.UI.Properties.Resources.run;
            this.btnBuildCode.Name = "btnBuildCode";
            this.btnBuildCode.Size = new System.Drawing.Size(49, 0);
            this.btnBuildCode.Text = "代码";
            this.btnBuildCode.ToolTipText = "生成代码";
            this.btnBuildCode.Click += new System.EventHandler(this.btnBuildCode_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 3);
            // 
            // AddinPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 461);
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "AddinPanel";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        private ToolStripButton btnViewSchema;
        private ToolStripButton btnBuildProj;
        private ToolStripButton btnBuildCode;
        private ToolStripButton btnViewTemplate;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton btnChangeMode;
        private ToolStripSeparator toolStripSeparator5;
    }
}