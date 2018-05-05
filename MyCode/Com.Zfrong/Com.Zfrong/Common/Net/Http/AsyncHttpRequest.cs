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
using Com.Zfrong.Common.Net.Http;
namespace Com.Zfrong.Common.Net.Http.Async
{
    //调用示例:
    //private void button1_Click(object sender, System.EventArgs e)
    //{
    //    HttpRequestResponse xx = new HttpRequestResponse();
    //    xx.DataArrival += new DataArrivalEventHandler(xx_DataArrival);
    //    xx.SendRequest("http://www.triaton.com.cn/dotnet/tree/index.asp","userid=老","Post");
    //    xx.SendRequest("http://www.triaton.com.cn/dotnet/tree/index.asp","userid=老","get");
    //}
    //private void xx_DataArrival(object sender, DataArrivalEventArgs e)
    //{
    //    Console.WriteLine(e.RecievedData);
    //    Console.WriteLine(e.IsComplete);
    //}
    ////========================
    class RequestState : Disposable
    {
        const int BUFFER_SIZE = 1024;
        public System.Text.StringBuilder RequestData;
        public List<byte> RecievedDataBytes;
        public byte[] BufferRead;
        public System.Net.HttpWebRequest Request;
        public System.IO.Stream ResponseStream;
        public Encoding StreamEncoding = Encoding.UTF8;
        public DateTime StartTime = DateTime.Now;
        public object[] Flag;
        public NameValueCollection Headers;
        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            RequestData = new System.Text.StringBuilder("");
            RecievedDataBytes = new List<byte>();//
            Request = null;
            ResponseStream = null;
            Flag = null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            RequestData.Remove(0, RequestData.Length);//
            //RecievedData.Remove(0, RecievedData.Length);//
            if (ResponseStream != null) ResponseStream.Dispose();

