using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Zfrong.Framework.Mvc.Filters
{
    public class ExceptionFilters : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            MyLogManager.Log.Error(string.Format( "Exception\tIP:{2}\tUser:{0}\tException:{1}\tURL:{3}",
                "",
                filterContext.Exception.Message,
                filterContext.RequestContext.HttpContext.Request.UserHostAddress,
                filterContext.RequestContext.HttpContext.Request.RawUrl));
            filterContext.ExceptionHandled = true;
        }
    }
}
