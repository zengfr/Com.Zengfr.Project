using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using cc;
using NCrawler;
using NCrawler.Interfaces;
using NCrawler.Services;

namespace Craw
{
    public class CCrawler: Crawler
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CCrawler));

        public CCrawler(Uri crawlStart, params IPipelineStep[] pipeline) : base(crawlStart, pipeline)
        {

        }
        public void Init()
        {
            ServicePointManager.MaxServicePoints = 65535;
            ServicePointManager.DefaultConnectionLimit = 65535;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;
            System.Net.ServicePointManager.UseNagleAlgorithm = false;

            this.MaximumThreadCount = 25;
            this.MaximumCrawlDepth = 10;
            this.UseCookies = true; 
            this.ExcludeFilter = new[] {
                new RegexFilter(
            new Regex(@"(\.svg|\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico|\.png|\.swf|\.fla|\.flv|\.pdf|\.mp3|\.mp4|\.m4v|\.3gp|\.wmv|\.wav|\.apk|\.rar|\.zip|\.7z|\.dll|\.chm|\.exe|\.ppt|\.doc|\.xls)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
            };
            
            this.AfterDownload += c_AfterDownload;
            this.DownloadException += c_DownloadException;
            this.PipelineException += c_PipelineException;
            this.DownloadProgress += c_DownloadProgress;
        }
        static void c_DownloadProgress(object sender, NCrawler.Events.DownloadProgressEventArgs e)
        {
            //if (e.PercentCompleted < 100)
            {
                //log.InfoFormat("DownloadProgress:{0},{1},{2},{3},{4}", e.DownloadTime.TotalMilliseconds, e.PercentCompleted, e.BytesReceived, e.TotalBytesToReceive, e.Step.Uri);
            }
        }

        static void c_PipelineException(object sender, NCrawler.Events.PipelineExceptionEventArgs e)
        {
            log.ErrorFormat("PipelineException:{0}", e.Exception.ToString());
        }

        static void c_DownloadException(object sender, NCrawler.Events.DownloadExceptionEventArgs e)
        {
            log.ErrorFormat("DownloadException:{0}", e.Exception.ToString());
        }
        static http_user_agent http_user_agent = new http_user_agent();
        static void c_AfterDownload(object sender, NCrawler.Events.AfterDownloadEventArgs e)
        {
            Crawler c = sender as Crawler;
            if (c != null)
            {
                c.UserAgent = http_user_agent.NextUserAgent();
            }
        }

       
    }
}
