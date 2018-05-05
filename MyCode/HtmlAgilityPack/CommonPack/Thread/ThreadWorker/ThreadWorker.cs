using System;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using HtmlAgilityPack;
using CommonPack;
using CommonPack.HtmlAgilityPack;
namespace ThreadWorker
{
    
    /// <summary>
    /// 线程工作类
    /// </summary>
    public class ThreadWorker
    {   
        #region 构造方法
        public ThreadWorker(ThreadManager workManager, int num)
        {
            WorkManager = workManager;//
            Number = num;//
        }
        public ThreadWorker()
        {
            
        }
        #endregion
        #region  字段
        private HtmlWeb HtmlWeb;
		public Thread Thread;
        public DocumentWithLinks DocWithLinks;
        public ThreadManager WorkManager;
        /// <summary>
        /// 线程标识
        /// </summary>
		public int Number;
        /// <summary>
        /// 当前处理的Url
        /// </summary>
        public PowerUri CurrentUri;
        #endregion
        #region 自定义事件
        /// <summary>
        /// 自定义事件--下载前
        /// </summary>
        public event WorkerEventHandler EventWorkerDownloadBefore;
        /// <summary>
        /// 自定义事件--下载后
        /// </summary>
        public event WorkerEventHandler EventWorkerDownloadAfter;
        /// <summary>
        /// 触发自定义事件--下载后
        /// </summary>
        /// <param name="e"></param>
        protected virtual bool OnWorkerDownloadAfter()
        {
            if (EventWorkerDownloadAfter != null)
                return EventWorkerDownloadAfter(this);
            return true;
        }
        /// <summary>
        /// 触发自定义事件--下载前
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool OnWorkerDownloadBefore()
        {
            if (EventWorkerDownloadBefore != null)
                return EventWorkerDownloadBefore(this);
            return true;
        }
        #endregion
        #region  公共方法
        public void Init()
        {
            HtmlWeb= new HtmlWeb();
           HtmlWeb.UseCookies = true;//
        }
		public void Start()
        {
			ThreadStart ts = new ThreadStart(this.Process);
			Thread = new Thread(ts);
			Thread.Start();
		}
        #endregion
        #region  私有方法
        private void Process()
        {
            while (!WorkManager.IsQuit && this.Number < WorkManager.ThreadsCount)
            {
                this.WorkManager.Done.WorkerBegin();
                this.CurrentUri = WorkManager.UriFromWaiting();
                try
                {
                    ParseLinks(this.CurrentUri);//

                    this.WorkManager.UriToProcessed(this.CurrentUri, UriStatus.SUCCESS);//
                }
                catch(Exception ex)
                {
                   // Log.Error(ex.Message+this.CurrentUri.ToString());//
                    this.WorkManager.UriToProcessed(this.CurrentUri, UriStatus.FAILED);//
                }
                this.CurrentUri = null;//
                this.WorkManager.Done.WorkerEnd();
            }
        }
        private void ParseLinks(PowerUri PowerUri)
        {
            //try
            //{
                DateTime t1 = DateTime.Now;
                if (!this.OnWorkerDownloadBefore())//激发事件
                    return;//
                HtmlDocument doc = HtmlWeb.Load(PowerUri.ToString());
            
                this.WorkManager.DownloadCount++;//
                this.WorkManager.ShowWorkerMessage("[" + this.Number.ToString().PadLeft(2, '0') + "] " + t1.ToString("mm:ss") + "/" + DateTime.Now.ToString("mm:ss") + " Down：" + PowerUri.ToString());
                DocWithLinks = new DocumentWithLinks(doc);
                DocWithLinks.IsTextContent = HtmlWeb.IsTextContent;//
                //this.DocWithLinks.GetReferences();//
                DocWithLinks.GetAllLinks();//
                if (this.OnWorkerDownloadAfter())//激发事件
                {
                    PowerUri newPowerUri;
                    foreach (string str in DocWithLinks.AllLinks)
                    {
                        newPowerUri = new PowerUri(PowerUri, str);
                        newPowerUri.Depth = PowerUri.Depth + 1;

                        WorkManager.UriToQueue(newPowerUri);
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex.StackTrace + ex.Message + " : " + this.CurrentUri.ToString());//
            //}
        }
        #endregion
	}
}
