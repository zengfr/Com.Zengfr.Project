using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Core.DataContract;
using Spring.Transaction.Interceptor;
namespace Zfrong.Framework.CoreBase.Service
{
    public abstract class ExtServiceBase<T> : IExtServicePlugin<T>
        {
        protected virtual  IExtServicePlugin<T> ServicePlugin { get; set; }
            public ExtServiceBase(IExtServicePlugin<T> plugin)
            {
                this.ServicePlugin = plugin;
            }
         [Transaction(ReadOnly = false)]
            public virtual ExtPagingResponse<T> Delete(ExtRequest<IList<T>> Data)
            {
                return this.ServicePlugin.Delete(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> Delete(ExtRequest<IList<ExtDictionary<string, object>>> Data)
            {
                return this.ServicePlugin.Delete(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> LogicDel(ExtRequest<IList<T>> Data)
            {
                return this.ServicePlugin.LogicDel(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> LogicDel(ExtRequest<IList<ExtDictionary<string, object>>> Data)
            {
                return this.ServicePlugin.LogicDel(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> Create(ExtRequest<IList<T>> Data)
            {
                return this.ServicePlugin.Create(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> Create(ExtRequest<IList<ExtDictionary<string, object>>> Data)
            {
                return this.ServicePlugin.Create(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> Update(ExtRequest<IList<T>> Data)
            {
                return this.ServicePlugin.Update(Data);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> Update(ExtRequest<IList<ExtDictionary<string, object>>> Data)
            {
                return this.ServicePlugin.Update(Data);
            }
        [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<T> SlicedFindAll(string start, string limit, string sort, string dir, ExtFilterItem[] filter)
            {
                return this.ServicePlugin.SlicedFindAll(start, limit, sort, dir, filter);
            }
         [Transaction(ReadOnly = false)]
        public virtual ExtPagingResponse<ExtKVItem<long>> SlicedFindAllPV(string start, string limit, string property, ExtFilterItem[] filter)
            {
                return this.ServicePlugin.SlicedFindAllPV(start, limit, property,filter);
            }
         [Transaction(ReadOnly = false)]
         public virtual ExtPagingResponse<ExtKVItem> SlicedFindAllPVDistinct(string start, string limit, string property, ExtFilterItem[] filter)
            {
                return this.ServicePlugin.SlicedFindAllPVDistinct(start, limit,  property,filter);
            }
        }
}
