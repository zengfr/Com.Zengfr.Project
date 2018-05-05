using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zfrong.Framework.Core.Model;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Utils.Token;
using DataPlatform.Model;
using DataPlatform.Contract.ServiceContract;
namespace DataPlatform.WebSiteB.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.RedirectToAction("In");
        }
         public ActionResult In()
        {
            return View();
        }
         public ActionResult DoIn(bool json)
        {
            HttpContext.Session.Clear();
            string name = Request.Form["username"];
            string pwd = Request.Form["pwd"];
            string PActions = "";
            bool success = false;
            TokenResponse  response=MyChannelFactory.GetToken("e_IUserTokenService",name,pwd);
            if (response.Message == "success")
                success = true;
            if (success)
            {
                PActions =""+response.Token;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, name, DateTime.Now, 
                    DateTime.Now.Add(FormsAuthentication.Timeout),
                    true, PActions);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(ticket));
                Response.Cookies.Add(cookie);
                HttpContext.Session["UserId"] = response.UserID;
            }
            
            if (json)
            {
                ExtResponse<string> obj = new ExtResponse<string>();
                obj.message = "帐号或密码错误！";
                obj.success = success;
                return Json(obj);//.JsonNet();
            }
            else
            {
                if (success)
                    return this.RedirectToAction("PIndex", "Home");
                return this.RedirectToAction("Index");
            }
           
           
        }
         public ActionResult Out()
        {            
            HttpContext.Session.Clear();
            FormsAuthentication.SignOut();
            return this.RedirectToAction("In");
        }
         public ActionResult PIndex()
        { 
            return View(); 
        }
        public ActionResult TreeData(string id)
        {
            return View("menu/TreeData"+id);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult E()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
    }
}
