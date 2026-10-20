using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace UniTask_UI
{
    /// <summary>  
    /// Usage:  
    ///     log4net.Config.BasicConfigurator.Configure();  
    ///     var logPattern = "%date [%thread] %-5level %logger !%M - %message%newline";  
    ///     var logAppender = new TextBoxBaseAppender()  
    ///     {  
    ///         TextBox = this.textBox2,  
    ///         Layout = new PatternLayout(logPattern)  
    ///     };  
    ///       
    ///     ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetLoggerRepository()).Root.AddAppender(logAppender);  
    /// </summary>  
    public class TextBoxBaseAppender : AppenderSkeleton
    {
        public TextBoxBase TextBox { get; set; }

        public TextBoxBaseAppender()
        {
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.TextBox == null)
            {
                return;
            }

            if (!this.TextBox.IsHandleCreated)
            {
                return;
            }

            if (this.TextBox.IsDisposed)
            {
                return;
            }

            var patternLayout = this.Layout as PatternLayout;

            var str = string.Empty;
            if (patternLayout != null)
            {
                str = patternLayout.Format(loggingEvent);

                if (loggingEvent.ExceptionObject != null)
                {
                    str += loggingEvent.ExceptionObject.ToString() + Environment.NewLine;
                }
            }
            else
            {
                str = loggingEvent.LoggerName + "-" + loggingEvent.RenderedMessage + Environment.NewLine;
            }

            if (!this.TextBox.InvokeRequired)
            {
                printf(str);
            }
            else
            {
                this.TextBox.BeginInvoke((MethodInvoker)delegate
                {
                    if (!this.TextBox.IsHandleCreated)
                    {
                        return;
                    }

                    if (this.TextBox.IsDisposed)
                    {
                        return;
                    }
                    printf(str);
                });
            }
        }

        private void printf(string str)
        {
            //若是超过10行 则Clear 
            if (TextBox.Lines.Length > 800)
            {
                //int l = TextBox.Text.Length;
                //if (l > 5)
                //    TextBox.Text = TextBox.Text.Substring(l * 4 / 5);
                TextBox.Clear();
            }
            this.TextBox.AppendText(str);
        }
    }
}
