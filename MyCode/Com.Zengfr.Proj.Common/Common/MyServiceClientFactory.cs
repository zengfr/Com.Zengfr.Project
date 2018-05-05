using System.Collections.Generic;
using ServiceStack.ServiceClient.Web;

namespace Com.Zengfr.Proj.Common
{
    public class MyServiceClientFactory
    {
        protected static Dictionary<string, ServiceClientBase> clients = new Dictionary<string, ServiceClientBase>();
        private static object lockObj = new object();

        public static string serviceUrl1 = string.Empty;

        static MyServiceClientFactory()
        {
            var url = AppSettingsUtils.Get("serviceUrl");
            if (!string.IsNullOrWhiteSpace(url))
            {
                serviceUrl1 = url;
            }
        }
        public static ServiceClientBase CreateServiceClient(string type)
        {
            return CreateServiceClient(type, serviceUrl1);
        }
        public static ServiceClientBase CreateServiceClient(string type, string url)
        {
            ServiceClientBase client = null;
            var key = string.Format("{0},{1}", type, url);
            if (clients.ContainsKey(key))
            {
                client = clients[key];
            }
            if (client == null && !string.IsNullOrWhiteSpace(type))
            {
                type = type.ToLower();
                switch (type)
                {
                    case "xml":
                        client = new XmlServiceClient(url);
                        break;
                    case "json":
                        client = new JsonServiceClient(url);
                        break;
                }
                if (!clients.ContainsKey(key))
                {
                    lock (lockObj)
                    {
                        if (!clients.ContainsKey(key))
                        {
                            client.Proxy = null;
                            clients.Add(key, client);
                        }
                    }
                }
            }
            return client;
        }
    }
}
