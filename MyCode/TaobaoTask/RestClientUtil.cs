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
        static IDictionary<string, IRestClient> clients = new Dictionary<string, IRestClient>();
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        public static IRestClient GetRestClient(string baseUrl, bool useClientCache = false)
        {
            return GetRestClient(baseUrl, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9", useClientCache);
        }
        public static IRestClient GetRestClient(string baseUrl, string accept, bool useClientCache = false)
        {
            IRestClient client = null;

            if (useClientCache)
            {
                var key = string.Format("{0},{1}", baseUrl, accept);
                if (!clients.ContainsKey(key))
                {
                    lock (clients)
                    {
                        if (!clients.ContainsKey(key))
                        {
                            client = CreateRestClient(baseUrl, accept);
                            clients[key] = client;
                        }
                    }
                }
                client = clients[key];
            }
            else
            {
                client = CreateRestClient(baseUrl, accept);
            }
            return client;
        }
        protected static IRestClient CreateRestClient(string baseUrl, string accept)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = 10240;
            ServicePointManager.MaxServicePoints = 10240;
            ServicePointManager.Expect100Continue = false;

            if (baseUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                // request.ProtocolVersion = HttpVersion.Version11;
                // 这里设置了协议类型。
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls1.2; 
                //request.KeepAlive = false;
            }
            var restClient = new RestClient(baseUrl);
            restClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36";
            restClient.RemoveDefaultParameter("accept");
            restClient.AddDefaultHeader("accept", accept);
            restClient.AddDefaultHeader("accept-language", "zh-CN,zh;q=0.9,en;q=0.8");
            restClient.AddDefaultHeader("accept-encoding", "gzip,deflate,br");//gzip,deflate,br,identity
            restClient.AddDefaultHeader("cache-control", "no-cache");
            restClient.AddDefaultHeader("pragma", "no-cache");
            restClient.AddDefaultHeader("cpragma", "no-cache");
            restClient.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
            restClient.AddDefaultHeader("referer", baseUrl);

            // restClient.AddDefaultHeader("X-Content-Type-Options", "nosniff");
            // restClient.AddDefaultHeader("X-Frame-Options", "SAMEORIGIN");
            // restClient.AddDefaultHeader("X-UA-Compatible", "chrome=1");
            //restClient.AddDefaultHeader("X-XSS-Protection", "1; mode=block");
            restClient.AddDefaultHeader("sec-ch-ua", "\"Google Chrome\"; v=\"95\", \"Chromium\"; v=\"95\", \";Not A Brand\"; v=\"99\"");
            restClient.AddDefaultHeader("sec-ch-ua-mobile", "?0");
            restClient.AddDefaultHeader("sec-ch-ua-platform", "\"Windows\"");
            restClient.AddDefaultHeader("Sec-Fetch-Dest", "document");
            restClient.AddDefaultHeader("Sec-Fetch-Mode", "cors");
            restClient.AddDefaultHeader("Sec-Fetch-Site", "same-origin");
            restClient.AddDefaultHeader("Sec-Fetch-User", "?1");

            restClient.CookieContainer = new CookieContainer() { MaxCookieSize = 1024 * 1024 * 10, PerDomainCapacity = 256 };
            restClient.FollowRedirects = true;
            restClient.Timeout = 5 * 60 * 1000;
            restClient.ReadWriteTimeout = 5 * 60 * 1000;
            restClient.Encoding = Encoding.UTF8;
            //restClient.AddDefaultHeader("connection","keep-alive");

            restClient.AddDefaultHeader("keep-alive", "" + 600 * 5);
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
        public static IRestResponse Get(string baseUrl, string url, int? timeOut, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            var request = new RestRequest(url, Method.GET);
            request.Timeout = timeOut ?? client.Timeout;
            request.ReadWriteTimeout = timeOut ?? client.ReadWriteTimeout;
            return Execute(client, request, console);
        }
        public static IRestResponse Execute(string baseUrl, RestRequest request, int? timeOut, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            request.Timeout = timeOut ?? client.Timeout;
            request.ReadWriteTimeout = timeOut ?? client.ReadWriteTimeout;
            return Execute(client, request, console);
        }
        public static IRestResponse Post(string baseUrl, string url, int? timeOut, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            var request = new RestRequest(url, Method.POST);
            request.Timeout = timeOut ?? client.Timeout;
            request.ReadWriteTimeout = timeOut ?? client.ReadWriteTimeout;
            return Execute(client, request, console);
        }
        public static IRestResponse Post<T>(string baseUrl, string url, T body, IWrite console = null)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            var request = new RestRequest(url, Method.POST);

            request.AddBody(body);
            return Execute(client, request, console);
        }
        public static IRestResponse PostForm(string baseUrl, string url, IDictionary<string, object> data, IWrite console = null)
        {
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            return PostForm(restClient, url, data, console);
        }
        public static IRestResponse PostForm(string baseUrl, string url, string csrfToken, IDictionary<string, object> data, IWrite console = null)
        {
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            return PostForm(restClient, url, csrfToken, data, console);
        }
        public static IRestResponse PostForm(IRestClient restClient, string url, IDictionary<string, object> data, IWrite console = null)
        {
            return PostForm(restClient, url, null, data, console);
        }
        public static IRestResponse PostForm(IRestClient restClient, string url, string csrfToken, IDictionary<string, object> data, IWrite console = null)
        {

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            if (!string.IsNullOrEmpty(csrfToken)) request.AddHeader("X-CSRF-Token", csrfToken);
            if (data != null && data.Count > 0)
            {
                request.AddHeader("referer", restClient.BaseUrl + url);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                foreach (var kv in data)
                {
                    request.AddParameter(kv.Key, kv.Value, ParameterType.GetOrPost); 
                    request.AddParameter(kv.Key, kv.Value, ParameterType.Cookie);
                }
            }
            return Execute(restClient, request, console);
        }
        #endregion
        public static void DownloadData(IRestClient client, string url, Method method, int? timeOut, string filePath, Action<long, long, long> progress, IWrite console = null)
        {
            var request = new RestRequest(url, Method.GET);
            request.Timeout = timeOut ?? -1;
            request.ReadWriteTimeout = timeOut ?? -1;
            using (var writer = new ProgressFileStream(filePath))
            {
                writer.WriteHandler += (t, f, c) =>
                {
                    progress.Invoke(t, f, c);
                };
                request.ResponseWriter = (responseStream) =>
                {
                    responseStream.CopyTo(writer);
                };
                client.DownloadData(request);
            }
        }
        #region
        public static IRestResponse Get(IRestClient client, string url, int? timeOut, IWrite console = null)
        {
            var request = new RestRequest(url, Method.GET);
            request.Timeout = timeOut ?? -1;
            request.ReadWriteTimeout = timeOut ?? -1;
            return Execute(client, request, console);
        }
        public static IRestResponse Post(IRestClient client, string url, int? timeOut, IWrite console = null)
        {
            var request = new RestRequest(url, Method.POST);
            request.Timeout = timeOut ?? -1;
            request.ReadWriteTimeout = timeOut ?? -1;
            return Execute(client, request, console);
        }
        public static IRestResponse Post<T>(IRestClient client, string url, T body, IWrite console = null)
        {
            var request = new RestRequest(url, Method.POST);
            request.AddBody(body);
            return Execute(client, request, console);
        }
        #endregion

        public static void SetDefaultHeader(IRestClient client, ICollection<Parameter> ps)
        {
            foreach (var p in ps)
            {
                if (p.Type.Equals(ParameterType.HttpHeader) && p.Name.ToLower().StartsWith("x-"))
                {
                    SetDefaultHeader(client, p.Name, "" + p.Value);
                }
            }
        }
        public static void SetDefaultHeader(IRestClient client, string name, string value)
        {
            client.RemoveDefaultParameter(name.ToLower());
            client.AddDefaultHeader(name.ToLower(), value);
        }
        public static void SetCookies(IRestClient client, ICollection<RestResponseCookie> cookies, bool replace = true, string cookiePath = "/")
        {
            foreach (var cookie in cookies)
            {
                var v = replace ? cookie.Value.Replace(",", "%2C") : cookie.Value;
                if ((!replace) && v.Contains(",")) v = "\"" + v + "\"";
                var c = new Cookie(cookie.Name, v, cookiePath);
                c.Value = v;
                client.CookieContainer.Add(client.BaseUrl, c);
            }
        }
        public static void SetCookies(IRestClient client, string cookieString, bool replace = true, string cookiePath = "/")
        {
            SetCookies(client, GetCookieDictionary(cookieString), replace, cookiePath);
        }
        public static void SetCookies(IRestClient client, IDictionary<string, string> cookies, bool replace = true, string cookiePath = "/")
        {
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    var v = replace ? cookie.Value.Replace(",", "%2C") : cookie.Value;
                    if ((!replace) && v.Contains(",")) v = "\"" + v + "\"";
                    var c = new Cookie(cookie.Key, v, cookiePath);
                    c.Value = v;
                    client.CookieContainer.Add(client.BaseUrl,c );
                }
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
                        cookieValue = parts[1].Trim();
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
            log = string.Format("  End {0},{1},{2},{3},{4},{5},{6}", request.Method, response.StatusCode, response.ContentLength, response.Content.Length, response.StatusDescription, response.ContentEncoding, Regex.Replace(request.Resource, @"(\.json|\.xml|\.do|\.action|\.aspx|\.jsp)", ""));

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
        public static void FixContentEncoding(IRestResponse resp, string encoding)
        {
            var content = Encoding.GetEncoding(encoding).GetString(resp.RawBytes);
            resp.Content = content;
        }
        public static bool IsSuccess(IRestResponse resp)
        {
            return resp.StatusCode == System.Net.HttpStatusCode.OK;
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
        public static IEnumerable<string> RegexGetStringsBySplit(string source, string separator, string pattern, int index)
        {
            var sources = source.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var source2 in sources)
            {
                var ms = Regex.Matches(source2, pattern, RegexOptions);
                foreach (Match m in ms)
                {
                    if (m.Success)
                    {
                        yield return m.Groups[index].Value;
                    }
                }
            }
        }
        public static IEnumerable<string> RegexGetStrings(string source, string pattern, int index)
        {
            var ms = Regex.Matches(source, pattern, RegexOptions);
            foreach (Match m in ms)
            {
                if (m.Success)
                {
                    yield return m.Groups[index].Value;
                }
            }
        }
        public static MatchCollection RegexGetMatchs(string source, string pattern)
        {
            var ms = Regex.Matches(source, pattern, RegexOptions);

            return ms;
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
        public static string FiterChars(string source)
        {
            return Regex.Replace(source, "[^\u4E00-\u9FFF_A-Za-z0-9,;\\.；。，“：]", "", RegexOptions);
        }
        static Random random = new Random();
        public static IEnumerable<T> GetRandomCollection<T>(IEnumerable<T> coll)
        {
            return coll.OrderBy(t => t, new RandomComparer<T>()).ThenByDescending(t => t, new RandomComparer<T>());
        }
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
                    int a = (int)Math.Pow(10, len - 1);
                    int b = (int)Math.Pow(10, len) - 1;
                    sb.Append(random.Next(a, b));
                }

            }
            return sb.ToString();
        }
        public static string BuildLog<T>(T item)
        {
            StringBuilder sb = new StringBuilder();
            var fl = new TaobaoTask.KC.FieldReader(item).ReadFieldList();
            if (fl.Fields.Length > 0)
            {
                foreach (var f in fl.Fields)
                {
                    if (f.Value != null && !string.IsNullOrWhiteSpace(f.Value.ToString()) && f.Value.ToString().Length < 20)
                    {
                        var text = string.Format(";{0}:{1}", f.Caption, f.Value);
                        sb.Append(text);
                    }
                }
                return sb.ToString().Substring(1);
            }
            return string.Empty;
        }
        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var setting = new JsonSerializerSettings();
            return setting;
        }
    }
}