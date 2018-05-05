using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zfrong.Framework.Mvc.Result;
using Zfrong.Framework.Mvc.Extensions;
using Zfrong.Framework.Mvc.Attributes;
using Zfrong.Framework.Mvc.Binder;
using Zfrong.Framework.Core.Model;
using Zfrong.Framework.Core.DataContract;
using System.Reflection;
using System.ComponentModel;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Utils.Token;
namespace Zfrong.Framework.Mvc.Controller
{
    public abstract class ExtTreeServiceControllerBase<T> : MvcControllerBase
        
    {
        protected IExtTreeServicePlugin<T> ServicePlugin { get; set; }
        public ExtTreeServiceControllerBase()
        {

        }
        public ExtTreeServiceControllerBase(string endpointConfigurationName)
        {
            this.ServicePlugin = MyChannelFactory.CreateChannelService<IExtTreeServicePlugin<T>>("" + endpointConfigurationName);
        }
        public virtual JsonNetResult NodeFIndAll(string node, string checkedValue)
        {
            IList<T> items = this.ServicePlugin.NodeFindAll(node, checkedValue);

            return Json(items, JsonRequestBehavior.AllowGet).JsonNet();
        }
        [HttpPost]
        public virtual JsonNetResult NodeCreate(long pId, string checkedValue, [ModelBinder(typeof(JsonBinder))] T dataItem)
        {
            ExtResponse<T> Result = this.ServicePlugin.NodeCreate(pId, checkedValue, dataItem);
            return Json(Result).JsonNet();
        }
        [HttpPost]
        public virtual JsonResult NodeRemove(long id)
        {
            ExtResponse<T> Result = this.ServicePlugin.NodeRemove(id);
            return Json(Result);
        }
        [HttpPost]
        public virtual JsonResult NodeUpdate(long id, [ModelBinder(typeof(JsonBinder))] T dataItem)
        {
            ExtResponse<T> Result = this.ServicePlugin.NodeUpdate(id, dataItem);
            return Json(Result);
        }
        [HttpPost]
        public virtual JsonResult NodeMove(long id, long parentId)
        {
            ExtResponse<T> Result = this.ServicePlugin.NodeMove(id, parentId);
              return Json(Result);
        }
       
        
    }
}