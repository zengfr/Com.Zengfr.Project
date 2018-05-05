using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate;
using System.ServiceModel;
namespace Zfrong.Framework.Core.IService.Plugin
{
    [ServiceContract]
    public interface IServicePlugin<T, TId>
    {
        #region
        [OperationContract]
        void Delete(T entity);
         [OperationContract(Name = "DeleteBy")]
        void Delete(params ICriterion[] criterion);
         [OperationContract(Name = "DeleteByKey")]
        void Delete(IList<TId> Ids);
        #endregion

        #region
         [OperationContract]
         T Get(TId id);
         [OperationContract(Name = "GetByLock")]
         T Get(TId id, LockMode lockMode);
         [OperationContract]
         IList<T> FindAll(Order[] orders, params ICriterion[] criterion);
         [OperationContract]
         IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, params ICriterion[] criterion);
         [OperationContract(Name = "SlicedFindAllByCriteria")]
         IList<T> SlicedFindAll(int first, int maxResult, Order[] orders, DetachedCriteria detachedCriteria);
        #endregion

        #region
         [OperationContract]
         TId Save(T entity);
         [OperationContract]
         void Update(T entity);
         [OperationContract]
         void SaveOrUpdate(T entity);   
        #endregion
    }
     [ServiceContract]
    public interface IServicePlugin<T> : IServicePlugin<T, long>
    {

    }
}
