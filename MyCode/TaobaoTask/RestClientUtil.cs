using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp;
using Konsole;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace TaobaoTask
{
    public class RestClientUtil
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(RestClientUtil));
        static DateTime lastTime = DateTime.Now.AddMinutes(-1);
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        public static IRestClient GetRestClient(string baseUrl)
        {
            if (baseUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                 
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
               // request.ProtocolVersion = HttpVersion.Version11;
                // 这里设置了协议类型。
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls1.2; 
                //request.KeepAlive = false;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;
            }
            var restClient = new RestClient(baseUrl);
            restClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
            restClient.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            restClient.AddDefaultHeader("Accept-Language", "en-US,en;q=0.5");
            restClient.AddDefaultHeader("Accept-Encoding", "gzip,deflate,br");
            restClient.AddDefaultHeader("Cache-Control", "max-age=0");
            restClient.AddDefaultHeader("Referer", baseUrl);
            restClient.CookieContainer = new CookieContainer() { MaxCookieSize = 1024 * 1024 };
            restClient.FollowRedirects = true;
            //var timeSpan = (lastTime.AddMilliseconds(511 * 1) - DateTime.Now);
            //if (timeSpan.TotalMilliseconds > 0)
            {
                //System.Threading.Thread.Sleep(timeSpan);
                System.Threading.Thread.Sleep(25);
            }
            lastTime = DateTime.Now;
            return restClient;

        }
        #region
        public static IRestResponse Get(string baseUrl, string url, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl);
            var request = new RestRequest(url, Method.GET);
            return Execute(client, request, console);
        }
        public static IRestResponse Post(string baseUrl, string url, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl);
            var request = new RestRequest(url, Method.POST);
            return Execute(client, request, console);
        }
        public static IRestResponse Post<T>(string baseUrl, string url, T body, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl);
            var request = new RestRequest(url, Method.POST);
            
            request.AddBody(body);
            return Execute(client, request, console);
        }
        #endregion
        #region
        public static IRestResponse Get(IRestClient client, string url, IWrite console = null)
        {
            var request = new RestRequest(url, Method.GET);
            return Execute(client, request, console);
        }
        public static IRestResponse Post(IRestClient client, string url, IWrite console = null)
        {
            var request = new RestRequest(url, Method.POST);
            return Execute(client, request, console);
        }
        public static IRestResponse Post<T>(IRestClient client, string url, T body, IWrite console = null)
        {
            var request = new RestRequest(url, Method.POST);
            request.AddBody(body);
            return Execute(client, request, console);
        }
        #endregion
        public static void SetCookies(IRestClient client, string cookieString)
        {
            SetCookies(client, GetCookieDictionary(cookieString));
        }
        public static void SetCookies(IRestClient client, IDictionary<string, string> cookies)
        {
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                    client.CookieContainer.Add(client.BaseUrl, new Cookie(cookie.Key, cookie.Value.Replace(",", "%2C"), "/"));
            }

        }
        private static IDictionary<string, string> GetCookieDictionary(string cookieString)
        {
            var cookieDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var values = cookieString.TrimEnd(';').Split(';');
            foreach (var parts in values.Select(c => c.Split(new[] { '=' }, 2)))
            {
                var cookieName = parts[0].Trim();
                if (!string.IsNullOrWhiteSpace(cookieName))
                {
                    string cookieValue;
                    if (parts.Length == 1)
                    {
                        cookieValue = string.Empty;
                    }
                    else
                    {
                        cookieValue = parts[1];
                    }
                    cookieDictionary[cookieName] = cookieValue;
                }
            }

            return cookieDictionary;
        }
        public static IRestResponse Execute(IRestClient client, IRestRequest request, IWrite console = null)
        {
            var log = string.Format("Start {0},{1}", request.Method, Regex.Replace(request.Resource, @"(\.json|\.xml|\.do|\.action|\.aspx|\.jsp)", ""));
            if (console != null)
            {
                console.WriteLine(log);
            }
            var response = client.Execute(request);
            log = string.Format("  End {0},{1},{2},{3},{4},{5},{6}", request.Method, response.StatusCode, response.ContentLength, response.Content.Length, response.StatusDescription, response.ContentEncoding,Regex.Replace(request.Resource, @"(\.json|\.xml|\.do|\.action|\.aspx|\.jsp)", ""));
            if (console != null)
            {
                console.WriteLine(log);
            }
            else
            {
               //logger.InfoFormat(log);
            }
            return response;
        }
        public static RegexOptions RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;

        public static string RegexGetString(string defaultV, string source, string pattern, int index)
        {
            var m = Regex.Match(source, pattern, RegexOptions);
            if (m.Success)
            {
                return m.Groups[index].Value;
            }
            return defaultV;
        }
        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        public static string FiterChars(string source) {
            return Regex.Replace(source, "[^\u4E00-\u9FFF_A-Za-z0-9,;\\.；。，“：]", "",RegexOptions);
        }
        static Random random = new Random();
        public static string GetRandomStr(int len)
        {
            StringBuilder sb = new StringBuilder();
            if (len > 0)
            {
                
                if (len > 9)
                {
                    sb.Append(GetRandomStr(9));
                    len -= 9;
                }
                if (len <= 9)
                {
                    int a = (int)Math.Pow(10, len-1);
                    int b = (int)Math.Pow(10, len)-1;
                    sb.Append(random.Next(a, b));
                }
               
            }
            return sb.ToString();
        }
        public static string BuildLog<T>(T item) {
            StringBuilder sb = new StringBuilder();
            var fl = new TaobaoTask.KC.FieldReader(item).ReadFieldList();
            if (fl.Fields.Length > 0)
            {
                foreach (var f in fl.Fields)
                {
                    if (f.Value != null && !string.IsNullOrWhiteSpace(f.Value.ToString()) && f.Value.ToString().Length<20)
                    {
                        var text = string.Format(";{0}:{1}", f.Caption, f.Value);
                        sb.Append(text);
                    }
                }
                return sb.ToString().Substring(1);
            }
            return string.Empty;
        }
        public static JsonSerializerSettings GetJsonSerializerSettings() {
            var setting = new JsonSerializerSettings();
            return setting;
        }
    }
}