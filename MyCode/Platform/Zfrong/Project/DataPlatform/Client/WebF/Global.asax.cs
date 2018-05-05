using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Web.Mvc;
using Zfrong.Framework.Mvc.Binder;
using Zfrong.Framework.Core.DataContract;
namespace System.My.WebSiteB
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication :  SpringMvcApplication

    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Nav", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(ExtFilterItem), new DataFilterItemModelBinder());
            Zfrong.Framework.Utils.Token.MyChannelFactory.GetAgentToken("e_IAgentTokenService");
        }
    }
}