using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Security;
using System.Threading;
using System.Collections.Specialized;
using System.Collections;

/// <summary>
/// Author: [ ChengKing(ZhengJian) ] 
///该源码下载自www.51aspx.com(５１ａｓｐｘ．ｃｏｍ)
/// 注：从网上找了个优秀代码
/// 扩展如下功能: 
///   1. 解决一些线程相关的Bug; 
///   2.扩展用控制文件实现断点续传功能.
/// </summary>
namespace DownLoadComponent
{
    /// <summary>
    /// 多线程辅助类(Add by ChengKing)
    /// </summary>
    public class Task
    {
        string _FromFileName;
        string _ToFileName;
        int _ThreadNum;

        public Task(string FromFileName, string ToFileName, int ThreadNum)
        {
            this._FromFileName = FromFileName;
            this._ToFileName = ToFileName;
            this._ThreadNum = ThreadNum;
        }

        public string FromFileName
        {
            get
            {
                return _FromFileName;
            }
            set
            {
                _FromFileName = value;
            }
        }
        public string ToFileName
        {
            get
            {
                return _ToFileName;
            }
            set
            {
                _ToFileName = value;
            }
        }
        public int ThreadNum
        {
            get
            {
                return _ThreadNum;
            }
            set
            {
                _ThreadNum = value;
            }
        }
    }

    /// <summary>
    /// 记录下载的字节位置
    /// </summary>
    public class DownLoadState
    {
        private string _FileName;

        private string _AttachmentName;
        private int _Position;
        private string _RequestURL;
        private string _ResponseURL;
        private int _Length;

        private byte[] _Data;

        public string FileName
        {
            get
            {
                return _FileName;
            }
        }

        public int Position
        {
            get
            {
                return _Position;
            }
        }

        public int Length
        {
            get
            {
                return _Length;
            }
        }


        public string AttachmentName
        {
            get
            {
                return _AttachmentName;
            }
        }

        public string RequestURL
        {
            get
            {
                return _RequestURL;
            }
        }

        public string ResponseURL
        {
            get
            {
                return _ResponseURL;
            }
        }


        public byte[] Data
        {
            get
            {
                return _Data;
            }
        }

        internal DownLoadState(string RequestURL, string ResponseURL, string FileName, string AttachmentName, int Position, int Length, byte[] Data)
        {
            this._FileName = FileName;
            this._RequestURL = RequestURL;
            this._ResponseURL = ResponseURL;
            this._AttachmentName = AttachmentName;
            this._Position = Position;
            this._Data = Data;
            this._Length = Length;
        }

        internal DownLoadState(string RequestURL, string ResponseURL, string FileName, string AttachmentName, int Position, int Length, ThreadCallbackHandler tch)
        {
            this._RequestURL = RequestURL;
            this._ResponseURL = ResponseURL;
            this._FileName = FileName;
            this._AttachmentName = AttachmentName;
            this._Position = Position;
            this._Length = Length;
            this._ThreadCallback = tch;
        }

        internal DownLoadState(string RequestURL, string ResponseURL, string FileName, string AttachmentName, int Position, int Length)
        {
            this._RequestURL = RequestURL;
            this._ResponseURL = ResponseURL;
            this._FileName = FileName;
            this._AttachmentName = AttachmentName;
            this._Position = Position;
            this._Length = Length;
        }

        private ThreadCallbackHandler _ThreadCallback;

        //
        internal void StartDownloadFileChunk()
        {
            if (this._ThreadCallback != null)
            {
                this._ThreadCallback(this._RequestURL, this._FileName, this._Position, this._Length);
            }
        }

    }

    //委托代理线程的所执行的方法签名一致
    public delegate void ThreadCallbackHandler(string S, string s, int I, int i);

    //异常处理动作
    public enum ExceptionActions
    {
        Throw,
        CancelAll,
        Ignore,
        Retry
    }

    /// <summary>
    /// 包含 Exception 事件数据的类
    /// </summary>
    public class ExceptionEventArgs : System.EventArgs
    {
        private System.Exception _Exception;
        private ExceptionActions _ExceptionAction;

