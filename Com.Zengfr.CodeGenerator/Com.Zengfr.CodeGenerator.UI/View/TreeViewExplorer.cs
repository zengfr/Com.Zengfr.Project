using Com.Zengfr.CodeGenerator.UI.Common;
using WeifenLuo.WinFormsUI.Docking;
namespace Com.Zengfr.CodeGenerator.UI.View
{
    using System.Windows.Forms;
    public abstract class TreeViewExplorer : DockContent, IMyDockContent
    {

        protected System.Windows.Forms.TreeView TreeView;

        private void InitializeComponent()
        {
            this.TreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            this.TreeView.CheckBoxes = true;
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(150, 150);
            this.TreeView.TabIndex = 0;
            this.TreeView.AfterCheck += new TreeViewEventHandler(TreeView_AfterCheck);
            this.TreeView.BeforeExpand += new TreeViewCancelEventHandler(TreeView_BeforeExpand);
            this.TreeView.AfterExpand += new TreeViewEventHandler(TreeView_AfterExpand);
            this.TreeView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(TreeView_NodeMouseDoubleClick);
            this.TreeView.MinimumSize = new System.Drawing.Size(100, 200);
            // 
            // TreeViewExplorer
            // 
            this.Controls.Add(this.TreeView);
            this.Name = "TreeViewExplorer";
            this.MinimumSize = new System.Drawing.Size(100, 200);
            this.ResumeLayout(false);

        }

        protected virtual void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
        protected virtual void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
                return;
            if (e.Node == null || e.Node.Tag == null || string.IsNullOrEmpty(e.Node.Tag.ToString()))
                return;

            if (e.Node.Nodes.Count == 1 && e.Node.FirstNode.Text == "Loading...")
            {
                System.Action action = delegate
                {
                    e.Node.Nodes.Clear();
                    AddChildsToNode(e.Node);
                };
                this.TreeView.BeginInvoke(action);
            }

        }
        public abstract void Bind(object taget);
        protected abstract void AddChildsToNode(TreeNode node);
        protected virtual void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.FirstNode.Text == "Loading...")
            {
                e.Node.Nodes.Clear();
            }
        }

        public TreeViewExplorer()
        {
            InitializeComponent();
        }


        #region IMyDockContent Members
        protected void AddLoadingNode(TreeNode parentNode)
        {
            System.Action action = delegate
              {
                  var loading = BuildNode("Loading...", "");
                  AddChildNode(parentNode, loading);
              };
            this.TreeView.BeginInvoke(action);
        }
        protected TreeNode BuildNode(string text, object taget)
        {
            TreeNode node = new TreeNode();
            node.Tag = taget;
            node.Text = text;
            return node;
        }
        protected void AddRootNode(TreeNode node)
        {
            System.Action action = delegate
            {
                this.TreeView.Nodes.Clear();
                this.TreeView.Nodes.Add(node);
            };
            this.TreeView.BeginInvoke(action);

        }
        protected void AddChildNode(TreeNode node, TreeNode childNode)
        {
            System.Action action = delegate
            {
                node.Nodes.Add(childNode);
            };
            this.TreeView.BeginInvoke(action);

        }
        #endregion
        protected virtual void TreeView_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {

            if (e.Action != TreeViewAction.Unknown)
            {
                SetNodeCheckStatus(e.Node, e.Node.Checked);
                e.Node.ExpandAll();
            }
        }
        private void SetNodeCheckStatus(TreeNode tn, bool isChecked)
        {
            if (tn == null) return;
            foreach (TreeNode tnChild in tn.Nodes)
            {
                tnChild.Checked = isChecked;
                SetNodeCheckStatus(tnChild, isChecked);
            }
        }
    }
}
