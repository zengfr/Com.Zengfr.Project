using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zfrong.Framework.Mvc.Controller;
using DataPlatform.Model;
using DataPlatform.Contract.ServiceContract;
using  Zfrong.Framework.Utils;
using Zfrong.Framework.Utils.Token;
using Zfrong.Framework.Mvc.Result;
using Zfrong.Framework.Core.DataContract;
namespace DataPlatform.WebSiteB.Controllers
{
    public class CompanyGroupController : ExtServiceControllerBase<CompanyGroup>
    {
        public CompanyGroupController(string endpointConfigurationName)
         {
             this.ServicePlugin = MyChannelFactory.CreateChannelService<ICompanyGroupExtService>(string.Format("{0}", endpointConfigurationName));
         }
        public ActionResult Index()
        {
            return RedirectToAction("list");
        }
        public ActionResult List()
        {
            return View();
        }
        public override JsonNetResult SlicedFindAll(string start, string limit, string sort, string dir, ExtFilterItem[] filter)
        {
            List<ExtFilterItem> items=new  List<ExtFilterItem>();
            MyObjectUtils.QueryStringToFilter<bool>(items, "Adversary");
            if (filter != null) items.AddRange(filter); 
            filter = items.ToArray();
            return base.SlicedFindAll(start, limit, sort, dir, filter);
        }
        public override JsonResult SlicedFindAllPV(string start, string limit, string property, ExtFilterItem[] filter)
        {
            List<ExtFilterItem> items = new List<ExtFilterItem>();
             MyObjectUtils.QueryStringToFilter<bool>(items,"Adversary");
             if (filter != null) items.AddRange(filter);
             filter = items.ToArray();
            return base.SlicedFindAllPV(start, limit, property, filter);
        }
        public override JsonResult SlicedFindAllPVDistinct(string start, string limit, string property, ExtFilterItem[] filter)
        {
            List<ExtFilterItem> items = new List<ExtFilterItem>();
             MyObjectUtils.QueryStringToFilter<bool>(items,"Adversary");
             if (filter != null) items.AddRange(filter);
             filter = items.ToArray();
            return base.SlicedFindAllPVDistinct(start, limit, property, filter);
        }
        
    }
}