            RequestData = null;
            //RecievedData = null;
            RecievedDataBytes.Clear(); RecievedDataBytes = null;
            BufferRead = null;
            Request = null;
            ResponseStream = null;
        }
        ~RequestState()
        {
            if (!IsDisposed)
                this.Dispose();//
        }
        #endregion
    }

    public class DataArrivalEventArgs : System.EventArgs, IDisposable
    {
        private object[] flag;
        private byte[] mRecievedData;
        private bool mIsComplete = false;
        public DataArrivalEventArgs(byte[] Data, bool Complete, object[] Flag)
        {
            flag = Flag;//
            mRecievedData = Data;
            mIsComplete = Complete;
        }
        public byte[] RecievedData
        {
            get
            {
                return mRecievedData;
            }
        }
        public object[] Flag
        {
            get
            {
                return flag;
            }
        }
        public bool IsComplete
        {
            get
            {
                return mIsComplete;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            mRecievedData = null;//
        }

        #endregion
    }
    public class DataCompleteEventArgs : System.EventArgs, IDisposable
    {
        private object[] flag;
        private byte[] mRecievedData;
        public double TimeSpan = 0;
        private NameValueCollection mRecievedHeaders;

        public DataCompleteEventArgs(byte[] Data, NameValueCollection Headers, double timeSpan, object[] Flag)
        {
            this.TimeSpan = timeSpan;
            flag = Flag;//
            mRecievedData = Data;
            mRecievedHeaders = Headers;
        }
        public byte[] RecievedData
        {
            get
            {
                return mRecievedData;
            }
        }
        public object[] Flag
        {
            get
            {
                return flag;
            }
        }
        public NameValueCollection RecievedHeaders
        {
            get
            {
                return mRecievedHeaders;
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            mRecievedData = null;//
        }

        #endregion
    }
    /// <summary>
    /// *  GET
    ///  通过请求URI得到资源
    /// * POST,
    ///  用于添加新的内容
    ///* PUT
    ///  用于修改某个内容
    ///* DELETE,
    ///  删除某个内容
    ///* CONNECT,
    ///  用于代理进行传输，如使用SSL
    ///* OPTIONS
    ///  询问可以执行哪些方法
    ///* PATCH,
    ///  部分文档更改
    ///* PROPFIND, (wedav)
    ///  查看属性
    ///* PROPPATCH, (wedav)
    ///  设置属性
    ///* MKCOL, (wedav)
    ///  创建集合（文件夹）
    ///* COPY, (wedav)
    ///  拷贝
    ///* MOVE, (wedav)
    ///  移动
    ///* LOCK, (wedav)
    ///  加锁
    ///* UNLOCK (wedav)
    ///  解锁
    ///* TRACE
    ///  用于远程诊断服务器
    ///* HEAD
    /// 类似于GET, 但是不返回body信息，用于检查对象是否存在，以及得到对象的元数据
    ///apache2中，可使用Limit，LimitExcept进行访问控制的方法包括：GET, POST, PUT, DELETE, CONNECT, OPTIONS, PATCH, PROPFIND, PROPPATCH, MKCOL, COPY, MOVE, LOCK, 和 UNLOCK.
    ///其中, HEAD GET POST OPTIONS PROPFIND是和读取相关的方法，MKCOL PUT DELETE LOCK UNLOCK COPY MOVE PROPPATCH是和修改相关的方法
    /// </summary>
    public enum Method
    {
        POST, GET, HEAD, OPTIONS, TRACE, PUT, CONNEC
    }
    public delegate void DataArrivalEventHandler(object sender, DataArrivalEventArgs e);
    public delegate void DataCompleteEventHandler(object sender, DataCompleteEventArgs e);
    public delegate void RequestEventHandler(object sender, HttpWebRequest req);
    public class AsyncHttpRequest
    {
        static AsyncHttpRequest() { ServicePointManager.MaxServicePoints = 250; ServicePointManager.DefaultConnectionLimit = 250; }
        public static System.Threading.ManualResetEvent allDone = new System.Threading.ManualResetEvent(false);
        const int BUFFER_SIZE = 1024;
        public event DataArrivalEventHandler DataArrival;
        public event DataCompleteEventHandler DataComplete;
        public event RequestEventHandler Beginning;
        /// <summary>
        ///  请求执行中...
        /// </summary>
        public int Working = 0;
        public int Sleep = 0;
        public int Finished = 0;
        public int Error = 0;
        public long Length = 0;
        public DateTime StartTime = DateTime.Now;
        /// <summary>
        /// 默认200
        /// </summary>
        public static int MaxConcurrency = 200;
        protected virtual void OnDataArrival(DataArrivalEventArgs e)
        {
            if (DataArrival != null)
            {
                DataArrival(this, e);
            }
        }
        protected virtual void OnDataComplete(DataCompleteEventArgs e)
        {
            if (DataComplete != null)
            {
                DataComplete(this, e);
            }
        }
        protected virtual void OnBeginning(HttpWebRequest e)
        {
            if (Beginning != null)
            {
                Beginning(this, e);
            }
        }
        /// <summary>
        /// 地址=url+data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="cookieHeader"></param>
        /// <param name="strReferer"></param>
        /// <param name="proxy"></param>
        /// <param name="flag"></param>
        public void Get(string url, string data, string encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            Send(url, data, Method.GET, false, Encoding.GetEncoding(encoding), cookieHeader, strReferer, userAgent, authorization, proxy, flag);
        }
        /// <summary>
        /// 地址=url+data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="cookieHeader"></param>
        /// <param name="strReferer"></param>
        /// <param name="proxy"></param>
        /// <param name="flag"></param>
        public void Get(string url, string data, Encoding encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            Send(url, data, Method.GET, false, encoding, cookieHeader, strReferer, userAgent, authorization, proxy, flag);
        }
        public void Post(string url, string data, bool isMultiPartFormData, string encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            Send(url, data, Method.POST, isMultiPartFormData, Encoding.GetEncoding(encoding), cookieHeader, strReferer, userAgent, authorization, proxy, flag);
        }
        public void Post(string url, string data, bool isMultiPartFormData, Encoding encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            Send(url, data, Method.POST, isMultiPartFormData, encoding, cookieHeader, strReferer, userAgent, authorization, proxy, flag);
        }


        private void Send(string url, string data, Method method, bool isMultiPartFormData, string encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            Send(url, data, method, isMultiPartFormData, Encoding.GetEncoding(encoding), cookieHeader, strReferer, userAgent, authorization, proxy, flag);
        }
        private void Send(string url, string data, Method method, bool isMultiPartFormData, Encoding encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            HttpWebRequest req = null;

            RequestState rs = BuildState(url, data, method, isMultiPartFormData, encoding, cookieHeader, strReferer, userAgent, authorization, proxy, flag);

            switch (method)
            {
                case Method.POST:
                    #region ###POST
                    req = (HttpWebRequest)WebRequest.Create(url);
                    if (isMultiPartFormData)
                    {
                        req.ContentType = Com.Zfrong.Common.Net.Http.Common.MultiPartFormData_ContentType;
                    }
                    else
                    {
                        req.ContentType = "application/x-www-form-urlencoded";
                    }
                    byte[] Buffer = encoding.GetBytes(rs.RequestData.ToString());
                    req.ContentLength = Buffer.Length;

                    InitRequest(req, method, cookieHeader, strReferer, userAgent, authorization, proxy);//--zfr

                    Stream stream = req.GetRequestStream();
                    stream.Write(Buffer, 0, Buffer.Length);
                    stream.Close(); stream.Dispose();
                    #endregion
                    break;
                case Method.GET:
                    #region ###GET
                    req = (HttpWebRequest)WebRequest.Create(url + "?" + rs.RequestData.ToString());
                    req.ContentType = "text/html";
                    InitRequest(req, method, cookieHeader, strReferer, userAgent, authorization, proxy);//zfr
                    #endregion
                    break;
                default: break;
            }
            OnBeginning(req);//
            this.Working++;
            AsyncSend(req, rs);//
            //rs = null; req.Abort(); req = null;
        }
        private void AsyncSend(HttpWebRequest req, RequestState rs)
        {
            rs.Request = req;
            //System.IAsyncResult r = (System.IAsyncResult)
            Sleep += 1;
            while (Working > MaxConcurrency)
            {
                System.Threading.Thread.Sleep(400);
            }//zfr防止大量并发
            Sleep -= 1;
            req.BeginGetResponse(new System.AsyncCallback(ResponseCallback), rs);
        }
        private RequestState BuildState(string url, string data, Method method, bool isMultiPartFormData, Encoding encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            RequestState rs = new RequestState();
            rs.StreamEncoding = encoding;//
            rs.Flag = flag;
            #region ###data
            if (data != null)
            {
                if (isMultiPartFormData)
                {
                    string str = Com.Zfrong.Common.Net.Http.Common.Build_MultiPartFormData(data, encoding);//
                    rs.RequestData.Remove(0, rs.RequestData.Length);
                    rs.RequestData.Append(str);//
                }
                else
                {
                    int i = 0, j;
                    Char[] reserved = { '?', '=', '&' };
                    while (i < data.Length)
                    {
                        j = data.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            rs.RequestData.Append(System.Web.HttpUtility.UrlEncode(data.Substring(i, data.Length - i), encoding));
                            break;
                        }
                        rs.RequestData.Append(System.Web.HttpUtility.UrlEncode(data.Substring(i, j - i), encoding));
                        rs.RequestData.Append(data.Substring(j, 1));
                        i = j + 1;
                    }
                    reserved = null;
                }
            }
            #endregion
            return rs;
        }
        public string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
        public bool AllowAutoRedirect = true;//
        private void InitRequest(HttpWebRequest httpWebRequest, Method method, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy)
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

        private void ResponseCallback(IAsyncResult ar)
        {
            RequestState rs = (RequestState)ar.AsyncState;

            System.Net.HttpWebRequest req = rs.Request;
            try
            {
                System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)req.EndGetResponse(ar);
                if ((int)resp.StatusCode > 333) { this.Error++; this.Working--; Console.WriteLine("StatusCode:" + resp.StatusCode.ToString()); return; }//zfr
                System.IO.Stream ResponseStream = resp.GetResponseStream();
                rs.Headers = resp.Headers as NameValueCollection;
                rs.ResponseStream = ResponseStream;

                //IAsyncResult iarRead =
                ResponseStream.BeginRead(rs.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), rs);

                //ResponseStream.Close(); ResponseStream.Dispose(); 
                ResponseStream = null; rs = null;
                //req.Abort(); 
                req = null; resp = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                this.Error++; this.Working--; return;
            }
        }
        private void ReadCallBack(IAsyncResult asyncResult)
        {

            RequestState rs = (RequestState)asyncResult.AsyncState;

            System.IO.Stream responseStream = rs.ResponseStream;
            int read = 0;
            try
            {
                read = responseStream.EndRead(asyncResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                this.Error++; this.Working--; return;
            }
            if (read > 0)
            {
                byte[] bb = new byte[read]; Length += read;
                Array.Copy(rs.BufferRead, 0, bb, 0, read);
                rs.RecievedDataBytes.AddRange(bb);
                OnDataArrival(new DataArrivalEventArgs(bb, false, rs.Flag)); bb = null;//
                try
                {
                    responseStream.BeginRead(rs.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), rs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.Error++; this.Working--; return;
                }

            }
            else
            {
                byte[] data;

                data=rs.RecievedDataBytes.ToArray();// 

                responseStream.Close(); responseStream.Dispose(); responseStream = null;
                OnDataArrival(new DataArrivalEventArgs(data, true, rs.Flag));
                this.Finished++; this.Working--;//
                TimeSpan ts = DateTime.Now - rs.StartTime;
                OnDataComplete(new DataCompleteEventArgs(data, rs.Headers, ts.TotalMilliseconds, rs.Flag));
                allDone.Set();
                data = null; rs = null;
            }
            responseStream = null; rs = null;
            return;
        }

    }
    /// <summary>
    /// 异步请求
    /// </summary>
    public class HttpProtocolUtils
    {
        public AsyncHttpRequest xx = new AsyncHttpRequest();
        public event DataCompleteEventHandler Complete;
        public HttpProtocolUtils() { xx.DataComplete += new DataCompleteEventHandler(xx_Complete); }
        public void Send(string url, bool isP, string data, bool isMPFD, string encoding, string cookieHeader, string strReferer, string userAgent, string authorization, IWebProxy proxy, params object[] flag)
        {
            if (!isP)
                xx.Get(url, data, encoding, cookieHeader, strReferer, userAgent, authorization, proxy, flag);
            else
                xx.Post(url, data, isMPFD, encoding, cookieHeader, strReferer, userAgent, authorization, proxy, flag);
        }

        void xx_Complete(object sender, DataCompleteEventArgs e)
        {
            if (Complete != null)
            {
                Complete(sender, e);
            }
        }

    }
}

