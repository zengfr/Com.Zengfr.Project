using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace UniTask_UI.common
{
   public class WebbrowserUtil
    {
        [DllImport("Kernel32.dll")]
        private extern static int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
         static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);
        /// <summary>
        /// 获取错误提示
        /// </summary>
        /// <returns></returns>
        public static string GetErrMsg()
        {
            var code = Marshal.GetLastWin32Error();
            var tempptr = IntPtr.Zero;
            string msg = null;
            FormatMessage(0x1300, ref tempptr, code, 0, ref msg, 255, ref tempptr);
            return msg;
        }
        public static bool SetCookie(string url, string cookieName,String v)
        {
            return  SetCookie(url, 
                new Cookie(HttpUtility.UrlEncode(cookieName).Replace("+", ""),
                HttpUtility.UrlEncode(v),  "/"));
        }
        public static bool SetCookie(string url, Cookie cookie)
        {
            return InternetSetCookie(url, null, cookie.ToString());
        }
        public static int SetCookies(string url, string cookieName, string cookieData, DateTime expiresDate)
        {

            cookieData = string.Format("{0}={1};path=/;expires={2}"
                , cookieName
                , cookieData
                , expiresDate.ToString("r"));
            return InternetSetCookieEx(url, null, cookieData, 0x40, IntPtr.Zero);
        }
        public static string GetCookies(
  WebBrowser browser)
        {
            HtmlDocument doc = browser.Document;
            
            string url = doc.Url.AbsoluteUri;
            //string cookies2 = GetCookies(url);

            //保留cookie
            // cookieString = doc.Cookie;
           var cookieString = GetCookies(url);
            return cookieString;
        }
        public static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }
    }
}
