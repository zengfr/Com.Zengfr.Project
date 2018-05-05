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
        /// ֩��ģʽ
        /// </summary>
        Spider = 0x1,
        /// <summary>
        /// ����ģʽ
        /// </summary>
        Monitor = 0x2,
        /// <summary>
        /// ������ģʽ
        /// </summary>
        SDomain = 0x4,
        /// <summary>
        /// ��������ģʽ
        /// </summary>
        FDomain = 0x8,
    }
    public delegate bool ManagerEventHandler(ThreadManager sender, PowerUri uri);
    public delegate bool WorkerEventHandler(ThreadWorker sender);
    public delegate void MessageEventHandler(string msg);
    /// <summary>
    /// �̹߳���������
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
        #region �ֶ�
        public BloomFilter BloomFilter;
        public string BloomFilterFileName;
        public HighResolutionTimer Timer;
        public Done Done;
        /// <summary>
        /// �Ƿ��Ѿ�����
        /// </summary>
        public bool IsStarted;
        /// <summary>
        /// �Ƿ���ͣ
        /// </summary>
        public bool IsPause;
        /// <summary>
        /// �Ƿ��˳�
        /// </summary>
        public bool IsQuit;
        /// <summary>
        /// �Ƿ���ʾ��Ϣ
        /// </summary>
        public bool IsShowManagerMessage;
        /// <summary>
        /// �Ƿ���ʾ��Ϣ
        /// </summary>
        public bool IsShowWorkerMessage;
        /// <summary>
        /// �����ļ��У�����
        /// </summary>
        public string TaskName;
        /// <summary>
        /// ����ƥ����ʽ
        /// </summary>
        private Regex QueueAllowPatternRegex;
        private Regex QueueForbidPatternRegex;
        /// <summary>
        /// ����ƥ����ʽ Ĭ��=this.BaseUrl.Host;//
        /// </summary>
        public string QueueAllowPattern;
        public string QueueForbidPattern;
        /// <summary>
        /// ����� ����:.html.aspx.php.xml.do.jsp.rails.css.js
        /// </summary>
        public string QueueAllowTypes;
        /// <summary>
        /// ��ֹ�� ����:.exe.com.msi/.rar.zip.z7.iso.img/.jpg.gif.png.ico/.swf.dat.mpeg.wmv.rm.mp3.wma/.doc.pdf.ppt.chm.mht
        /// </summary>
        public string QueueForbidTypes;
        /// <summary>
        /// �߳��� Ĭ��20;
        /// </summary>
        public int ThreadsCount;
        /// <summary>
        /// ��ʼ��URL
        /// </summary>
        public PowerUri BaseUrl;
        /// <summary>
        /// ��ҳ��� Ĭ��0=���� -1=����Ӷ��е�UnProcessed
        /// </summary>
        public int QueueMaxDepth;
        /// <summary>
        /// ����ģʽ
        /// </summary>
        public ModeType ModeType;
        /// <summary>
        /// �̼߳���
        /// </summary>
        public IList<ThreadWorker> ThreadWorkers;
        /// <summary>
        /// �Ѵ����URLs-�ɹ�
        /// </summary>
        public IList<PowerUri> SuccessURLs;
        /// <summary>
        /// ׼�������URLs����
        /// </summary>
        public Queue<PowerUri> WaitingURLs;
        /// <summary>
        /// �Ѵ����URLs-ʧ��
        /// </summary>
        public IList<PowerUri> ErrorURLs;
        /// <summary>
        /// δ�����URLs
        /// </summary>
        public IList<PowerUri> UnProcessedURLs;
        /// <summary>
        /// ���ؽ����˵�urlͳ��
        /// </summary>
        public int DownloadCount = 0;//
        #endregion
        #region �Զ����¼�
        public event ManagerEventHandler EventEnQueueBefore;
        public event ManagerEventHandler EventEnQueueAfter;
        /// <summary>
        /// �Զ����¼�--�������
        /// </summary>
        public event ManagerEventHandler EventEndTasks;
        /// <summary>
        /// �Զ����¼�--�߳�����ǰ
        /// </summary>
        public event WorkerEventHandler EventWorkerDownloadBefore;
        /// <summary>
        /// �Զ����¼�--�߳����غ�
        /// </summary>
        public event WorkerEventHandler EventWorkerDownloadAfter;
        public event MessageEventHandler EventManagerMessageChanged;
        public event MessageEventHandler EventWorkerMessageChanged;
        /// <summary>
        /// �����Զ����¼�--����ǰ
        /// </summary>
        /// <param name="e"></param>
        public virtual bool OnWorkerDownloadBefore(ThreadWorker sender)
        {
            if (EventWorkerDownloadBefore != null)
                return EventWorkerDownloadBefore(sender);
            return true;
        }
        /// <summary>
        ///  �����Զ����¼�--���غ�
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
        /// �����Զ����¼�--���б仯
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
        /// �����Զ����¼�--�������
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnEndTasks(PowerUri e)
        {
            if (EventEndTasks != null)
                EventEndTasks(this, e);
        }
        #endregion
        #region ���д���
        /// <summary>
        ///     Spiderģʽʱ=UriToUnProcessed//
        ///     Monitorģʽʱ=UriToWaiting
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
        /// ��ӵ�δ�������
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
                        if (OnEnQueueBefore(PowerUri))////�����¼�
                        {
                            UnProcessedURLs.Add(PowerUri);
                            OnEnQueueAfter(PowerUri);////�����¼�
                        }
                    }
                    Monitor.PulseAll(this.Done.SyncObj);//
                }
            }
        }
        /// <summary>
        /// ��ӵ��ȴ��������
        /// </summary>
        /// <param name="PowerUri"></param>
        private void UriToWaiting(PowerUri PowerUri)
        {
            if (this.ModeType == ModeType.Monitor && PowerUri.Depth > 1)
                return;//����ģʽ��Depth > 1�Ĳ����
            lock (this.Done.SyncObj)
            {
                if (!WaitingURLs.Contains(PowerUri))
                {
                    WaitingURLs.Enqueue(PowerUri);
                }
                if (WaitingURLs.Count < this.ThreadsCount + 1)//�еȴ��߳�
                    Monitor.PulseAll(this.Done.SyncObj);//
            }
        }
        /// <summary>
        /// ��ӵ��Ѵ������
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
                Monitor.PulseAll(this.Done.SyncObj);//֪ͨ
            }
        }
        /// <summary>
        /// ��ȡ�ȴ����е���Url
        /// </summary>
        /// <returns></returns>
        public PowerUri UriFromWaiting()
        {
            lock (this.Done.SyncObj)
            {
                while (WaitingURLs.Count ==0)
                {
                    Monitor.Wait(this.Done.SyncObj, 1000);
                    if (WaitingURLs.Count == 0)//�޸��ȴ�2008.10.15
                    {//�޸�
                        this.QueueSave();
                        this.QueueLoad();
                    }
                    if (this.IsQuit) break;//�޸�2008.08.15
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
                    else if (this.IsQuit) break;//�޸�2008.08.15
                    else
                        Monitor.Wait(this.Done.SyncObj, 1000);
                        
                }
                return uri;// next;
            }
        }
        #endregion
        #region  �������� GetPath
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
        #region  ��������1
        /// <summary>
        /// -1=��ֹ 1=���� 0=δ����
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
                bool rtn = this.QueueForbidTypes.IndexOf(ext) != -1 || this.QueueForbidTypes == "*";//��ֹ�Ĵ���
                if (rtn) return -1;//
                rtn = this.QueueAllowTypes.IndexOf(ext) != -1 || this.QueueAllowTypes == "*";//����Ĵ���
                if (rtn) return 1;//
            }
            catch { }
            return 0;//
        }
        /// <summary>
        /// -1=��ֹ 1=���� 0=δ����
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
        #region  ��������2
        /// <summary>
        /// ��ʼ��/
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
            //this.TaskLoad();//��������
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
            this.Timer.Start();//��ʼ��ʱ
            ChangeThreadCount(this.ThreadsCount);
            // now wait to be done
            Done.WaitBegin();
           //this.IsShowWorkerMessage = false;
            //this.IsShowManagerMessage = false;
            Done.WaitDone();
        }
        /// <summary>
        /// �����߳�����
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
                threadWorker.EventWorkerDownloadBefore += new WorkerEventHandler(this.OnWorkerDownloadBefore);//�¼�
                threadWorker.EventWorkerDownloadAfter += new WorkerEventHandler(this.OnWorkerDownloadAfter);//�¼�
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
            this.ShowManagerMessage("�˳�/����...�߳�������");//
            this.IsShowManagerMessage = true;
            this.IsShowWorkerMessage = true;

            this.Quit();//
            this.ShowManagerMessage("�ȴ�ֱ��ȫ���߳�ֹͣ...");//
            this.Done.WaitDone();

            this.StateSave();
            this.QueueSave();//
            this.ConfigSave();//
        }
        /// <summary>
        /// ��ʾ��Ϣ
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
        #region ����/����State

        /// <summary>
        /// �Զ�����State 30����
        /// </summary>
        public void StateSaveAuto()
        {
            ThreadStart ts = new ThreadStart(AutoStateSave);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
            this.ShowManagerMessage("�Զ�����State �߳�������");//
        }
        /// <summary>
        /// �Զ�����State 30����
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
        /// ����״̬
        /// </summary>
        public void StateLoad()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("����State ��ʼ...");//

                string root = this.GetSavePath_State();
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//

                this.ConfigLoad();//

               // ConfigX.LoadList(root + @"UnProcessedURLs.dat", true, this.UnProcessedURLs);//
               // ConfigX.LoadList(root + @"SuccessURLs.dat", true, this.SuccessURLs);//
               // ConfigX.LoadList(root + @"ErrorURLs.dat", true, this.ErrorURLs);//
               // ConfigX.LoadQueue(root + @"WaitingURLs.dat", true, this.WaitingURLs);//
                BloomFilterFactory.Load(this.BloomFilterFileName, GetBloomFilterFilePath());//
                this.ShowManagerMessage("����State ����...");//
                Monitor.PulseAll(this.Done.SyncObj);//֪ͨ
                //Monitor.Wait(this.Done);//�ͷ�
            }
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        public void StateSave()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("����״̬ ��ʼ...");//
                string root = this.GetSavePath_State();
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//
                ConfigSave();//
                ConfigX.SaveEnumerator(root + @"SuccessURLs.dat", this.SuccessURLs.GetEnumerator(), true, false);//
                ConfigX.SaveEnumerator(root + @"ErrorURLs.dat", this.ErrorURLs.GetEnumerator(), true, false);//
                ConfigX.SaveEnumerator(root + @"WaitingURLs.dat", this.WaitingURLs.GetEnumerator(), true, false);//
                ConfigX.SaveEnumerator(root + @"UnProcessedURLs.dat", this.UnProcessedURLs.GetEnumerator(), true, false);//

                BloomFilterFactory.Save(this.BloomFilterFileName, GetBloomFilterFilePath());//
                this.ShowManagerMessage("����״̬ ����...");//
                Monitor.PulseAll(this.Done.SyncObj);//֪ͨ
                //Monitor.Wait(this.Done);//�ͷ�
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
        #region ����/�������
        /// <summary>
        /// �Զ�������� 20����
        /// </summary>
        public void QueueSaveAuto()
        {
            ThreadStart ts = new ThreadStart(QueueAutoSave);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
            this.ShowManagerMessage("�Զ�������� �߳�������");//
        }
        /// <summary>
        /// �Զ�������� 20����
        /// </summary>
        private void QueueAutoSave()
        {
            while (!this.IsQuit)
            { //�����ʼ�ɼ��ȴ�������
                Thread.Sleep(2 * 60 * 1000);//
                QueueSave();
                if (this.WaitingURLs.Count > 6000)
                    Thread.Sleep(18 * 60 * 1000);//
            }
        }
        /// <summary>
        /// �Զ����ض��� 3����
        /// </summary>
        public void QueueLoadAuto()
        {
            ThreadStart ts = new ThreadStart(QueueAutoLoad);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
            this.ShowManagerMessage("�Զ����ض��� �߳�������");//
        }
        /// <summary>
        /// �Զ����ض��� 3����
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
        /// �������
        /// </summary>
        public void QueueSave()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("������� ��ʼ...");//
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
                this.ShowManagerMessage("������� ����...");//
                Monitor.PulseAll(this.Done.SyncObj);//֪ͨ 
                //Monitor.Wait(this.Done);//�ͷ�
            }
        }
        /// <summary>
        /// ���ض���
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
                this.ShowManagerMessage("����Watch���� ��ʼ...");//
                string root = this.GetSavePath_Mode() + @"\Watch\";
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//
                string[] files = Directory.GetFiles(root, "*.*");
                if (files.Length != 0)
                {
                    this.ShowManagerMessage("����Watch�����ļ� ��ʼ...");//
                    bool b = false;//�Ƿ���ѹ��
                    if (Path.GetExtension(files[0]) == ".dat")
                        b = true;//
                    //ConfigX.LoadQueue(files[0], b, this.WaitingURLs);//
                    this.ShowManagerMessage("����Watch�����ļ� ����...");//
                }
                this.ShowManagerMessage("����Watch���� ����...");//
                Monitor.PulseAll(this.Done.SyncObj);//֪ͨ
                //Monitor.Wait(this.Done);//�ͷ�
            }
        }
        private void QueueLoadFromUnProcessedURLs()
        {
            lock (this.Done.SyncObj)
            {
                this.ShowManagerMessage("����UnProcessed���� ��ʼ...");//
                string root = this.GetSavePath_Mode() + @"\UnProcessedURLs\";
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);//
                string[] files;
                while (this.WaitingURLs.Count < 6000)
                {
                    files = Directory.GetFiles(root, "*.dat");
                    if (files.Length != 0)
                    {
                        this.ShowManagerMessage("���ض����ļ� ��ʼ...");//
                       // ConfigX.LoadQueue(files[0], true, this.WaitingURLs);//
                        File.Delete(files[0]);//ɾ��
                        this.ShowManagerMessage("���ض����ļ� ����...");//
                    }
                    else
                    {
                        break;
                    }
                }
                this.ShowManagerMessage("����UnProcessed���� ����...");//
                Monitor.PulseAll(this.Done.SyncObj);//֪ͨ
                //Monitor.Wait(this.Done);//�ͷ�
            }
        }
        #endregion
        #endregion
    }
}
