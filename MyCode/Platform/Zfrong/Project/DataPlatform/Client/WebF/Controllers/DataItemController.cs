﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zfrong.Framework.Mvc.Controller;
using DataPlatform.Model;
using DataPlatform.Contract.ServiceContract;
using Zfrong.Framework.Utils.Token;
namespace DataPlatform.WebSiteB.Controllers
{
    public class DataItemController : ExtServiceControllerBase<DataItem>
    {
        public DataItemController(string endpointConfigurationName)
         {
             this.ServicePlugin = MyChannelFactory.CreateChannelService<IDataItemExtService>(string.Format("{0}", endpointConfigurationName));
         }
        public ActionResult Index()
        {
            return RedirectToAction("list");
        }
        public ActionResult List()
        {
            return View();
        }
    }
}
