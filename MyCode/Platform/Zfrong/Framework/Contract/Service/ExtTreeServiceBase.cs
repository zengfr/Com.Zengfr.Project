using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Core.DataContract;
using Spring.Transaction.Interceptor;
namespace Zfrong.Framework.CoreBase.Service
{
    public abstract class ExtTreeServiceBase<T> : IExtTreeServicePlugin<T>
        {
        protected virtual IExtTreeServicePlugin<T> ServicePlugin { get; set; }
            public ExtTreeServiceBase(IExtTreeServicePlugin<T> plugin)
            {
                this.ServicePlugin = plugin;
            }
          [Transaction(ReadOnly = false)]
            public virtual IList<T> NodeFindAll(string parentNode, string checkedValue)
            {
                return this.ServicePlugin.NodeFindAll(parentNode, checkedValue);
            }
         [Transaction(ReadOnly = false)]
          public virtual ExtResponse<T> NodeCreate(long parentId, string checkedValue, T dataItem)
            {
                return this.ServicePlugin.NodeCreate(parentId, checkedValue, dataItem);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtResponse<T> NodeRemove(long id)
            {
                return this.ServicePlugin.NodeRemove(id);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtResponse<T> NodeUpdate(long id, T dataItem)
            {
                return this.ServicePlugin.NodeUpdate(id, dataItem);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtResponse<T> NodeMove(long id, long parentId)
            {
                return this.ServicePlugin.NodeMove(id, parentId);
            }
        }
}
