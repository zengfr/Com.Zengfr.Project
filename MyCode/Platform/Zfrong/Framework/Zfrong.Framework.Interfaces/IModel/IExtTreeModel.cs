using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.Model;
namespace Zfrong.Framework.Core.IModel
{
    public interface IExtTreeModel<T,TId> : IModel<TId>
    {
        string cls { get; set; }
        string iconCls { get; set; }
        bool leaf { get; set; }
        string Name { get; set; }
        T Parent { get; set; }
        string text { get; set; }
        string url { get; set; }
    }
    public interface IExtTreeModel<T> : IExtTreeModel<T,long>,IModel<long>
    {
        
    }
}
