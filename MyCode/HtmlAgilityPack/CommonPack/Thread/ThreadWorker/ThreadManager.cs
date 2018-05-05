using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using CommonPack;
using Com.Zfrong.Timers;
using Com.Zfrong.Xml;
using Com.Zfrong.Configuration;
namespace ThreadWorker
{
    public enum UriStatus : byte
    {
        NONE = 0x0,
        QUEUED = 0x1,
        FAILED = 0x2,
        SUCCESS = 0x4
    }
    public enum ModeType : byte
    {

        /// <summary>
        /// 蜘蛛模式
        /// </summary>
        Spider = 0x1,
        /// <summary>
        /// 监视模式
        /// </summary>
        Monitor = 0x2,
        /// <summary>
        /// 单域名模式
        /// </summary>
        SDomain = 0x4,
        /// <summary>
        /// 多子域名模式
        /// </summary>
        FDomain = 0x8,
    }
    public delegate bool ManagerEventHandler(ThreadManager sender, PowerUri uri);
    public delegate bool WorkerEventHandler(ThreadWorker sender);
    public delegate void MessageEventHandler(string msg);
    /// <summary>
    /// 线程工作管理类
    /// </summary>
    public class ThreadManager
    {
        static ThreadManager()
        {
            System.Net.ServicePointManager.MaxServicePoints = 40;
            System.Net.ServicePointManager.DefaultConnectionLimit = 200;//zfr 2009.06.02
        }
        public ThreadManager()
        {
            Init();//
        }
        #region 字段
        public BloomFilter BloomFilter;
        public string BloomFilterFileName;
        public HighResolutionTimer Timer;
        public Done Done;
        /// <summary>
        /// 是否已经启动
        /// </summary>
        public bool IsStarted;
        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPause;
        /// <summary>
        /// 是否退出
        /// </summary>
        public bool IsQuit;
        /// <summary>
        /// 是否显示消息
        /// </summary>
        public bool IsShowManagerMessage;
        /// <summary>
        /// 是否显示消息
        /// </summary>
        public bool IsShowWorkerMessage;
        /// <summary>
        /// 任务（文件夹）名称
        /// </summary>
        public string TaskName;
        /// <summary>
        /// 队列匹配表达式
        /// </summary>
        private Regex QueueAllowPatternRegex;
        private Regex QueueForbidPatternRegex;
        /// <summary>
        /// 队列匹配表达式 默认=this.BaseUrl.Host;//
        /// </summary>
        public string QueueAllowPattern;
        public string QueueForbidPattern;
        /// <summary>
        /// 允许的 比如:.html.aspx.php.xml.do.jsp.rails.css.js
        /// </summary>
        public string QueueAllowTypes;
        /// <summary>
        /// 禁止的 比如:.exe.com.msi/.rar.zip.z7.iso.img/.jpg.gif.png.ico/.swf.dat.mpeg.wmv.rm.mp3.wma/.doc.pdf.ppt.chm.mht
        /// </summary>
        public string QueueForbidTypes;
        /// <summary>
        /// 线程数 默认20;
        /// </summary>
        public int ThreadsCount;
        /// <summary>
        /// 起始的URL
        /// </summary>
        public PowerUri BaseUrl;
        /// <summary>
        /// 网页深度 默认0=任意 -1=不添加队列到UnProcessed
        /// </summary>
        public int QueueMaxDepth;
        /// <summary>
        /// 运行模式
        /// </summary>
        public ModeType ModeType;
        /// <summary>
        /// 线程集合
        /// </summary>
        public IList<ThreadWorker> ThreadWorkers;
        /// <summary>
        /// 已处理的URLs-成功
        /// </summary>
        public IList<PowerUri> SuccessURLs;
        /// <summary>
        /// 准备处理的URLs队列
        /// </summary>
        public Queue<PowerUri> WaitingURLs;
        /// <summary>
        /// 已处理的URLs-失败
        /// </summary>
        public IList<PowerUri> ErrorURLs;
        /// <summary>
        /// 未处理的URLs
        /// </summary>
        public IList<PowerUri> UnProcessedURLs;
        /// <summary>
        /// 下载解析了的url统计
        /// </summary>
        public int DownloadCount = 0;//
        #endregion
        #region 自定义事件
        public event ManagerEventHandler EventEnQueueBefore;
        public event ManagerEventHandler EventEnQueueAfter;
        /// <summary>
        /// 自定义事件--任务结束
        /// </summary>
        public event ManagerEventHandler EventEndTasks;
        /// <summary>
        /// 自定义事件--线程下载前
        /// </summary>
        public event WorkerEventHandler EventWorkerDownloadBefore;
        /// <summary>
        /// 自定义事件--线程下载后
        /// </summary>
        public event WorkerEventHandler EventWorkerDownloadAfter;
        public event MessageEventHandler EventManagerMessageChanged;
        public event MessageEventHandler EventWorkerMessageChanged;
        /// <summary>
        /// 触发自定义事件--下载前
        /// </summary>
        /// <param name="e"></param>
        public virtual bool OnWorkerDownloadBefore(ThreadWorker sender)
        {
            if (EventWorkerDownloadBefore != null)
                return EventWorkerDownloadBefore(sender);
            return true;
        }
        /// <summary>
        ///  触发自定义事件--下载后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual bool OnWorkerDownloadAfter(ThreadWorker sender)
        {
            if (EventWorkerDownloadAfter != null)
                return EventWorkerDownloadAfter(sender);
            return true;
        }
        /// <summary>
        /// 触发自定义事件--队列变化
        /// </summary>
        /// <param name="e"></param>
        public virtual bool OnEnQueueBefore(PowerUri e)
        {
            if (EventEnQueueBefore != null)
                return EventEnQueueBefore(this, e);
            return true;
        }
        public virtual bool OnEnQueueAfter(PowerUri e)
        {
            if (EventEnQueueAfter != null)
                return EventEnQueueAfter(this, e);
            return true;
        }
        public virtual void OnManagerMessageChanged(string msg)
        {
            if (EventManagerMessageChanged != null)
                EventManagerMessageChanged(msg);
        }
        public virtual void OnWorkerMessageChanged(string msg)
        {
            if (EventWorkerMessageChanged != null)
                EventWorkerMessageChanged(msg);
        }
        /// <summary>
        /// 触发自定义事件--任务结束
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnEndTasks(PowerUri e)
        {
            if (EventEndTasks != null)
                EventEndTasks(this, e);
        }
        #endregion
        #region 队列处理
        /// <summary>
        ///     Spider模式时=UriToUnProcessed//
        ///     Monitor模式时=UriToWaiting
        /// </summary>
        /// <param name="PowerUri"></param>
        public void UriToQueue(PowerUri PowerUri)
        {
            switch (this.ModeType)
            {
                case ModeType.Spider: UriToUnProcessed(PowerUri); break;//
                case ModeType.Monitor: UriToWaiting(PowerUri); break;//
                default: UriToUnProcessed(PowerUri); break;//
            }
        }
        /// <summary>
        /// 添加到未处理队列
        /// </summary>
        /// <param name="PowerUri"></param>
        private void UriToUnProcessed(PowerUri PowerUri)
        {
            if (this.QueueMaxDepth == 0 || PowerUri.Depth <= this.QueueMaxDepth)
            {
                lock (this.Done.SyncObj)
                {
                    if (!BloomFilter.IsRepeat(PowerUri.ToString()))
                    {
                        if (OnEnQueueBefore(PowerUri))////激发事件
                        {
                            UnProcessedURLs.Add(PowerUri);
                            OnEnQueueAfter(PowerUri);////激发事件
                        }
                    }
                    Monitor.PulseAll(this.Done.SyncObj);//
                }
            }
        }
        /// <summary>
        /// 添加到等待处理队列
        /// </summary>
        /// <param name="PowerUri"></param>
        private void UriToWaiting(PowerUri PowerUri)
        {
            if (this.ModeType == ModeType.Monitor && PowerUri.Depth > 1)
                return;//监视模式下Depth > 1的不添加
            lock (this.Done.SyncObj)
            {
                if (!WaitingURLs.Contains(PowerUri))
                {
                    WaitingURLs.Enqueue(PowerUri);
                }
                if (WaitingURLs.Count < this.ThreadsCount + 1)//有等待线程
                    Monitor.PulseAll(this.Done.SyncObj);//
            }
        }
        /// <summary>
        /// 添加到已处理队列
        /// </summary>
        /// <param name="PowerUri"></param>
        /// <param name="state"></param>
        public void UriToProcessed(PowerUri PowerUri, UriStatus state)
        {
            lock (this.Done.SyncObj)
            {
                switch (state)
                {
                    case UriStatus.SUCCESS:
                        if (!SuccessURLs.Contains(PowerUri))
                        {
                            SuccessURLs.Add(PowerUri);
                        }
                        break;
                    case UriStatus.FAILED:
                        if (!ErrorURLs.Contains(PowerUri))
                        {
                            ErrorURLs.Add(PowerUri);
                        }
                        break;
                }
                Monitor.PulseAll(this.Done.SyncObj);//通知
            }
        }
        /// <summary>
        /// 获取等待队列的新Url
        /// </summary>
        /// <returns></returns>
        public PowerUri UriFromWaiting()
        {
            lock (this.Done.SyncObj)
            {
                while (WaitingURLs.Count ==0)
                {
                    Monitor.Wait(this.Done.SyncObj, 1000);
                    if (WaitingURLs.Count == 0)//修复等待2008.10.15
                    {//修复
                        this.QueueSave();
                        this.QueueLoad();
                    }
                    if (this.IsQuit) break;//修复2008.08.15
                }
                while (this.IsPause)
                {
                    Monitor.Wait(this.Done.SyncObj, 500);
                }
                PowerUri uri = WaitingURLs.Dequeue();
                while (QueuePatternAllowOrForbid(uri) == -1 || QueueTypeAllowOrForbid(uri) ==-1)
                {
                    if (WaitingURLs.Count != 0)
                        uri = WaitingURLs.Dequeue();
                    else if (this.IsQuit) break;//修复2008.08.15
                    else
                        Monitor.Wait(this.Done.SyncObj, 1000);
                        
                }
                return uri;// next;
            }
        }
        #endregion
        #region  公共方法 GetPath
        private static string GetPath_AppRoot()
        {
            return TaskManager.AppPath;
        }
        private static string GetSavePath_Root()
        {
            return TaskManager.SavePathRoot + @"\";
        }
        private static string GetSavePath_Queue()
        {
            return TaskManager.SavePathRoot + @"\Queue\";
        }
        private string GetSavePath_Task()
        {
            return GetSavePath_Queue() + this.TaskName + @"\";
        }
        private string GetSavePath_Mode()
        {
            return this.GetSavePath_Task() + this.ModeType + @"\";
        }
        private string GetSavePath_State()
        {
            return this.GetSavePath_Mode() + @"State\";
        }
        private string GetPath_ConfigFile()
        {
            return this.GetPath_ConfigFile(this.TaskName,this.ModeType.ToString()); 
        }
        private string GetPath_ConfigFile(string taskName,string modeType)
        {
            string root = GetPath_AppRoot() + @"\Config\Manage\";//
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);//
            return root +@"\"+ taskName+@"\"+modeType+ @".xml";
        }
        private string GetBloomFilterFilePath()
        {
            string root = GetSavePath_Root() + @"\BloomFilter\";//
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);//

