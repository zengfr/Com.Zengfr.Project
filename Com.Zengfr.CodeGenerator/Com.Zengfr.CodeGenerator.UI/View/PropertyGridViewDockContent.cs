using Com.Zengfr.CodeGenerator.UI.Common;
using WeifenLuo.WinFormsUI.Docking;
namespace Com.Zengfr.CodeGenerator.UI.View
{
    public class PropertyGridViewDockContent : DockContent, IMyDockContent
    {

        private System.Windows.Forms.PropertyGrid propertyGrid1;

        private void InitializeComponent()
        {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(292, 273);
            this.propertyGrid1.TabIndex = 0;
            // 
            // PropertyGridViewDockContent
            this.Controls.Add(this.propertyGrid1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PropertyGridViewDockContent";
            this.MinimumSize = new System.Drawing.Size(100, 200);
            this.ResumeLayout(false);

        }
        public virtual System.Windows.Forms.PropertyGrid GetPropertyGrid()
        {
            return propertyGrid1;
        }
        public PropertyGridViewDockContent()
        {
            InitializeComponent();
        }
        public void Bind(object objectTarget)
        {
            if (objectTarget != null && this.propertyGrid1 != null)
            {
                this.propertyGrid1.SelectedObject = objectTarget;
            }

        }
    }
}
