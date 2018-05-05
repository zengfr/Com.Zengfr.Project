using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace Com.Zengfr.Proj.Common.Web
{
    public class AccountAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
#if DEBUG
          //  return;
#endif
            //filterContext.RequestContext.HttpContext.Response.Expires = -1;
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-origin", "http://localhost:55815");
            FormsAuthenticationUtils.OnAuthorization(filterContext);

            if (!string.IsNullOrWhiteSpace(this.Roles))
            {
                var isInRoles = FormsAuthenticationUtils.CurrentCookies.IsInRoles(this.Roles);

                if (!isInRoles)
                {
                    filterContext.Result = new RedirectResult("/Home/SignOut");
                    return;
                }
            }
            base.OnAuthorization(filterContext);
        }

    }
}
