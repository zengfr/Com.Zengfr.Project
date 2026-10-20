using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class ToolWindow : DockContent
    {
        public ToolWindow()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            HideOnClose = true;
        }

        private void ToolWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}