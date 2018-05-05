using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.InteropServices;
namespace Com.Zfrong.Common.Win32.Console
{
   public  class ConsoleProgram
   {
       [DllImport("user32.dll")]
       static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
       [DllImport("user32.dll")]
       static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
       internal const UInt32 SC_CLOSE = 0xF060;
       internal const UInt32 MF_ENABLED = 0x00000000;
       internal const UInt32 MF_GRAYED = 0x00000001;
       internal const UInt32 MF_DISABLED = 0x00000002;
       internal const uint MF_BYCOMMAND = 0x00000000;
       public static void EnableCloseButton(bool bEnabled)
       {
           IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;
           EnableCloseButton(windowHandle, bEnabled);
       }
       public static void EnableCloseButton(IntPtr windowHandle, bool bEnabled)
       {
           IntPtr hSystemMenu = GetSystemMenu(windowHandle, false);
           EnableMenuItem(hSystemMenu, SC_CLOSE, (uint)(MF_ENABLED | (bEnabled ? MF_ENABLED : MF_GRAYED)));
       }
       public static void SetConfiguration(string key, string value)
       {
          // ConfigurationManager.AppSettings[key].Value = value;
        System.Configuration. Configuration c= ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
          if (c.AppSettings.Settings[key] != null)
              c.AppSettings.Settings[key].Value = value;
          else
              c.AppSettings.Settings.Add(key, value);
          c.Save();
       }
       public static string GetConfiguration(string key)
       {
           return ConfigurationManager.AppSettings[key];
       }
       public static void Sleep(int sec)
       {
           ShowInfo("Thread.Sleep:{0}秒",sec);
           System.Threading.Thread.Sleep(1000 * sec);
       }
       public static string RunCmd(string cmdStr)
       {
           //实例化一个进程类
           Process cmd = new Process();

           //获得系统信息，使用的是 systeminfo.exe 这个控制台程序
           cmd.StartInfo.FileName ="cmd.exe";
           cmd.StartInfo.Arguments="/c "+cmdStr;
           //将cmd的标准输入和输出全部重定向到.NET的程序里

           cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常

           cmd.StartInfo.RedirectStandardInput = true; //标准输入
           cmd.StartInfo.RedirectStandardOutput = true; //标准输出

           //不显示命令行窗口界面
           cmd.StartInfo.CreateNoWindow = true;
           cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

           cmd.Start(); //启动进程

           //获取输出
           //需要说明的：此处是指明开始获取，要获取的内容，
           //[color=#FF0000]只有等进程退出后才能真正拿到[/color]
           string rtn= cmd.StandardOutput.ReadToEnd();

           cmd.WaitForExit();//等待控制台程序执行完成
           cmd.Close();//关闭该进程
           return rtn;
       }
        static void Show(string f, params object[] args)
       {
           try
           {
               System.Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("MMdd HH:mm:ss fff"), string.Format(f, args)));//
           }
           catch { };
       }
       public static bool IsShowInfo;
       public static bool IsShowSuccess;
       public static bool IsShowError;
       public static bool IsShowWarn;
       public static void ShowData(string f, params object[] args)
       {
               System.Console.ForegroundColor = ConsoleColor.White;
               Show(f, args);
               System.Console.ResetColor();
       }
       public static void ShowInfo(string f,params object[] args)
       {
           if (IsShowInfo)
           {
               System.Console.ForegroundColor = ConsoleColor.White;
               Show(f, args);
               System.Console.ResetColor();
           }
       }
       public static void ShowSuccess(string f, params object[] args)
       {if (IsShowSuccess)
           {
           System.Console.ForegroundColor = ConsoleColor.Green;
           Show(f, args);
           System.Console.ResetColor();}
       }
       public static void ShowError(string f, params object[] args)
       {
           if (IsShowError)
           {
           System.Console.ForegroundColor = ConsoleColor.Red;
           Show(f, args);
           System.Console.ResetColor();
       }
       }
       public static void ShowWarn(string f, params object[] args)
       {
           if (IsShowWarn)
           {
               System.Console.ForegroundColor = ConsoleColor.Yellow;
               Show(f, args);
               System.Console.ResetColor();
           }
       }
       public static void ShowException(Exception ex)
       {
           if (ex != null)
           {
               ShowError("Exception:{0}" ,ex.Message);
               ShowError("StackTrace:{0}", ex.StackTrace);
               ShowException(ex.InnerException);
           }
       }
       /// <summary>
       ///colors are 0=black 1=blue 2=green 4=red and so on to 15=white
