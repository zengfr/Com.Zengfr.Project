using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Zfrong.Amib.Threading;
using HtmlAgilityPack;
namespace TaskApplication.Task
{
    public class SpiderWorker : WorkerBase
    {
        static Hashtable host = new Hashtable();
        protected override void DoTask<T>(T t)
        {
            SpiderTask task = t as SpiderTask;
            if (task != null)
            {
                string url = task.URL;
                if (url.IndexOf('?') != -1)
                    url = url.Replace("?", "?qq=362505707&");
                else
                    url += "?qq=362505707";
                Uri currentUri = new Uri(url);
                //if (host.ContainsKey(currentUri.Host))
                 //   return;
                //host.Add(currentUri.Host, null);
                HtmlWeb hw = new HtmlWeb();
                hw.UseCookies = true; hw.UsingCache = false;
                hw.AutoDetectEncoding = true; 
               //url = "http://www.baidu.com";
                string html = GetHtml(url);
                HtmlDocument doc = new HtmlDocument(); doc.LoadHtml(html); html = null;
               //  hw.Load(url,"GET",new System.Net.WebProxy(),null);
                task.TaskInfo.StartTime = DateTime.Now;
                HtmlAttribute att; Uri nextUri;
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                   att = link.Attributes["href"];
                   Uri.TryCreate(currentUri, att.Value, out nextUri);
                     //string[] links=GetHtmlUrlList(doc.DocumentNode.OuterHtml);
                    // foreach (string link in links)
                     {
                        // Uri.TryCreate(currentUri, link, out nextUri);
                         //if (!host.ContainsKey(nextUri.Host))
                         // if(MatchHost(currentUri.Host,nextUri.Host))
                         {
                             SpiderQueue.Instance.Enqueue(nextUri.AbsoluteUri);
                         }
                     }
                }
                hw = null; 
                doc = null; url = null; att = null; nextUri = null; currentUri = null;
            }
        }
        static string GetHtml(string url)
        {
            string rtn = string.Empty;
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ServicePoint.ConnectionLimit = 65535;
            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.Expect100Continue = false;
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            WebProxy myProxy = new WebProxy();
            myProxy.IsBypassed(new Uri(url));
            request.Proxy = myProxy;
            Console.WriteLine("request:{0}", url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream, true))
                    {
                        rtn = reader.ReadToEnd();
                    }
                }
            }
            //阻塞
            if (request != null)
            {
                request.Abort();
            }
            request = null;
            return rtn;
        }
        static bool MatchHost(string h1, string h2)
        {
            return GetDomainName(h1) == GetDomainName(h2);
        }
        static string GetDomainName(string host)
        {
            string domain = host;

            string[] sectons = domain.Split('.');

            if (sectons.Length >= 3)
            {
                domain = string.Join(".", sectons, sectons.Length - 2, 2);
            }

            return domain;
        }
        // 定义正则表达式用来匹配 a 标签
        static Regex hr = new Regex(@"<a\b[^<>]*?\bhref[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<hrURL>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
        public static string[] GetHtmlUrlList(string sHtmlText)
        {

            // 搜索匹配的字符串
            MatchCollection matches = hr.Matches(sHtmlText);

            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["hrURL"].Value;
            matches = null;
            return sUrlList;
        }




    }
}
