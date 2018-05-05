using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CommonPack;
using System.Threading;
namespace ThreadWorker
{
   public class TaskManager
   {
       #region Instance
       private TaskManager()
       { 

       }
        static TaskManager taskManager;
       public static TaskManager CreatInstance()
       { 
           if(taskManager==null)
                taskManager=new TaskManager();//
           return taskManager;//
       }
       static TaskManager()
       {
           ThreadStart ts = new ThreadStart(GCCollect);
           Thread t = new Thread(ts);//
           t.IsBackground = true;//
           t.Start();//
       }
       #endregion
       #region 字段
       /// <summary>
       /// QueueSave路径
       /// </summary>
       public static string SavePathRoot = System.AppDomain.CurrentDomain.BaseDirectory + @"\";
       public static string AppPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\";
       public IList<ThreadManager> ThreadManagers= new List<ThreadManager>();
       private int CurrentIndex;
       private static bool IsQuit;
       #endregion
       #region Event
       /// <summary>
       /// 自定义事件--线程下载后
       /// </summary>
       public event WorkerEventHandler EventWorkerDownloadAfter;

       public event MessageEventHandler EventManagerMessageChanged;
       public event MessageEventHandler EventWorkerMessageChanged;
       /// <summary>
       ///  触发自定义事件--下载后
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       /// <returns></returns>
       protected virtual bool OnWorkerDownloadAfter(ThreadWorker sender)
       {
           if (EventWorkerDownloadAfter != null)
               return EventWorkerDownloadAfter(sender);
           return true;
       }
       protected virtual void OnManagerMessageChanged(string msg)
       {
           if (EventManagerMessageChanged != null)
               EventManagerMessageChanged(msg);
       }
       protected virtual void OnWorkerMessageChanged(string msg)
       {
           if (EventWorkerMessageChanged != null)
               EventWorkerMessageChanged(msg);
       }
#endregion
       #region Current
       public void Current_ChangeIsShowWorkerMessage()
       {
           this.ChangeIsShowWorkerMessage(CurrentIndex);//
       }
       public void Current_ChangeIsShowManagerMessage()
       {
           this.ChangeIsShowManagerMessage(CurrentIndex);//
       }
       public void Current_ChangeThreadCount(int count)
       {
           this.ChangeThreadCount(CurrentIndex,count);//
       }
       public string Current_GetName()
       {
         return  this.GetName(CurrentIndex);//
       }
       public void Current_Pause()
       {
           this.Pause(CurrentIndex);//
       }
       public void Current_QueueLoad()
       {
           this.QueueLoad(CurrentIndex);//
       }
       public void Current_QueueSave()
       {
           this.QueueSave(CurrentIndex);//
       }
       public void Current_Quit()
       {
           this.Quit(CurrentIndex);//
       }
       public void Current_Resume()
       {
           this.Resume(CurrentIndex);//
       }
       public void Current_Start()
       {
           this.Start(CurrentIndex);//
       }
       public void Current_StateLoad()
       {
           this.StateLoad(CurrentIndex);//
       }
       public void Current_StateSave()
       {
           this.StateSave(CurrentIndex);//
       }
       #endregion
       #region Current Show
       public void Current_ShowErrorURLs()
       {
           this.ShowErrorURLs(CurrentIndex);//
       }
       public void Current_ShowInfo()
       {
           this.ShowInfo(CurrentIndex);//
       }
       public void Current_ShowSuccessURLs()
       {
           this.ShowSuccessURLs(CurrentIndex);//
       }
       public void Current_ShowThreadWorks()
       {
           this.ShowThreadWorks(CurrentIndex);//
       }
       public void Current_ShowUnProcessedURLs()
       {
           this.ShowUnProcessedURLs(CurrentIndex);//
       }
       public void Current_ShowWaitingURLs()
       {
           this.ShowWaitingURLs(CurrentIndex);//
       }
       #endregion
       #region Index
       private void Start(int index)
       {
           ThreadManagers[index].IsShowManagerMessage = true;
           ThreadManagers[index].IsShowWorkerMessage = true;

           ThreadManagers[index].EventWorkerDownloadAfter += new WorkerEventHandler(OnWorkerDownloadAfter);
           ThreadManagers[index].EventManagerMessageChanged += new MessageEventHandler(OnManagerMessageChanged);
           ThreadManagers[index].EventWorkerMessageChanged += new MessageEventHandler(OnWorkerMessageChanged);
           ThreadManagers[index].StateLoad();//

           ThreadStart ts = new ThreadStart(ThreadManagers[index].Start);
           Thread t = new Thread(ts);//
           t.Start();//
           Thread.Sleep(1000);//
           ThreadManagers[index].QueueSaveAuto();//
           Thread.Sleep(1000);//
           ThreadManagers[index].QueueLoadAuto();//
           Thread.Sleep(1000);//
           ThreadManagers[index].StateSaveAuto();//

           ThreadManagers[index].IsShowManagerMessage = false;
           ThreadManagers[index].IsShowWorkerMessage = false;
       }
       private void Quit(int index)
       {
           //ThreadManagers[index].Quit();//
           ThreadStart ts = new ThreadStart(ThreadManagers[index].QuitAndSave);
           Thread t = new Thread(ts);//
           t.Start();//
       }
       private void Pause(int index)
       {
           ThreadManagers[index].Pause();//
       }
       private void Resume(int index)
       {
           ThreadManagers[index].Resume();//
       }
       private void StateSave(int index)
       {
           ThreadManagers[index].StateSave();//
       }
       private void StateLoad(int index)
       {
           ThreadManagers[index].StateLoad();//
       }
       private void QueueSave(int index)
       {
           ThreadManagers[index].QueueSave();//
       }
       private void QueueLoad(int index)
       {
           ThreadManagers[index].QueueLoad();//
       }
       private void ChangeThreadCount(int index, int count)
       {
           ThreadManagers[index].ChangeThreadCount(count);
       }
       private void ChangeIsShowManagerMessage(int index)
       {
           ThreadManagers[index].IsShowManagerMessage = !ThreadManagers[index].IsShowManagerMessage;
       }
       private void ChangeIsShowWorkerMessage(int index)
       {
           ThreadManagers[index].IsShowWorkerMessage = !ThreadManagers[index].IsShowWorkerMessage;
       }
       private string GetName(int index)
       {
           if (ThreadManagers.Count == 0 || ThreadManagers[index] == null)
               return "[None]";//
           return "["+index+@"]\"+ThreadManagers[index].TaskName + @"\" + ThreadManagers[index].ModeType.ToString();//
       }
       #endregion
       #region Index  Show

