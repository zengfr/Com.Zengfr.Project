using System;
using System.Drawing;
using System.Windows.Forms;
using Common.Logging;

namespace ConsoleControlEx
{
    public partial class ConsoleTabPage : TabPage, IDisposable
    {
        protected static ILog log = LogManager.GetLogger(typeof(ConsoleTabPage));
        private ConsoleControl.ConsoleControl consoleControl;
        public static int TextBoxLinesLength = 1000;
        public ConsoleTabPage()
        {
            //InitializeComponent();

            InitializeComponent2();

        }

        private void InitializeComponent2()
        {
            InitConsoleControl();

            this.SuspendLayout();
            this.Controls.Add(this.consoleControl);
            this.Name = "ConsoleTabPage";

            this.ResumeLayout(false);

            this.consoleControl.OnConsoleOutput += new ConsoleControl.ConsoleEventHandler(consoleControl_OnConsoleOutput);

        }

        private void InitConsoleControl()
        {
            this.consoleControl = new ConsoleControl.ConsoleControl();
            this.consoleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleControl.IsInputEnabled = true;
            this.consoleControl.Location = new System.Drawing.Point(0, 0);
            this.consoleControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.consoleControl.Name = "ConsoleControl";
            this.consoleControl.SendKeyboardCommandsToProcess = false;
            this.consoleControl.ShowDiagnostics = false;
            this.consoleControl.Size = new System.Drawing.Size(784, 514);
            this.consoleControl.TabIndex = 0;
            this.consoleControl.ForeColor = Color.Green;

        }


        void consoleControl_OnConsoleOutput(object sender, ConsoleControl.ConsoleEventArgs args)
        {
            Invoke((Action)(() =>
            {
                // log.Info("ClearTextAndScrollToCaret");
                ClearTextAndScrollToCaret();
            }));
        }

        private void ClearTextAndScrollToCaret()
        {
            if (this.consoleControl.InternalRichTextBox.Lines.Length > TextBoxLinesLength)
            {
                this.consoleControl.InternalRichTextBox.Clear();
            }
            //else
            //{
            //    this.consoleControl.InternalRichTextBox.ScrollToCaret();
            //}
        }
        public ConsoleControl.ConsoleControl GetConsoleControl()
        {
            return this.consoleControl;
        }


        void IDisposable.Dispose()
        {
            if (this.consoleControl != null && !this.consoleControl.IsDisposed)
            {
                this.consoleControl.StopProcess();
                this.consoleControl = null;
            }
        }
    }
}
