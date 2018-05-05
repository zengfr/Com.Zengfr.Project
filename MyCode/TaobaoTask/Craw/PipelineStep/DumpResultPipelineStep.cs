using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrawler;
using NCrawler.Interfaces;
using log4net;
using System.Threading;

namespace  Craw
{
    public class DumpResultProcessor : IPipelineStep
    {
        protected static log4net.ILog logger = LogManager.GetLogger(typeof(DumpResultProcessor));
        protected long VisitedCount = 0;
        protected DateTime VisitedStart = DateTime.Now;
        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            Interlocked.Increment(ref VisitedCount);
            var sp = (DateTime.Now - VisitedStart).TotalMilliseconds / VisitedCount;
            var info = string.Format("{0},{1},{2},{3}", crawler.ThreadsInUse, VisitedCount,crawler.WaitingQueueLength, sp.ToString("0.00"));
            var info2 =string.Format("{0,7},{1,3},{2,10},{3,10},{4},{5},{6}",
                 (int)propertyBag.StatusCode,
                 propertyBag.DownloadTime.TotalMilliseconds.ToString("0.0"),
                 propertyBag.CharacterSet,
                 propertyBag["DetectEncoding"].Value,//--

                 propertyBag.Step.Depth,
                 propertyBag.Title,
                 propertyBag.Step.Uri);

            logger.InfoFormat("F:{0},{1}", info, info2);
            info = null;info2 = null;

            ClearResult(propertyBag);
        }
        public static void ClearResult(PropertyBag propertyBag)
        {
            propertyBag.Text = null;
            propertyBag.Title = null;
            
            propertyBag["HtmlDoc"].Value = null;
            propertyBag["Meta"].Value = null;

             propertyBag.GetResponse = null;
            var a = propertyBag.Headers;a = null;
            var b = propertyBag.Referrer; b = null;
            var c= propertyBag.Step; c = null;
            propertyBag = null;
        }
    }
}