        private DownLoadState _DownloadState;

        public DownLoadState DownloadState
        {
            get
            {
                return _DownloadState;
            }
        }

        public Exception Exception
        {
            get
            {
                return _Exception;
            }
        }

        public ExceptionActions ExceptionAction
        {
            get
            {
                return _ExceptionAction;
            }
            set
            {
                _ExceptionAction = value;
            }
        }

        internal ExceptionEventArgs(System.Exception e, DownLoadState DownloadState)
        {
            this._Exception = e;
            this._DownloadState = DownloadState;
        }
    }

    /// <summary>
    /// 包含 DownLoad 事件数据的类
    /// </summary>
    public class DownLoadEventArgs : System.EventArgs
    {
        private DownLoadState _DownloadState;

        public DownLoadState DownloadState
        {
            get
            {
                return _DownloadState;
            }
        }

        public DownLoadEventArgs(DownLoadState DownloadState)
        {
            this._DownloadState = DownloadState;
        }

    }

    /// <summary>
    /// 支持断点续传多线程下载的类
    /// </summary>
    public class HttpWebClient
    {
        private static object _SyncLockObject = new object();

        public delegate void DataReceiveEventHandler(HttpWebClient Sender, DownLoadEventArgs e);

        public event DataReceiveEventHandler DataReceive; //接收字节数据事件

        public delegate void ExceptionEventHandler(HttpWebClient Sender, ExceptionEventArgs e);

        public event ExceptionEventHandler ExceptionOccurrs; //发生异常事件

        private int _FileLength; //下载文件的总大小

        public static ArrayList threads;

        public int FileLength
        {
            get
            {
                return _FileLength;
            }
        }

