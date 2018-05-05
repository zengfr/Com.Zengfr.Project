using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Timers;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Com.Zengfr.Proj.Common
{
    public class HtmlCapture : IDisposable
    {
        #region
        [ComVisible(true), ComImport()]
        [GuidAttribute("0000010d-0000-0000-C000-000000000046")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IViewObject
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Draw(
                [MarshalAs(UnmanagedType.U4)] UInt32 dwDrawAspect,
                int lindex,
                IntPtr pvAspect,
                [In] IntPtr ptd,
                IntPtr hdcTargetDev,
                IntPtr hdcDraw,
                [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcBounds,
                [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcWBounds,
                IntPtr pfnContinue,
                [MarshalAs(UnmanagedType.U4)] UInt32 dwContinue);
            [PreserveSig]
            int GetColorSet([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
               int lindex, IntPtr pvAspect, [In] IntPtr ptd,
                IntPtr hicTargetDev, [Out] IntPtr ppColorSet);
            [PreserveSig]
            int Freeze([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                            int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);
            [PreserveSig]
            int Unfreeze([In, MarshalAs(UnmanagedType.U4)] int dwFreeze);
            void SetAdvise([In, MarshalAs(UnmanagedType.U4)] int aspects,
              [In, MarshalAs(UnmanagedType.U4)] int advf,
              [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);
            void GetAdvise([In, Out, MarshalAs(UnmanagedType.LPArray)] int[] paspects,
              [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] advf,
              [In, Out, MarshalAs(UnmanagedType.LPArray)] IAdviseSink[] pAdvSink);
        }

        #endregion


        #region
        public static Bitmap CaptureSync(string url, int width, int height)
        {
            Bitmap image = null;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                System.Threading.CountdownEvent countdownEvent = new System.Threading.CountdownEvent(1);
                CaptureAsync(url, width, height, (img) =>
                {
                    image = img;
                    countdownEvent.Signal();
                });
                countdownEvent.Wait();
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return image;
        }
        public static void CaptureAsync(string url, int width, int height, Action<Bitmap> callback)
        {
            HtmlCapture hc = new HtmlCapture();
            hc.HtmlImageCapture += new HtmlCapture.HtmlCaptureEvent((s, u, b) =>
            {
                callback(b);
            });
            if (width <= 0 || height <= 0)
            {
                hc.Create(url);
            }
            else
            {
                hc.Create(url, new Size(width, height));
            }
        }
        #endregion
        private WebBrowser web;
        private System.Windows.Forms.Timer tready;
        private Rectangle screen;
        private Size? imgsize = null;

        //an event that triggers when the html document is captured
        public delegate void HtmlCaptureEvent(object sender,
                             Uri url, Bitmap image);
        public event HtmlCaptureEvent HtmlImageCapture;

        //class constructor
        public HtmlCapture()
        {
            //initialise the webbrowser and the timer
            web = new WebBrowser();
            tready = new System.Windows.Forms.Timer();
            tready.Interval = 2000;
            screen = Screen.PrimaryScreen.Bounds;
            //set the webbrowser width and hight
            web.Width = screen.Width;
            web.Height = screen.Height;
            //suppress script errors and hide scroll bars
            web.ScriptErrorsSuppressed = true;
            web.ScrollBarsEnabled = false;
            //attached events
            web.Navigating +=
              new WebBrowserNavigatingEventHandler(web_Navigating);
            web.DocumentCompleted += new
              WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);
            tready.Tick += new EventHandler(tready_Tick);
        }

        #region Public methods
        public void Create(string url)
        {
            imgsize = null;
            web.Navigate(url);
        }

        public void Create(string url, Size imgsz)
        {
            this.imgsize = imgsz;
            web.Navigate(url);
        }
        #endregion

        #region Events
        void web_DocumentCompleted(object sender,
                 WebBrowserDocumentCompletedEventArgs e)
        {
            //start the timer
            tready.Start();
        }

        void web_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //stop the timer   
            tready.Stop();
        }

        void tready_Tick(object sender, EventArgs e)
        {
            //stop the timer
            tready.Stop();

            //capture html as an image
            Rectangle body = web.Document.Body.ScrollRectangle;

            //check if the document width/height is greater than screen width/height
            Rectangle docRectangle = new Rectangle()
            {
                Location = new Point(0, 0),
                Size = new Size(body.Width > screen.Width ? body.Width : screen.Width,
                 body.Height > screen.Height ? body.Height : screen.Height)
            };
            //set the width and height of the WebBrowser object
            web.Width = docRectangle.Width;
            web.Height = docRectangle.Height;

            //if the imgsize is null, the size of the image will 
            //be the same as the size of webbrowser object
            //otherwise  set the image size to imgsize
            Rectangle imgRectangle;
            if (imgsize == null)
                imgRectangle = docRectangle;
            else
                imgRectangle = new Rectangle()
                {
                    Location = new Point(0, 0),
                    Size = imgsize.Value
                };
            //create a bitmap object 
            Bitmap bitmap = new Bitmap(imgRectangle.Width, imgRectangle.Height);
            //get the viewobject of the WebBrowser
            IViewObject ivo = web.Document.DomDocument as IViewObject;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //get the handle to the device context and draw
                IntPtr hdc = g.GetHdc();
                ivo.Draw(1, -1, IntPtr.Zero, IntPtr.Zero,
                         IntPtr.Zero, hdc, ref imgRectangle,
                         ref docRectangle, IntPtr.Zero, 0);
                g.ReleaseHdc(hdc);
            }
            //invoke the HtmlImageCapture event
            HtmlImageCapture(this, web.Url, bitmap);
        }
        #endregion




        public void Dispose()
        {
            if (!this.web.IsDisposed)
                this.web.Dispose();
            this.tready = null;
        }
    }
}
