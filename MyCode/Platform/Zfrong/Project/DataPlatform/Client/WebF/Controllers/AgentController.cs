using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataPlatform.Model;
using DataPlatform.Contract.ServiceContract;
//using NHibernate.Criterion;
//using NHibernate;
//using Newtonsoft.Json;
using System.Reflection;
using System.ComponentModel;
using Zfrong.Framework.Mvc.Attributes;
using Zfrong.Framework.Mvc.Controller;
using Zfrong.Framework.Mvc.Result;
using Zfrong.Framework.Mvc.Extensions;
using Zfrong.Framework.CoreBase.Model;
using Zfrong.Framework.CoreBase.Service;
using Zfrong.Framework.Utils.Token;
namespace DataPlatform.WebSiteB.Controllers
{
    public class AgentController :ExtServiceControllerBase<Agent>
    {
        public ActionResult test()
        {
            return View();
        }
        public AgentController(string endpointConfigurationName)
         {
            // this.ServicePlugin = MyChannelFactory.CreateChannelService<IAgentExtService>("" + endpointConfigurationName);
     
        }
        public ActionResult Index()
        {
            return this.RedirectToAction("List");
        }
         public ActionResult List()
        {
            return View();
        }
        
        public ActionResult Role()
        {
            return View("User_Role");
        }
        public JsonNetResult getRolesId(int userId)
        {
            ArrayList objs = new ArrayList();
            return Json(objs, JsonRequestBehavior.AllowGet).JsonNet();
        }
        public JsonNetResult getMenusId(int userId)
        {
            var objs = new ArrayList();
            return Json(objs, JsonRequestBehavior.AllowGet).JsonNet();
        }
        public JsonNetResult getActionsId(int userId)
        {
            var objs = new ArrayList();
           
            return Json(objs, JsonRequestBehavior.AllowGet).JsonNet();
        }
         public JsonNetResult UpdateUserRoles(int userId, int[] rolesId)
        {
            User user = null;
            return Json(user, JsonRequestBehavior.AllowGet).JsonNet();
        }
    }
}
