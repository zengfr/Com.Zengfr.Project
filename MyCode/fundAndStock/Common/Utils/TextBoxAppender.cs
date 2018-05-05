using System;
using System.Windows.Forms;
using log4net.Appender;


namespace log4net.Appender.Impl.Ex
{
    public class TextBoxAppender : AppenderSkeleton
    {
        private TextBox AppenderTextBox { get; set; }
        public string FormName { get; set; }
        public string TextBoxName { get; set; }

        private Control FindControlRecursive(Control root, string textBoxName)
        {
            if (root.Name == textBoxName) return root;
            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, textBoxName);
                if (t != null) return t;
            }
            return null;
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (AppenderTextBox == null)
            {
                if (String.IsNullOrEmpty(FormName) ||
                    String.IsNullOrEmpty(TextBoxName))
                    return;

                Form form = Application.OpenForms[FormName];
                if (form == null)
                    return;

                AppenderTextBox = (TextBox)FindControlRecursive(form, TextBoxName);
                if (AppenderTextBox == null)
                    return;

                form.FormClosing += (s, e) => AppenderTextBox = null;
            }
            AppenderTextBox.BeginInvoke((MethodInvoker)delegate
            {
                AppenderTextBox.AppendText(RenderLoggingEvent(loggingEvent));
            });
        }
    }
}
