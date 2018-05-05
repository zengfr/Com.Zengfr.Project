using System;
using System.Drawing;
using System.IO;
using System.Web;
using NReco.ImageGenerator;
namespace Com.Zengfr.Proj.Common
{
    public class HtmlToImageConverterUtils
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HtmlToImageConverterUtils));
        private static HtmlToImageConverter InitHtmlToImageConverter(HttpCookieCollection cookies, int height = 0, double zoom = 1, int javascript_delay = 200)
        {
            var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();
            htmlToImageConv.Height = height;
            //htmlToImageConv.CustomArgs = " --load-media-error-handling ignore ";
            htmlToImageConv.CustomArgs = string.Format("--encoding {0} --zoom {1} --javascript-delay {2} --no-stop-slow-scripts --load-error-handling ignore", "UTF-8", zoom, javascript_delay);

            string cookieArgs = "";
            if (cookies != null)
            {
                var sb = new System.Text.StringBuilder();

                // you probably only need the ".ASPXFORMSAUTH"
                // and "ASP.NET_SessionId" cookies
                // but I pass everything just in case
                foreach (string key in cookies.AllKeys)
                {
                    string value = cookies[key].Value;
                    sb.AppendFormat("--cookie {0} {1} ", key, value);
                }
                cookieArgs = sb.ToString();
            }
            if (!string.IsNullOrWhiteSpace(cookieArgs))
                htmlToImageConv.CustomArgs += " " + cookieArgs + " -";
            return htmlToImageConv;
        }
        public static System.Drawing.Image HtmlToImageConverter(string url, string html)
        {
            return HtmlToImageConverter(url, html, NReco.ImageGenerator.ImageFormat.Jpeg);
        }
        public static System.Drawing.Image HtmlToImageConverter(string url, string html, string imageFormat, int height = 0, double zoom = 1, int javascript_delay = 200)
        {
            DateTime start = DateTime.Now;
            var htmlToImageConv = InitHtmlToImageConverter(null, height, zoom, javascript_delay);
            var bytes = htmlToImageConv.GenerateImage(html, imageFormat);
            var image = BytesToImage(bytes);
            var file = string.Format("{0}{1}.jpg", HttpContext.Current.Request.MapPath("~"), DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            logger.InfoFormat("HtmlToImageConverter->Delay:{0},TimeSpan:{1},Url:{2},{3}", javascript_delay, (DateTime.Now - start).TotalSeconds, file, url);
            bytes = null;
            return image;

        }
        public static System.Drawing.Image HtmlToImageConverterViaUrl(string url, HttpCookieCollection cookies, string imageFormat, int height = 0, double zoom = 1, int javascript_delay = 200)
        {
            DateTime start = DateTime.Now;
            var htmlToImageConv = InitHtmlToImageConverter(cookies, height, zoom, javascript_delay);
            var bytes = htmlToImageConv.GenerateImageFromFile(url, imageFormat);
            var image = BytesToImage(bytes);
            var file = string.Format("{0}{1}.jpg", HttpContext.Current.Request.MapPath("~"), DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            //image.Save(file);
            logger.InfoFormat("HtmlToImageConverterViaUrl->Delay:{0},TimeSpan:{1},Url:{2},{3}", javascript_delay, (DateTime.Now - start).TotalSeconds, file, url);
            bytes = null;
            return image;

        }
        private static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms, true, true);
            return image;
        }
    }
}
