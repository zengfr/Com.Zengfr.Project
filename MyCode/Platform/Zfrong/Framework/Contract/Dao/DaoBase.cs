using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IDao.Plugin;

namespace Zfrong.Framework.CoreBase.Dao
{
    public abstract class DaoBase<T> : IDaoPlugin<T>
        {
            protected virtual IDaoPlugin<T> DaoPlugin { get; set; }
            public DaoBase(IDaoPlugin<T> plugin)
            {
                this.DaoPlugin = plugin;
            }

            public virtual void Delete(T entity)
            {
                this.DaoPlugin.Delete(entity);
            }

            public virtual void Delete(params ICriterion[] criterion)
            {
                this.DaoPlugin.Delete(criterion);
            }

            public virtual void Delete(IList<long> Ids)
            {
                this.DaoPlugin.Delete(Ids);
            }

            public virtual T Get(long id)
            {
                return this.DaoPlugin.Get(id);
            }

            public virtual T Get(long id, LockMode lockMode)
            {
                return this.DaoPlugin.Get(id, lockMode);
            }

            public virtual IList<T> FindAll(Order[] orders, params ICriterion[] criterion)
            {
                return this.DaoPlugin.FindAll(orders, criterion);
            }

            public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, params ICriterion[] criterion)
            {
                return this.DaoPlugin.SlicedFindAll(first,  maxResult,orders, criterion);
            }

            public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, DetachedCriteria detachedCriteria)
            {
                return this.DaoPlugin.SlicedFindAll(first,maxResult,orders,detachedCriteria);
            }

            public virtual int Count(DetachedCriteria detachedCriteria)
            {
                return this.DaoPlugin.Count(detachedCriteria);
            }

            public virtual long Save(T entity)
            {
                return this.DaoPlugin.Save(entity);
            }

            public virtual void Update(T entity)
            {
                  this.DaoPlugin.Update(entity);
            }

            public virtual void SaveOrUpdate(T entity)
           {
                 this.DaoPlugin.SaveOrUpdate(entity);
            }
            public virtual int Count(params ICriterion[] criterion)
            {
                return this.DaoPlugin.Count(criterion);
            }
        }
}
