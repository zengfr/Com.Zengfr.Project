using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate;
using Zfrong.Framework.Core.IModel;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.IRepository;
using Zfrong.Framework.Utils;
namespace Zfrong.Framework.Repository
{
    public partial class ExtTreeNHibernateRepository<T> : NHibernateRepository<T>, IExtTreeNHibernateRepository<T>
         where T : class,IExtTreeModel<T>, new()
    {
        #region
        public virtual IList<T> NodeFindAll(string node, string checkedValue)
        {
            IList<T> objs = null;
            if (node != null && node.Length != 0)
            {
                if (node != "0")
                {
                    objs = FindAll(null, Expression.Eq("Parent.ID", long.Parse(node)));
                }
                else
                    objs = FindAll(null, Expression.IsNull("Parent.ID"));
            }
            else
                objs = new T[] { new T { Name = "ItemName" } };
            //IList<D> items = new List<D>();
            //D item;
            //foreach (E obj in objs)
            //{
            //    item = new R();
            //    BindProperties<E, D>(item, obj);
            //    if (checkedValue != null && checkedValue.Length > 3)
            //        item.Checked = checkedValue == "false" ? false : true;
            //    item.expanded = false;
            //    item.expandable = true;
            //    items.Add(item);
            //}

            return objs;
        }
        public virtual T NodeCreate(long parentId, string checkedValue, T dataItem)
        {
            T obj = dataItem;
            //BindProperties<T>(obj, dataItem);
            obj.Parent = parentId == 0 ? default(T) : Get(parentId);
            obj.leaf = false;
            Save(obj);
            //D item = new D();
            //BindProperties<E, D>(item, obj);
            //item.expandable = true;
            //if (checkedValue != null && checkedValue.Length > 3)
            //    item.Checked = checkedValue == "false" ? false : true;
            return obj;
        }
        public virtual void NodeRemove(long id)
        {
            Delete(Expression.Eq("ParentId", id));
            Delete(Expression.Eq("ID", id));
            
        }
        public virtual T NodeUpdate(long id, T dataItem)
        {
             T obj=default(T);
            if (id > 0)
            {
                obj = Get(id); 
                //清除缓存 持久对象变成脱管对象 ，更新时和缓存对比不一致会报错
                EvictALL(obj);
                MyObjectUtils.BindProperties<T>(obj, dataItem);
                obj.ID = id;
                Update(obj);
            }
            return obj;
        }
        public virtual T NodeMove(long id, long parentId)
        {
            T obj = default(T);
            if (id > 0)
            {
                obj = Get(id);
                obj.Parent = parentId == 0 ? default(T) : Get(parentId);
                Update(obj);
            }
            return obj;
        }
        #endregion
    }
}