/// colorattribute = foreground + background * 16
/// to get red text on yellow use 4 + 14*16 = 228
/// light red on yellow would be 12 + 14*16 = 236
       /// </summary>
       /// <param name="hConsoleOutput"></param>
       /// <param name="wAttributes"></param>
       /// <returns></returns>
[System.Runtime.InteropServices. DllImport("kernel32.dll")]
public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput,
int wAttributes);
[DllImport("kernel32.dll")]
public static extern IntPtr GetStdHandle(uint nStdHandle);
public static void Init()
{
    //AppDomainSetup domaininfo = new AppDomainSetup();
    //domaininfo.PrivateBinPath = "bin;bin\bin;..\bin";
    //System.Security.Policy.Evidence adevidence = AppDomain.CurrentDomain.Evidence;
    //AppDomain domain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);

    System.Console.BackgroundColor = ConsoleColor.Black;// ConsoleColor.Blue;
    System.Console.ForegroundColor = ConsoleColor.Gray;// ConsoleColor.Green;
    System.Console.WindowWidth = 45;// System.Console.LargestWindowWidth - 20;
    System.Console.WindowHeight = 6;// System.Console.LargestWindowHeight / 2;
    System.Console.BufferWidth = System.Console.LargestWindowWidth*2;
    System.Console.BufferHeight = System.Console.LargestWindowHeight*2;
    System.Console.Clear();
    byte[] inputBuffer = new byte[8000];
    System.IO.Stream inputStream = System.Console.OpenStandardInput(8000);//bytes.Length);
    System.Console.SetIn(new System.IO.StreamReader(inputStream));
    // if (System.Diagnostics.Debugger.IsAttached)
    //throw new Exception();
    //uint STD_OUTPUT_HANDLE = 0xfffffff5;
    //IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

    //for (int k = 1; k < 255; k++)
    //{
       // SetConsoleTextAttribute(hConsole, k);
        //System.Console.WriteLine("{0:d3} I want to be nice today!", k);

        //SetConsoleTextAttribute(hConsole, 236);
    //}
       // System.Console.WriteLine("Press Enter to exit ...");
}
       //public static void Run()
       //{
          
       //    System.Threading.ThreadStart ts2 = new System.Threading.ThreadStart(Run2);
       //    System.Threading.ThreadStart ts3 = new System.Threading.ThreadStart(Run3);
       //    System.Threading.ThreadStart ts4 = new System.Threading.ThreadStart(Run4);
       //    System.Threading.ThreadStart ts5 = new System.Threading.ThreadStart(Run5);
       //    //System.Threading.Thread t2 = new System.Threading.Thread(ts2); t2.IsBackground = true; t2.Start();
       //   //System.Threading.Thread t3 = new System.Threading.Thread(ts3); t3.IsBackground = true; t3.Start();
       //   // System.Threading.Thread t4 = new System.Threading.Thread(ts4); t4.IsBackground = true; t4.Start();
       //    System.Threading.Thread t5 = new System.Threading.Thread(ts5); t5.IsBackground = true; t5.Start();
       //    ConsoleKeyInfo key;
       //    do
       //    {
       //        System.Console.WriteLine("按ESC->退出");//
       //        key = System.Console.ReadKey();
       //    } while (key.Key != ConsoleKey.Escape);
       //    return;//
       //     string m=""; MethodInfo mInfo=null;
       //     while (mInfo == null)
       //     {
       //         System.Console.WriteLine("输入名称:"); m = System.Console.ReadLine();

       //         mInfo = typeof(Zfrong.Project.Logic).GetMethod(m);
       //         if (mInfo == null)
       //             System.Console.WriteLine("输入->错误.....请重新输入");//
       //     }
       //     mInfo.Invoke(null, null);//34593458
       //     System.Console.WriteLine("按任意键->退出");//
       //     System.Console.ReadKey();//
       // }
       //public static void Run2()
       //{

       //    Zfrong.Project.Tianya t = new Zfrong.Project.Tianya();
       //    t.name = "zfrong3000";// "韩模黄美姬"
       //    t.pwd = "qwertyppaspp";
       //    t.Init();
       //    t.Login();
       //    //t.AddF(34072268, 34394866);
       //}
       //public static void Run3()
       //{

       //    Zfrong.Project.Tianya t = new Zfrong.Project.Tianya();
       //    t.name = "妞在上海";
       //    t.pwd = "qwertyppaspp";
       //    t.Init();
       //    t.Login();
       //    t.AddF(26000000, 26234608);
       //}
       //public static void Run4()
       //{

       //    Zfrong.Project.Tianya t = new Zfrong.Project.Tianya();
       //    t.name = "021sayok";// "抱抱呵呵";"021sayok";// 
       //    t.pwd = "qwertyas";
       //    t.Init();
       //    t.Login();
       //    //t.leaveWordForRecommend();//
       //    System.Threading.ParameterizedThreadStart ts1 = new System.Threading.ParameterizedThreadStart(t.AddF);
       //    System.Threading.Thread t1 = new System.Threading.Thread(ts1); t1.IsBackground = true; t1.Start(new int[] { 01307463, 10000000 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t2 = new System.Threading.Thread(ts1); t2.IsBackground = true; t2.Start(new int[] { 10047124, 15000000 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t3 = new System.Threading.Thread(ts1); t3.IsBackground = true; t3.Start(new int[] { 15072231, 20000000 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t4 = new System.Threading.Thread(ts1); t4.IsBackground = true; t4.Start(new int[] { 20079279, 25000000 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t5 = new System.Threading.Thread(ts1); t5.IsBackground = true; t5.Start(new int[] { 25074881, 30000000 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t6 = new System.Threading.Thread(ts1); t6.IsBackground = true; t6.Start(new int[] { 30075318, 35000000 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t7 = new System.Threading.Thread(ts1); t7.IsBackground = true; t7.Start(new int[] { 35075245, 37221238 }); System.Threading.Thread.Sleep(5000);//
       //    System.Threading.Thread t8 = new System.Threading.Thread(ts1); t8.IsBackground = true;// t8.Start(new int[] { 28051736, 29000000 });
       //    System.Threading.Thread t9 = new System.Threading.Thread(ts1); t9.IsBackground = true; //t9.Start(new int[] { 27051657, 28000000 });
       //    System.Threading.Thread t0 = new System.Threading.Thread(ts1); t0.IsBackground = true;// t0.Start(new int[] { 26051871, 27000000 });

       //}
       // //var resultList = {"0":"您的请求申请已经发送,请等待对方通过请求！","1":"您还没登录","2":"不能添加自己为好友",
       // //"3":"用户不存在","4":"您还没激活 ","5":"对方没激活","6":"您已经在对方的黑名单中",
       // //"7":"您已经有这个好友","8":"您已经提交过请求了","9":"申请添加好友出错"};


       //public static void Run5()
       //{

       //    Zfrong.Project.Tianya t = new Zfrong.Project.Tianya();
       //    t.name = "经济大砖家";// "我在哈哈镜";// "联系abc2010";// "搜索丰富a";// "啊啊哦哦呵呵";// "换一张换一张";// "不允许修改1";// "听听由于u";// "刚刚好和w";// "物是人非q";// "千千万万哦哦";// "出差vv白白";// "老子孙子傻子";// "监管2012";// "qweyyu";// "爷爷我爱";// "噢噢拉拉";// "你奶奶上网";//"娃娃刚刚是";// "无处不在的2010";// "流水年华3000";// "日日你马";//水水水哈哈";// "92497522";// "波罗地海海盗";// "阿斯顿飞呵呵";// "yyooxx2000";// "爱你的全部YY";// "果果牛牛刀";// "哈哈哇哈哈";// "伟大的母亲2000"; //"已知该用户名存在";// "美女多多2010";// "长青树YY"; //"激情亚运2010";// "妹妹好好PP";// "没有邮箱啊";// "格子间里2010";// "天天听后感";// "妞妞熟女";// "爱你上海滩2000";
       //    t.pwd = "qwerty";
       //    t.Init();
       //    t.Login();
       //    t.PostALL(1000*300);
           
       //}
    }
}
