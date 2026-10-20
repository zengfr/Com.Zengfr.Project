
namespace Com.Zengfr.CodeGenerator.UI.View
{
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Com.Zengfr.CodeGenerator.UI.Common;
    public class TemplateTreeViewExplorer : TreeViewExplorer
    {
        public static string[] Extensions { get; set; }
        public TemplateTreeViewExplorer()
        {
            Extensions = new string[] { ".tt" };//{".cs|",".cst|",".tt" };
        }
        public override void Bind(object data)
        {
            Init(data as string);
        }
        public void Init(string path)
        {
            var root = BuildNode("Templates", path);
            AddRootNode(root);
            System.Action action = delegate
          {
              AddChildsToNode(root);
          };
            this.BeginInvoke(action);
        }
        protected override void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            base.TreeView_NodeMouseDoubleClick(sender, e);
            if (e.Node.Tag != null)
            {
                EditFile(e.Node.Tag as string);
            }
        }
        protected override void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            base.TreeView_AfterExpand(sender, e);
        }
        protected override void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                base.TreeView_BeforeExpand(sender, e);
            }
            catch (System.Exception ex)
            {
                VarUtils.ShowError(ex);
            }
        }
        protected override void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            base.TreeView_AfterCheck(sender, e);
            CheckAllChildNodes(e.Node, e.Node.Checked);
        }
        private void CheckAllChildNodes(TreeNode root, bool nodeChecked)
        {
            SyncTemplate(root.Checked, root.Tag as string);
            foreach (TreeNode node in root.Nodes)
            {
                SyncTemplate(node.Checked, node.Tag as string);
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void EditFile(string file)
        {
            if (!File.Exists(file))
                return;
            if (VarUtils.DTEObject != null)
            {
                VarUtils.DTEObject.ItemOperations.OpenFile(file);
            }
        }
        void SyncTemplate(bool nodeChecked, string path)
        {

            if (!string.IsNullOrEmpty(path)
                && Extensions != null && string.Join("|", Extensions).IndexOf(Path.GetExtension(path)) != -1)
            {
                var name = Path.GetFileName(path);
                if (nodeChecked)
                {
                    if (File.Exists(path) && !VarUtils.Config.Templates.Contains(path))
                    {
                        VarUtils.Config.Templates.Add(path);
                        VarUtils.ShowInfo(string.Format("Template->{0,-6}:{1}", "Add", name));
                    }
                }
                else
                {
                    if (VarUtils.Config.Templates.Contains(path))
                    {
                        VarUtils.Config.Templates.Remove(path);
                        VarUtils.ShowInfo(string.Format("Template->{0,-6}:{1}", "Remove", name));
                    }
                }
            }

        }
        protected override void AddChildsToNode(TreeNode pNode)
        {
            string path = pNode.Tag as string;
            if (!string.IsNullOrEmpty(path))
            {
                var directory = new DirectoryInfo(path);
                if (directory != null && directory.Exists && (directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                   && (directory.Attributes & FileAttributes.System) != FileAttributes.System)
                {
                    int count = 0;
                    foreach (var extension in Extensions)
                    {
                        var files = directory.GetFiles("*" + extension, SearchOption.TopDirectoryOnly);
                        foreach (var file in files)
                        {
                            AddFileNode(pNode, file);
                        }
                        count += files.Length;
                    }

                    var dirs = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (var dir in dirs)
                    {
                        if (GetFilesCount(dir, Extensions) != 0)
                        {
                            AddFolderNode(pNode, dir);
                        }
                    }
                    if (count == 0)
                    {
                        pNode.Expand();
                    }
                }
            }

        }
        string GetSearchPattern(string[] extensions)
        {
            var rtn = string.Join("|", Extensions).Replace(".", "*.");
            return rtn;
        }
        int GetFilesCount(DirectoryInfo di, string[] extensions)
        {
            FileInfo[] fi = extensions.SelectMany(i => di.GetFiles("*" + i, SearchOption.AllDirectories))
     .Distinct().ToArray();
            return fi.Length;
        }
        void AddFolderNode(TreeNode pNode, DirectoryInfo dir)
        {
            var node = BuildNode(dir.Name, dir.FullName);
            AddLoadingNode(node);
            AddChildNode(pNode, node);
        }

        void AddFileNode(TreeNode pNode, FileInfo file)
        {
            var node = BuildNode(file.Name, file.FullName);
            AddChildNode(pNode, node);
        }

    }
}
