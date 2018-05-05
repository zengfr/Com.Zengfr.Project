using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Web;

namespace Com.Zengfr.Proj.Common
{
    public class Thumbnail
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Thumbnail));
        public string Url { get; set; }
        public Bitmap ThumbnailImage { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BrowserWidth { get; set; }
        public int BrowserHeight { get; set; }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);


        public Thumbnail(string url, int browserWidth, int browserHeight, int thumbnailWidth, int thumbnailHeight)
        {
            Init(url, browserWidth, browserHeight, thumbnailWidth, thumbnailHeight);
        }
        public Thumbnail(string url, HttpCookieCollection cookies, int browserWidth, int browserHeight, int thumbnailWidth, int thumbnailHeight)
        {
            Init(url, browserWidth, browserHeight, thumbnailWidth, thumbnailHeight);
            if (cookies != null)
            {
                for (int j = 0; j < cookies.Count; j++)
                {
                    HttpCookie cookie = cookies.Get(j);
                    InternetSetCookie(url, cookie.Name, cookie.Value);
                }
            }
        }
        private void Init(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            this.Url = Url;
            this.BrowserWidth = BrowserWidth;
            this.BrowserHeight = BrowserHeight;
            this.Height = ThumbnailHeight;
            this.Width = ThumbnailWidth;
        }
        public Bitmap GenerateThumbnail()
        {
            var startTime = DateTime.Now;
            Thread thread = new Thread(new ThreadStart(GenerateThumbnailInteral));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(1000 * 120);
            logger.InfoFormat("GenerateThumbnail->{0},{1}", (DateTime.Now - startTime).TotalMilliseconds, this.Url);
            return ThumbnailImage;
        }
        private void GenerateThumbnailInteral()
        {
            HtmlToPdfUtils.HttpInit();
            using (WebBrowser webBrowser = new WebBrowser())
            {
                webBrowser.Navigate("about:blank");
                webBrowser.ScrollBarsEnabled = false;
                webBrowser.ScriptErrorsSuppressed = true;
                webBrowser.Navigate(this.Url);
                webBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(webBrowser_ProgressChanged);
                webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                while (webBrowser.ReadyState != WebBrowserReadyState.Complete || CheckIsNotCompleted(webBrowser.Url))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);
                }
                webBrowser.Dispose();
            }
        }


        private bool CheckIsNotCompleted(Uri uri)
        {
            var url = uri == null ? string.Empty : uri.ToString();
            if (url.Length <= 12 || url != this.Url)
            {
                return true;
            }
            return false;
        }
        void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            WebBrowser webBrowser = (WebBrowser)sender;

            if (CheckIsNotCompleted(webBrowser.Url))
            {
                return;
            }
            if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                DrawToBitmap(webBrowser);
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (CheckIsNotCompleted(e.Url))
            {
                return;
            }
            WebBrowser webBrowser = (WebBrowser)sender;
            DrawToBitmap(webBrowser);
        }
        private void DrawToBitmap(WebBrowser webBrowser)
        {
            webBrowser.ClientSize = new Size(this.BrowserWidth, this.BrowserHeight);
            webBrowser.ScrollBarsEnabled = false;
            this.ThumbnailImage = new Bitmap(webBrowser.Bounds.Width, webBrowser.Bounds.Height);
            webBrowser.BringToFront();
            webBrowser.DrawToBitmap(ThumbnailImage, webBrowser.Bounds);
            this.ThumbnailImage = (Bitmap)ThumbnailImage.GetThumbnailImage(Width, Height, null, IntPtr.Zero);
        }
    }
}
