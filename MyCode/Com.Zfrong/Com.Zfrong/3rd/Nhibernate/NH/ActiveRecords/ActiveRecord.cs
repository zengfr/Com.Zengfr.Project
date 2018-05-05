// Copyright 2004-2006 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Com.Zfrong.Common.Data.NH.ActiveRecords
{
	using System;
    using System.Collections;
    using System.Collections.Generic;
    using NHibernate;
    using NHibernate.Criterion;
    using Com.Zfrong.Common.Data.NH.SessionStorage;


	/// <summary>
    /// 移植 Castle 的 ActiveRecord 直接为 NHibernate 所用 
    /// 
    /// 我非常不喜欢把业务操作移殖到实体层去，层与层之间的职任应该划分得很清晰，
    /// 要不然我看了就觉得不舒服，可能有人会说如果不喜欢我完全可以不去理会它，
    /// 当它不存在就是了，但它却确确实实地存在着，还是 public ，
    /// 无论走到那里都能看到自己不喜欢的东西我还能无动于衷吗？而且我们还有同样方便方法来代替，
    /// 稍后将会讲到，所以我要把 ActiveRecordBase 改造成为 ActiveRecord 并且只留下静态方法
    /// 
	/// Base class for all ActiveRecord classes. Implements 
	/// all the functionality to simplify the code on the 
	/// subclasses.
	/// </summary>
	[Serializable]
    public class ActiveRecord
	{

		/// <summary>
		/// Constructs an ActiveRecordBase subclass.
		/// </summary>
        public ActiveRecord()
        {
        }

        #region static methods
        #region Execute
        /// <summary>
        /// Invokes the specified delegate passing a valid 
        /// NHibernate session. Used for custom NHibernate queries.
        /// </summary>
        /// <param name="targetType">The target ActiveRecordType</param>
        /// <param name="call">The delegate instance</param>
        /// <param name="instance">The ActiveRecord instance</param>
        /// <returns>Whatever is returned by the delegate invocation</returns>
        //public static object Execute(Type targetType, NHibernateDelegate call, object instance)
        //{
        //    if (targetType == null) throw new ArgumentNullException("targetType", "Target type must be informed");
        //    if (call == null) throw new ArgumentNullException("call", "Delegate must be passed");

        //    EnsureInitialized(targetType);

        //    ISession session = _holder.CreateSession( targetType );

        //    try
        //    {
        //        return call(session, instance);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new ActiveRecordException("Error performing Execute for " + targetType.Name, ex);
        //    }
        //    finally
        //    {
        //        _holder.ReleaseSession(session);
        //    }
        //}
        #endregion
        #region FindByPrimaryKey
        /// <summary>
        /// Finds an object instance by a unique ID
        /// </summary>
        /// <param name="targetType">The AR subclass type</param>
        /// <param name="id">ID value</param>
        /// <param name="throwOnNotFound"><c>true</c> if you want to catch an exception 
        /// if the object is not found</param>
        /// <returns></returns>
        /// <exception cref="ObjectNotFoundException">if <c>throwOnNotFound</c> is set to 
        /// <c>true</c> and the row is not found</exception>

        public static T FindByPrimaryKey<T>(object id, bool throwOnNotFound) where T : class
        {
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                return session.Load<T>(id);
            }
            catch (ObjectNotFoundException ex)
            {
                if (throwOnNotFound)
                {
                    String message = String.Format("Could not find {0} with id {1}", typeof(T).Name, id);
                    throw new NotFoundException(message, ex);
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform Load (Find by id) for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }

        /// <summary>
        /// Finds an object instance by a unique ID
        /// </summary>
        /// <param name="targetType">The AR subclass type</param>
        /// <param name="id">ID value</param>
        /// <returns></returns>
        public static T FindByPrimaryKey<T>(object id) where T : class
        {
            return FindByPrimaryKey<T>(id, true);
        }
        #endregion
        #region FindAll
        /// <summary>
        /// Returns all instances found for the specified type.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static IList<T> FindAll<T>() where T : class
        {
            return FindAll<T>((Order[])null);
        }
        /// <summary>
        /// Returns all instances found for the specified type 
        /// using sort orders and criterias.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="orders"></param>
        /// <param name="criterias"></param>
        /// <returns></returns>
        public static IList<T> FindAll<T>(Order[] orders, params ICriterion[] criterias) where T : class
        {
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));

                foreach (ICriterion cond in criterias)
                {
                    criteria.Add(cond);
                }

                if (orders != null)
                {
                    foreach (Order order in orders)
                    {
                        criteria.AddOrder(order);
                    }
                }

                return criteria.List<T>();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform FindAll for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }

        /// <summary>
        /// Returns all instances found for the specified type 
        /// using criterias.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="criterias"></param>
        /// <returns></returns>
        public static IList<T> FindAll<T>(params ICriterion[] criterias) where T : class
        {
            return FindAll<T>(null, criterias);
        }
        #endregion
        #region FindAllByProperty
        /// <summary>
        /// Finds records based on a property value
        /// </summary>
        /// <remarks>
        /// Contributed by someone on the forum
        /// http://forum.castleproject.org/posts/list/300.page
        /// </remarks>
        /// <param name="targetType">The target type</param>
        /// <param name="property">A property name (not a column name)</param>
        /// <param name="value">The value to be equals to</param>
        /// <returns></returns>
        public static IList<T> FindAllByProperty<T>(String property, object value) where T : class
        {
            return FindAll<T>(Expression.Eq(property, value));
        }

        /// <summary>
        /// Finds records based on a property value
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="orderByColumn">The column name to be ordered ASC</param>
        /// <param name="property">A property name (not a column name)</param>
        /// <param name="value">The value to be equals to</param>
        /// <returns></returns>
        public static IList<T> FindAllByProperty<T>(String orderByColumn, String property, object value) where T : class
        {
            return FindAll<T>(new Order[] { Order.Asc(orderByColumn) }, Expression.Eq(property, value));
        }
        #endregion
        #region FindAllByQueryName
        public static IList<T> FindAllByQueryName<T>(string queryName) where T : class
        {
            return FindAllByQueryName<T>(queryName, null);
        }

        public static IList<T> FindAllByQueryName<T>(string queryName, QueryParameter[] parameters) where T : class
        {
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));

                IQuery query = session.GetNamedQuery(queryName);

                if (parameters != null)
                {
                    foreach (QueryParameter parameter in parameters)
                    {
                        if (!QueryParameter.IsAvail(parameter))
                            continue;

                        if (parameter.Value is ICollection)
                        {
                            if (parameter.Type == null)
                            {
                                query.SetParameterList(parameter.Name, (ICollection)parameter.Value);
                            }
                            else
                            {
                                query.SetParameterList(parameter.Name, (ICollection)parameter.Value, parameter.Type);
                            }
                        }
                        else
                        {
                            if (parameter.Type == null)
                            {
                                query.SetParameter(parameter.Name, parameter.Value);
                            }
                            else
                            {
                                query.SetParameter(parameter.Name, parameter.Value, parameter.Type);
                            }
                        }
                    }
                }

                return query.List<T>();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform FindAllByQueryName for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        #endregion
        #region FindFrist
        /// <summary>
        /// Searches and returns the first row.
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="criterias">The criteria expression</param>
        /// <returns>A <c>targetType</c> instance or <c>null</c></returns>
        public static T FindFirst<T>(params ICriterion[] criterias) where T : class
        {
            return FindFirst<T>(null, criterias);
        }

        /// <summary>
        /// Searches and returns the first row.
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="orders">The sort order - used to determine which record is the first one</param>
        /// <param name="criterias">The criteria expression</param>
        /// <returns>A <c>targetType</c> instance or <c>null</c></returns>
        public static T FindFirst<T>(Order[] orders, params ICriterion[] criterias) where T : class
        {
            IList<T> result = SlicedFindAll<T>(0, 1, orders, criterias);
            return (result != null && result.Count > 0 ? result[0] : default(T));
        }
        #endregion
        #region FindOne
        /// <summary>
        /// Searches and returns the a row. If more than one is found, 
        /// throws <see cref="ActiveRecordException"/>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="criterias">The criteria expression</param>
        /// <returns>A <c>targetType</c> instance or <c>null</c></returns>
        public static T FindOne<T>(params ICriterion[] criterias) where T : class
        {
            IList<T> result = SlicedFindAll<T>(0, 2, criterias);

            if (result.Count > 1)
            {
                throw new ActiveRecordException(typeof(T).Name + ".FindOne returned " + result.Count + " rows. Expecting one or none");
            }

            return (result == null || result.Count == 0) ? default(T) : result[0];
        }
        #endregion
        #region FindAllByAggregate
        /// <summary>
        /// 通过聚合函数查找
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <typeparam name="M">返回值类型</typeparam>
        /// <param name="aggregate"></param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static M FindByAggregate<T, M>(AggregateEnum aggregate, string propertyName) where T : class
        {
            Type type = typeof(T);
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            string modelClass = typeof(T).Name;
            string query = string.Empty;

            if (aggregate == AggregateEnum.CountDistinct)
            {
                query = string.Format("select Count(distinct model.{0}) from {1} as model", propertyName, modelClass);
            }
            else
            {
                query = string.Format("select {0}(model.{1}) from {2} as model", aggregate, propertyName, modelClass);
            }

            try
            {
                IQuery q = session.CreateQuery(query);
                return (M)q.UniqueResult();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform FindByAggregate for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        #endregion
        #region ExecuteQuery
        //public static object ExecuteQuery(IActiveRecordQuery q)
        //{
        //    Type targetType = q.TargetType;

        //    EnsureInitialized(targetType);

        //    ISession session = _holder.CreateSession( targetType );

        //    try
        //    {
        //        return q.Execute(session);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ActiveRecordException("Could not perform Execute for " + targetType.Name, ex);
        //    }
        //    finally
        //    {
        //        _holder.ReleaseSession(session);
        //    }
        //}
        #endregion
        #region SlicedFindAll
        /// <summary>
        /// Returns a portion of the query results (sliced)
        /// </summary>
        public static IList<T> SlicedFindAll<T>(int firstResult, int maxresults, Order[] orders, params ICriterion[] criterias) where T : class
        {
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));

                foreach (ICriterion cond in criterias)
                {
                    criteria.Add(cond);
                }

                if (orders != null)
                {
                    foreach (Order order in orders)
                    {
                        criteria.AddOrder(order);
                    }
                }

                criteria.SetFirstResult(firstResult);
                criteria.SetMaxResults(maxresults);

                return criteria.List<T>();

            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform SlicedFindAll for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }

        /// <summary>
        /// Returns a portion of the query results (sliced)
        /// </summary>
        public static IList<T> SlicedFindAll<T>(int firstResult, int maxresults, params ICriterion[] criterias) where T : class
        {
            return SlicedFindAll<T>(firstResult, maxresults, null, criterias);
        }


        #endregion

        #region Delete
        /// <summary>
        /// Deletes the instance from the database.
        /// </summary>
        /// <param name="instance"></param>
        public static void Delete<T>(T instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                session.Delete(instance);

                session.Flush();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform Delete for " + instance.GetType().Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        public static void DeleteAll<T>() where T : class
        {
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                session.Delete(String.Format("from {0}", typeof(T).Name));

                session.Flush();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform DeleteAll for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        public static void DeleteAll<T>(string where) where T : class
        {
            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                session.Delete(String.Format("from {0} where {1}", typeof(T).Name, where));

                session.Flush();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform DeleteAll for " + typeof(T).Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        public static int DeleteAll<T>(IEnumerable pkValues) where T : class
        {
            if (pkValues == null)
                return 0;

            int c = 0;
            foreach (int pk in pkValues)
            {
                T obj = FindByPrimaryKey<T>(pk, false);
                if (obj != null)
                {
                    //ActiveRecord arBase = obj as ActiveRecord;
                    //if (arBase != null)
                        //arBase.Delete(); // in order to allow override of the virtual "Delete()" method
                    //else
                        ActiveRecord.Delete(obj);
                    c++;
                }
            }
            return c;
        }

        #endregion
        #region Create
        /// <summary>
        /// Creates (Saves) a new instance to the database.
        /// </summary>
        /// <param name="instance"></param>
        public static void Create<T>(T instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                session.Save(instance);

                session.Flush();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform Save for " + instance.GetType().Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        #endregion
        #region Save
        /// <summary>
        /// Saves the instance to the database
        /// </summary>
        /// <param name="instance"></param>
        public static void SaveOrUpdate<T>(T instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                session.SaveOrUpdate(instance);

                session.Flush();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform Save for " + instance.GetType().Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        #endregion
        #region Update
        /// <summary>
        /// Persists the modification on the instance
        /// state to the database.
        /// </summary>
        /// <param name="instance"></param>
        public static void Update<T>(T instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            SessionObject sessionObj = NHibernateDatabaseFactory.CreateSession();

            ISession session = sessionObj.Session;

            try
            {
                session.Update(instance);

                session.Flush();
            }
            catch (Exception ex)
            {
                throw new ActiveRecordException("Could not perform Save for " + instance.GetType().Name, ex);
            }
            finally
            {
                if (sessionObj.IsNeedClose)
                    session.Close();
            }
        }
        public static void UpdateProperty<T>(T instance, string propertyName)
        {
            Type t = typeof(T);//zfr
            object v = t.GetProperty(propertyName).GetValue(instance, null);
            //ExecuteNonQuery("Update " + GetTableName() + " Set " + GetColumnName(propertyName) + "=" + v + " Where ID=" + "");//
         }
        #endregion
        #endregion

    }
}
