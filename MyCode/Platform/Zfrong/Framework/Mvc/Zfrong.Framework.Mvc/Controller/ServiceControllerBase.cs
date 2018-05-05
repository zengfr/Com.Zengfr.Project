using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Zfrong.Framework.Mvc.Result;
using Zfrong.Framework.Mvc.Extensions;
using Zfrong.Framework.Mvc.Attributes;
using Zfrong.Framework.Mvc.Binder;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.Model;
using Zfrong.Framework.Core.IService.Plugin;
using System.Reflection;
using System.ComponentModel;
using NHibernate.Criterion;
using Zfrong.Framework.Utils.Token;
namespace Zfrong.Framework.Mvc.Controller
{
    public abstract class ServiceControllerBase<T> : MvcControllerBase
       
    {
        protected IServicePlugin<T> ServicePlugin { get; set; }
        public ServiceControllerBase()
        {

        }
        public ServiceControllerBase(string endpointConfigurationName)
        {
            this.ServicePlugin = MyChannelFactory.CreateChannelService<IServicePlugin<T>>("" + endpointConfigurationName);
        }
        public virtual JsonNetResult SlicedFindAll(int start, int limit, Order[] orders, ICriterion[] criterion)
        {

            ExtPagingResponse<T> objs = new ExtPagingResponse<T>();
             objs.items=this.ServicePlugin.SlicedFindAll(start, limit, orders, criterion);
             objs.success = true;
             objs.totalCount = 999;
            return Json(objs).JsonNet();
        }
        [HttpPost]
        public virtual JsonResult Create([ModelBinder(typeof(JsonBinder))] T Data)
        {
            ExtPagingResponse<T> objs = null;
                this.ServicePlugin.Save(Data);
            return Json(objs);
        }
        [HttpPost]
        public virtual JsonResult Update([ModelBinder(typeof(JsonBinder))] T Data)
        {
            ExtPagingResponse<T> objs = null;
                this.ServicePlugin.Update(Data);
            return Json(objs);
        }
        

        [HttpPost]
        public virtual JsonResult SaveOrUpdate([ModelBinder(typeof(JsonBinder))] T Data)
        {
            ExtPagingResponse<T> objs = null;
                this.ServicePlugin.SaveOrUpdate(Data);
            return Json(objs);
        }
        [HttpPost]
        public virtual JsonResult Delete([ModelBinder(typeof(JsonBinder))] T Data)
        {
            ExtPagingResponse<T> objs = null;
            this.ServicePlugin.Delete(Data);
            return Json(objs);
        }
        [HttpPost]
        public virtual JsonResult Get([ModelBinder(typeof(JsonBinder))] long id)
        {
            ExtPagingResponse<T> objs = null;
                this.ServicePlugin.Get(id);
            return Json(objs);
        }
       
    }
}