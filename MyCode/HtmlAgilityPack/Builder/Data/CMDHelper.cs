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
           ShowMenuName("��Դ��Ϣ");
           ShowMessageLineF("{0,-18}{1,-18}", "Email.Count:", ResBuilder.EmailList.Count);//
           ShowMessageLineF("{0,-18}{1,-18}", "Mobile.Count:", ResBuilder.MobileList.Count);//

           //ShowMessageLineF("{0,-18}{1,-18}", "Subject.Count:", SubjectBuilder.List.Count);//

           ShowMessageLineF("{0,-18}{1,-18}", "BloomFilter.Count:", BloomFilterFactory.BloomFilters.Count);//
           ShowMessageLineF("{0,-18}{1,-18}", "Config.Count:", MyConfigHelper.ConfigDictionary.Count);//
          ShowMessageLine();//
       }
       static void Gloal_ShowHelp()
       {
           ShowMenuName("Gloalϵͳ����");
           string[] str = new string[]{
           "F1=�����˵�","F2=�鿴��Դ��Ϣ","F3=","F4=","F5=All����","F6=Allֹͣ","F7=All��ͣ","F8=All�ָ�",
           "F9=All�������","F10=All���ض���","F11=All����״̬","F12=All����״̬",
           "1=������Ϣ","2=All�����߳���","3=On/Off��Ϣ","4=On/Off������Ϣ","5=������������","6=������������","7=�������","8=����Current"};
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[0], str[1], str[2], str[3]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[4], str[5], str[6], str[7]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[8], str[9], str[10], str[11]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[12], str[13], str[14], str[15]);//
           ShowMessageLineF("{0,-18}{1,-18}{2,-18}{3,-18}", str[16], str[17], str[18], str[19]);//
           ShowMessageLine();//
       }
       static void Current_ShowHelp()
       {
           ShowMenuName("ϵͳ����");
           string[] str = new string[]{
           "F1=�����˵�","F2=","F3=","F4=","F5=����","F6=ֹͣ","F7=��ͣ","F8=�ָ�",
           "F9=�������","F10=���ض���","F11=����״̬","F12=����״̬",
           "1=������Ϣ","2=�߳�״̬","3=On/Off��Ϣ","4=�����߳���","5=�ȴ�����","6=δ�������","7=�ɹ�����","8=�������"};
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
           //string str = "��sh@hotmail.com :a@b.comp ��cc@cc.ne����123@123.com 123r@123r.rrr";//
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
           ShowMessageLine("��������:");//
           string name = Console.ReadLine();//
           if(name.Length==0)
               name="������"+tm.ThreadManagers.Count;
           foreach (string s in Enum.GetNames(typeof(ModeType)))
               Console.Write(" " + s);//
           ShowMessageLine("\r\n����ModeType:");//
           string type = Console.ReadLine();//
           tm.Gloal_AddTask(name, type);//
       }
       static void Gloal_ChangeCurrentIndex()
       {
           ShowMessageLine("����Index:");//
           int c = 0;//
           int.TryParse(Console.ReadLine(), out c);//
           tm.Gloal_ChangeCurrentIndex(c);//
       }
        static void Current_ChangeThreadCount()
       {
           ShowMessageLine("�����߳���:");//
           int c = 0;//
           int.TryParse(Console.ReadLine(),out c);//
           tm.Current_ChangeThreadCount(c);//
       }
        static void All_ChangeThreadCount()
       {
           ShowMessageLine("�����߳���:");//
           int c = 0;//
           int.TryParse(Console.ReadLine(), out c);//
           tm.All_ChangeThreadCount(c);//
       }
       #endregion
       #region
      
        #endregion
   }
}
