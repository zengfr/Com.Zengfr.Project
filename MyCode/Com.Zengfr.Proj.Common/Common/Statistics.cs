using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Collections.Concurrent;
namespace Com.Zengfr.Proj.Common.Web
{
    public class Statistics
    {
        public class RequestLog
        {
            public virtual double TimeSpan { get; set; }

            public virtual DateTime DateTime { get; set; }
            public virtual int Millisecond { get; set; }
            public virtual int ContentLength { get; set; }

            public virtual string Type { get; set; }
            public virtual string Ip { get; set; }
            public virtual string SessionId { get; set; }

            public virtual string BrowserPlatform { get; set; }
            public virtual string Browser { get; set; }
            public virtual string BrowserVersion { get; set; }
            public virtual string UserAgent { get; set; }

            public virtual string UrlPage { get; set; }
            public virtual string ReferrerPage { get; set; }

            public virtual string Url { get; set; }
            public virtual string ReferrerUrl { get; set; }

            public RequestLog(string type)
            {
                TimeSpan = -1;
                if (HttpContext.Current.Items.Contains("start"))
                {
                    TimeSpan = (DateTime.Now - (DateTime)HttpContext.Current.Items["start"]).TotalMilliseconds;
                }
                Type = type;
                var request = HttpContext.Current.Request;

                DateTime = DateTime.Now;
                Millisecond = DateTime.Now.Millisecond;
                ContentLength = request.ContentLength;

                Ip = request.ServerVariables["REMOTE_ADDR"];
                ReferrerUrl = request.UrlReferrer != null ? request.UrlReferrer.ToString() : string.Empty;
                ReferrerPage = request.UrlReferrer != null ? request.UrlReferrer.LocalPath : string.Empty;
                Url = request.Url.ToString();
                UrlPage = request.Url.LocalPath;

                UserAgent = request.UserAgent;
                BrowserPlatform = request.Browser.Platform;
                Browser = request.Browser.Browser;
                BrowserVersion = request.Browser.Version;

                SessionId = HttpContext.Current.Session == null ? string.Empty : HttpContext.Current.Session.SessionID;
                if (string.IsNullOrWhiteSpace(SessionId))
                {
                    string cookie = request["http_cookie"];
                    SessionId = cookie != null && cookie.Length >= 18 + 24 ? cookie.Substring(18, 24) : string.Empty;
                }
                if (string.IsNullOrWhiteSpace(SessionId))
                {
                    SessionId = request.AnonymousID;
                }
            }
        }
        private static Statistics instance = null;
        private static object lockObj = new object();

        public static Statistics Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Statistics();
                    }
                }
                return instance;
            }
        }
        public SynchronizedCollection<RequestLog> RequestLogs { get; set; }
        public Statistics()
        {
            RequestLogs = new SynchronizedCollection<RequestLog>();
        }
        public void AddLog()
        {
            AddLog(new Statistics.RequestLog("0"));
        }
        public void AddLog(RequestLog log)
        {
            RequestLogs.Add(log);
            if (RequestLogs.Count > 1000)
            {
                RequestLogs.RemoveAt(0);
            }

        }
    }
}
