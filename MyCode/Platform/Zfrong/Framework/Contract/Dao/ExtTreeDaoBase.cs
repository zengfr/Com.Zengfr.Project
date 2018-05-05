using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IDao.Plugin;

namespace Zfrong.Framework.CoreBase.Dao
{
    public abstract class ExtTreeDaoBase<T> : IExtTreeDaoPlugin<T>
        {
            protected virtual IExtTreeDaoPlugin<T> DaoPlugin { get; set; }
            public ExtTreeDaoBase(IExtTreeDaoPlugin<T> plugin)
            {
                this.DaoPlugin = plugin;
            }

            public virtual T NodeCreate(long parentId, string checkedValue, T dataItem)
            {
                return this.DaoPlugin.NodeCreate(parentId, checkedValue, dataItem);
            }

            public virtual void NodeRemove(long id)
            {
                this.DaoPlugin.NodeRemove(id);
            }

            public virtual T NodeUpdate(long id, T dataItem)
            {
                return this.DaoPlugin.NodeUpdate(id, dataItem);
            }

            public virtual T NodeMove(long id, long parentId)
            {
                return this.DaoPlugin.NodeMove(id,parentId);
            }

            public virtual IList<T> NodeFindAll(string parentNode, string checkedValue)
            {
              return  this.DaoPlugin.NodeFindAll(parentNode, checkedValue);
            }
        }
}
