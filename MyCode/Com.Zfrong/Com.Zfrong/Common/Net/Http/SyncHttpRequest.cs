using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
namespace Com.Zfrong.Common.Net.Http
{
    /// <summary>
    /// 作者：曾繁荣 2008.11 同步请求
    /// </summary>
    public partial class SyncHttpRequest
    {
        #region ####Post提交网页

        public static string Post(string url, IList<KeyValuePair<string, string>> data, bool isMultiPartFormData, string cookieHeader, string encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            return Post(url, Common.KVToString(data), isMultiPartFormData, cookieHeader, encoding, strReferer, userAgent, authorization, proxy);
        }
        public static string Post(string url, Dictionary<string, string> data, bool isMultiPartFormData, string cookieHeader, string encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            return Post(url, Common.KVToString(data), isMultiPartFormData, cookieHeader, encoding, strReferer, userAgent, authorization, proxy);
        }
        public static string Post(string url, string dataStr, bool isMultiPartFormData, string cookieHeader, string encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            return Post(url, dataStr, isMultiPartFormData, cookieHeader, Encoding.GetEncoding(encoding), strReferer, userAgent, authorization, proxy);
        }
        public static string Post(string url, string dataStr, bool isMultiPartFormData, string cookieHeader, Encoding encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";//"application/x-www-form-urlencoded; charset=utf-8";//

            if (isMultiPartFormData)
            {
                httpWebRequest.ContentType = Common.MultiPartFormData_ContentType;
                dataStr = Common.Build_MultiPartFormData(dataStr, encoding);//
            }
            InitRequest(httpWebRequest, cookieHeader, strReferer, userAgent, authorization, proxy);//

            byte[] setb = encoding.GetBytes(dataStr);
            httpWebRequest.ContentLength = setb.Length;

            System.IO.Stream s = httpWebRequest.GetRequestStream();
            s.Write(setb, 0, setb.Length);
            s.Close(); s = null; setb = null;
            Common.Show(string.Format("发送-->地址:{0}", url));
            //Show(string.Format("发送-->长度:{0}/{1}", httpWebRequest.Headers.Count, setb.Length));//
            //Show(string.Format("发送-->数据:{0}", dataStr));
            string rtn = SendAndGetResult(httpWebRequest, encoding);
            if (httpWebRequest != null)
                httpWebRequest.Abort();
            httpWebRequest = null;
            return rtn;
        }

        #region ####InitRequest
        public static string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
        public static bool AllowAutoRedirect = true;//
        private static void InitRequest(HttpWebRequest httpWebRequest, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            // ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            httpWebRequest.AllowAutoRedirect = AllowAutoRedirect;
            httpWebRequest.MaximumResponseHeadersLength = -1;
            // CookieContainer cookieContain = new CookieContainer();
            // httpWebRequest.CookieContainer = cookieContain;
            httpWebRequest.UserAgent = userAgent;
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            // httpWebRequest.Accept ="text/html,application/xhtml+xml,text/javascript,text/xml,application/xml,application/x-javascript;q=0.9,*/*;q=0.8";
            httpWebRequest.Headers[HttpRequestHeader.CacheControl] = "no-cache";//"max-age=0";//

            httpWebRequest.Headers["Accept-Encoding"] = "gzip,deflate";
            httpWebRequest.Headers["Accept-Charset"] = "UTF-8,*";//ISO-8859-1,
            httpWebRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

            //httpWebRequest.Headers["Proxy-Connection"] = "keep-alive";

            httpWebRequest.Headers["Keep-Alive"] = "60";//timeout=5, max=200";//
            httpWebRequest.KeepAlive = true;// false;//
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;//.Version10; 
            Common.SetHeaderValue(httpWebRequest.Headers, "Connection", "Keep-Alive");
            //httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
            // httpWebRequest.Headers["X-Prototype-Version"] = "1.6.1";

            // httpWebRequest.IfModifiedSince =DateTime.Today.AddDays(-8);
            httpWebRequest.Timeout = 1000 * 60;//
            httpWebRequest.ReadWriteTimeout = 1000 * 60;
            //httpWebRequest.SendChunked = true;
            //httpWebRequest.TransferEncoding = "chunked";
            httpWebRequest.Headers["Pragma"] = "no-cache";//
            if (proxy != null)
            {
                httpWebRequest.Proxy = proxy;
                //if (IsDebug){ Show("Set->Proxy:" + proxy.Credentials.)}
            };
            if (strReferer != "" && strReferer != null)
            {
                httpWebRequest.Referer = strReferer;
                 Common.Show("SET->Referer:" + strReferer);
            }
            if (cookieHeader != "" && cookieHeader != null)
            {
                httpWebRequest.Headers.Add("Cookie:" + cookieHeader);
                Common.Show("SET->Cookies:" + cookieHeader); 
            }
        }
        #endregion

        #endregion

        #region ####Get获取网页

        public static string Get(string url, string cookieHeader, string encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            return Get(url, cookieHeader, Encoding.GetEncoding(encoding), strReferer, userAgent, authorization, proxy);
        }
        public static string Get(string url, string cookieHeader, Encoding encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            // Show(string.Format("开始-->初始化"));//
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/html";
            httpWebRequest.Method = "GET";
            InitRequest(httpWebRequest, cookieHeader, strReferer, userAgent, authorization, proxy);//
            Common.Show(string.Format("获取-->地址:{0}", url));
            string rtn = SendAndGetResult(httpWebRequest, encoding);
            if (httpWebRequest != null)
                httpWebRequest.Abort();
            httpWebRequest = null;
            return rtn;
        }
        public static byte[] GetBytes(string url, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            Common.Show(string.Format("HTTP->INIT:{0}", url));
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/html";
            httpWebRequest.Method = "GET";
            InitRequest(httpWebRequest, cookieHeader, strReferer, userAgent,null, proxy);//
            if (authorization != null)
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = authorization;//
            Common.Show(string.Format("HTTP->GET:{0}", url));
            byte[] data = SendAndGetResultForBytes(httpWebRequest);
            if (httpWebRequest != null)
            {
                httpWebRequest.Abort(); httpWebRequest = null;
            }
            return data;
        }
        #endregion

