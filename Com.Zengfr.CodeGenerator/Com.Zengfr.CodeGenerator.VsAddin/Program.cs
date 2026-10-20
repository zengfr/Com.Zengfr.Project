using System;
using System.Windows.Forms;

namespace Com.Zengfr.CodeGenerator.VsAddin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var f = new AddinForm();
                f.WindowState = FormWindowState.Maximized;
                Application.Run(f);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
