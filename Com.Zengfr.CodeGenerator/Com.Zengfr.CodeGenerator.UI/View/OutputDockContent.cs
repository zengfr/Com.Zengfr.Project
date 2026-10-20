using WeifenLuo.WinFormsUI.Docking;
namespace Com.Zengfr.CodeGenerator.UI.View
{
    using Com.Zengfr.CodeGenerator.UI.Common;
    public class OutputDockContent : DockContent, IMyDockContent
    {

        private System.Windows.Forms.TextBox textBox1;

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;

            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(292, 273);
            this.textBox1.TabIndex = 0;
            this.textBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(textBox1_MouseDoubleClick);
            // 
            // OutputDockContent
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "OutputDockContent";
            this.Text = "Output";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void textBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.textBox1 != null)
            {
                this.textBox1.Text = string.Empty;
            }
        }

        public OutputDockContent()
        {
            InitializeComponent();
        }


        #region IMyDockContent Members

        public void Bind(object taget)
        {
            this.textBox1.AppendText(taget as string);
            if (this.textBox1.Text.Length > 999999)
            {
                this.textBox1.Clear();
            }
        }

        #endregion
    }
}
