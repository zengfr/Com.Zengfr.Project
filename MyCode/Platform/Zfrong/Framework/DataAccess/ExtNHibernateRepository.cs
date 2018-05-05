using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate;
using System.Reflection;
using System.ComponentModel;
using Zfrong.Framework.Core.IModel;
using Zfrong.Framework.Core.IRepository;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Utils;
using Zfrong.Framework.Repository.Utils;
namespace Zfrong.Framework.Repository
{
    public partial class ExtNHibernateRepository<T> : NHibernateRepository<T>, IExtNHibernateRepository<T>
    where T : class,IModel<long>, new()
    {
         #region
        public virtual IList<T> SlicedFindAll(out int totalCount, string start, string limit, string sort, string dir, ExtFilterItem[] filter)
        {
            int first, maxResult; totalCount = 0;
            int.TryParse(start, out first); int.TryParse(limit, out maxResult);
            if (sort == null || sort == "") sort = "ID";
           Order[] orders;
            switch (dir)
            {
                case "ASC": orders = new Order[] { Order.Asc(sort) }; break;
                default: orders = new Order[] { Order.Desc(sort) }; break;
            }
            ICriterion[] criterionList =  ExtFilterItemHelper.ToCriterion(filter);
            totalCount = this.Count(criterionList);
            IList<T> objs = this.SlicedFindAll(first, maxResult, orders, criterionList);
           return objs;
        }
        public virtual IList<long> Create(IList<T> Data)
        {
            IList<long> objs=new List<long>();
            if (Data != null)
            {
                foreach (T obj in Data)
                {
                   objs.Add(this.Save(obj));
                }
            }
            return objs;
        }
        public virtual IList<long> Create(IList<IDictionary<string, object>> Data) 
        {
            IList<long> objs = new List<long>();
            if (Data != null)
            {
                T t ;//= default(T);
                foreach (IDictionary<string, object> obj in Data)
                {
                    t = new T();// default(T);
                    MyObjectUtils.BindDictionaryProperties(t, obj);
                    objs.Add(this.Save(t));
                }
            }

            return objs;
        }
        public virtual void Update(IList<T> Data) 
        {
            long id;
            if (Data != null)
            {
                T tobj;
                foreach (T sobj in Data)
                {
                    long.TryParse(sobj.ID.ToString(), out id);
                     if (id > 0)
                     {
                         tobj = Get(id); 
                         //清除缓存 持久对象变成脱管对象 ，更新时和缓存对比不一致会报错
                         EvictALL(tobj);
                         if (tobj != null)
                         {
                            MyObjectUtils.BindProperties<T>(tobj, sobj);
                             Update(tobj);
                         }
                     }
                }
            }
        }
        public virtual void Update(IList<IDictionary<string, object>> Data)  
        {
            long id;
            if (Data != null)
            {
                T t = default(T);
                foreach (IDictionary<string, object> obj in Data)
                {
                    long.TryParse(obj["ID"].ToString(), out id);
                    if (id > 0)
                    {
                        t = Get(id); 
                        //清除缓存 持久对象变成脱管对象 ，更新时和缓存对比不一致会报错
                        EvictALL(t);
                        if (t != null)
                        {
                            MyObjectUtils.BindDictionaryProperties(t, obj);
                            Update(t);
                        }
                    }
                }
            }
        }
        public virtual void Delete(IList<T> Data)
        {
            List<long> ids = new List<long>(); 
            if (Data != null)
            {
                foreach (T obj in Data)
                {
                    ids.Add(obj.ID);
                }
            }
            Delete(ids);
        }
        public virtual void Delete(IList<IDictionary<string, object>> Data) 
        {
            List<long> ids = new List<long>(); long id;
            if (Data != null)
            {
                foreach (IDictionary<string, object> obj in Data)
                {
                    if (obj.ContainsKey("ID") && obj["ID"] != null)
                    {
                        long.TryParse(obj["ID"].ToString(), out id);
                        ids.Add(id);
                    }
                }
            }
            Delete(ids);
        }
        public virtual void LogicDel(IList<T> Data) 
        {
            List<long> ids = new List<long>();
            if (Data != null)
            {
                foreach (T obj in Data)
                {
                    ids.Add(obj.ID);
                }
            }
            if (ids.Count > 0)
            {
                IQuery query = this.CurrentSession.CreateQuery(
                    "Update " + typeof(T).Name + " obj set obj.IsDelete=True Where ID in (" + string.Join(",", ids) + ")");
                query.ExecuteUpdate(); 
            }
            
        }
        public virtual void LogicDel(IList<IDictionary<string, object>> Data) 
        {
            List<long> ids = new List<long>(); long id;
            if (Data != null)
            {
                foreach (IDictionary<string, object> obj in Data)
                {
                    if (obj.ContainsKey("ID")&&obj["ID"]!=null)
                    {
                        long.TryParse(obj["ID"].ToString(), out id);
                        ids.Add(id);
                    }
                }
            }
            if (ids.Count > 0)
            {
                IQuery query = this.CurrentSession.CreateQuery(
                    "Update " + typeof(T).Name + " obj set obj.IsDelete=True Where ID in (" + string.Join(",", ids) + ")");
                query.ExecuteUpdate(); 
            }
           
        }
        /// <summary>
        /// 获取列表(列:某属性的值,ID)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual IList<ExtKVItem<long>> SlicedFindAllPV(out int totalCount, string start, string limit, string property, ExtFilterItem[] filter) 
        {
            int first, maxResult; totalCount = 0;
            int.TryParse(start, out first); int.TryParse(limit, out maxResult);
            IList<ExtKVItem<long>> objs = null;
            ICriteria criteria;
            if (!string.IsNullOrEmpty(property))
            {
                ICriterion[] criterionList = ExtFilterItemHelper.ToCriterion(filter);

                totalCount = this.Count(criterionList);

                criteria = this.CurrentSession.CreateCriteria(typeof(T));
                AddCriterionToCriteria(criteria,criterionList);
                criteria.SetProjection(
                    Projections.ProjectionList().Add(Projections.Property(property),"Key")
                    .Add(Projections.Property("ID"),"Value"));
                criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(ExtKVItem<long>)));
                criteria.SetFirstResult(first).SetMaxResults(maxResult); 
                //criteria.SetCacheable(true);
                objs = criteria.List<ExtKVItem<long>>();
            }
           return objs;
        }
        /// <summary>
        /// 获取不可重复列表(列:某属性的值)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual IList<ExtKVItem> SlicedFindAllPVDistinct(out int totalCount, string start, string limit, string property, ExtFilterItem[] filter)
        {
            int first, maxResult; totalCount = 0;
            int.TryParse(start, out first); int.TryParse(limit, out maxResult);
            IList<string> objs = null;
            ICriteria criteria;
            if (!string.IsNullOrEmpty(property))
            {
                ICriterion[] criterionList = ExtFilterItemHelper.ToCriterion(filter);

                totalCount = this.Count(criterionList);

                criteria = this.CurrentSession.CreateCriteria(typeof(T));
                AddCriterionToCriteria(criteria, criterionList);
                criteria.SetProjection(Projections.Distinct(Projections.Property(property)));
                criteria.SetFirstResult(first).SetMaxResults(maxResult); //criteria.SetCacheable(true);
                objs = criteria.List<string>();
                //session.Close();
            }
              IList<ExtKVItem>  items = new List<ExtKVItem>();
            if (objs != null)
                foreach (string obj in objs)
                {
                    if (obj == null || obj.Length == 0) continue;
                    items.Add(new ExtKVItem(obj, obj));
                }
            return items;
        }
        #endregion
    }
    
}
