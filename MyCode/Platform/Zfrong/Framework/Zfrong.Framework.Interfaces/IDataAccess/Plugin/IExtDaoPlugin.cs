using NHibernate.Criterion;
using System.Collections.Generic;
using Zfrong.Framework.Core;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.Core.IDao.Plugin
{
    public interface IExtDaoPlugin<T, TId>
    { 
         #region
         void Delete(IList<T> Data);
         void Delete(IList<IDictionary<string, object>> Data);
         void LogicDel(IList<T> Data);
         void LogicDel(IList<IDictionary<string, object>> Data);
         #endregion

         #region
         IList<T> SlicedFindAll(out int totalCount, string start, string limit, string sort, string dir, ExtFilterItem[] filter);
         IList<ExtKVItem<TId>> SlicedFindAllPV(out int totalCount, string start, string limit, string property, ExtFilterItem[] filter);
         IList<ExtKVItem> SlicedFindAllPVDistinct(out int totalCount, string start, string limit, string property, ExtFilterItem[] filter);
     
         #endregion

         #region
         IList<TId> Create(IList<T> Data);
         IList<TId> Create(IList<IDictionary<string, object>> Data);
         void Update(IList<T> Data);
         void Update(IList<IDictionary<string, object>> Data);
         #endregion
    }
    public interface IExtDaoPlugin<T> : IExtDaoPlugin<T, long>
    {

    }
}