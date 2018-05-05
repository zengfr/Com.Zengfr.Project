using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.DataContract;
using NHibernate.Criterion;
using NHibernate;
namespace Zfrong.Framework.Core.IDao.Plugin
{
    public interface IExtTreeDaoPlugin<T, TId>
    {
       IList<T> NodeFindAll(string parentNode, string checkedValue);
       T NodeCreate(TId parentId, string checkedValue, T dataItem);
       void NodeRemove(TId id);
       T NodeUpdate(TId id, T dataItem);
       T NodeMove(TId id, TId parentId);
    }
    public interface IExtTreeDaoPlugin<T> : IExtTreeDaoPlugin<T, long>
    {

    }
   
}