        #region ####Get获取图片/验证码
        public static Bitmap GetImage(string url)
        {
            return GetImage(url, "", "");
        }
        public static Bitmap GetImage(string url, string cookieHeader)
        {
            return GetImage(url, cookieHeader, url);
        }
        public static Bitmap GetImage(string url, string cookieHeader, string strReferer)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            if (strReferer != "")
                httpWebRequest.Referer = strReferer;
            if (cookieHeader != "")
                httpWebRequest.Headers.Add("Cookie:" + cookieHeader);
            Common.Show(string.Format("请求-->地址:{0}", url));
            Bitmap rtn = SendAndGetResult(httpWebRequest);
            if (httpWebRequest != null)
                httpWebRequest.Abort();
            httpWebRequest = null;
            return rtn;//
        }

        #endregion

        #region #####SendAndGetResult发送返回
        private static byte[] SendAndGetResultForBytes(HttpWebRequest httpWebRequest)
        {
            byte[] data = null;
            try
            {
                while (!Common.IsConnected)
                {
                    Common.Show("STAT->网线断开...暂停8秒..."); System.Threading.Thread.Sleep(1000 * 8);//
                }
                Common.Show("SEND->GET->Result...");
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Common.Show(string.Format("RTN->STAT:{0}\t{1}", response.StatusCode, response.StatusDescription));//
                MemoryStream memoryStream = new MemoryStream(0x51200);

                using (Stream responseStream = Common.GetStream(response))
                {
                    byte[] buffer = new byte[0x5120];
                    int bytes;
                    while ((bytes = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytes);
                    }
                }
                data = memoryStream.ToArray(); memoryStream = null;
                Common.Show(string.Format("RTN->LEN:{0}\r\n", data.Length));//
                response.Close(); response = null;
            }
            catch (Exception ex)
            {
                Common.Show(string.Format("ERR->{0}", ex.Message));//
                //throw ex;
            }
            return data;
        }
        private static Bitmap SendAndGetResult(HttpWebRequest httpWebRequest)
        {
            Bitmap bmp = null;
            try
            {
                while (!Common.IsConnected)
                {
                    Common.Show("Stat->网线断开...暂停8秒..."); System.Threading.Thread.Sleep(1000 * 8);//
                }
                Common.Show("SEND->GET->Result...");
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Common.Show(string.Format("RTN->STAT:{0}\t{1}", response.StatusCode, response.StatusDescription));//
                bmp = new Bitmap(Common.GetStream(response));
                response.Close(); response = null;
                //Show(string.Format("请求-->标头:{0}", httpWebRequest.Headers.ToString()));
            }
            catch (Exception ex)
            {
                Common.Show(string.Format("ERR->{0}", ex.Message));//
                //throw ex;
            }
            return bmp;
        }
        private static string SendAndGetResult(HttpWebRequest httpWebRequest, Encoding encoding)
        {
            string outPut;
            try
            {
                while (!Common.IsConnected)
                {
                    Common.Show("Stat->网线断开...暂停8秒..."); System.Threading.Thread.Sleep(1000 * 8);//
                }
                Common.Show("SEND->GET->Result...");
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string[] cookies = httpWebResponse.Headers.GetValues("Set-Cookie");
                string location = httpWebResponse.Headers.Get("Location");
                Common.Show(string.Format("RTN->STAT:{0}\t{1}", httpWebResponse.StatusCode, httpWebResponse.StatusDescription));//

                Stream stream; System.IO.StreamReader sr;
                stream = Common.GetStream(httpWebResponse);
                sr = new System.IO.StreamReader(stream, encoding);
                //Show(string.Format("请求-->标头:{0}", httpWebRequest.Headers.ToString()));
                outPut = sr.ReadToEnd();
                sr.Close(); sr.Dispose(); stream.Close(); stream.Dispose(); httpWebResponse.Close();
                sr = null; stream = null; httpWebResponse = null;
                if (cookies != null && cookies.Length != 0)
                {
                    outPut += "<cookies>";
                    foreach (string cookie in cookies)
                        outPut += string.Format("{0};", cookie);//
                    outPut += "</cookies>";
                }
                if (location != null && location.Length != 0)
                    outPut += string.Format("<location>{0}</location>", location);//
                if (outPut.Length < 250)
                {
                    Common.Show(string.Format("RTN->DATA:{0}", Regex.Replace(outPut, "[^\u4e00-\u9fa5]", "")));//
                }
                Common.Show(string.Format("RTN->LEN:{0}\r\n", outPut.Length));//
                cookies = null; location = null;
            }
            catch (Exception ex)
            {
                Common.Show(string.Format("ERR->{0}", ex.Message));//
                outPut = ex.Message;//
                //throw ex;
            }
            return outPut;
        }
        private static string SendAndGetResultF(HttpWebRequest httpWebRequest, Encoding encoding)
        {
            string outPut = "";
            try
            {
                Common.Show("SendAndGetResult...");
                Common.Show(string.Format("RTN->STAT:{0}\t{1}\r\n", "OK", "OK"));
                Common.Show(string.Format("RTN->LEN:0\r\n"));//
            }
            catch (Exception ex)
            {
                Common.Show(string.Format("ERR->{0}\r\n", ex.Message));//
            }
            return outPut;
        }
         #endregion

       
    }
}
