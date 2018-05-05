using System.Windows.Forms;

namespace ConsoleControlEx
{
    public class ConsoleTabControl : TabControl
    {
        public ConsoleTabControl()
        {
            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            AddConsoleTabPage("default", null);
        }

        public void AddConsoleTabPage(string name, object tag)
        {
            this.SuspendLayout();

            var c = BuildConsoleTabPage(name, tag);
            this.Controls.Add(c);

            this.ResumeLayout(false);
        }

        public ConsoleTabPage GetConsoleTabPage(string name)
        {
            foreach (TabPage tabPage in this.TabPages)
            {
                if (tabPage.Name == name)
                {
                    return tabPage as ConsoleTabPage;
                }
            }
            return null;
        }

        public ConsoleTabPage GetSelectedConsoleTabPage()
        {
            return this.SelectedTab as ConsoleTabPage;
        }

        public ConsoleTabPage BuildConsoleTabPage(string name, object tag)
        {
            var c = new ConsoleTabPage();
            c.Name = name;
            c.Text = name;
            c.Tag = tag;
            return c;
        }
    }
}