        /// <summary>
        /// 分块下载文件
        /// </summary>
        /// <param name="Address">URL 地址</param>
        /// <param name="FileName">保存到本地的路径文件名</param>
        /// <param name="ChunksCount">块数,线程数</param>
        public void DownloadFile(string Address, string FileName, int ChunksCount)
        {
            int p = 0; // position
            int s = 0; // chunk size
            string a = null;
            HttpWebRequest hwrq;
            HttpWebResponse hwrp = null;
            try
            {

                hwrq = (HttpWebRequest)WebRequest.Create(this.GetUri(Address));
                //hwrq.Timeout = 20000000;
                //if (hwrq.HaveResponse == false)
                //    return;
                //hwrq.ProtocolVersion =HttpVersion.Version10;
                //WebProxy wp = WebProxy.GetDefaultProxy();
                //hwrq.Proxy = wp;
                hwrq.Method = "GET";
                try
                {
                    hwrp = (HttpWebResponse)hwrq.GetResponse();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                long L = hwrp.ContentLength;

                //如果文件太小, 就不用分多线程, 用一个线程下载即可. (目前控制在800K) 
                //if (L < 800000)
                //{
                //    ChunksCount = 1;
                //}

                hwrq.Credentials = this.m_credentials;

                L = ((L == -1) || (L > 0x7fffffff)) ? ((long)0x7fffffff) : L; //Int32.MaxValue 该常数的值为 2,147,483,647; 即十六进制的 0x7FFFFFFF

                int l = (int)L;

                this._FileLength = l;

                bool b = (hwrp.Headers["Accept-Ranges"] != null & hwrp.Headers["Accept-Ranges"] == "bytes");
                a = hwrp.Headers["Content-Disposition"]; //attachment
                if (a != null)
                {
                    a = a.Substring(a.LastIndexOf("filename=") + 9);
                }
                else
                {
                    a = FileName;
                }

                int ss = s;
                if (b)
                {
                    if (ExistControlFile(FileName)) //是否存在文件
                    {
                        string[] strBlocks = this.ReadInfFromControlFile(FileName).Split(new char[2] { '\r', '\n' });
                        for (int i = 0; i < strBlocks.Length; i++)
                        {
                            if (strBlocks[i].Trim().Length != 0 && strBlocks[i].Substring(strBlocks[i].Length - 1) == "0")
                            {
                                string[] strRecord = strBlocks[i].Split(',');
                                int p2 = int.Parse(strRecord[0]);
                                int s2 = int.Parse(strRecord[1]);
                                DownLoadState x = new DownLoadState(Address, hwrp.ResponseUri.AbsolutePath, FileName, a, p2, s2, new ThreadCallbackHandler(this.DownloadFileChunk));
                                Thread t = new Thread(new ThreadStart(x.StartDownloadFileChunk));
                                if (threads == null)
                                {
                                    threads = new ArrayList();
                                }
                                threads.Add(t);
                                t.Start();
                            }


                        }

                    }
                    else
                    {
                        //建立控制文件
                        FileStream fs = File.Create(this.GetControlFileName(FileName));
                        fs.Close();

                        if (File.Exists(FileName))
                        {
                            FileInfo fi = new FileInfo(FileName);
                            if (fi.Length == L)
                            {
                                this.AddendInfToControlFile(FileName, 0, 0);
                                this.UpdateControlFile(FileName, 0, 0);
                                return;
                            }
                        }

                        s = l / ChunksCount;
                        if (s < 2 * 64 * 1024) //块大小至少为 128 K 字节
                        {
                            s = 2 * 64 * 1024;
                        }
                        ss = s;
                        int i = 0;
                        while (l >= s)
                        {
                            l -= s;
                            if (l < s)
                            {
                                s += l;
                            }
                            if (i++ > 0)
                            {
                                DownLoadState x = new DownLoadState(Address, hwrp.ResponseUri.AbsolutePath, FileName, a, p, s, new ThreadCallbackHandler(this.DownloadFileChunk));

                                AddendInfToControlFile(FileName, p, s);
                                Thread t = new Thread(new ThreadStart(x.StartDownloadFileChunk));
                                if (threads == null)
                                {
                                    threads = new ArrayList();
                                }
                                threads.Add(t);
                                t.Start();

                            }
                            p += s;
                        }
                        s = ss;

                        AddendInfToControlFile(FileName, 0, s);
                        DownLoadState x1 = new DownLoadState(Address, hwrp.ResponseUri.AbsolutePath, FileName, a, 0, s, new ThreadCallbackHandler(this.DownloadFileChunk));
                        Thread t2 = new Thread(new ThreadStart(x1.StartDownloadFileChunk));
                        if (threads == null)
                        {
                            threads = new ArrayList();
                        }
                        threads.Add(t2);
                        t2.Start();
                    }
                }
                //如果服务器不支持断点续传(Accept-Range), 则使用单线程下载
                else
                {
                    AddendInfToControlFile(FileName, 0, l);
                    DownLoadState x = new DownLoadState(Address, hwrp.ResponseUri.AbsolutePath, FileName, a, 0, l, new ThreadCallbackHandler(this.DownloadFileChunk));
                    Thread t = new Thread(new ThreadStart(x.StartDownloadFileChunk));
                    if (threads == null)
                    {
                        threads = new ArrayList();
                    }
                    threads.Add(t);
                    t.Start();
                }
            }
            catch (Exception e)
            {
                //if (blnReturn == true)
                //{
                //    return;
                //}

                ExceptionActions ea = ExceptionActions.Throw;
                if (ea == ExceptionActions.Throw)
                {
                    if (!(e is WebException) && !(e is SecurityException))
                    {
                        throw new WebException("net_webclient", e);
                    }
                    throw;
                }


                //if (this.ExceptionOccurrs != null)
                //{                    
                //    DownLoadState x = new DownLoadState(Address, hwrp.ResponseUri.AbsolutePath, FileName, a, p, s);

                //    ExceptionEventArgs eea = new ExceptionEventArgs(e, x);
                //    ExceptionOccurrs(this, eea);
                //    ea = eea.ExceptionAction;
                //}

            }

        }

        #region 操作控制文件(By King Zheng)

        /// <summary>
        /// 插入文件块信息到控制文件(Add by ChengKing)
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Position"></param>
        /// <param name="Length"></param>
        private void AddendInfToControlFile(string FileName, int Position, int Length)
        {


            try
            {
                lock (_SyncLockObject)
                {
                    string strControlFile = GetControlFileName(FileName);


                    //if (File.Exists(strControlFile) == false)
                    //{
                    //    return;
                    //}

                    using (StreamWriter sw = new StreamWriter(strControlFile, true, Encoding.Default))
                    {
                        //sw.NewLine = "$";
                        sw.WriteLine(Position.ToString() + "," + Length.ToString() + "," + "0");
                    }
                    //using (System.IO.FileStream sw = new System.IO.FileStream(strControlFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite))
                    //{
                    //    //sw.Position = e.DownloadState.Position;
                    //    sw.Write(Position.ToString() + "," + Length.ToString() + "," + "0"); 
                    //    sw.Close();
                    //}



                }
            }
            catch (Exception e)
            {
                throw new Exception("写控制文件出错!" + e.Message);
            }

        }

        /// <summary>
        /// 更新控制文件(Add by ChengKing)
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Position"></param>
        /// <param name="Length"></param>
        private void UpdateControlFile(string FileName, int Position, int Length)
        {
            try
            {
                lock (_SyncLockObject)
                {
                    string strControlFile = GetControlFileName(FileName);


                    //if (File.Exists(strControlFile) == false)
                    //{
                    //    return;
                    //}

                    string s = null;
                    using (StreamReader sr = new System.IO.StreamReader(strControlFile))
                    {
                        s = sr.ReadToEnd();
                        s = s.Replace(Position.ToString() + "," + Length.ToString() + "," + "0", Position.ToString() + "," + Length.ToString() + "," + "1");
                    }
                    using (StreamWriter sw = new StreamWriter(strControlFile, false, Encoding.Default))
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("更新控制文件出错!" + e.Message);
            }

        }

        /// <summary>
        /// 读取所有信息从控制文件(Add by ChengKing)
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private string ReadInfFromControlFile(string FileName)
        {
            try
            {
                lock (_SyncLockObject)
                {
                    string strControlFile = GetControlFileName(FileName);

                    string s = null;
                    using (StreamReader sr = new System.IO.StreamReader(strControlFile))
                    {
                        s = sr.ReadToEnd();

                    }
                    return s;
                }
            }
            catch (Exception e)
            {
                throw new Exception("读控制文件出错!" + e.Message);
            }
        }

        /// <summary>
        /// 根据目标文件名得到控制文件名(Add by ChengKing)
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string GetControlFileName(string FileName)
        {
            string strPath = Path.GetDirectoryName(FileName);

            //string strFileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileName);
            string strFileNameWithoutExtension = Path.GetFileName(FileName);
            string strControlFile = Path.Combine(strPath, strFileNameWithoutExtension + "_Control.txt");
            return strControlFile;
        }

        /// <summary>
        /// 判断控制文件是否存在
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private bool ExistControlFile(string FileName)
        {
            string strControlFile = GetControlFileName(FileName);
            if (File.Exists(strControlFile))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断控制文件是否完成
        /// </summary>
        /// <param name="strControlFile"></param>
        /// <returns></returns>
        public bool JudgeControlFileIfFinished(string strControlFile)
        {
            try
            {
                string s = null;
                lock (_SyncLockObject)
                {
                    using (StreamReader sr = new System.IO.StreamReader(strControlFile))
                    {
                        s = sr.ReadToEnd();
                    }
                }
                if (s + String.Empty == String.Empty)
                {
                    return false;
                }
                string[] strBlocks = s.Split(new char[2] { '\r', '\n' });
                for (int i = 0; i < strBlocks.Length; i++)
                {
                    if (strBlocks[i].Trim().Length != 0 && strBlocks[i].Substring(strBlocks[i].Length - 1) == "0")
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception e)
            {
                throw new Exception("判断控制文件是否完成时, 读取文件出错!" + e.Message);
            }
        }

        /// <summary>
        /// 删除控制文件(Add by ChengKing)
        /// </summary>
        /// <param name="strControlFile"></param>
        /// <returns></returns>
        public bool DeleteControlFile(string strControlFile)
        {
            try
            {
                lock (_SyncLockObject)
                {
                    if (File.Exists(strControlFile))
                    {
                        File.Delete(strControlFile);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("删除控制文件出错!" + e.Message);
            }
        }

        #endregion

        /// <summary>
        /// 下载一个文件块,利用该方法可自行实现多线程断点续传
        /// </summary>
        /// <param name="Address">URL 地址</param>
        /// <param name="FileName">保存到本地的路径文件名</param>
        /// <param name="Length">块大小</param>
        public void DownloadFileChunk(string Address, string FileName, int FromPosition, int Length)
        {
            HttpWebResponse hwrp = null;
            string a = null;
            try
            {
                //this._FileName = FileName;
                HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(this.GetUri(Address));
                //hwrq.Credentials = this.m_credentials;

                hwrq.AddRange(FromPosition);

                hwrp = (HttpWebResponse)hwrq.GetResponse();

                //hwrp.Headers.Add("Content-Range", FromPosition.ToString());  //Test

                a = hwrp.Headers["Content-Disposition"]; //attachment
                if (a != null)
                {
                    a = a.Substring(a.LastIndexOf("filename=") + 9);
                }
                else
                {
                    a = FileName;
                }

                byte[] buffer = this.ResponseAsBytes(Address, hwrp, Length, FileName);
                //   lock (_SyncLockObject)
                //   {
                //    this._Bytes += buffer.Length;
                //   }
            }
            catch (Exception e)
            {
                ExceptionActions ea = ExceptionActions.Throw;
                if (this.ExceptionOccurrs != null)
                {
                    DownLoadState x = new DownLoadState(Address, hwrp.ResponseUri.AbsolutePath, FileName, a, FromPosition, Length);
                    ExceptionEventArgs eea = new ExceptionEventArgs(e, x);
                    ExceptionOccurrs(this, eea);
                    ea = eea.ExceptionAction;
                }

                if (ea == ExceptionActions.Throw)
                {
                    if (!(e is WebException) && !(e is SecurityException))
                    {
                        throw new WebException("net_webclient", e);
                    }
                    throw;
                }
            }
        }

        internal byte[] ResponseAsBytes(string RequestURL, WebResponse Response, long Length, string FileName)
        {
            string a = null; //AttachmentName
            int P = 0; //整个文件的位置指针
            int num2 = 0;
            int num3 = 0;
            int intFrom = 0;
            try
            {
                a = Response.Headers["Content-Disposition"]; //attachment
                if (a != null)
                {
                    a = a.Substring(a.LastIndexOf("filename=") + 9);
                }

                long num1 = Length; //Response.ContentLength;
                bool flag1 = false;
                if (num1 == -1)
                {
                    flag1 = true;
                    num1 = 0x10000; //64k
                }
                byte[] buffer1 = new byte[(long)num1];


                int p = 0; //本块的位置指针

                string s = Response.Headers["Content-Range"];
                //string s = hwrq.Headers["Range"];

                if (s != null)
                {
                    s = s.Replace("bytes ", "");
                    s = s.Substring(0, s.IndexOf("-"));
                    P = Convert.ToInt32(s);
                    intFrom = P;

                }

                //int num3 = 0;

                Stream S = Response.GetResponseStream();

                int count = 0;

                int bufferSize = 65535; //允许读取的最大字节

                int times;
                do
                {
                    times = 0;

                    //num2 = S.Read(buffer1, num3, ((int)num1) - num3);

                    //限制最大读取字节
                    if (bufferSize < ((int)num1) - num3)
                    {
                        num2 = S.Read(buffer1, num3, bufferSize);
                    }
                    else
                    {
                        num2 = S.Read(buffer1, num3, ((int)num1) - num3);
                    }

                    //网络短时间的不稳定
                    if (num2 == 0)
                    {
                        Thread.Sleep(50);
                        times++;

                        if (times > 100)
                        {
                            throw new Exception("网络传输层错误");
                        }

                    }

                    num3 += num2;
                    if (flag1 && (num3 == num1))
                    {
                        num1 += 0x10000;
                        byte[] buffer2 = new byte[(int)num1];
                        Buffer.BlockCopy(buffer1, 0, buffer2, 0, num3);
                        buffer1 = buffer2;
                    }

                    //    lock (_SyncLockObject)
                    //    {
                    //     this._bytes += num2;
                    //    }
                    if (num2 > 0)
                    {
                        if (this.DataReceive != null)
                        {
                            byte[] buffer = new byte[num2];
                            Buffer.BlockCopy(buffer1, p, buffer, 0, buffer.Length);
                            DownLoadState dls = new DownLoadState(RequestURL, Response.ResponseUri.AbsolutePath, FileName, a, P, num2, buffer);
                            DownLoadEventArgs dlea = new DownLoadEventArgs(dls);

                            //触发事件
                            this.OnDataReceive(dlea);
                            //System.Threading.Thread.Sleep(100);                            

                        }
                        p += num2; //本块的位置指针
                        P += num2; //整个文件的位置指针
                    }
                    else
                    {
                        break;
                    }

                }
                while (num2 != 0);

                count++;

                int c = count;

                S.Close();
                S = null;
                if (flag1)
                {
                    byte[] buffer3 = new byte[num3];
                    Buffer.BlockCopy(buffer1, 0, buffer3, 0, num3);
                    buffer1 = buffer3;
                }

                UpdateControlFile(FileName, intFrom, (int)Length);

                return buffer1;
            }
            catch (Exception e)
            {
                ExceptionActions ea = ExceptionActions.Throw;
                if (this.ExceptionOccurrs != null)
                {
                    Thread.Sleep(100);
                    //DownLoadState x = new DownLoadState(RequestURL, Response.ResponseUri.AbsolutePath, FileName, a, P, num2);
                    //DownLoadState x = new DownLoadState(RequestURL, Response.ResponseUri.AbsolutePath, FileName, a, P, (int)(Length - num3));
                    DownLoadState x = new DownLoadState(RequestURL, Response.ResponseUri.AbsolutePath, FileName, a, P, (int)Length);

                    ExceptionEventArgs eea = new ExceptionEventArgs(e, x);
                    ExceptionOccurrs(this, eea);
                    ea = eea.ExceptionAction;
                }

                if (ea == ExceptionActions.Throw)
                {
                    if (!(e is WebException) && !(e is SecurityException))
                    {
                        throw new WebException("net_webclient", e);
                    }
                    throw;
                }
                return null;
            }
        }

        private void OnDataReceive(DownLoadEventArgs e)
        {
            //触发数据到达事件
            DataReceive(this, e);
        }

        public byte[] UploadFile(string address, string fileName)
        {
            return this.UploadFile(address, "POST", fileName, "file");
        }

        public string UploadFileEx(string address, string method, string fileName, string fieldName)
        {
            return Encoding.ASCII.GetString(UploadFile(address, method, fileName, fieldName));
        }

        public byte[] UploadFile(string address, string method, string fileName, string fieldName)
        {
            byte[] buffer4;
            FileStream stream1 = null;
            try
            {
                fileName = Path.GetFullPath(fileName);
                string text1 = "---------------------" + DateTime.Now.Ticks.ToString("x");

                string text2 = "application/octet-stream";

                stream1 = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                WebRequest request1 = WebRequest.Create(this.GetUri(address));
                request1.Credentials = this.m_credentials;
                request1.ContentType = "multipart/form-data; boundary=" + text1;

                request1.Method = method;
                string[] textArray1 = new string[7] { "--", text1, "\r\nContent-Disposition: form-data; name=\"" + fieldName + "\"; filename=\"", Path.GetFileName(fileName), "\"\r\nContent-Type: ", text2, "\r\n\r\n" };
                string text3 = string.Concat(textArray1);
                byte[] buffer1 = Encoding.UTF8.GetBytes(text3);
                byte[] buffer2 = Encoding.ASCII.GetBytes("\r\n--" + text1 + "\r\n");
                long num1 = 0x7fffffffffffffff;
                try
                {
                    num1 = stream1.Length;
                    request1.ContentLength = (num1 + buffer1.Length) + buffer2.Length;
                }
                catch
                {
                }
                byte[] buffer3 = new byte[Math.Min(0x2000, (int)num1)];
                using (Stream stream2 = request1.GetRequestStream())
                {
                    int num2;
                    stream2.Write(buffer1, 0, buffer1.Length);
                    do
                    {
                        num2 = stream1.Read(buffer3, 0, buffer3.Length);
                        if (num2 != 0)
                        {
                            stream2.Write(buffer3, 0, num2);
                        }
                    }
                    while (num2 != 0);
                    stream2.Write(buffer2, 0, buffer2.Length);
                }
                stream1.Close();
                stream1 = null;
                WebResponse response1 = request1.GetResponse();

                buffer4 = this.ResponseAsBytes(response1);
            }
            catch (Exception exception1)
            {
                if (stream1 != null)
                {
                    stream1.Close();
                    stream1 = null;
                }
                if (!(exception1 is WebException) && !(exception1 is SecurityException))
                {
                    //throw new WebException(SR.GetString("net_webclient"), exception1);
                    throw new WebException("net_webclient", exception1);
                }
                throw;
            }
            return buffer4;
        }

        private byte[] ResponseAsBytes(WebResponse response)
        {
            int num2;
            long num1 = response.ContentLength;
            bool flag1 = false;
            if (num1 == -1)
            {
                flag1 = true;
                num1 = 0x10000;
            }
            byte[] buffer1 = new byte[(int)num1];
            Stream stream1 = response.GetResponseStream();
            int num3 = 0;
            do
            {
                num2 = stream1.Read(buffer1, num3, ((int)num1) - num3);
                num3 += num2;
                if (flag1 && (num3 == num1))
                {
                    num1 += 0x10000;
                    byte[] buffer2 = new byte[(int)num1];
                    Buffer.BlockCopy(buffer1, 0, buffer2, 0, num3);
                    buffer1 = buffer2;
                }
            }
            while (num2 != 0);
            stream1.Close();
            if (flag1)
            {
                byte[] buffer3 = new byte[num3];
                Buffer.BlockCopy(buffer1, 0, buffer3, 0, num3);
                buffer1 = buffer3;
            }
            return buffer1;
        }

        private NameValueCollection m_requestParameters;
        private Uri m_baseAddress;
        private ICredentials m_credentials = CredentialCache.DefaultCredentials;

        public ICredentials Credentials
        {
            get
            {
                return this.m_credentials;
            }
            set
            {
                this.m_credentials = value;
            }
        }

        public NameValueCollection QueryString
        {
            get
            {
                if (this.m_requestParameters == null)
                {
                    this.m_requestParameters = new NameValueCollection();
                }
                return this.m_requestParameters;
            }
            set
            {
                this.m_requestParameters = value;
            }
        }

        public string BaseAddress
        {
            get
            {
                if (this.m_baseAddress != null)
                {
                    return this.m_baseAddress.ToString();
                }
                return string.Empty;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    this.m_baseAddress = null;
                }
                else
                {
                    try
                    {
                        this.m_baseAddress = new Uri(value);
                    }
                    catch (Exception exception1)
                    {
                        throw new ArgumentException("value", exception1);
                    }
                }
            }
        }

        public Uri GetUri(string path)
        {
            Uri uri1;
            try
            {
                if (this.m_baseAddress != null)
                {
                    uri1 = new Uri(this.m_baseAddress, path);
                }
                else
                {
                    uri1 = new Uri(path);
                }
                if (this.m_requestParameters == null)
                {
                    return uri1;
                }
                StringBuilder builder1 = new StringBuilder();
                string text1 = string.Empty;
                for (int num1 = 0; num1 < this.m_requestParameters.Count; num1++)
                {
                    builder1.Append(text1 + this.m_requestParameters.AllKeys[num1] + "=" + this.m_requestParameters[num1]);
                    text1 = "&";
                }
                UriBuilder builder2 = new UriBuilder(uri1);
                builder2.Query = builder1.ToString();
                uri1 = builder2.Uri;
            }
            catch (UriFormatException)
            {
                uri1 = new Uri(Path.GetFullPath(path));
            }
            return uri1;
        }

    }

}