            return root + this.BloomFilterFileName + @".dat";
        }
        #endregion
        #region  公共方法1
        /// <summary>
        /// -1=禁止 1=允许 0=未定义
        /// </summary>
        /// <param name="PowerUri"></param>
        /// <returns></returns>
        private int QueueTypeAllowOrForbid(PowerUri PowerUri)
        {
            try
            {
                string ext = Path.GetExtension(UrlReWritor.RemoveForbidChar(PowerUri.LocalPath)).ToLower();//

                if (ext == "" && PowerUri.LocalPath.IndexOf(@"/") != -1)
                    return 1;//
                bool rtn = this.QueueForbidTypes.IndexOf(ext) != -1 || this.QueueForbidTypes == "*";//禁止的存在
                if (rtn) return -1;//
                rtn = this.QueueAllowTypes.IndexOf(ext) != -1 || this.QueueAllowTypes == "*";//允许的存在
                if (rtn) return 1;//
            }
            catch { }
            return 0;//
        }
        /// <summary>
        /// -1=禁止 1=允许 0=未定义
        /// </summary>
        /// <param name="PowerUri"></param>
        /// <returns></returns>
        private int QueuePatternAllowOrForbid(PowerUri PowerUri)
        {

            if (!PowerUri.Scheme.ToLower().Equals("http") &&
                    !PowerUri.Scheme.ToLower().Equals("https"))
                return -1;
            switch (this.ModeType)
            {
                case ModeType.SDomain:
                    if (!CompareHost(PowerUri.Host, PowerUri.BaseUri.Host))
                        return -1;//
                    return 1;//
                    break;//
                case ModeType.FDomain:
                    if (!CompareHost(GetMainDomain(PowerUri.Host), GetMainDomain(PowerUri.BaseUri.Host)))
                        return -1;//
                    return 1;//
                    break;//
            }

            MatchCollection matchList;//

            if (this.QueueForbidPatternRegex != null)
            {
                matchList = this.QueueForbidPatternRegex.Matches(PowerUri.ToString());
                if (matchList.Count > 0)
                    return -1;//
            }

            matchList = this.QueueAllowPatternRegex.Matches(PowerUri.ToString());
            if (matchList.Count != 0)
                return 1;//
            else
                return -1;//
            return 0;//
        }
        private bool CompareHost(string host1, string host2)
        {
            return host1.ToLower().Equals(host2.ToLower());//
        }
        private string GetMainDomain(string host)
        {
            int i = host.IndexOf(".");//
            if (i != -1)
                return host.Substring(i);//
            return host;//
        }
        #endregion
        #region  公共方法2
        /// <summary>
        /// 初始化/
        /// </summary>
        private void Init()
        {
            this.Timer = new HighResolutionTimer();
            this.TaskName = "NoName";
            this.BloomFilterFileName = "NoName";//
            this.IsShowManagerMessage = false;//
            this.IsShowWorkerMessage = false;//
            this.IsQuit = false;
            this.Done = new Done();
            this.ThreadWorkers = new List<ThreadWorker>();//;

            this.SuccessURLs = new List<PowerUri>();
            this.ErrorURLs = new List<PowerUri>();
            this.WaitingURLs = new Queue<PowerUri>();
            this.UnProcessedURLs = new List<PowerUri>();

            this.QueueMaxDepth = 0;//
            this.ThreadsCount = 20;//
            this.BaseUrl = new PowerUri("http://www.google.com/");// ;
            this.IsStarted = false;
            this.BloomFilter = BloomFilterFactory.Get(this.BloomFilterFileName);//
            this.ModeType = ModeType.Spider;//

            this.QueueAllowTypes = ".html.aspx.php.jsp.do.xml";//
            this.QueueForbidTypes = ".css.js/.exe.com.msi/.rar.zip.z7.iso.img/.jpg.gif.png.ico/.swf.dat.mpeg.wmv.rm.mp3.wma/.doc.pdf.ppt.chm.mht";

            this.QueueAllowPattern = "";
            this.QueueForbidPattern = "";//
            //this.TaskLoad();//家载任务
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (this.IsStarted)
                return;

            if (this.BaseUrl == null)
                return;
            this.IsStarted = true;
            //this.IsShowWorkerMessage = true;
            //this.IsShowManagerMessage = true;

            if (this.QueueAllowPattern == "")
                this.QueueAllowPattern = this.BaseUrl.Host;//
            this.QueueAllowPatternRegex = new Regex(this.QueueAllowPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

            if (this.QueueForbidPattern != "")
                this.QueueForbidPatternRegex = new Regex(this.QueueForbidPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

            if (this.WaitingURLs.Count == 0)
                this.UriToWaiting(this.BaseUrl);

            //if (this.BloomFilterFileName == null || this.BloomFilterFileName.Length == 0)
            //this.BloomFilterFileName = this.TaskName;//

            this.Done.Reset();

            // startup the threads
            this.Timer.Start();//开始记时
            ChangeThreadCount(this.ThreadsCount);
            // now wait to be done
            Done.WaitBegin();
           //this.IsShowWorkerMessage = false;
            //this.IsShowManagerMessage = false;
            Done.WaitDone();
        }
        /// <summary>
        /// 更改线程数量
        /// </summary>
        /// <param name="count"></param>
        public void ChangeThreadCount(int count)
        {
            int ii = count - this.ThreadWorkers.Count;
            this.ThreadsCount = count;//
            ThreadWorker threadWorker;
            for (int i = 0; i < ii; i++)
            {
                threadWorker = new ThreadWorker(this, this.ThreadWorkers.Count);
                threadWorker.EventWorkerDownloadBefore += new WorkerEventHandler(this.OnWorkerDownloadBefore);//事件
                threadWorker.EventWorkerDownloadAfter += new WorkerEventHandler(this.OnWorkerDownloadAfter);//事件
                this.ThreadWorkers.Add(threadWorker);//
                threadWorker.Init();//
                threadWorker.Start();
            }
        }
        public void Pause()
        {
            this.IsPause = true;//
        }
        public void Resume()
        {
            this.IsPause = false;//
        }
        public void Quit()
        {
            this.IsQuit = true;//
            this.IsStarted = false;//
            this.IsPause = false;//
        }
        public void QuitAndSave()
        {
            this.ShowManagerMessage("退出/保存...线程已启动");//
            this.IsShowManagerMessage = true;
            this.IsShowWorkerMessage = true;

            this.Quit();//
            this.ShowManagerMessage("等待直到全部线程停止...");//
            this.Done.WaitDone();

            this.StateSave();
            this.QueueSave();//
            this.ConfigSave();//
        }
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="str"></param>
        public void ShowWorkerMessage(string msg)
        {
            if (this.IsShowWorkerMessage)
            {
                this.OnWorkerMessageChanged(msg);//
            }
        }
        public void ShowManagerMessage(string msg)
        {
            if (this.IsShowManagerMessage)
            {
                this.OnManagerMessageChanged(msg);//
            }
        }
        #endregion
        #region 加载/保存State

        /// <summary>
        /// 自动保存State 30分钟
        /// </summary>
        public void StateSaveAuto()
        {
            ThreadStart ts = new ThreadStart(AutoStateSave);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
            this.ShowManagerMessage("自动保存State 线程已启动");//
        }
        /// <summary>
        /// 自动保存State 30分钟
        /// </summary>
        private void AutoStateSave()
        {
            while (!this.IsQuit)
            {
                Thread.Sleep(30 * 60 * 1000);//
                StateSave();//
            }
        }
        #region

        public void StateLoad(string taskName, byte modeType)
        {
            ModeType m = (ModeType)Enum.Parse(typeof(ModeType), modeType.ToString());//
            StateLoad(taskName, m);//
        }
        public void StateLoad(string taskName, ModeType modeType)
        {
            this.TaskName = taskName;
            this.ModeType = modeType;//
            StateLoad();//
        }
        /// <summary>
        /// 加载状态
        /// </summary>
        public void StateLoad()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("加载State 开始...");//

                string root = this.GetSavePath_State();
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//

                this.ConfigLoad();//

               // ConfigX.LoadList(root + @"UnProcessedURLs.dat", true, this.UnProcessedURLs);//
               // ConfigX.LoadList(root + @"SuccessURLs.dat", true, this.SuccessURLs);//
               // ConfigX.LoadList(root + @"ErrorURLs.dat", true, this.ErrorURLs);//
               // ConfigX.LoadQueue(root + @"WaitingURLs.dat", true, this.WaitingURLs);//
                BloomFilterFactory.Load(this.BloomFilterFileName, GetBloomFilterFilePath());//
                this.ShowManagerMessage("加载State 结束...");//
                Monitor.PulseAll(this.Done.SyncObj);//通知
                //Monitor.Wait(this.Done);//释放
            }
        }
        /// <summary>
        /// 保存状态
        /// </summary>
        public void StateSave()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("保存状态 开始...");//
                string root = this.GetSavePath_State();
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//
                ConfigSave();//
                ConfigX.SaveEnumerator(root + @"SuccessURLs.dat", this.SuccessURLs.GetEnumerator(), true, false);//
                ConfigX.SaveEnumerator(root + @"ErrorURLs.dat", this.ErrorURLs.GetEnumerator(), true, false);//
                ConfigX.SaveEnumerator(root + @"WaitingURLs.dat", this.WaitingURLs.GetEnumerator(), true, false);//
                ConfigX.SaveEnumerator(root + @"UnProcessedURLs.dat", this.UnProcessedURLs.GetEnumerator(), true, false);//

                BloomFilterFactory.Save(this.BloomFilterFileName, GetBloomFilterFilePath());//
                this.ShowManagerMessage("保存状态 结束...");//
                Monitor.PulseAll(this.Done.SyncObj);//通知
                //Monitor.Wait(this.Done);//释放
            }
        }
        private void ConfigLoad()
        {
            string root = System.AppDomain.CurrentDomain.BaseDirectory + @"\Config\Manage\";//
            ConfigLoad(root + @"default.xml");//
            ConfigLoad(root + this.TaskName + @"\default.xml");//
            ConfigLoad(this.GetPath_ConfigFile());//
        }
        private void ConfigLoad(string file)
        {
            bool b = File.Exists(file);
            MyConfig myConfig = MyConfigHelper.Get(file);

            //this.TaskName = f.Get("TaskName", this.TaskName);//
            //this.ModeType = (ModeType)Enum.Parse(typeof(ModeType), f.Get("ModeType", this.ModeType.ToString()));


            string u = this.BaseUrl.ToString();
            myConfig.GetParamValue("BaseUrl", ref u);//
            this.BaseUrl = new PowerUri(u);

            myConfig.GetParamValue("ThreadsCount", ref this.ThreadsCount);
            myConfig.GetParamValue("QueueMaxDepth", ref this.QueueMaxDepth);

            myConfig.GetParamValue("BloomFilterFileName", ref this.BloomFilterFileName);
            myConfig.GetParamValue("BloomFilter.Count", ref BloomFilter.Count);

            myConfig.GetParamValue("QueueAllowPattern", ref this.QueueAllowPattern);
            myConfig.GetParamValue("QueueForbidPattern", ref this.QueueForbidPattern);
            myConfig.GetParamValue("QueueAllowTypes", ref this.QueueAllowTypes);
            myConfig.GetParamValue("QueueForbidTypes", ref this.QueueForbidTypes);
            if (!b)
                myConfig.Save();
        }
        
        private void ConfigSave()
        { 
            ConfigSave(this.GetPath_ConfigFile());// 
        }
        private  void ConfigSave(string file)
        {
            MyConfig myConfig = MyConfigHelper.Get(file);

            myConfig.SetParamValue("TaskName", this.TaskName);//
            myConfig.SetParamValue("Modetype", this.ModeType.ToString());//

            myConfig.SetParamValue("BaseUrl", this.BaseUrl.ToString());
            myConfig.SetParamValue("ThreadsCount", this.ThreadsCount);
            myConfig.SetParamValue("QueueMaxDepth", this.QueueMaxDepth);

            myConfig.SetParamValue("BloomFilterFileName", this.BloomFilterFileName);
            myConfig.SetParamValue("BloomFilter.Count", BloomFilter.Count);

            myConfig.SetParamValue("QueueAllowPattern", this.QueueAllowPattern);
            myConfig.SetParamValue("QueueForbidPattern", this.QueueForbidPattern);
            myConfig.SetParamValue("QueueAllowTypes", this.QueueAllowTypes);
            myConfig.SetParamValue("QueueForbidTypes", this.QueueForbidTypes);

            myConfig.Save();
        }
        #endregion
        #endregion
        #region 加载/保存队列
        /// <summary>
        /// 自动保存队列 20分钟
        /// </summary>
        public void QueueSaveAuto()
        {
            ThreadStart ts = new ThreadStart(QueueAutoSave);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
            this.ShowManagerMessage("自动保存队列 线程已启动");//
        }
        /// <summary>
        /// 自动保存队列 20分钟
        /// </summary>
        private void QueueAutoSave()
        {
            while (!this.IsQuit)
            { //解决开始采集等待的问题
                Thread.Sleep(2 * 60 * 1000);//
                QueueSave();
                if (this.WaitingURLs.Count > 6000)
                    Thread.Sleep(18 * 60 * 1000);//
            }
        }
        /// <summary>
        /// 自动加载队列 3分钟
        /// </summary>
        public void QueueLoadAuto()
        {
            ThreadStart ts = new ThreadStart(QueueAutoLoad);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
            this.ShowManagerMessage("自动加载队列 线程已启动");//
        }
        /// <summary>
        /// 自动加载队列 3分钟
        /// </summary>
        private void QueueAutoLoad()
        {
            while (!this.IsQuit)
            {
                Thread.Sleep(2 * 60 * 1000);//
                QueueLoad();//
                if (this.WaitingURLs.Count > 6000)
                    Thread.Sleep(1 * 60 * 1000);//
            }
        }
        #region
        /// <summary>
        /// 保存队列
        /// </summary>
        public void QueueSave()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("保存队列 开始...");//
                string name = DateTime.Now.ToString("yyMMdd-HHmmss")+"-";// +".dat";//
                string root = this.GetSavePath_Mode();
                if (this.SuccessURLs.Count > 4000)
                {
                    ConfigX.SaveEnumerator(root + @"\SuccessURLs\" + name + this.SuccessURLs.Count + ".dat", this.SuccessURLs.GetEnumerator(), true, true);//
                    this.SuccessURLs.Clear();//
                }
                if (this.ErrorURLs.Count > 4000)
                {
                    ConfigX.SaveEnumerator(root + @"\ErrorURLs\" + name + this.ErrorURLs.Count + ".dat", this.ErrorURLs.GetEnumerator(), true, true);//
                    this.ErrorURLs.Clear();//
                }
                if (this.UnProcessedURLs.Count != 0)
                {
                    ConfigX.SaveEnumerator(root + @"\UnProcessedURLs\" + name + this.UnProcessedURLs.Count + ".dat", this.UnProcessedURLs.GetEnumerator(), true, true);//
                    this.UnProcessedURLs.Clear();//
                }
                this.ShowManagerMessage("保存队列 结束...");//
                Monitor.PulseAll(this.Done.SyncObj);//通知 
                //Monitor.Wait(this.Done);//释放
            }
        }
        /// <summary>
        /// 加载队列
        /// </summary>
        public void QueueLoad()
        {
            switch (this.ModeType)
            {
                case ModeType.Monitor:
                    this.QueueLoadFromWatch(); break;//
                //case ModeType.Spider:
                //    this.QueueLoadFromUnProcessedURLs();//
                //    break;//
            }
            this.QueueLoadFromUnProcessedURLs();//
        }
        private void QueueLoadFromWatch()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("加载Watch队列 开始...");//
                string root = this.GetSavePath_Mode() + @"\Watch\";
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//
                string[] files = Directory.GetFiles(root, "*.*");
                if (files.Length != 0)
                {
                    this.ShowManagerMessage("加载Watch队列文件 开始...");//
                    bool b = false;//是否是压缩
                    if (Path.GetExtension(files[0]) == ".dat")
                        b = true;//
                    //ConfigX.LoadQueue(files[0], b, this.WaitingURLs);//
                    this.ShowManagerMessage("加载Watch队列文件 结束...");//
                }
                this.ShowManagerMessage("加载Watch队列 结束...");//
                Monitor.PulseAll(this.Done.SyncObj);//通知
                //Monitor.Wait(this.Done);//释放
            }
        }
        private void QueueLoadFromUnProcessedURLs()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("加载UnProcessed队列 开始...");//
                string root = this.GetSavePath_Mode() + @"\UnProcessedURLs\";
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//
                string[] files;
                while (this.WaitingURLs.Count < 6000)
                {
                    files = Directory.GetFiles(root, "*.dat");
                    if (files.Length != 0)
                    {
                        this.ShowManagerMessage("加载队列文件 开始...");//
                       // ConfigX.LoadQueue(files[0], true, this.WaitingURLs);//
                        File.Delete(files[0]);//删除
                        this.ShowManagerMessage("加载队列文件 结束...");//
                    }
                    else
                    {
                        break;
                    }
                }
                this.ShowManagerMessage("加载UnProcessed队列 结束...");//
                Monitor.PulseAll(this.Done.SyncObj);//通知
                //Monitor.Wait(this.Done);//释放
            }
        }
        #endregion
        #endregion
    }
}
