using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zfrong.Framework.Mvc.Result;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.Mvc.Attributes
{
    /// <summary>
    /// 拦截Action的异常，输出Json给EXT捕获(目前loaddata类操作在JS中暂时没有处理)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExtFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ExtResultAttribute), true);
                if (attrs.Length == 1)
                {
                    string msgTmp;
                    msgTmp = "<b>Message:  </b>{0}</p><b>Action:  </b>{1}</p><b>Type:  </b>{2}";
                    var excResult = new JsonResult();
                    excResult.Data = new ExtResponse
                    { 
                        success = false,
                        message = string.Format(msgTmp, 
                                filterContext.Exception.GetBaseException().Message,
                                filterContext.ActionDescriptor.ActionName,
                                filterContext.Exception.GetBaseException().GetType().ToString())
                    };
                    filterContext.Result = excResult;
                }
                else
                {
                    var excResult = new ContentResult();
                    excResult.Content = String.Format(@"JsHelper.ShowError('<b>异常消息:  </p>{0}</br><b>触发Action:  </p>{1}</br><b>异常类型:  </b>{2}')",
                        filterContext.Exception.GetBaseException().Message,
                        filterContext.ActionDescriptor.ActionName,
                        filterContext.Exception.GetBaseException().GetType().ToString());
                    filterContext.Result = excResult;

                }
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
