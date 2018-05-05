using System;
//using iTextSharp.tool.xml.css;
//using iTextSharp.tool.xml;
//using iTextSharp.tool.xml.pipeline.css;
//using iTextSharp.tool.xml.parser;
//using iTextSharp.text;
//using iTextSharp.tool.xml.pipeline.html;
//using iTextSharp.tool.xml.html;
//using iTextSharp.tool.xml.pipeline.end;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Com.Zengfr.Proj.Common
{
    public partial class HtmlToPdfUtils
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HtmlToPdfUtils));
        public static void HttpInit()
        {
            System.GC.Collect();
            ThreadPool.SetMaxThreads(512, 512);
            ServicePointManager.MaxServicePoints = 1024;
            ServicePointManager.DefaultConnectionLimit = 1024;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.EnableDnsRoundRobin = true;
            WebRequest.DefaultWebProxy = null;
            HttpWebRequest.DefaultWebProxy = null;
        }
        #region
        public static string DownloadText(string url, HttpCookieCollection cookies, string encoding, bool async = true)
        {
            var cookiesStr = string.Empty;
            if (cookies != null)
            {
                for (int j = 0; j < cookies.Count; j++)
                {
                    HttpCookie cookie = cookies.Get(j);
                    cookiesStr += string.Format("{0}={1};", cookie.Name, cookie.Value);
                }
            }
            return DownloadText(url, cookiesStr, encoding, async);
        }
        public static string DownloadText(string url, string cookies, string encoding, bool async = true)
        {
            HttpInit();
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(url));
            servicePoint.ConnectionLimit = 1024;
            servicePoint.Expect100Continue = false;
            servicePoint.UseNagleAlgorithm = false;

            var request = WebRequest.Create(url) as HttpWebRequest;

            var servicePoint2 = ServicePointManager.FindServicePoint(new Uri(url));
            servicePoint2.ConnectionLimit = 1024;
            servicePoint2.Expect100Continue = false;
            servicePoint2.UseNagleAlgorithm = false;

            string content = string.Empty;
            var connectionGroupName = request.RequestUri.ToString().GetHashCode().ToString();
            DateTime start = DateTime.Now;
            try
            {
                request.UnsafeAuthenticatedConnectionSharing = true;
                request.UseDefaultCredentials = false;
                request.Proxy = null;
                //request.Connection = "Close";
                //request.Headers.Clear();
                //request.Headers.Add(HttpRequestHeader.Connection, "Close");
                request.Method = "GET";
                request.KeepAlive = false;
                request.AllowWriteStreamBuffering = false;
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.ConnectionLimit = 1024;
                request.ServicePoint.UseNagleAlgorithm = false;
                request.SendChunked = false;
                request.ConnectionGroupName = connectionGroupName;
                request.Headers.Add(HttpRequestHeader.Cookie, cookies);
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:38.0) Gecko/20100101 Firefox/38.0";
                logger.InfoFormat("DownloadText->Async:{0},{1},{2},{3},Url:{4}", async,
                    request.ServicePoint.Expect100Continue,
                    request.ServicePoint.ConnectionLimit,
                    request.ServicePoint.CurrentConnections, url);
                if (async)
                {
                    CountdownEvent countdown = new CountdownEvent(2);
                    request.BeginGetResponse(new AsyncCallback((asyncResult) =>
                    {
                        logger.InfoFormat("DownloadText.BeginGetResponse->Async:{0},TimeSpan:{1},Url:{2}", async, (DateTime.Now - start).TotalSeconds, url);
                        countdown.Signal();
                        try
                        {
                            var req = (asyncResult.AsyncState as HttpWebRequest);
                            using (HttpWebResponse resp = req.EndGetResponse(asyncResult) as HttpWebResponse)
                            {
                                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(encoding)))
                                {
                                    content = reader.ReadToEnd();
                                    reader.Close();
                                }
                                resp.Close();
                            }
                        }
                        catch (Exception ex) { content += ex.Message; }
                        countdown.Signal();
                    }), request);
                    countdown.Wait();
                    countdown.Dispose(); countdown = null;
                }
                else
                {
                    using (var resp = request.GetResponse())
                    {
                        using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(encoding)))
                        {
                            content = reader.ReadToEnd();
                        }
                        resp.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                content += ex.Message;
            }
            finally
            {
                request.ServicePoint.CloseConnectionGroup(connectionGroupName);
                request.Abort();
                request = null;
            }
            logger.InfoFormat("DownloadText->Async:{0},TimeSpan:{1},Url:{2}", async, (DateTime.Now - start).TotalSeconds, url);
            return content;
        }

    }
    public partial class HtmlToPdfUtils
    {
        public static string GetSiteBaseURL()
        {
            string siteUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            siteUrl = siteUrl.Replace(HttpContext.Current.Request.Url.Query, "");
            siteUrl = siteUrl.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");

            if (siteUrl.EndsWith("/"))
                siteUrl = siteUrl.Substring(0, siteUrl.LastIndexOf("/"));
            return siteUrl;
        }
        #endregion
        /// <summary>
        /// 图片生成PDF
        /// </summary>
        /// <param name="iamge"></param>
        /// <returns></returns>
        public static MemoryStream ImageToPdf(ImageFormat imageFormat, params System.Drawing.Image[] images)
        {
            //http://www.mikesdotnetting.com/article/87/itextsharp-working-with-images
            var ms = new MemoryStream();
            using (var document = new Document())
            {
                document.SetPageSize(iTextSharp.text.PageSize.A4);
                var h = document.PageSize.Height - document.TopMargin - document.BottomMargin;
                var w = document.PageSize.Width - document.RightMargin - document.LeftMargin;
                using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
                {
                    writer.CloseStream = false;
                    document.Open();
                    foreach (var image in images)
                    {
                        if (image != null)
                        {
                            try
                            {
                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(image, imageFormat);

                                if (img.Height > h)
                                {
                                    img.ScaleToFit(w, h);
                                }
                                else if (image.Width > w)
                                {
                                    img.ScaleToFit(w, h);
                                }
                                //img.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                                document.Add(img);
                            }
                            catch (Exception ex)
                            {
                                logger.InfoFormat("ImageToPdf->Exception:{0},{1}", ex.Message, ex.StackTrace);
                            }
                        }
                    }
                    document.Close();
                }
            }
            if (ms.CanSeek)
                ms.Position = 0;
            return ms;
        }

    }
}
