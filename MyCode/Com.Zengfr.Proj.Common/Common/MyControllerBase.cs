using System.Web.Mvc;
using System.Web.Security;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using System.Web;
using System.Web.Caching;
using System;
using System.Linq;

namespace Com.Zengfr.Proj.Common.Web
{

    public abstract class MyControllerBase : System.Web.Mvc.Controller
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MyControllerBase));
        protected virtual string getSplitString(string value, int index)
        {
            var values = value.Split(',');
            if (index < values.Length)
                return values[index];
            return value;
        }
        protected virtual AjaxPagedResult<T> BuildAjaxPagedResult<T>(int pageIndex, int totalCount, IEnumerable<T> objs)
        {
            AjaxPagedResult<T> result = new AjaxPagedResult<T>();
            result.page = pageIndex;
            result.total = totalCount;
            result.items =objs.ToArray();
            return result;
        }
        #region cache
        public static T CacheGet<T>(string cacheKey)
        {
            return (T)HttpRuntime.Cache.Get(cacheKey);
        }
        public static void CacheInsert<T>(string cacheKey,T obj, int minutes)
        {
            CacheInsertNoAbsoluteExpiration(cacheKey, obj, minutes);
        }
        public static void CacheInsertNoAbsoluteExpiration(string cacheKey,object obj,int minutes)
        {
            HttpRuntime.Cache.Insert(cacheKey, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes));
        }
        #endregion
        #region client
        protected virtual Resp Post<Req, Resp>(ServiceClientBase client,Req req) where Req : IReturn<Resp>
        {
            return client.Post<Resp>(req);
        }
        protected virtual void Post<Req>(ServiceClientBase client,Req req) where Req : IReturnVoid
        {
            client.Post(req);
        }
        #endregion
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {

            base.OnAuthorization(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            ExceptionUtils.ErrorFormat(logger,filterContext.Exception,string.Format("url:{0}", filterContext.HttpContext.Request.Url));
        }

        public virtual ActionResult Index()
        {
            return List();
        }
        public virtual ActionResult List()
        {
            return View();
        }
        [AccountAuthorize]
        public virtual ActionResult Edit()
        {
            return View();
        }
        [AccountAuthorize]
        public virtual ActionResult Detail()
        {
            return View();
        }
    }
}
