using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UniTask_UI.common;

namespace TaobaoTask
{
    public class BeepUtil
    {
        public enum MessageBeepType : int
        {
            Default = -1,
            Ok = 0x00000000,
            Error = 0x00000010,
            Question = 0x00000020,
            Warning = 0x00000030,
            Information = 0x00000040,
        }
        public const int MB_ICONEXCLAMATION = 48;
        /// <summary>
        /// //Ok = 0x00000000,  
        //Error = 0x00000010,  
        //Question = 0x00000020,  
        //Warning = 0x00000030,  
        //Information = 0x00000040
        /// </summary>
        /// <param name="uType"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);
    }
    public class RestClientUtil
    {
        static RestClientUtil() {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = 10240;
            ServicePointManager.MaxServicePoints = 10240;
            ServicePointManager.Expect100Continue = false;
        }
      
        static DateTime lastTime = DateTime.Now.AddMinutes(-1);
        static IDictionary<string, IRestClient> clients = new Dictionary<string, IRestClient>();
        static IDictionary<string, SemaphoreSlim> mres = new Dictionary<string, SemaphoreSlim>();
        static IDictionary<string, object> locks = new Dictionary<string, object>();
        public static object GetLock(string key)
        {
            object mre = null;

            if (!locks.ContainsKey(key))
            {
                lock (locks)
                {
                    if (!locks.ContainsKey(key))
                    {
                        mre = new object();
                        locks[key] = mre;
                        return mre;
                    }
                }
            }
            mre = locks[key];
            return mre;
        }
        public static SemaphoreSlim GetMre(string key)
        {
            SemaphoreSlim mre = null;

            if (!mres.ContainsKey(key))
            {
                lock (mres)
                {
                    if (!mres.ContainsKey(key))
                    {
                        mre = new SemaphoreSlim(1,1);
                        mres[key] = mre;
                        return mre;
                    }
                }
            }
            mre = mres[key];
            return mre;
        }
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
            restClient.AddDefaultHeader("Sec-Fetch-Mode", "navigate");
            restClient.AddDefaultHeader("Sec-Fetch-Site", "same-origin");
            restClient.AddDefaultHeader("Sec-Fetch-User", "?1");

            restClient.CookieContainer = new CookieContainer() { MaxCookieSize = 1024 * 1024 * 10, PerDomainCapacity = 256 };
            restClient.FollowRedirects = true;
            restClient.Timeout = 5 * 60 * 1000;
            restClient.ReadWriteTimeout = 5 * 60 * 1000;
            restClient.Encoding = Encoding.UTF8;
            //restClient.AddDefaultHeader("connection","keep-alive");

