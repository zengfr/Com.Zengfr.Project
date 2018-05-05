using System;
using System.Web.Mvc;

namespace Com.Zengfr.Proj.Common.Web
{
    public class MyControllerFactory : DefaultControllerFactory
    {
        protected static string controllersNamespace = "";
        static MyControllerFactory()
        {
            var key = "controllersNamespace";
            var namespaceString = AppSettingsUtils.Get(key);
            if (!string.IsNullOrWhiteSpace(namespaceString))
            {
                controllersNamespace = namespaceString;
            }
        }
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            if (!string.IsNullOrWhiteSpace(controllerName) && controllerName.LastIndexOf(".") <= 0)
            {
                string controllerType = string.Empty;
                controllerType = string.Format("{0}.{1}Controller", controllersNamespace, controllerName);
                var type = Type.GetType(controllerType);
                if (type != null)
                {
                    IController controller = Activator.CreateInstance(Type.GetType(controllerType)) as IController;
                    return controller;
                }
                return base.CreateController(requestContext, controllerName);
            }
            else
                return null;
            return base.CreateController(requestContext, controllerName);

        }

    }
}
