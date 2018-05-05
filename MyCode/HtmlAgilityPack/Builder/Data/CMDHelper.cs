using System;
using System.Collections.Generic;
using System.Text;
using ThreadWorker;
using CommonPack;
using System.Threading;
namespace Builder.Data
{
   public class CMDHelper
   {
       #region
       private static TaskManager tm =TaskManager.CreatInstance();
       private static bool IsQuit;
       private static ConsoleKeyInfo keyInfo;
       private static bool IsDoAll = true;//
       #endregion
       #region
       #region Static
       #region Message
       static void ShowMessageLine(string msg)
       {
           ShowMessageLine(msg, -1);
       }
       static void ShowMessageLineF(string format, params object[] arg)
       {
           ShowMessageLineF(-1, -1, format, arg);
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
       static void ShowMessageLineF(int top, int left, string format, params object[] arg)
       {
           if (top != -1)
               Console.CursorTop = top;
           if (left != -1)
               Console.CursorLeft = left;
           Console.WriteLine(format, arg);
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
       #endregion
       #region
       static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
       {
           e.Cancel = true;//
       }
    public   static void  Start(string[] args)
       {
           //CommonPack.HtmlAgilityPack.DocumentWithScript.Test();
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/"));//
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/?id=4"));//
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp"));//
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp?"));//
           // Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp?id=4"));
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp?id=4&we=5"));//
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp?id=4&we=5&pp=8"));//
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp?id=4&w=5&u=http://www.baidu.com/a.asp?id=5"));//
           //Console.WriteLine(UrlReWritor.ReWriteUrl("/keyInfo.asp?id=4&we=5&url=http://www.baidu.com/qeqeqqqeq/qeqeqeqqeq/a.asp?id=5&p=24732242424242424242"));//
           //Console.ReadKey();//
           //return;
           //for (int i = 0; i <10000; i++)
           //{
           //    IndexBuilder index = new IndexBuilder();//
           //    index.Title = ChineseCode.GetRegionChineseString(6);//
           //    index.Url = i.ToString();//
           //    index.Text.Append(ChineseCode.GetRegionChineseString(100));//
           //    index.Start();//
           //}
           //IndexBuilder.Close();//
           Start();//
       }
       static void Start()
       {
           Console.WindowWidth = Console.LargestWindowWidth;//
           if (Console.WindowWidth > 128)
               Console.WindowWidth = 128;

           Console.BufferHeight = Console.WindowHeight*3;
           Console.BufferWidth = Console.WindowWidth;
           
           Console.ForegroundColor = ConsoleColor.White;
           Console.BackgroundColor = ConsoleColor.DarkBlue;
           Console.Clear();//
           Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
           tm.EventWorkerDownloadAfter += new WorkerEventHandler(m_EventWorkerDownloadAfter);
           tm.EventManagerMessageChanged += new MessageEventHandler(m_EventManagerMessageChanged);
           tm.EventWorkerMessageChanged += new MessageEventHandler(m_EventWorkerMessageChanged);
           DoWhile();//
       }
       static void ChangeGloalOrCurrent()
       {
           IsDoAll = !IsDoAll;//
       }
        static void DoWhile()
       {
           do
           {
               ShowMessageLine(":");
           }
           while (Console.ReadLine() != "341205");

           while (!IsQuit)
           {
               Show_CmdLine();//
               keyInfo = Console.ReadKey(true);
               ShowMessageLine("");//
               if (IsDoAll)
                   DoAll();//
               else
                   DoCurrent();//

               DoGloal();//
           }
       }
       static void DoGloal()
       {
           switch (keyInfo.Key)
           {
               case ConsoleKey.Tab:
                   ChangeGloalOrCurrent(); break;
               case ConsoleKey.OemPlus:
               case ConsoleKey.DownArrow:
               case ConsoleKey.Add:
                   tm.Gloal_CurrentIndexAdd(); break;

               case ConsoleKey.OemMinus:
               case ConsoleKey.UpArrow:
               case ConsoleKey.Subtract:
                   tm.Gloal_CurrentIndexSub(); break;
               case ConsoleKey.Delete:
                   Console.Clear(); break;
           }
       }
       static void DoAll()
       {
           switch (keyInfo.Key)
           {
               case ConsoleKey.F1: Gloal_ShowHelp();break;
               case ConsoleKey.F2:
                   Gloal_DataInfo(); break;
               case ConsoleKey.F3:break;
               case ConsoleKey.F4:break;

               case ConsoleKey.F5:
                   tm.All_Start(); break;
               case ConsoleKey.F6:
                   tm.All_Quit();
                 ResBuilder.IsQuit = true; SubjectBuilder.IsQuit = true;//
                 ResBuilder.ListToSave(); SubjectBuilder.SaveToDB();//
                   break;
               case ConsoleKey.F7:
                   tm.All_Pause(); break;
               case ConsoleKey.F8:
                   tm.All_Resume(); break;
              
               case ConsoleKey.F9:
                   tm.All_QueueSave(); break;
               case ConsoleKey.F10:
                   tm.All_QueueLoad(); break;
               case ConsoleKey.F11:
                   tm.All_StateSave(); break;
               case ConsoleKey.F12:
                   tm.All_StateLoad(); break;

               case ConsoleKey.D1:
                   tm.All_ShowInfo(); break;
               case ConsoleKey.D2:
                   All_ChangeThreadCount(); break;
               case ConsoleKey.D3:
                   tm.All_ChangeIsShowWorkerMessage(); tm.Current_ChangeIsShowManagerMessage();
                   break;
               case ConsoleKey.D4:
                   ResBuilder.IsShowMessage =! ResBuilder.IsShowMessage; 
                   break; 

               case ConsoleKey.D5:
                   tm.Gloal_ConfigLoad(); break;
               case ConsoleKey.D6:
                   tm.Gloal_ConfigSave(); break;
               case ConsoleKey.D7:
                   Gloal_AddTask(); break;
               case ConsoleKey.D8:
                   Gloal_ChangeCurrentIndex();break;
              
           }
       }
        static void DoCurrent()
       {
           switch (keyInfo.Key)
           {
               case ConsoleKey.F1: Current_ShowHelp();break;
               case ConsoleKey.F2:break;
               case ConsoleKey.F3:break;//
               case ConsoleKey.F4: break;//

               case ConsoleKey.F5:
                   tm.Current_Start(); break;
               case ConsoleKey.F6:
                   tm.Current_Quit(); break;
               case ConsoleKey.F7:
                   tm.Current_Pause(); break;
               case ConsoleKey.F8:
                   tm.Current_Resume(); break;
               

               case ConsoleKey.F9:
                   tm.Current_QueueSave(); break;
               case ConsoleKey.F10:
                   tm.Current_QueueLoad(); break;
               case ConsoleKey.F11:
                   tm.Current_StateSave(); break;
               case ConsoleKey.F12:
                   tm.Current_StateLoad(); break;


               case ConsoleKey.D1:
                   tm.Current_ShowInfo(); break;
               case ConsoleKey.D2:
                   tm.Current_ShowThreadWorks(); break;
               case ConsoleKey.D3:
                   tm.Current_ChangeIsShowManagerMessage(); tm.Current_ChangeIsShowWorkerMessage();  
                   break;//
               case ConsoleKey.D4: Current_ChangeThreadCount(); break;//

               case ConsoleKey.D5:
                   tm.Current_ShowWaitingURLs(); break;
               case ConsoleKey.D6:
                   tm.Current_ShowUnProcessedURLs(); break;
               case ConsoleKey.D7:
                   tm.Current_ShowSuccessURLs(); break;
               case ConsoleKey.D8:
                   tm.Current_ShowErrorURLs(); break;
           }
       }
       static void Gloal_DataInfo()
       {
           ShowMenuName("资源信息");
           ShowMessageLineF("{0,-18}{1,-18}", "Email.Count:", ResBuilder.EmailList.Count);//
           ShowMessageLineF("{0,-18}{1,-18}", "Mobile.Count:", ResBuilder.MobileList.Count);//

           //ShowMessageLineF("{0,-18}{1,-18}", "Subject.Count:", SubjectBuilder.List.Count);//

           ShowMessageLineF("{0,-18}{1,-18}", "BloomFilter.Count:", BloomFilterFactory.BloomFilters.Count);//
           ShowMessageLineF("{0,-18}{1,-18}", "Config.Count:", MyConfigHelper.ConfigDictionary.Count);//
          ShowMessageLine();//
       }
       static void Gloal_ShowHelp()
       {
           ShowMenuName("Gloal系统帮助");
           string[] str = new string[]{
           "F1=帮助菜单","F2=查看资源信息","F3=","F4=","F5=All启动","F6=All停止","F7=All暂停","F8=All恢复",
           "F9=All保存队列","F10=All加载队列","F11=All保存状态","F12=All加载状态",
           "1=任务信息","2=All更改线程数","3=On/Off消息","4=On/Off数据消息","5=加载任务设置","6=保存任务设置","7=添加任务","8=更改Current"};
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[0], str[1], str[2], str[3]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[4], str[5], str[6], str[7]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[8], str[9], str[10], str[11]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[12], str[13], str[14], str[15]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[16], str[17], str[18], str[19]);//
           ShowMessageLine();//
       }
       static void Current_ShowHelp()
       {
           ShowMenuName("系统帮助");
           string[] str = new string[]{
           "F1=帮助菜单","F2=","F3=","F4=","F5=启动","F6=停止","F7=暂停","F8=恢复",
           "F9=保存队列","F10=加载队列","F11=保存状态","F12=加载状态",
           "1=队列信息","2=线程状态","3=On/Off消息","4=更改线程数","5=等待队列","6=未处理队列","7=成功队列","8=错误队列"};
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[0], str[1], str[2], str[3]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[4], str[5], str[6], str[7]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[8], str[9], str[10], str[11]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[12], str[13], str[14], str[15]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[16], str[17], str[18], str[19]);//
           ShowMessageLine();//
       }
       static void Show_CmdLine()
       {
           //ShowMessageLine();//
           if (IsDoAll)
           {
               Console.Write(@"Cmd:Gloal\>");//
           }
           else {
               Console.Write(@"Cmd:Gloal\" + tm.Current_GetName() + @"\>");//
           }
       }
       #endregion
       #region
       static bool m_EventWorkerDownloadAfter(ThreadWorker.ThreadWorker sender)
       {
           //string str = "：sh@hotmail.com :a@b.comp 我cc@cc.ne哈　123@123.com 123r@123r.rrr";//
           // ResBuilder.GetEmail(str);//
            //return true;//

           //ResBuilder resBuilder = new ResBuilder();//
           //resBuilder.Init(sender.CurrentUri.ToString(), sender.DocWithLinks, sender.WorkManager.TaskName, sender.WorkManager.ModeType);//
           //resBuilder.ThreadStart();//

          // return true;//
//
           SubjectBuilder sb = new SubjectBuilder();
           sb.Init(sender.CurrentUri.ToString(), sender.DocWithLinks, sender.WorkManager.TaskName, sender.WorkManager.ModeType);//
           sb.ThreadStart();//

           return true;//

           //PageBuilder pb = new PageBuilder();
           //pb.Init(sender.CurrentUri.ToString(), sender.DocWithLinks, sender.WorkManager.TaskName, sender.WorkManager.ModeType);//
           //pb.ThreadStart();//

           //return true;//
       }
       static void m_EventManagerMessageChanged(string msg)
       {
           ShowMessageLine(msg);//, -1, 0,30);
       }
       static void m_EventWorkerMessageChanged(string msg)
       {
           ShowMessageLine(msg);//, -1, 30, 70);
       }
        #endregion
       
       #region Gloal
       static void Gloal_AddTask()
       {
           ShowMessageLine("输入名称:");//
           string name = Console.ReadLine();//
           if(name.Length==0)
               name="新任务"+tm.ThreadManagers.Count;
           foreach (string s in Enum.GetNames(typeof(ModeType)))
               Console.Write(" " + s);//
           ShowMessageLine("\r\n输入ModeType:");//
           string type = Console.ReadLine();//
           tm.Gloal_AddTask(name, type);//
       }
       static void Gloal_ChangeCurrentIndex()
       {
           ShowMessageLine("输入Index:");//
           int c = 0;//
           int.TryParse(Console.ReadLine(), out c);//
           tm.Gloal_ChangeCurrentIndex(c);//
       }
        static void Current_ChangeThreadCount()
       {
           ShowMessageLine("输入线程数:");//
           int c = 0;//
           int.TryParse(Console.ReadLine(),out c);//
           tm.Current_ChangeThreadCount(c);//
       }
        static void All_ChangeThreadCount()
       {
           ShowMessageLine("输入线程数:");//
           int c = 0;//
           int.TryParse(Console.ReadLine(), out c);//
           tm.All_ChangeThreadCount(c);//
       }
       #endregion
       #region
      
        #endregion
   }
}
