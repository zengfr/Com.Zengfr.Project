using System;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Security.Principal;
using System.Collections.Specialized;
using System.Reflection;

namespace Com.Zengfr.Proj.Common.Web
{
    public class FormsAuthenticationUtils
    {
        public static CookiesData CurrentCookies
        {
            get
            {
                CookiesData cookiesData;
                IsAuthenticated(out cookiesData);
                return cookiesData;
            }
        }
        public static void OnAuthorization(AuthorizationContext filterContext)
        {
            var isAuthenticated = false;
            if (filterContext != null)
            {
                CookiesData cookiesData = FormsAuthenticationUtils.CurrentCookies;
                if (cookiesData != null && cookiesData.IsAuthenticated)
                {
                    isAuthenticated = cookiesData.IsAuthenticated;
                    filterContext.HttpContext.User = new GenericPrincipal(filterContext.HttpContext.User.Identity, cookiesData.UserRoles);
                }
            }
#if DEBUG
            //isAuthenticated = true;
#endif
            if (!isAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Home/SignOut");
                return;
            }

            //base.OnAuthorization(filterContext);
        }
        protected static void IsAuthenticated(out CookiesData cookiesData)
        {
            cookiesData = new CookiesData();
            cookiesData.IsAuthenticated = false;

            try
            {

                var token = GetRequestHeadersToken();
                FormsAuthenticationTicket ticket = null;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    ticket = FormsAuthentication.Decrypt(token);
                }
                else
                {
                    var userName = HttpContext.Current.User.Identity.Name;
                    var isAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
                    if (!string.IsNullOrEmpty(userName) && isAuthenticated)
                    {
                        var cookieTokenValue = GetRequestCookiesToken();
                        if (!string.IsNullOrWhiteSpace(cookieTokenValue))
                        {
                            ticket = FormsAuthentication.Decrypt(cookieTokenValue);
                        }
                    }
                }
                if (ticket != null && !ticket.Expired)
                {
                    cookiesData = CookiesData.GetUserData(ticket.UserData);
                    cookiesData.IsAuthenticated = true;

                }
            }
            catch (Exception ex)
            {
                cookiesData.ExceptionMessage = ex.Message + ex.StackTrace;
            }
        }
        public static string GetRequestHeadersToken()
        {
            return HttpContext.Current.Request.Headers["TOKEN"];
        }
        public static string GetRequestCookiesToken()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                return cookie.Value;
            }
            return string.Empty;
        }
        public static void SetRequestHeaders(HttpRequestBase request, string name, string value)
        {
            NameValueCollection headers = request.Headers;
            //get a type
            Type t = headers.GetType();
            //get the property
            PropertyInfo p = t.GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            //unset readonly
            p.SetValue(headers, false, null);

            t.InvokeMember("BaseAdd", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, headers, new object[] { name, value });

        }
        public static void SignIn(ICookiesData cookiesData)
        {
            if (cookiesData != null)
            {
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptCookiesValue(cookiesData));
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

        }
        public static void SignOut()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }
        public static string Md5(string data)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data, "MD5");
        }
        public static string EncryptCookiesValue(ICookiesData cookiesData)
        {
            if (cookiesData != null)
            {
                var userData = cookiesData.ToUserData();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                        (1, cookiesData.CookiesName, DateTime.Now, DateTime.Now.AddMinutes(cookiesData.ExpiredMinutes),
                 true, userData, "/");
                return FormsAuthentication.Encrypt(ticket);
            }
            return string.Empty;
        }
        public static CookiesData DecryptCookiesValue(string cookiesValue)
        {
            var cookiesData = new CookiesData();
            try
            {
                var ticket = FormsAuthentication.Decrypt(cookiesValue);
                if (ticket != null && !ticket.Expired)
                {
                    cookiesData = CookiesData.GetUserData(ticket.UserData);
                }
            }
            catch (Exception ex)
            {
                cookiesData.ExceptionMessage = ex.Message + ex.StackTrace;
            }
            return cookiesData;
        }
    }
}
