using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IDao.Plugin;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.CoreBase.Dao
{
    public abstract class ExtDaoBase<T> : IExtDaoPlugin<T>
        {
            protected virtual IExtDaoPlugin<T> DaoPlugin { get; set; }
            public ExtDaoBase(IExtDaoPlugin<T> plugin)
            {
                this.DaoPlugin = plugin;
            }



            public virtual void Delete(IList<T> Data)
            {
                this.DaoPlugin.Delete(Data);
            }

            public virtual void Delete(IList<IDictionary<string, object>> Data)
            {
                this.DaoPlugin.Delete(Data);
            }

            public virtual void LogicDel(IList<T> Data)
            {
                this.DaoPlugin.LogicDel(Data);
            }

            public virtual void LogicDel(IList<IDictionary<string, object>> Data)
            {
                this.DaoPlugin.LogicDel(Data);
            }


            public virtual IList<long> Create(IList<T> Data)
            {
               return   this.DaoPlugin.Create(Data);
            }

            public virtual IList<long> Create(IList<IDictionary<string, object>> Data)
            {
               return   this.DaoPlugin.Create(Data);
            }

            public virtual void Update(IList<T> Data)
            {
                 this.DaoPlugin.Update(Data);
            }

            public virtual void Update(IList<IDictionary<string, object>> Data)
            {
                 this.DaoPlugin.Update(Data);
            }


            public virtual IList<T> SlicedFindAll(out int totalCount, string start, string limit, string sort, string dir, ExtFilterItem[] filter)
            {
                return this.DaoPlugin.SlicedFindAll(out totalCount, start, limit, sort, dir, filter);
            }

            public virtual IList<ExtKVItem<long>> SlicedFindAllPV(out int totalCount, string start, string limit, string property, ExtFilterItem[] filter)
            {
                return this.DaoPlugin.SlicedFindAllPV(out totalCount, start, limit, property,filter);
            }

            public virtual IList<ExtKVItem> SlicedFindAllPVDistinct(out int totalCount, string start, string limit, string property, ExtFilterItem[] filter)
            {
                return this.DaoPlugin.SlicedFindAllPVDistinct(out totalCount, start, limit, property,filter);
            }
        }
}
