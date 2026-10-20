using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TaobaoTask;
using UniTask_UI.common;

namespace UniTask_UI.p1.c
{
    [Serializable]
    public class loginReq : baseReq
    {
        /// <summary>
        /// 
        /// </summary>
        public string webUserid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string passWord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }


        public loginReq()
        {
            webUserid = "uqrokz30847@chacuo.net";
            passWord = "pApXtFbV999";
            code = "";
            appId = "HZMBWEB_HK";
            joinType = "WEB";
            version = "2.7.202208.1302";
            equipment = "PC";
        }

    }

    [Serializable]
    public class baseReq
    {
        public string appId { get; set; }
        public string joinType { get; set; }
        public string version { get; set; }
        public string equipment { get; set; }
        public baseReq()
        {
            appId = "HZMBWEB_HK";
            joinType = "WEB";
            version = "2.7.202208.1302";
            equipment = "PC";
        }
    }

    [Serializable]
    public class baseResp
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public string message { get; set; }
    }
    [Serializable]
    public class loginResp : baseResp
    {


        /// <summary>
        /// responseData
        /// </summary>
        //public responseData responseData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string jwt { get; set; }

    }
    [Serializable]
    public class MyBookingReq : baseReq
    {
        public String bookDate { get; set; }
        public String lineCode { get; set; }
    }
    [Serializable]
    public class MyBookingResp : baseResp
    {
        public List<ProductDetail> responseData { get; set; }
    }
    [Serializable]
    public class MyPriceReq : baseReq
    {
        public String buyDate { get; set; }
        public String lineCode { get; set; }
    }
    [Serializable]
    public class PriceItem
    {/// <summary>
     /// 
     /// </summary>
        [DisplayName("路线")]
        public string lineCode { get; set; }

        [DisplayName("预定日期")]
        public DateTime buyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("价格1")]
        public int adultCNY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("价格2")]
        public int childrenCNY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int adultHKD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int adultMOP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string boardingPointCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string breakoutPointCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int childrenHKD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int childrenMOP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int elderlyCNY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int elderlyHKD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int elderlyMOP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endStationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int feeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string luggageCNY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string luggageHKD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string luggageMOP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startStationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sysTime { get; set; }
    }
    [Serializable]
    public class MyPriceResp : baseResp
    {

        /// <summary>
        /// 
        /// </summary>
        public List<PriceItem> responseData { get; set; }
    }

    [Serializable]
    public class TicketsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int idType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string telNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ticketType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userName { get; set; }
    }
    [Serializable]
    public class OrderReq
    {
        /// <summary>
        /// 
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string boardingPointCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bookBeginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bookEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string breakoutPointCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string captcha { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endStationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int feeType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lineCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sessionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startStationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ticketCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ticketData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TicketsItem> tickets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalBalpay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalNeedpay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalVoucherpay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int voucherNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voucherStr { get; set; }
    }
    [Serializable]
    public class OrderResp : baseResp
    {

        /// <summary>
        /// 
        /// </summary>
        public OrderData responseData { get; set; }
    }
    [Serializable]
    public class OrderData
    {
        /// <summary>
        /// 
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bookBeginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bookEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detailList { get; set; }
        /// <summary>
        /// 單程:珠海(珠海口岸)->香港(香港口岸), 2022-09-16, 成人:1人,
        /// </summary>
        public string orderDecribe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderReqno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sysTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ticketCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ticketData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TicketsItem> tickets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalNeedpay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalVoucherpay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tradeBtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tradeEtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int voucherNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wxAttach { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wxGoodstag { get; set; }
    }
    [Serializable]
    public class CannelOrderReq
    {
        public string orderNo { get; set; }
    }
    public class MyRestUtil2
    {//{
     //  "buyDate": "2022-09-20",
     //  "lineCode": "ZHOHKG",
     //  "appId": "HZMBWEB_HK",
     //  "joinType": "WEB",
     //  "version": "2.7.202208.1302",
     //  "equipment": "PC"
     //}https://i.hzmbus.com/webhtml/login https://gt.e-ctb.com/
     //https://i.hzmbus.com/webh5api/captcha?1
     //https://i.hzmbus.com/webh5api/ticket/query.line.ticket.price
     //https://i.hzmbus.com/webh5api/manage/query.book.info.data
    }
    public class MyRestUtil
    {
        static DateTime lastTime = DateTime.Now.AddMinutes(-1);
        static String baseUrl1 = "https://wx.hzmbus.com/";
        static String url10 = "/webapi/ticket/query.line.ticket.price";
        static String url11 = "/webapi/manage/query.book.info.data";
        static String url12 = "/hzmb/webapi/captcha?";
        static String url13 = "/webapi/ticket/buy.ticket";
        static String url14 = "/webapi/wx/cancel.wx.order";
        static String baseUrl2 = "https://i.hzmbus.com/";
        static String url20 = "/webh5api/ticket/query.line.ticket.price";
        static String url21 = "/webh5api/manage/query.book.info.data";
        static String url22 = "/webh5api/captcha?";
        static String url23 = "/webh5api/ticket/buy.ticket"; //h5
        static IDictionary<string, IRestClient> clients = new Dictionary<string, IRestClient>();
        public static IRestClient GetRestClient(string baseUrl, string accept, string cookies, bool useClientCache = false)
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
                            RestClientUtil.SetCookies(client, cookies);
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
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        protected static IRestClient CreateRestClient(string baseUrl, string accept)
        {


            if (baseUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request.ProtocolVersion = HttpVersion.Version11;
                // 这里设置了协议类型。
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls1.2; 
                //request.KeepAlive = false;
            }
            var restClient = new RestClient(baseUrl);
            restClient.UserAgent = "Mozilla/5.0 (Linux; Android 8.0.0; KNT-UL10 Build/HUAWEIKNT-UL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/86.0.4240.99 XWEB/4343 MMWEBSDK/20220903 Mobile Safari/537.36 MMWEBID/3179 MicroMessenger/8.0.28.2240(0x28001C57) WeChat/arm64 Weixin NetType/WIFI Language/zh_CN ABI/arm64";
            restClient.RemoveDefaultParameter("accept");
            restClient.AddDefaultHeader("accept", accept);
            restClient.AddDefaultHeader("accept-language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            restClient.AddDefaultHeader("accept-encoding", "gzip,deflate");//gzip,deflate,br,identity
            restClient.AddDefaultHeader("cache-control", "no-cache");
            restClient.AddDefaultHeader("pragma", "no-cache");
            restClient.AddDefaultHeader("cpragma", "no-cache");
            restClient.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
            restClient.AddDefaultHeader("referer", baseUrl);

            // restClient.AddDefaultHeader("X-Content-Type-Options", "nosniff");
            // restClient.AddDefaultHeader("X-Frame-Options", "SAMEORIGIN");
            // restClient.AddDefaultHeader("X-UA-Compatible", "chrome=1");
            //restClient.AddDefaultHeader("X-XSS-Protection", "1; mode=block");
            //restClient.AddDefaultHeader("sec-ch-ua", "\"Google Chrome\"; v=\"95\", \"Chromium\"; v=\"95\", \";Not A Brand\"; v=\"99\"");
            restClient.AddDefaultHeader("sec-ch-ua-mobile", "?0");
            //restClient.AddDefaultHeader("sec-ch-ua-platform", "\"Windows\"");
            restClient.AddDefaultHeader("Sec-Fetch-Dest", "document");
            restClient.AddDefaultHeader("Sec-Fetch-Mode", "cors");
            restClient.AddDefaultHeader("Sec-Fetch-Site", "same-origin");
            restClient.AddDefaultHeader("Sec-Fetch-User", "?1");

            restClient.CookieContainer = new CookieContainer() { MaxCookieSize = 1024 * 1024 * 10, PerDomainCapacity = 288 };
            restClient.FollowRedirects = true;
            restClient.Timeout = 6 * 1000;
            restClient.ReadWriteTimeout = 6 * 1000;
            restClient.Encoding = Encoding.UTF8;
            //restClient.AddDefaultHeader("connection","keep-alive");

            restClient.AddDefaultHeader("keep-alive", "" + 60 * 30);
            //var timeSpan = (lastTime.AddMilliseconds(511 * 1) - DateTime.Now);
            //if (timeSpan.TotalMilliseconds > 0)
            {
                //System.Threading.Thread.Sleep(timeSpan);
                // System.Threading.Thread.Sleep(25);
            }
            lastTime = DateTime.Now;
            restClient.AddDefaultHeader("connection", "keep-alive");
            restClient.AddDefaultHeader("X-Requested-With", "com.tencent.mm");
            //restClient.AddDefaultHeader("Origin", "https://wx.hzmbus.com");
            //restClient.AddDefaultHeader("Authorization", "");

            return restClient;

        }

        public static IRestClient GetRestClient(Account account, string accept, bool useClientCache = false)
        {
            string baseUrl = account.Email != null && account.Email.Length > 1 ? baseUrl2 : baseUrl1;
            var restClient = GetRestClient(baseUrl, accept, account.Cookies, useClientCache);
            var success = login(restClient, account, 0);
            if (!success)
            {
                var key = string.Format("{0},{1}", baseUrl, accept);
                if (clients.ContainsKey(key))
                {
                    lock (clients)
                    {
                        clients.Remove(key);
                    }
                }
            }
            return restClient;
        }
        static public bool login(IRestClient restClient, Account account, int sleepMs)
        {
            if (account.Email != null && account.Email.Length > 1)
            {
                if (account.Cookies.Length > 0 && account.Authorization.Length > 0)
                {
                    restClient.AddDefaultHeader("Authorization", account.Authorization.Trim());
                    return true;
                }
                var url = "https://i.hzmbus.com/webh5api/login";// account.Email.Length > 1 ? url20 : url10;
                loginReq req = new loginReq();
                req.webUserid = account.Email;
                req.passWord = account.Pwd;
                var resp = RestClientUtil.Post(restClient, url, req, DataFormat.Json);
                LoggerUtil.Debug("{0},{1},{2}", resp.StatusCode, resp.StatusDescription, resp.Content);
                if (resp != null && resp.IsSuccessful)
                {
                    try
                    {
                        if (!resp.Content.StartsWith("<script"))
                        {
                            loginResp loginResp = RestClientUtil.DeserializeObject<loginResp>(resp.Content);
                            if (loginResp != null && loginResp.jwt != null && loginResp.jwt.Length > 0)
                            {
                                LoggerUtil.Debug("{0},{1},{2}", resp.StatusCode, resp.StatusDescription, loginResp.jwt);
                                restClient.AddDefaultHeader("Authorization", loginResp.jwt.Trim());
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerUtil.Error("", ex);
                    }
                }

            }
            return account.Cookies != null && account.Cookies.Length > 1;
        }
        static public IRestResponse getbookingprice(IRestClient restClient, MyPriceReq req, Account account, int sleepMs)
        {

            //MyData d = MyData.Instance;
            //var gs = d.goalSetting;
            //var enable = gs.isStart && (gs.isStart4p);
            //if (!enable)
            //{
            //    LoggerUtil.Info("enable....{0}", enable);
            //    return null;
            //}
            IRestResponse resp = null;
            var url = account.Email != null && account.Email.Length > 1 ? url20 : url10;
            wait(restClient, account, sleepMs, 0, () =>
            {
                resp = RestClientUtil.Post(restClient, url, req, DataFormat.Json);
            });
            LoggerUtil.Info("{0},{1},{2},{3},{4}", resp.StatusCode, resp.StatusDescription, resp.Cookies.Count, resp.ErrorMessage, resp.Content);
            return resp;
        }
        static public IRestResponse getbooking(IRestClient restClient, MyBookingReq req, Account account, int sleepMs)
        {


            MyData d = MyData.Instance;
            var gs = d.goalSetting;
            var enable = gs.isStart && (gs.isStart4q);
            if (!enable)
            {
                LoggerUtil.Info("enable....{0}", enable);
                return null;
            }
            var url = account.Email != null && account.Email.Length > 1 ? url21 : url11;
            IRestResponse resp = null;
            wait(restClient, account, sleepMs, 2, () =>
            {
                resp = RestClientUtil.Post(restClient, url, req, DataFormat.Json);
            });
            var cc = resp.Content;
            if (cc != null && (cc.Contains("<html") || cc.Contains("<script")))
            {
                var js = Regex.Match(cc, "<script>(.*?)</script>", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value;
                var r = "function setCookie(name,value){";
                var xjs = js.Replace(r, " var zfrqqq='nn';" + r + " zfrqqq=value;document.title=zfrqqq; ");
                xjs = xjs.Replace("setInterval(function(){_0x4db1c();},0xfa0);", "");
                xjs = xjs.Replace("document.location.reload();", "");
                xjs = xjs.Replace(@"\n", @" ");
                xjs = xjs.Replace(@"\\x", @"\x");
                xjs = xjs.Replace("\\\"", "\"");
                xjs = xjs.Replace(@"x73']){}", @"x73']){break;}");
                xjs = xjs.Replace(@"(/([.$?*|{}()[]\\/+^])/g", @"'(/([.$?*|{}()[]\\/+^])/g'");
                xjs = xjs.Replace(@"!!window['\x61\x64\x64\x45\x76\x65\x6e\x74\x4c\x69\x73\x74\x65\x6e\x65\x72']", @" true ");
                xjs = xjs.Replace(@"~-0x1>>0x1+0xff%0x0", @"0");
                xjs = xjs.Replace(@"~-0x4>>0x1+0xff%0x0", @"3");
                xjs = xjs.Replace(@"_0xd79219(_0x178627);", @"");
                var file = "js-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff") + ".html";
                File.WriteAllText(file, "<html><script>" + xjs + "</script></html>");
                LoggerUtil.Info("exec:{0} {1}", js.Length, xjs.Length);
                // var acw_sc__v2 = ChromeDriverUtil.ExecuteScript("zfrqqq", xjs + " function qqqqq(){ return zfrqqq;}");
                var acw_sc__v2 = ChromeDriverUtil.ExecuteScript(file, "");
                File.Delete(file);
                LoggerUtil.Info("result:{0} {1} {2}", js.Length, xjs.Length, acw_sc__v2);
                if (acw_sc__v2 != null && acw_sc__v2.Length > 8)
                {
                    var ccs = RestClientUtil.GetCookieDictionary(account.Cookies);
                    ccs["acw_sc__v2"] = acw_sc__v2;
                    RestClientUtil.SetCookies(restClient, ccs);
                    cc = "cookies自动保持";
                }
                else
                    cc = "服务器返回异常,预测cookies无效.Length:" + cc.Length;
                // LoggerUtil.Debug("Cookies {0}", account.Cookies);
                //queryindexpage(restClient, account, sleepMs,0);
                //queryindexpage(restClient, account, sleepMs,1);
                //queryindexpage(restClient, account, sleepMs,2);
                //querywxuserinfo(restClient, account, sleepMs);

                //LoggerUtil.Debug("Cookies {0}", account.Cookies);
            }
            else if (account.Use % 50 == 49)
            {
                //queryindexpage(restClient, account, sleepMs, 0);
                //queryindexpage(restClient, account, sleepMs, 1);
                //queryindexpage(restClient, account, sleepMs, 2);
                //querywxuserinfo(restClient, account, sleepMs);
            }
            account.Cookies = RestClientUtil.GetCookieString(restClient);
            LoggerUtil.Info("{0},{1},{2},{3},{4},{5}", resp.StatusCode, resp.StatusDescription, resp.Headers.Count, resp.Cookies.Count, resp.ErrorMessage, cc);
            return resp;
        }
        static public void queryindexpage(IRestClient restClient, Account account, int sleepMs, int index)
        {
            var url222 = "https://wx.hzmbus.com/wxhtml/index.html?lan=zh";
            var url = account.Email != null && account.Email.Length > 1 ? url222 : url222;
            // IRestResponse resp = null;
            wait(restClient, account, sleepMs, 0, () =>
            {
                if (index < 1)
                {
                    var cookieString = RestClientUtil.GetCookieString(restClient);
                    var ccs = RestClientUtil.GetCookieDictionary(cookieString);
                    ccs.Remove("PHPSESSID");
                    ccs.Remove("acw_tc");
                    ccs.Remove("acw_sc__v2");
                    // RestClientUtil.SetCookies(restClient, ccs);
                    foreach (var cookie in ccs)
                    {
                        WebbrowserUtil.SetCookie(url222, cookie.Key, cookie.Value);
                    }
                    // MyCtrl.Instance.webBrowser1.Url =new Uri( url222);
                    // MyCtrl.Instance.webBrowser1.Navigate(url222);
                    // Thread.Sleep(4000);
                    // var cookies= WebbrowserUtil.GetCookies(url222);
                    //  LoggerUtil.Info("SetCookies:{0}", cookies);
                    //  RestClientUtil.SetCookies(restClient, cookies);

                }

                var resp = RestClientUtil.Get(restClient, url, 3000);
                LoggerUtil.Info("{0},{1},{2},{3},{4}", resp.StatusCode, resp.StatusDescription, resp.Cookies.Count, resp.ErrorMessage, resp.Content?.Length);

            });
        }
        static public void querywxuserinfo(IRestClient restClient, Account account, int sleepMs)
        {
            var url222 = "https://wx.hzmbus.com/webapi/wx/query.wx.user.information";
            var url = account.Email != null && account.Email.Length > 1 ? url222 : url222;
            // IRestResponse resp = null;
            wait(restClient, account, sleepMs, 0, () =>
            {
                //var ccs = RestClientUtil.GetCookieDictionary(account.Cookies);
                //ccs.Remove("PHPSESSID");
                //RestClientUtil.SetCookies(restClient, ccs);
                var resp = RestClientUtil.Post(restClient, url, new object(), DataFormat.Json);
                LoggerUtil.Info("{0},{1},{2},{3},{4}", resp.StatusCode, resp.StatusDescription, resp.Cookies.Count, resp.ErrorMessage, resp.Content?.Length);

            });
        }
        static int vi = 0;
        static public void getvimg(IRestClient restClient, String file, Account account, int sleepMs)
        {
            restClient.AddDefaultHeader("Sec-Fetch-Dest", "image");
            Interlocked.Increment(ref vi);
            var url = account.Email != null && account.Email.Length > 1 ? url22 : url12;
            // IRestResponse resp = null;
            wait(restClient, account, sleepMs, 0, () =>
            {
                RestClientUtil.DownloadData(restClient, url + (vi % 100), Method.GET, 1500, file, null);
            });
        }
        static public IRestResponse getordering(IRestClient restClient, OrderReq req, Account account)
        {
            var url = account.Email != null && account.Email.Length > 1 ? url23 : url13;
            var resp = RestClientUtil.Post(restClient, url, req, DataFormat.Json);
            LoggerUtil.Info("{0},{1},{2},{3},{4}", resp.StatusCode, resp.StatusDescription, resp.Cookies.Count, resp.ErrorMessage, resp.Content);
            return resp;
        }
        static public IRestResponse cancelordering(IRestClient restClient, string orderNo, Account account)
        {
            var url = account.Email != null && account.Email.Length > 1 ? url14 : url14;
            var req = new CannelOrderReq();
            var resp = RestClientUtil.Post(restClient, url, req, DataFormat.Json);
            LoggerUtil.Info("{0},{1},{2},{3},{4}", resp.StatusCode, resp.StatusDescription, resp.Cookies.Count, resp.ErrorMessage, resp.Content);
            return resp;
        }
        static bool wait(IRestClient restClient, Account account, int sleepMs, int mode, Action act)
        {
            bool b = false;

            //var mre = RestClientUtil.GetMre(mode > 0 ? "mode-" + mode : account.Name);
            // LoggerUtil.Info("wait ....{0}", account.Name);
            // mre.Wait();
            //LoggerUtil.Info("enter....{0}", account.Name);
            account.Use++;
            var now = DateTime.Now;
            var m = (now - account.LastUse).TotalMilliseconds;
            var t = sleepMs - m;
            LoggerUtil.Info("{0:d06} {1} {2} {3} {4:.000} {5:.000}", account.Use, sleepMs, now.ToString("ss.ffff"), account.LastUse.ToString("ss.ffff"), m, t);

            account.LastUse = DateTime.Now;
            b = t > 0;
            if (b)
            {
                // LoggerUtil.Info("sleep:{0} {1:d06} {2}", t, account.Use, account.Name);
                //Thread.Sleep((int)t);
                // account.LastUse = DateTime.Now;
            }
            LoggerUtil.Info("doing....{0}{1}", account.Name, "".PadLeft(40, '-'));
            //mre.Release();
            act.Invoke();
            LoggerUtil.Info("done ....{0}", account.Name, "".PadLeft(40, '-'));
            if (account.Use % 88 == 77)
            {
                account.Cookies = RestClientUtil.GetCookieString(restClient);
            }
            return b;
        }
    }
}
