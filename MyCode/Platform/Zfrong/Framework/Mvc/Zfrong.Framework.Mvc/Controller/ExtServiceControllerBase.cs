using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zfrong.Framework.Mvc.Result;
using Zfrong.Framework.Mvc.Extensions;
using Zfrong.Framework.Mvc.Attributes;
using Zfrong.Framework.Mvc.Binder;
using Zfrong.Framework.Core.DataContract;
using System.Reflection;
using System.ComponentModel;
using Zfrong.Framework.Core.Model;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Utils.Token;
namespace Zfrong.Framework.Mvc.Controller
{
    public abstract class ExtServiceControllerBase<T> : MvcControllerBase
    {
        protected IExtServicePlugin<T> ServicePlugin { get; set; }
        public ExtServiceControllerBase()
        {

        }
         public ExtServiceControllerBase(string endpointConfigurationName)
        {
            this.ServicePlugin = MyChannelFactory.CreateChannelService<IExtServicePlugin<T>>("" + endpointConfigurationName);
        }
         public virtual JsonNetResult SlicedFindAll(string start, string limit, string sort, string dir, ExtFilterItem[] filter)
        {

            ExtPagingResponse<T> objs = this.ServicePlugin.SlicedFindAll(start, limit, sort, dir, filter);

            return Json(objs).JsonNet();
        }
        [HttpPost]
         public virtual JsonResult Create([ModelBinder(typeof(JsonBinder))]ExtRequest<IList<ExtDictionary<string, object>>> Data)
        {
            ExtPagingResponse<T> objs = this.ServicePlugin.Create(Data);
            return Json(objs);
        }
        [HttpPost]
        public virtual JsonResult Update([ModelBinder(typeof(JsonBinder))] ExtRequest<IList<ExtDictionary<string, object>>> Data)
        {
            ExtPagingResponse<T> objs = this.ServicePlugin.Update(Data);
            return Json(objs);
        }
        [HttpPost]
       public virtual JsonResult Delete([ModelBinder(typeof(JsonBinder))] ExtRequest<IList<ExtDictionary<string, object>>> Data)
        {
            ExtPagingResponse<T> objs = this.ServicePlugin.Delete(Data);
            return Json(objs);
        }
        [HttpPost]
        public virtual JsonResult LogicDel([ModelBinder(typeof(JsonBinder))] ExtRequest<IList<ExtDictionary<string, object>>> Data)
        {
            ExtPagingResponse<T> objs = this.ServicePlugin.LogicDel(Data);
            return Json(objs);
        }
        public virtual JsonResult SlicedFindAllPVDistinct(string start, string limit, string property, ExtFilterItem[] filter)
        {
            ExtPagingResponse<ExtKVItem> objs = this.ServicePlugin.SlicedFindAllPVDistinct(start, limit, property,filter);
            return Json(objs, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult SlicedFindAllPV(string start, string limit, string property,ExtFilterItem[] filter)
        {
            ExtPagingResponse<ExtKVItem<long>> objs = this.ServicePlugin.SlicedFindAllPV(start, limit, property,filter);
            return Json(objs, JsonRequestBehavior.AllowGet).JsonHTML();
        }
    }
}