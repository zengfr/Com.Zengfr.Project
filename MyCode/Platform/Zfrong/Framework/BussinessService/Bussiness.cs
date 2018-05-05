using System;
using System.Collections.Generic;
using Zfrong.Framework.Core.IBussiness;
using Zfrong.Framework.Core.IRepository;
using NHibernate;
using NHibernate.Criterion;

namespace Zfrong.Framework.Bussiness
{
    public  class Bussiness<T, TId> : IBussiness<T, TId> where T : class
    {
        public virtual INHibernateRepository<T, TId> CurrentRepository { get; set; }
        public virtual T Get(TId id)
        {
            T obj = this.CurrentRepository.Get(id);
           return obj;
        }
        public virtual T Get(TId id, LockMode lockMode)
        {
            return this.CurrentRepository.Get(id, lockMode);
        }

        public virtual TId Save(T entity)
        {
            return this.CurrentRepository.Save(entity);
        }
        public virtual void Update(T entity)
        {
            this.CurrentRepository.Update(entity);
        }
        public virtual void SaveOrUpdate(T entity)
        {
            this.CurrentRepository.SaveOrUpdate(entity);
        }
        public virtual void Delete(T entity)
        {
            this.CurrentRepository.Delete(entity);
        }

        public virtual void Delete(params ICriterion[] criterion)
        {
            this.CurrentRepository.Delete( criterion);
        }
        public virtual void Delete(IList<TId> Ids)
        {
           this.CurrentRepository.Delete(Ids);
        }
       
        public virtual IList<T> FindAll(Order[] orders, params ICriterion[] criterion)
        {
            
            return this.CurrentRepository.FindAll(orders, criterion);
        }
         public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, params ICriterion[] criterion)
        {
            
            return this.CurrentRepository.SlicedFindAll(first, maxResult, orders, criterion);
        }
        public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, DetachedCriteria detachedCriteria)
        {
            
            return this.CurrentRepository.SlicedFindAll(first, maxResult, orders, detachedCriteria);
        }
        public virtual int Count(DetachedCriteria detachedCriteria)
        {
            return this.CurrentRepository.Count(detachedCriteria);
        }

    }
    public class Bussiness<T> : Bussiness<T, long>,IBussiness<T> where T : class
    {

    }
}