            restClient.AddDefaultHeader("keep-alive", "" + 60 * 30);
            //var timeSpan = (lastTime.AddMilliseconds(511 * 1) - DateTime.Now);
            //if (timeSpan.TotalMilliseconds > 0)
            {
                //System.Threading.Thread.Sleep(timeSpan);
                System.Threading.Thread.Sleep(25);
            }
            lastTime = DateTime.Now;
            return restClient;

        }
        static Random rnd = new Random();
        static public void randomUserAgent(IRestClient restClient)
        {
            restClient.UserAgent = Regex.Replace(restClient.UserAgent, "^([0-9])", "" + rnd.Next(1, 10));
        }
        #region
        public static IRestResponse Get(string baseUrl, string url, int? timeOut)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            var request = new RestRequest(url, Method.GET);
            request.Timeout = timeOut ?? client.Timeout;
            request.ReadWriteTimeout = timeOut ?? client.ReadWriteTimeout;
            return Execute(client, request);
        }
        public static IRestResponse Execute(string baseUrl, RestRequest request, int? timeOut)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            request.Timeout = timeOut ?? client.Timeout;
            request.ReadWriteTimeout = timeOut ?? client.ReadWriteTimeout;
            return Execute(client, request);
        }
        public static IRestResponse Post(string baseUrl, string url, int? timeOut)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            var request = new RestRequest(url, Method.POST);
            request.Timeout = timeOut ?? client.Timeout;
            request.ReadWriteTimeout = timeOut ?? client.ReadWriteTimeout;
            return Execute(client, request);
        }
        public static IRestResponse Post<T>(string baseUrl, string url, T body, DataFormat f)
        {
            var client = RestClientUtil.GetRestClient(baseUrl, true);
            var request = new RestRequest(url, Method.POST);
            request.RequestFormat = f;
            request.AddBody(body);
            return Execute(client, request);
        }
        public static IRestResponse PostForm(string baseUrl, string url, IDictionary<string, object> data)
        {
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            return PostForm(restClient, url, data);
        }
        public static IRestResponse PostForm(string baseUrl, string url, string csrfToken, IDictionary<string, object> data)
        {
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            return PostForm(restClient, url, csrfToken, data);
        }
        public static IRestResponse PostForm(IRestClient restClient, string url, IDictionary<string, object> data)
        {
            return PostForm(restClient, url, null, data);
        }
        public static IRestResponse PostForm(IRestClient restClient, string url, string csrfToken, IDictionary<string, object> data)
        {

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            if (!string.IsNullOrEmpty(csrfToken)) request.AddHeader("X-CSRF-Token", csrfToken);
            if (data != null)
            {
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                foreach (var kv in data)
                {
                    request.AddParameter(kv.Key, kv.Value);
                }
            }
            return Execute(restClient, request);
        }
        #endregion
        public static void DownloadData(IRestClient client, string url, Method method, int? timeOut, string filePath, Action<long, long, long> progress)
        {
            var request = new RestRequest(url, method);
            request.Timeout = timeOut ?? -1;
            request.ReadWriteTimeout = timeOut ?? -1;
            using (var writer = new ProgressFileStream(filePath))
            {
                writer.WriteHandler += (t, f, c) =>
                {
                    if (progress != null)
                    {
                        progress.Invoke(t, f, c);
                    }
                };
                request.ResponseWriter = (responseStream) =>
                {
                    responseStream.CopyTo(writer);
                };
                client.DownloadData(request);
            }
        }
        #region
        public static IRestResponse Get(IRestClient client, string url, int? timeOut)
        {
            var request = new RestRequest(url, Method.GET);
            request.Timeout = timeOut ?? -1;
            request.ReadWriteTimeout = timeOut ?? -1;
            request.UseDefaultCredentials = true;
            return Execute(client, request);
        }
        public static IRestResponse Post(IRestClient client, string url, int? timeOut)
        {
            var request = new RestRequest(url, Method.POST);
            request.Timeout = timeOut ?? -1;
            request.ReadWriteTimeout = timeOut ?? -1;
            return Execute(client, request);
        }
        public static IRestResponse Post<T>(IRestClient client, string url, T body, DataFormat f)
        {
            var request = new RestRequest(url, Method.POST);
            request.RequestFormat = f;
            request.AddBody(body);
            request.UseDefaultCredentials = true;
            return Execute(client, request);
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
        public static string GetCookieString(IRestClient client)
        {
            return GetCookieString(client.CookieContainer);
        }
        public static string GetCookieString(System.Net.CookieContainer cc)
        {
            string str = "";
            List<Cookie> listcookie = GetAllCookies(cc);
            foreach (Cookie c in listcookie)
            {
                string name = c.Name;
                string value = c.Value;
                string domain = c.Domain;
                str += name + "=" + value + ";domain=" + domain + ";";
            }

            return str;
        }
        private static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            if (cc == null)
            {
                return new List<Cookie>();
            }
            List<Cookie> lstCookies = new List<Cookie>();

            System.Collections.Hashtable table = (System.Collections.Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                System.Collections.SortedList lstCookieCol = (System.Collections.SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }

            return lstCookies;
        }
        public static void SetCookies(IRestClient client, ICollection<RestResponseCookie> cookies, string cookiePath = "/")
        {
            foreach (var cookie in cookies)
                client.CookieContainer.Add(client.BaseUrl, new Cookie(cookie.Name, cookie.Value.Replace(",", "%2C"), cookiePath));
        }
        public static void SetCookies(IRestClient client, string cookieString, string cookiePath = "/")
        {
            SetCookies(client, GetCookieDictionary(cookieString), cookiePath);
        }
        public static void SetCookies(IRestClient client, IDictionary<string, string> cookies, string cookiePath = "/")
        {
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                    client.CookieContainer.Add(client.BaseUrl, new Cookie(cookie.Key, cookie.Value.Replace(",", "%2C"), cookiePath));
            }

        }
        public static IDictionary<string, string> GetCookieDictionary(string cookieString)
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
        public static IRestResponse Execute(IRestClient client, IRestRequest request)
        {
            //var log = string.Format("Start {0},{1}", request.Method, Regex.Replace(request.Resource, @"(\.json|\.xml|\.do|\.action|\.aspx|\.jsp)", ""));
            var start = DateTime.Now;
           var response = client.Execute(request);
            //log = string.Format("  End {0},{1},{2},{3},{4},{5},{6}", request.Method, response.StatusCode, response.ContentLength, response.Content.Length, response.StatusDescription, response.ContentEncoding, Regex.Replace(request.Resource, @"(\.json|\.xml|\.do|\.action|\.aspx|\.jsp)", ""));
            var log2 = LogRequest(request, response, (DateTime.Now-start).TotalMilliseconds);
            //logger.DebugFormat("ps: {0}",JsonConvert.SerializeObject(client.DefaultParameters));

            LoggerUtil.Debug("{0}",log2);
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
        public static string execCmd(string _arguments, bool rtnLastLine)
        {
            return execCommand("cmd.exe", "/c " + _arguments, rtnLastLine);
        }
        public static string execCommand(string _command, string _arguments, bool rtnLastLine)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(_command, _arguments);
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            if (rtnLastLine)
            {
                StreamReader sr = p.StandardOutput;//获取返回值
                string line = "";
                String str = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        str = line;
                    }
                }
                return str;
            }
            return (p.StandardOutput.ReadToEnd());
        }
        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var setting = new JsonSerializerSettings();
            return setting;
        }
        private static String LogRequest(IRestRequest request, IRestResponse response, double durationMs)
        {

            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = request.Resource,
            };

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                cookies =response.Cookies.Select(p => new
                {
                    name = p.Name,
                    value = p.Value
                }),
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            return string.Format("Execute:{0} ms, Request: {1}, Response: {2}",
                durationMs, JsonConvert.SerializeObject(requestToLog),
                JsonConvert.SerializeObject(responseToLog));
        }
        public static T DeserializeObject<T>(String content)
        {
            if(content!=null&&content.Length>0&&(content.StartsWith("{")|| content.StartsWith("[")))
             return JsonConvert.DeserializeObject<T>(content);
            return default(T);
        }
        
    }
}