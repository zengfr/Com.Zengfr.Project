using NHibernate.Criterion;
using NHibernate;
using System.Collections.Generic;
namespace Zfrong.Framework.Core.IDao.Plugin
{
    public interface IDaoPlugin<T, TId>
    {
        #region
        void Delete(T entity);
        void Delete(params ICriterion[] criterion);
        void Delete(IList<TId> Ids);
        #endregion
        #region
        T Get(TId id);
        T Get(TId id, LockMode lockMode);

        IList<T> FindAll(Order[] orders, params ICriterion[] criterion);

        IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, params ICriterion[] criterion);
        IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, DetachedCriteria detachedCriteria);
        
        int Count(params ICriterion[] criterion);
        int Count(DetachedCriteria detachedCriteria);
        #endregion
        #region
       TId Save(T entity);
       void Update(T entity);
       void SaveOrUpdate(T entity);
        #endregion

    }
    public interface IDaoPlugin<T> : IDaoPlugin<T, long>
    {

    }
}