       private void ShowWaitingURLs(int index)
       {
           ShowMenuName("WaitingURLs");
           lock (ThreadManagers[index].Done.SyncObj)
           {
               foreach (PowerUri uri in ThreadManagers[index].WaitingURLs)
               {
                   ShowMessageLine(uri.ToString());//
               }
               Monitor.PulseAll(ThreadManagers[index].Done.SyncObj);//
           }
       }
       private void ShowUnProcessedURLs(int index)
       {
           ShowMenuName("UnprocessedURLs");
           lock (ThreadManagers[index].Done.SyncObj)
           {
               foreach (PowerUri uri in ThreadManagers[index].UnProcessedURLs)
               {
                   ShowMessageLine(uri.ToString());//
               }
               Monitor.PulseAll(ThreadManagers[index].Done.SyncObj);//
           }
       }
       private void ShowSuccessURLs(int index)
       {
           ShowMenuName("SuccessURLs");
           lock (ThreadManagers[index].Done.SyncObj)
           {
               foreach (PowerUri uri in ThreadManagers[index].SuccessURLs)
               {
                   ShowMessageLine(uri.ToString());//
               }
               Monitor.PulseAll(ThreadManagers[index].Done.SyncObj);//
           }
       }
       private void ShowErrorURLs(int index)
       {
           ShowMenuName("ErrorURLs");
           lock (ThreadManagers[index].Done.SyncObj)
           {
               foreach (PowerUri uri in ThreadManagers[index].ErrorURLs)
               {
                   ShowMessageLine(uri.ToString());//
               }
               Monitor.PulseAll(ThreadManagers[index].Done.SyncObj);//
           }
       }
       private void ShowThreadWorks(int index)
       {
           ShowMenuName("ThreadWorkers");
           lock (ThreadManagers[index].Done.SyncObj)
           {
               for (int i = 0; i < ThreadManagers[index].ThreadWorkers.Count; )
               { if (ThreadManagers[index].ThreadWorkers[i].Thread.IsAlive)
                   { i++; continue; }
                   ThreadManagers[index].ThreadWorkers.Remove(ThreadManagers[index].ThreadWorkers[i]);
               }
               ShowMessageLineF("[{0,3}] [{1,4}] [{2,10}] [{3,8}] [{4,8}] [{5,8}] [{6}]",
                    "Num", "ID", "Priority", "Alive", "Background", "PoolThread", "CurrentUri");//
               foreach (ThreadWorker tk in ThreadManagers[index].ThreadWorkers)
               {
                   ShowMessageLineF("[{0,3}]{1,4}{2,10}{3,8}{4,8}{5,8} {6}",
                    tk.Number, tk.Thread.ManagedThreadId, tk.Thread.Priority.ToString(), tk.Thread.IsAlive, tk.Thread.IsBackground, tk.Thread.IsThreadPoolThread, tk.CurrentUri);//
               }
               Monitor.PulseAll(ThreadManagers[index].Done.SyncObj);//
           }
       }
       private void ShowInfo(int index)
       {
           ShowMenuName("Info");
           lock (ThreadManagers[index].Done.SyncObj)
           {
               ShowMessageLine("TaskName:" + ThreadManagers[index].TaskName);//
               ShowMessageLine("ModeType:" + ThreadManagers[index].ModeType);//
               ShowMessageLine("BaseUrl:" + ThreadManagers[index].BaseUrl);//
               ShowMessageLine("ThreadsCount:" + ThreadManagers[index].ThreadsCount);//
               ShowMessageLine("QueueMaxDepth:" + ThreadManagers[index].QueueMaxDepth);//

               ShowMessageLine("QueueAllowPattern:" + ThreadManagers[index].QueueAllowPattern);//
               ShowMessageLine("QueueForbidPattern:" + ThreadManagers[index].QueueForbidPattern);//
               ShowMessageLine("QueueAllowTypes:" + ThreadManagers[index].QueueAllowTypes);//
               ShowMessageLine("QueueForbidTypes:" + ThreadManagers[index].QueueForbidTypes);//

               ShowMessageLine("已下载:" + ThreadManagers[index].DownloadCount + " 用时/秒:" + ThreadManagers[index].Timer.GetElapsedTime() + " 速度:" + ThreadManagers[index].DownloadCount / ThreadManagers[index].Timer.GetElapsedTime());//

               ShowMessageLine("WaitingQueue:" + ThreadManagers[index].WaitingURLs.Count + " UnProcessedQueue:" + ThreadManagers[index].UnProcessedURLs.Count);//
               ShowMessageLine("SuccessQueue:" + ThreadManagers[index].SuccessURLs.Count + " ErrorQueue:" + ThreadManagers[index].ErrorURLs.Count);//

               ShowMessageLine("BloomFilterFilename:" + ThreadManagers[index].BloomFilterFileName);//
               ShowMessageLine("BloomFilter:" + ThreadManagers[index].BloomFilter.Count);//
           }
           ShowMessageLine();//
       }
       #endregion
       #region Static
       #region
       static void ShowMessageLine(string msg)
       {
           ShowMessageLine(msg, -1);
       }
       static void ShowMessageLine(string msg, int top)
       {
           ShowMessageLine(msg, top, -1);
       }
       static void ShowMessageLine(string msg, int top, int left)
       {
           ShowMessageLine(msg, top, left, -1);
       }
       static void ShowMessageLine(string msg, int top, int left, int length)
       {
           if (top != -1)
               Console.CursorTop = top;
           if (left != -1)
               Console.CursorLeft = left;
           if (length != -1 && length < msg.Length)
               Console.WriteLine(msg.Substring(0, length));
           else
               Console.WriteLine(msg);
       }
       static void ShowMessageLineF(string format, params object[] arg)
       {
           ShowMessageLineF(-1, -1, format, arg);
       }
       static void ShowMessageLineF( int top, int left,string format,params object[] arg)
       {
           if (top != -1)
               Console.CursorTop = top;
           if (left != -1)
               Console.CursorLeft = left;
               Console.WriteLine(format,arg);
       }
       static void ShowMenuName(string name)
       {
           ShowMessageLine();
           Console.WriteLine("{0,60}{1,60}", name, "");
           ShowMessageLine();
       }
       static void ShowMessageLine()
       {
           Console.WriteLine("-".PadLeft(120, '-'));
       }
         #endregion
       #endregion
       #region All
       public void All_ShowInfo()
       {
           ShowMenuName("ShowInfoAll");
           string[] str = new string[]{
           "Num","TaskName","ModeType","Threads","Start","Pause","Quit","Waiting","UnProcess","Downloads",
           "Time","Speed"};
           ShowMessageLineF("{0,3}{1,15}{2,10}{3,7}{4,7}{5,7}{6,7}{7,10}{8,10}{9,10}{10,12}{11,10}",
               str[0], str[1], str[2], str[3],
               str[4], str[5], str[6], str[7], str[8], str[9], str[10], str[11]);//
           int totalDownloadCount = 0;
           float maxTime=0;
           for (int i = 0; i < this.ThreadManagers.Count; i++)
           {
               ShowMessageLineF("{0,3}{1,15}{2,10}{3,7}{4,7}{5,7}{6,7}{7,10}{8,10}{9,10}{10,12}{11,10}",
                   i, ThreadManagers[i].TaskName, ThreadManagers[i].ModeType, ThreadManagers[i].ThreadsCount,
                   ThreadManagers[i].IsStarted, ThreadManagers[i].IsPause, ThreadManagers[i].IsQuit,
                   ThreadManagers[i].WaitingURLs.Count, ThreadManagers[i].UnProcessedURLs.Count, ThreadManagers[i].DownloadCount,
                    ThreadManagers[i].Timer.GetElapsedTime(), ThreadManagers[i].DownloadCount / ThreadManagers[i].Timer.GetElapsedTime());//
               
               totalDownloadCount += ThreadManagers[i].DownloadCount;//总下载
               
               if(maxTime<ThreadManagers[i].Timer.GetElapsedTime())
                   maxTime=ThreadManagers[i].Timer.GetElapsedTime();//
           }
           ShowMessageLine("Speed:"+totalDownloadCount/maxTime+" Page/s");//
           ShowMessageLine();//

       }
       public void All_ChangeThreadCount(int count)
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
               this.ThreadManagers[i].ChangeThreadCount(count);//
       }
       public void All_Start()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
               this.Start(i);//
       }
       public void All_Quit()
       {
           IsQuit = true;//

           for (int i = 0; i < this.ThreadManagers.Count; i++)
           this.Quit(i);//
       }
       public void All_Resume()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
           this.Resume(i);//
       }
       public void All_Pause()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
           this.Pause(i);//
       }
       public void All_QueueLoad()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
           this.QueueLoad(i);//
       }
       public void All_QueueSave()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
           this.QueueSave(i);//
       }
       public void All_StateLoad()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
               this.StateLoad(i);//
       }
       public void All_StateSave()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
               this.StateSave(i);//
       }
       public void All_ChangeIsShowManagerMessage()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
               this.ChangeIsShowManagerMessage(i);//
       }
       public void All_ChangeIsShowWorkerMessage()
       {
           for (int i = 0; i < this.ThreadManagers.Count; i++)
               this.ChangeIsShowWorkerMessage(i);//
       }
       #endregion
       #region Gloal
       public void Gloal_AddTask(string taskName, string modeType)
       {
           ThreadManager m = new ThreadManager();

           m.TaskName = taskName;//
           m.ModeType = (ModeType)Enum.Parse(typeof(ModeType), modeType, false);//
           this.ThreadManagers.Add(m);//
       }

       public void Gloal_ConfigLoad()
       {
           Gloal_ConfigLoad(AppPath + @"\App.xml");
       } 
       public void Gloal_ConfigLoad(string file)
       {
           bool b = File.Exists(file);
           MyConfig myConfig =MyConfigHelper.Get(file);//
           
               myConfig.GetParamValue("SavePathRoot",ref SavePathRoot);//
               IDictionary<string,string> taskList =myConfig.Get("task");
               this.ThreadManagers.Clear();
               foreach (KeyValuePair<string,string> kv in taskList)
               {
                       Gloal_AddTask(kv.Key, kv.Value);//
               }
           if (!b)
               myConfig.Save();
       }
       public void Gloal_ConfigSave()
       {
           Gloal_ConfigSave(AppPath + @"\App.xml");
       }
       public void Gloal_ConfigSave(string file)
       {
           MyConfig myConfig = MyConfigHelper.Get(file);//
           foreach (ThreadManager m in this.ThreadManagers)
           {
               myConfig.Get("task").Add( m.TaskName, m.ModeType.ToString());//
           }
           myConfig.SetParamValue("SavePathRoot", SavePathRoot);//
           myConfig.Save();
        }

       public void Gloal_CurrentIndexAdd()
       {
           if (++CurrentIndex == this.ThreadManagers.Count)
               CurrentIndex = 0;//
       }
       public void Gloal_CurrentIndexSub()
       {
           if (--CurrentIndex <0)
               CurrentIndex = this.ThreadManagers.Count-1;//
       }
       public void Gloal_ChangeCurrentIndex(int index)
       {
           if(index>=0&&index<this.ThreadManagers.Count)
                 CurrentIndex=index;//
       }
       #endregion
       #region GCCollect
      
       /// <summary>
       /// 垃圾收集
       /// </summary>
       private static void GCCollect()
       {
           while (!IsQuit)
           {
               Thread.Sleep(20*1000);
               GC.Collect();//
           }
       }
       #endregion
   }
}
