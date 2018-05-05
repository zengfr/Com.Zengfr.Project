using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.IModel;
using Zfrong.Framework.Core.Model;
namespace Zfrong.Framework.CoreBase.Model
{

    public abstract class ExtTreeModelBase<T, TId> : ModelBase<TId>, IExtTreeModel<T,TId>
        {
            public virtual string text
            {
                get { return Name; }
                set { Name = value; }
            }
            public virtual bool leaf { get; set; }
            public virtual string cls { get; set; }
            public virtual string iconCls { get; set; }
            public virtual string url { get; set; }
            public virtual string Name { get; set; }
            public virtual T Parent
            { get; set; }
        }
    public abstract class ExtTreeModelBase<T> : ExtTreeModelBase<T,long>, IExtTreeModel<T>
    { 

    }
}
