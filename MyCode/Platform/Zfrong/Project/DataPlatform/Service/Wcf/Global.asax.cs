using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
namespace DataPlatform.Wcf
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            //Zfrong.Framework.Utils.Token.MyChannelFactory.GetAgentToken("e_IAgentTokenService");
        }
    }
}