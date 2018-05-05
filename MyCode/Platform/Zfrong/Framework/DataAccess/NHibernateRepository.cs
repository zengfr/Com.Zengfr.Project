using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Spring.Data.NHibernate;
using Spring.Data.NHibernate.Generic.Support;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Repository
{

    public class NHibernateRepository<T, TId> : INHibernateRepository<T, TId> where T : class
    {   
        #region Thread-safeÎ¨Ò»ÊµÀý
        private static object locker = new object();
        static NHibernateRepository<T, TId> instance;
        public static NHibernateRepository<T, TId> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new NHibernateRepository<T, TId>();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public virtual ISessionFactory SessionFactory { get; set; }
        protected virtual ISession CurrentSession { get { return SessionFactory.GetCurrentSession(); } }

         #region 
        public virtual void Evict(object obj)
        {
            this.CurrentSession.Evict(obj);
        }
        public virtual void EvictALL(object obj)
        {
             Evict(obj);
             if (obj != null)
             {
                 System.Type t = obj.GetType();
                 System.Reflection.PropertyInfo[] properties = t.GetProperties();
                 object v;
                 foreach (System.Reflection.PropertyInfo p in properties)
                 {
                     if (p.CanWrite)
                     {
                         v = p.GetValue(obj, null) as Zfrong.Framework.Core.Model.ModelBase;
                         if (v != null)
                         {
                             Evict(v);
                         }
                     }
                 }
             }
        }
        public virtual T Get(TId id)
        {
            return this.CurrentSession.Get<T>(id);
        }
        public virtual T Get(TId id, LockMode lockMode)
        {
            return this.CurrentSession.Get<T>(id, lockMode);
        }

        public virtual TId Save(T entity)
        {
            return (TId)this.CurrentSession.Save(entity);
        }
        public virtual void Update(T entity)
        {
            this.CurrentSession.Update(entity);
        }
        public virtual void SaveOrUpdate(T entity)
        {
            this.CurrentSession.SaveOrUpdate(entity);
        }
        public virtual void Delete(T entity)
        {
            this.CurrentSession.Delete(entity);
        }

        public virtual void Delete(params ICriterion[] criterion)
        {
            IList<T> objs=FindAll(null,criterion);
            foreach (T obj in objs)
            { 
                Delete(obj);
            }
        }
        public virtual void Delete(IList<TId> Ids)
        {
            foreach (TId pk in Ids)
            {
                T obj = Get(pk);

                if (obj != null)
                {
                   Delete(obj);
                }
            }

        }

        public virtual IList<T> FindAll(Order[] orders, params ICriterion[] criterion) 
        {
            ICriteria criteria = this.CurrentSession.CreateCriteria<T>();
            AddCriterionToCriteria(criteria,criterion);
            AddOrdersToCriteria(criteria, orders);
             IList<T>  objs=criteria.List<T>();
             return objs;
        }
        public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, params ICriterion[] criterions)
        {
            ICriteria criteria = this.CurrentSession.CreateCriteria(typeof(T));
            AddCriterionToCriteria(criteria, criterions);
                AddOrdersToCriteria(criteria, orders);
                criteria.SetFirstResult(first);
                criteria.SetMaxResults(maxResult);  

                IList<T> objs = criteria.List<T>();
                return objs;
        }
        public virtual IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, DetachedCriteria detachedCriteria)
        {
            ICriteria executableCriteria = detachedCriteria.GetExecutableCriteria(this.CurrentSession);
            AddOrdersToCriteria(executableCriteria, orders);
            executableCriteria.SetFirstResult(first);
            executableCriteria.SetMaxResults(maxResult);

            return executableCriteria.List<T>();
        }
        public virtual int Count(DetachedCriteria detachedCriteria)
        {
            ICriteria executableCriteria = detachedCriteria.GetExecutableCriteria(this.CurrentSession);
            executableCriteria.SetProjection(Projections.Count(Projections.Id()));
            return executableCriteria.UniqueResult <int>();
        }
        public virtual int Count(params ICriterion[] criterions)
        {
            ICriteria criteria = this.CurrentSession.CreateCriteria(typeof(T));
            AddCriterionToCriteria(criteria, criterions);
            criteria.SetProjection(Projections.Count(Projections.Id()));
            return criteria.UniqueResult<int>();
        }
        #endregion

        protected static void AddOrdersToCriteria(ICriteria criteria, IEnumerable<Order> orders)
        {
            if (orders != null)
            {
                foreach (Order order in orders)
                {
                    if (order != null)
                    criteria.AddOrder(order);
                }
            }
        }
        protected static void AddCriterionToCriteria(ICriteria criteria, IEnumerable<ICriterion> criterionList)
        {
            if (criterionList != null)
            {
                foreach (ICriterion c in criterionList)
                {
                    if (c != null)
                        criteria.Add(c);
                }
            }
        }
    }
    public class NHibernateRepository<T> : NHibernateRepository<T, long>,INHibernateRepository<T> where T : class
    {

    }
}