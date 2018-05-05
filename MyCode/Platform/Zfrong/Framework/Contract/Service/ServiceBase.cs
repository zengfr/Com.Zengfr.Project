using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IService.Plugin;
using Spring.Transaction.Interceptor;
namespace Zfrong.Framework.CoreBase.Service
{
     public abstract class ServiceBase<T> : IServicePlugin<T>
        {
         protected virtual IServicePlugin<T> ServicePlugin { get; set; }
            public ServiceBase(IServicePlugin<T> plugin)
            {
                this.ServicePlugin = plugin;
            }

          [Transaction(ReadOnly = false)]
            public virtual void Delete(T entity)
            {
                this.ServicePlugin.Delete(entity);
            }
          [Transaction(ReadOnly = false)]
          public virtual void Delete(params ICriterion[] criterion)
            {
                this.ServicePlugin.Delete(criterion);
            }
          [Transaction(ReadOnly = false)]
          public virtual void Delete(IList<long> Ids)
            {
                this.ServicePlugin.Delete(Ids);
            }
          [Transaction(ReadOnly = false)]
          public virtual T Get(long id)
            {
                return this.ServicePlugin.Get(id);
            }
          public virtual T Get(long id, LockMode mode)
            {
              return  this.ServicePlugin.Get(id, mode);
            }
           [Transaction(ReadOnly = false)]
          public virtual IList<T> FindAll(Order[] orders, params ICriterion[] criterion)
            { 
               IList<T> objs=this.ServicePlugin.FindAll(orders, criterion);
               return objs;
            }
            [Transaction(ReadOnly = false)]
           public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, params ICriterion[] criterion)
            {
                IList<T> objs = this.ServicePlugin.SlicedFindAll( first,maxResult,orders, criterion);
                return objs;
            }
            [Transaction(ReadOnly = false)]
            public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, DetachedCriteria detachedCriteria)
            {
                IList<T> objs = this.ServicePlugin.SlicedFindAll(first, maxResult,orders, detachedCriteria);
                return objs;
            }
          [Transaction(ReadOnly = false)]
            public virtual long Save(T entity)
            {
                return this.ServicePlugin.Save(entity);
            }
          [Transaction(ReadOnly = false)]
          public virtual void Update(T entity)
            {
                this.ServicePlugin.Update(entity);
            }
          [Transaction(ReadOnly = false)]
          public virtual void SaveOrUpdate(T entity)
            {
                this.SaveOrUpdate(entity);
            }
        }
}
