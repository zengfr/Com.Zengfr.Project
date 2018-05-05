using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.Core;
using System.Reflection;
using System.ComponentModel;
using Zfrong.Framework.Core.IBussiness;
using Zfrong.Framework.Core.IModel;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.IService;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Bussiness
{
    public partial class ExtBussiness<T> : IExtBussiness<T> where T : class
    {
        public virtual IExtNHibernateRepository<T, long> CurrentRepository { get; set; }
         #region
        public virtual ExtPagingResponse<T> SlicedFindAll(string start, string limit, string sort, string dir, ExtFilterItem[] filter)
        {
            int totalCount;

            IList<T> objs = this.CurrentRepository.SlicedFindAll(out totalCount,start,limit,  sort,dir,  filter);

            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.totalCount = totalCount;
            response.items = objs;
            response.success = true;
            response.message = "SlicedFindAll";

            return response;
        }
        public virtual ExtPagingResponse<T> Create(ExtRequest<IList<T>> Data)
        {

            this.CurrentRepository.Create(Data.data);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "Create";
            return response;
        }
        public virtual ExtPagingResponse<T> Create(ExtRequest<IList<ExtDictionary<string, object>>> Data) 
        {
            this.CurrentRepository.Create(Data.data as IList<IDictionary<string, object>>);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "Create";
            return response;
        }
        public virtual ExtPagingResponse<T> Update(ExtRequest<IList<T>> Data) 
        {
            this.CurrentRepository.Update(Data.data);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "Update";
            return response;
        }
        public virtual ExtPagingResponse<T> Update(ExtRequest<IList<ExtDictionary<string, object>>> Data)  
        {
            this.CurrentRepository.Update(Data.data as IList<IDictionary<string, object>>);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "Update";
            return response;
        }
        public virtual ExtPagingResponse<T> Delete(ExtRequest<IList<T>> Data)
        {
            this.CurrentRepository.Delete(Data.data);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "Delete";
            return response;
        }
        public virtual ExtPagingResponse<T> Delete(ExtRequest<IList<ExtDictionary<string, object>>> Data) 
        {
            this.CurrentRepository.Delete(Data.data as IList<IDictionary<string, object>>);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "Delete";
            return response;
        }
        public virtual ExtPagingResponse<T> LogicDel(ExtRequest<IList<T>> Data) 
        {
            this.CurrentRepository.LogicDel(Data.data);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "LogicDel";
            return response;
        }
        public virtual ExtPagingResponse<T> LogicDel(ExtRequest<IList<ExtDictionary<string, object>>> Data) 
        {
            this.CurrentRepository.LogicDel(Data.data as IList<IDictionary<string, object>>);
            ExtPagingResponse<T> response = new ExtPagingResponse<T>();
            response.success = true;
            response.message = "LogicDel";
            return response;
        }
        /// <summary>
        /// 获取列表(列:某属性的值,ID)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual ExtPagingResponse<ExtKVItem<long>> SlicedFindAllPV(string start, string limit, string property, ExtFilterItem[] filter) 
        {
            int totalCount;
            IList<ExtKVItem<long>> objs = this.CurrentRepository.SlicedFindAllPV(out totalCount, start, limit, property,filter);
            ExtPagingResponse<ExtKVItem<long>> response = new ExtPagingResponse<ExtKVItem<long>>();
            response.success = true;
            response.message = "SlicedFindAllPV";
            response.items = objs;
            response.totalCount = totalCount;
            return response;
        }
        /// <summary>
        /// 获取不可重复列表(列:某属性的值)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual ExtPagingResponse<ExtKVItem> SlicedFindAllPVDistinct(string start, string limit, string property, ExtFilterItem[] filter)
        {
            int totalCount;
            IList<ExtKVItem> objs = this.CurrentRepository.SlicedFindAllPVDistinct(out totalCount, start, limit, property,filter);
            ExtPagingResponse<ExtKVItem> response = new ExtPagingResponse<ExtKVItem>();
            response.success = true;
            response.message = "SlicedFindAllPVDistinct";
            response.items = objs;
            response.totalCount = totalCount;
            return response;
        }
        #endregion
    }
    
}
