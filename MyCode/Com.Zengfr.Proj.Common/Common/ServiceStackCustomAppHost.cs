using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using Services.NHJunk;
using FluentNHibernate.Utils;
using NHibernate;
//using Com.Zengfr.Proj.DataAccess;
//using Com.Zengfr.Proj.IDataAccess;
//using ServiceStack.Host;
//using ServiceStack.Web;
//using ServiceStack.Host.AspNet;
//using Com.Zengfr.Proj.ServiceStack.Services;
using ServiceStack;
using ServiceStack.Common.Web;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
namespace Com.Zengfr.Proj.Common.Web
{
    public class ServiceStackCustomAppHost : AppHostBase//ServiceStackHost 
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ServiceStackCustomAppHost));
        private readonly IContainerAdapter _containerAdapter = null;

        public ServiceStackCustomAppHost(string serivceName, string rootPath, ISessionFactory sessionFactory, Assembly[] daoAssemblies, Assembly[] idaoAssemblies, Assembly[] serivceAssemblies)
            : base(serivceName, serivceAssemblies)
        {
            log4net.Config.XmlConfigurator.Configure();

            base.Container.Register<ISessionFactory>(sessionFactory);

            var interfaces = new List<Type>();
            var interfacesDict = new Dictionary<string, Type>();
            foreach (var idaoAssembly in idaoAssemblies)
            {
                interfaces.AddRange(idaoAssembly.
                    GetTypes().Where(x => x.Name.EndsWith("Dao") && !x.Name.EndsWith("BaseDao")));
            }
            interfacesDict = interfaces.ToDictionary(t => t.Name);

            foreach (var daoAssembly in daoAssemblies)
            {
                daoAssembly.GetTypes()
    .Where(x => x.Name.EndsWith("Dao") && !x.Name.EndsWith("BaseDao"))
    .Each(x =>
    {
        base.Container.RegisterAutoWiredType(x, interfacesDict["I" + x.Name]);
    });
            }
            EndpointHostConfig endpointHostConfig = new EndpointHostConfig()
            {
                DebugMode = true,  // HandlerFactoryPath = "api",
                //WebHostUrl = "api2",
                ServiceStackHandlerFactoryPath = string.Format("{0}", rootPath),// "api",
                MetadataRedirectPath = string.Format("{0}/metadata", rootPath),// "api/metadata",
                //DebugAspNetHostEnvironment=true,
                // DebugHttpListenerHostEnvironment=true,
                WriteErrorsToResponse = true,
                //EnableFeatures = Features.All,
                DefaultContentType = ContentType.Json,
                ReturnsInnerException=true,

            };
            base.SetConfig(endpointHostConfig);
            LogManager.LogFactory = new DebugLogFactory();

            MetadataFeature metadataFeature = new MetadataFeature();
            RequestLogsFeature requestLogsFeature = new RequestLogsFeature();
            this.LoadPlugin(metadataFeature, requestLogsFeature);

            JsConfig.EmitCamelCaseNames = true;
            JsConfig.IncludeTypeInfo = true;
            JsConfig.ThrowOnDeserializationError = true;
            JsConfig.ConvertObjectTypesIntoStringDictionary = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;

            this.PreRequestFilters.Add((req, res) =>
            {
                // Trace.WriteLine(string.Format("PreRequestFilters:{0},{1},{2}", req.GetRawBody(), string.Empty, string.Empty));
            });
            this.ServiceExceptionHandler = (httpReq, request, exception) =>
            {
                var responseStatus = new ResponseStatus();
                responseStatus.Message = ExceptionUtils.GetDetailMessages(exception);
                responseStatus.StackTrace = ExceptionUtils.GetDetailStackTraces(exception);
                ExceptionUtils.ErrorFormat(logger, exception,string.Format("request:{0}", request.ToXml()));
                return DtoUtils.CreateErrorResponse(request, exception, responseStatus);
            };
        }

        public override void Configure(Funq.Container container)
        {
            container.Adapter = _containerAdapter;
        }

        private static void RegisterAutoWiredType(Assembly Assembly)
        {

        }
    }
}
