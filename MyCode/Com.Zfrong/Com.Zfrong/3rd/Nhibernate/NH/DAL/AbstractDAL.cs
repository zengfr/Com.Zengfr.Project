using System;
using System.Collections.Generic;
using System.Text;
using Com.Zfrong.Common.Data.NH.ActiveRecords;
using NHibernate;
using NHibernate.Criterion;
namespace Com.Zfrong.Common.Data.NH.DAL
{
    /// <summary>
    /// 单个实体类增删改查功能(CRUD)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDALBase<T> where T : class
    {
        T FindByPrimaryKey(object id);

        IList<T> FindAll();
        IList<T> FindAll(Order[] orders, params ICriterion[] criterias);
        IList<T> FindAll(params ICriterion[] criterias);

        IList<T> FindAllByProperty(String property, object value);
        IList<T> FindAllByProperty(String orderByColumn, String property, object value);

        #region SlicedFindAll
        /// <summary>
        /// Returns a portion of the query results (sliced)
        /// </summary>
        IList<T> SlicedFindAll(int firstResult, int maxresults, Order[] orders, params ICriterion[] criterias);
        /// <summary>
        /// Returns a portion of the query results (sliced)
        /// </summary>
        IList<T> SlicedFindAll(int firstResult, int maxresults, params ICriterion[] criterias);
        #endregion
        /**/
        /// <summary>
        /// Creates (Saves) a new instance to the database.
        /// </summary>
        /// <param name="instance"></param>
        void Create(T instance);

        /**/
        /// <summary>
        /// Saves the instance to the database
        /// </summary>
        /// <param name="instance"></param>
        void SaveOrUpdate(T instance);

        /**/
        /// <summary>
        /// Persists the modification on the instance
        /// state to the database.
        /// </summary>
        /// <param name="instance"></param>
        void Update(T instance);

        /**/
        /// <summary>
        /// Deletes the instance from the database.
        /// </summary>
        /// <param name="instance"></param>
        void Delete(T instance);

        void DeleteAll();
    }

    /// <summary>
    /// 单个实体类增删改查功能(CRUD)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DALObjectBase<T> : IDALBase<T> where T : class
    {
        #region Constructors

        public DALObjectBase()
        {
        }

        #endregion
        #region Methods

        public virtual T FindByPrimaryKey(object id)
        {
            return ActiveRecord.FindByPrimaryKey<T>(id);
        }
        #region FindAll
        public virtual IList<T> FindAll()
        {
            return ActiveRecord.FindAll<T>();
        }
        public virtual IList<T> FindAll(Order[] orders, params ICriterion[] criterias)
        {
            return ActiveRecord.FindAll<T>(orders, criterias);//
        }
        public virtual IList<T> FindAll(params ICriterion[] criterias)
        {
            return ActiveRecord.FindAll<T>(criterias);//
        }
        #endregion

        #region FindAllByProperty
        public virtual IList<T> FindAllByProperty(String property, object value)
        {
            return ActiveRecord.FindAllByProperty<T>(property, value);
        }
        public virtual IList<T> FindAllByProperty(String orderByColumn, String property, object value)
        {
            return ActiveRecord.FindAllByProperty<T>(orderByColumn, property, value);
        }
        #endregion
        #region SlicedFindAll
        /// <summary>
        /// Returns a portion of the query results (sliced)
        /// </summary>
        public virtual IList<T> SlicedFindAll(int firstResult, int maxresults, Order[] orders, params ICriterion[] criterias)
        {
            return ActiveRecord.SlicedFindAll<T>(firstResult, maxresults, orders, criterias);
        }

        /// <summary>
        /// Returns a portion of the query results (sliced)
        /// </summary>
        public virtual IList<T> SlicedFindAll(int firstResult, int maxresults, params ICriterion[] criterias)
        {
            return SlicedFindAll(firstResult, maxresults, null, criterias);
        }
        #endregion

        public virtual void Create(T instance)
        {
            ActiveRecord.Create(instance);
        }
        public virtual void SaveOrUpdate(T instance)
        {
            ActiveRecord.SaveOrUpdate(instance);
        }
        public virtual void Update(T instance)
        {
            ActiveRecord.Update(instance);
        }
        public virtual void Delete(T instance)
        {
            ActiveRecord.Delete(instance);
        }
        public virtual void DeleteAll()
        {
            ActiveRecord.DeleteAll<T>();
        }
        public virtual void DeleteAll(string where)
        {
            ActiveRecord.DeleteAll<T>(where);
        }
        #endregion
    }
}
