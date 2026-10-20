
namespace Com.Zengfr.CodeGenerator.UI.View
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Com.Zengfr.CodeGenerator.Config;
    using Com.Zengfr.CodeGenerator.Config.Impl;
    using Com.Zengfr.CodeGenerator.UI.Common;
    using SchemaExplorer;
    public class DBTreeViewExplorer : TreeViewExplorer
    {
        public DBTreeViewExplorer()
        {
           
        }
        public override void Bind(object data)
        {
            Init(data as DatabaseSchema);
        }
        public void Init(DatabaseSchema databaseSchema)
        {
          
            if (databaseSchema != null)
            {
                var root = BuildNode(databaseSchema.Name, databaseSchema);
                AddRootNode(root);

                System.Action action = delegate
                {
                    AddChildsToNode(root);
                };
                this.BeginInvoke(action);
            }
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
            SyncDB(root.Checked, root.Tag);
            foreach (TreeNode node in root.Nodes)
            {
                SyncDB(node.Checked, node.Tag);
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }
        void SyncDB(bool nodeChecked, object schema)
        {
            if (schema != null)
            {
                if (schema is TableSchema)
                {
                    SyncDB(nodeChecked, schema as TableSchema);
                }
                else if (schema is ViewSchema)
                {
                    SyncDB(nodeChecked, schema as ViewSchema);
                }
                //else if (schema is ColumnSchema)
                //{
                //    SyncDB(nodeChecked, schema as ColumnSchema);
                //}
            }

        }
        void SyncDB(bool nodeChecked, TableSchema schema)
        {
            if (schema != null)
            {
                var name = schema.Name;
                if (nodeChecked)
                {
                    if (!GetConfig().Tables.Contains(name))
                    {
                        GetConfig().Tables.Add(name);
                        VarUtils.ShowInfo(string.Format("Table->{0,-6}:{1}", "Add", name));
                    }
                }
                else
                {
                    if (GetConfig().Tables.Contains(name))
                    {
                        GetConfig().Tables.Remove(name);
                        VarUtils.ShowInfo(string.Format("Table->{0,-6}:{1}", "Remove", name));
                    }
                }
            }
        }
        void SyncDB(bool nodeChecked, ViewSchema schema)
        {
            if (schema != null)
            {
                var name = schema.Name;
                if (nodeChecked)
                {
                    if (!GetConfig().Views.Contains(name))
                    {
                        GetConfig().Views.Add(name);
                        VarUtils.ShowInfo(string.Format("View->{0,-6}:{1}", "Add", name));
                    }
                }
                else
                {
                    if (GetConfig().Views.Contains(name))
                    {
                        GetConfig().Views.Remove(name);
                        VarUtils.ShowInfo(string.Format("View->{0,-6}:{1}", "Remove", name));
                    }
                }
            }
        }
        //void SyncDB(bool nodeChecked, ColumnSchema schema)
        //{
        //    if (schema != null)
        //    {
        //        var name = string.Format("{0}.{1}", schema.Table.Name, schema.Name);
        //        if (nodeChecked)
        //        {
        //            if (!VarUtils.DataAbstractConfig.Columns.Contains(name))
        //            {
        //                VarUtils.DataAbstractConfig.Columns.Add(name);
        //                DTEUtils.ShowInfo(string.Format("Column->{0,-6}:{1}", "Add", name));
        //            }
        //        }
        //        else
        //        {
        //            if (VarUtils.DataAbstractConfig.Columns.Contains(name))
        //            {
        //                VarUtils.DataAbstractConfig.Columns.Remove(name);
        //                DTEUtils.ShowInfo(string.Format("Column->{0,-6}:{1}", "Remove", name));
        //            }
        //        }
        //    }
        //}
        #region
        private DataAbstractConfig GetConfig()
        {
            return VarUtils.Config as DataAbstractConfig;
        }
        #endregion
        #region  AddNode
        protected override void AddChildsToNode(TreeNode pNode)
        {
            object tag = pNode.Tag;
            if (tag is DatabaseSchema)
            {
                var obj = tag as DatabaseSchema;
                AddCollectionNode(pNode, "Tables", obj.Tables);
                AddCollectionNode(pNode, "Views", obj.Views);
                AddCollectionNode(pNode, "Commands", obj.Commands);
            }
            else if (tag is TableSchema)
            {
                var obj = tag as TableSchema;
                AddCollectionNode(pNode, "Columns", obj.Columns);
                AddCollectionNode(pNode, "Indexes", obj.Indexes);
                AddCollectionNode(pNode, "Keys", obj.Keys);
            }
            else if (tag is ViewSchema)
            {
                var obj = tag as ViewSchema;
                AddCollectionNode(pNode, "Columns", obj.Columns);
            }
            else if (tag is CommandSchema)
            {
                var obj = tag as CommandSchema;
                AddCollectionNode(pNode, "Parameters", obj.Parameters);
                AddCollectionNode(pNode, "CommandResults", obj.CommandResults);
            }
            else if (tag is CommandResultSchema)
            {
                var obj = tag as CommandResultSchema;
                AddCollectionNode(pNode, "Columns", obj.Columns);
            }
            else if (tag is System.Collections.ICollection)
            {
                AddToCollectionNode(pNode);
            }

        }
        void AddSchemaObjectNode(TreeNode pNode, object target)
        {
            if (target is ColumnSchema)
            {
                AddColumnSchemaNode(pNode, target);
            }
            else if (target is IndexSchema)
            {
                AddIndexSchemaNode(pNode, target);
            }
            else if (target is TableKeySchema)
            {
                AddTableKeySchemaNode(pNode, target);
            }
            else if (target is ViewColumnSchema)
            {
                AddViewColumnSchemaNode(pNode, target);
            }
            else if (target is ParameterSchema)
            {
                AddParameterSchemaNode(pNode, target);
            }

            else if (target is CommandResultColumnSchema)
            {
                AddCommandResultColumnSchemaNode(pNode, target);
            }
            else if (target is SchemaObjectBase)
            {
                var node = BuildNode(((SchemaObjectBase)target).Name, target);
                AddLoadingNode(node);
                AddChildNode(pNode, node);
            }
        }
        void AddCollectionNode(TreeNode pNode, string name, object target)
        {
            var node = BuildNode(name, target);
            System.Action action = delegate
            {
                AddLoadingNode(node);
                AddChildNode(pNode, node);
            };
            this.BeginInvoke(action);
        }

        void AddToCollectionNode(TreeNode pNode)
        {
            var objs = pNode.Tag as System.Collections.ICollection;
            foreach (var obj in objs)
            {
                AddSchemaObjectNode(pNode, obj);
            }
        }


        void AddColumnSchemaNode(TreeNode pNode, object target)
        {
            var obj = target as ColumnSchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}{1}",
                   obj.Name, ShowInfo(
                    obj.NativeType, obj.Size,
                    obj.AllowDBNull ? "null" : "not null",
                    obj.IsForeignKeyMember ? "ForeignKey" : "",
                    obj.IsPrimaryKeyMember ? "PrimaryKey" : "",
                    obj.IsUnique ? "Unique" : "",
                    obj.Description)
                    ), obj);
               AddChildNode(pNode,node);
            }
        }
        void AddIndexSchemaNode(TreeNode pNode, object target)
        {
            var obj = target as IndexSchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}[{1}]{2}",
                   obj.Name, ShowNames(obj.MemberColumns), ShowInfo(
                    obj.IsClustered ? "Clustered" : "",
                    obj.IsPrimaryKey ? "PrimaryKey" : "",
                    obj.IsUnique ? "Unique" : "",
                    obj.Description)
                    ), obj);
                AddChildNode(pNode, node);
            }
        }

        void AddTableKeySchemaNode(TreeNode pNode, object target)
        {
            var obj = target as TableKeySchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}{1}",
                 obj.Name, ShowInfo(obj.Description)
                    ), obj);
                AddChildNode(pNode, node);
            }
        }
        void AddViewColumnSchemaNode(TreeNode pNode, object target)
        {
            var obj = target as ViewColumnSchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}{1}",
                   obj.Name, ShowInfo(
                    obj.NativeType, obj.Size,
                    obj.AllowDBNull ? "null" : "not null", obj.Description)
                    ), obj);
                AddChildNode(pNode, node);
            }
        }
        void AddParameterSchemaNode(TreeNode pNode, object target)
        {
            var obj = target as ParameterSchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}{1}",
                   obj.Name, ShowInfo(
                    obj.NativeType, obj.Size, obj.Direction,
                    obj.AllowDBNull ? "null" : "not null", obj.Description)
                    ), obj);
                AddChildNode(pNode, node);
            }
        }
        void AddCommandResultSchemaNode(TreeNode pNode, object target)
        {
            var obj = target as CommandResultSchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}{1}",
                   obj.Name, ShowInfo("")
                    ), obj);
                AddChildNode(pNode, node);
            }
        }
        void AddCommandResultColumnSchemaNode(TreeNode pNode, object target)
        {
            var obj = target as CommandResultColumnSchema;
            if (obj != null)
            {
                var node = BuildNode(string.Format("{0}{1}",
                   obj.Name, ShowInfo(
                    obj.NativeType, obj.Size,
                    obj.AllowDBNull ? "null" : "not null", obj.Description)
                    ), obj);
                AddChildNode(pNode, node);
            }
        }

        string ShowNames(ColumnSchemaCollection schemas)
        {
            var names = new List<string>();
            foreach (var schema in schemas)
            {
                names.Add(schema.Name);
            }
            return string.Join(",", names);
        }

        string ShowInfo(params object[] objs)
        {
            var info = string.Join(",", objs);
            if (info != null)
            {
                info = System.Text.RegularExpressions.Regex.Replace(info, ",,{0,}", ",");
                if (info.Length > 1)
                {
                    return string.Format("({0})", info.Trim(','));
                }
            }
            return string.Empty;
        }
        #endregion
    }
}
