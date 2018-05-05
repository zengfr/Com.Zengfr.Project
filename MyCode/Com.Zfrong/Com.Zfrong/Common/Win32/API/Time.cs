using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
namespace Com.Zfrong.Common.Win32.API
{
    public partial class Time
    {
        [DllImport("kernel32.dll", EntryPoint = "GetFileTime")]
        public static extern bool GetFileTime(HandleRef hFile, out FILETIME lpCreationTime, out FILETIME lpLastAccessTime, out FILETIME lpLastWriteTime);

        // 文件时间转为本地定义的格式64位
        [DllImport("kernel32.dll", EntryPoint = "FileTimeToLocalFileTime")]
        public static extern bool FileTimeToLocalFileTime(out FILETIME lpFileTime, out FileTime system);

        // 修改文件时间
        [DllImport("kernel32.dll", EntryPoint = "SetFileTime")]
        public static extern Int32 SetFileTime(HandleRef hFile, out long lpCreationTime, out long lpLastAccessTime, out long lpLastWriteTime);

        // 定义时间格式为64位
        [StructLayout(LayoutKind.Sequential)]
        public struct FileTime
        {
            public long dwLowDateTime;
            public long dwHighDateTime;

        }

        #region 修改本地文件的时间
        public static bool UpdateFileTime(string fileName)
        {

            bool flag = false;


            FileInfo file = new FileInfo(fileName);

            // 记录本地文件时间
            long fileCreateTime = file.CreationTime.ToFileTime();
            long fileAccessTime = file.LastAccessTime.ToFileTime();


            // 读取本地文件并得到其句柄
            FileStream fs = new FileStream(fileName, FileMode.Open);
            HandleRef hr = new HandleRef(fs, fs.Handle);

            // 定义文件时间
            FILETIME createTime, lastAccessTime, lastWriteTime;
            createTime = new FILETIME();
            lastAccessTime = new FILETIME();
            lastWriteTime = new FILETIME();

            //// 获取文件时间 调用Api
            //bool ww = GetFileTime(hr, out createTime, out lastAccessTime, out lastWriteTime);

            //// 转换为本地的
            //FileTime fc = new FileTime();
            //FileTime fla = new FileTime();
            //FileTime flw = new FileTime();

            //ww = FileTimeToLocalFileTime(out createTime, out fc);
            //ww = FileTimeToLocalFileTime(out lastAccessTime, out fla);
            //ww = FileTimeToLocalFileTime(out lastWriteTime, out flw);


            //Console.WriteLine("--------------------------------------------------------");
            //Console.WriteLine("创建时间：" + DateTime.FromFileTime(fc.dwHighDateTime + fc.dwLowDateTime).AddHours(-8).ToString());
            //Console.WriteLine("写入时间：" + DateTime.FromFileTime(flw.dwHighDateTime + flw.dwLowDateTime).AddHours(-8).ToString());
            //Console.WriteLine("访问时间：" + DateTime.FromFileTime(fla.dwHighDateTime + fla.dwLowDateTime).AddHours(-8).ToString());


            // 修改文件时间
            //long lfWriteTime = SFileDate.ToFileTime(); // 在下载的时候已记录此时间

            //try
            //{

            //    Int32 rr = SetFileTime(hr, out fileCreateTime, out fileAccessTime, out lfWriteTime);

            //    if (rr == 1)
            //    {
            //        flag = true;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    WriteLog(ex.Message);
            //}

            //Console.WriteLine(rr.ToString());



            //// 获取文件时间
            //bool ww = GetFileTime(hr, out createTime, out lastAccessTime, out lastWriteTime);



            //// 转换为本地的
            //FileTime fc = new FileTime();
            //FileTime fla = new FileTime();
            //FileTime flw = new FileTime();

            //ww = FileTimeToLocalFileTime(out createTime, out fc);
            //ww = FileTimeToLocalFileTime(out lastAccessTime, out fla);
            //ww = FileTimeToLocalFileTime(out lastWriteTime, out flw);


            //Console.WriteLine("修改后 --------------------------------------------------------");
            ////Console.WriteLine("创建时间：" + DateTime.FromFileTime(fc.dwHighDateTime + fc.dwLowDateTime).AddHours(-8).ToString());
            //Console.WriteLine("写入时间：" + DateTime.FromFileTime(flw.dwHighDateTime + flw.dwLowDateTime).AddHours(-8).ToString());
            ////Console.WriteLine("访问时间：" + DateTime.FromFileTime(fla.dwHighDateTime + fla.dwLowDateTime).AddHours(-8).ToString());

            fs.Close();


            return flag;


        }
        public static DateTime GetMaxDateTimeForURL(DateTime t, string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/html";
                httpWebRequest.Method = "GET";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string date = httpWebResponse.Headers.Get("Date");
                string expires = httpWebResponse.Headers.Get("Expires");
                DateTime d1, d2;
                if (date != null && date.Length > 0)
                {
                    d1 = DateTime.Parse(date);
                    if (d1 > t) return d1;
                }
                if (expires != null && expires.Length > 4)
                {
                    d2 = DateTime.Parse(expires);
                    if (d2 > t) return d2;
                }
            }
            catch { }
            return t;
        }
        public static DateTime GetTimeForSys()
        {
            string[] ss = new string[] { "c", "d", "e", "f", "g", "h", "i", "j", "k", "x", "y", "z" };
            string f = "{0}:\\pagefile.sys";
            DateTime tMin = DateTime.Parse("1601-01-01 08:00:00"), tmp, t, tMax = t = DateTime.Today;
            foreach (string s in ss)
            {
                FileInfo file = new FileInfo(string.Format(f, s));
                tmp = file.LastWriteTime;
                if (tmp < DateTime.Parse("1601-01-01 08:08:08"))
                    continue;
                else if (tmp >= tMax)
                {
                    t = tmp;
                }
                else if (tmp <= tMax)
                {
                    t = tMax;
                }
            }
            t = GetMaxDateTimeForURL(t, "http://www.baidu.com/");
            if (t > tMax) return t;

            t = GetMaxDateTimeForURL(t, "http://www.google.com/");
            if (t > tMax)
                return t;
            return tMax;
        }
        #endregion
    }
}
