using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using log4net;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using UniTask_UI.common;
using WeifenLuo.WinFormsUI.Docking;

namespace UniTask_UI
{
    public partial class MyLogForm : ToolWindow
    {
        public Level LevelToMatch = Level.Info;
        public String StringToMatch = null;
        private bool isinit = false;
        public MyLogForm()
        {
            InitializeComponent();
        }
        static MyLogForm()
        {
            log4net.Config.BasicConfigurator.Configure();
        }
        public void init2()
        {
            if (isinit)
                return;
            this.textBox1.MaxLength = int.MaxValue - 1024;
            this.textBox1.Multiline = true;
      
            var logPattern = "%d{dd HH:mm:ss.ffff} %-5p [%04t] - %m%newline";
            var logAppender = new TextBoxBaseAppender()  
            {
                TextBox = this.textBox1,
                Threshold = LevelToMatch,
                Layout = new PatternLayout(logPattern)
            };
            if (LevelToMatch != null)
            {
                LevelRangeFilter filter = new log4net.Filter.LevelRangeFilter();
                filter.LevelMin = LevelToMatch;
                filter.LevelMax = Level.Error;
                filter.AcceptOnMatch = true;
                filter.ActivateOptions();
                logAppender.AddFilter(filter);
                this.TabText = String.Format("{0}日志", LevelToMatch);
            }

            if (StringToMatch != null&&StringToMatch.Length>0)
            {
                StringMatchFilter filter = new log4net.Filter.StringMatchFilter();
                filter.StringToMatch = StringToMatch;

                filter.AcceptOnMatch = true;
                filter.ActivateOptions();
                logAppender.AddFilter(filter);
                this.TabText = String.Format("{0}日志", StringToMatch);
            }
            logAppender.AddFilter(new log4net.Filter.DenyAllFilter());
            logAppender.ActivateOptions();
            log4net.Repository.Hierarchy.Hierarchy hierarchy = ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository());
            hierarchy.Root.AddAppender(logAppender);
          
            
            textBox1.DoubleClick += TextBox1_DoubleClick;

            isinit = true;
        }

        private void TextBox1_DoubleClick(object sender, EventArgs e)
        {
            int l = textBox1.Text.Length;
            if(l>5)
                textBox1.Text = textBox1.Text.Substring(l*4/5);
            else
                textBox1.Clear();
        }

        public void binddata()
        {
            if (LevelToMatch != null)
            { 
                this.TabText = String.Format("{0}日志", LevelToMatch);
            }

            if (StringToMatch != null && StringToMatch.Length > 0)
            { 
                this.TabText = String.Format("{0}日志", StringToMatch);
            }
        }
        protected override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return GetType().ToString() + "," + LevelToMatch + "," + StringToMatch;
        }
        private void MyLogForm_Load(object sender, EventArgs e)
        {
            init2();
            LoggerUtil.Info("init:"+ this.TabText);
        }
    }
    
}
