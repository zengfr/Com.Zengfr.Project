using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Zfrong.Framework.Mvc.Filters
{
    public class ActionFilters : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            MyLogManager.InfoFormat("ActionExecuted\tIP:{3}\tUser:{0}\tController:{1}\tAction:{2}\tURL:{4}",
                "",
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName,
                filterContext.RequestContext.HttpContext.Request.UserHostAddress,
                filterContext.RequestContext.HttpContext.Request.RawUrl);

        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MyLogManager.InfoFormat("ActionExecuting\tIP:{3}\tUser:{0}\tController:{1}\tAction:{2}\tURL:{4}",
                  "",
                  filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                  filterContext.ActionDescriptor.ActionName,
                  filterContext.RequestContext.HttpContext.Request.UserHostAddress,
                  filterContext.RequestContext.HttpContext.Request.RawUrl);
        }
    }
}
