using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
namespace RSSBuilder
{
    partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        public static void Main(string[] args)
        {//sc create NewService binpath= c:\windows\system32\NewServ.exe type= share start= auto depend= "+TDI Netbios" 
//sc create RssAA binpath= "F:\dev project\新建文件夹\HtmlAgilityPack\bin\Debug\RSSBuilder.exe 123" start= auto
//[SC] CreateService FAILED 1072:

            //Program.Init();//
            //RssManager.xxx();//
            //return;
           Run(args);//
        }
        static Service1 x = null;//
        static int argsLen = 0;//
         static void Run(string[] args)
        {
            //if (RunningInstance.ExistRunningInstance())
            //{
                
            //    Console.ReadLine();
            //    return;//
            //}
            if (x == null)
            {
                x = new Service1();
            }
            //else
            //{
            //    Console.ReadLine();
            //    return;//
            //}
            if (args != null)
            {
                argsLen = args.Length;
            }

            //if (argsLen > 0) //有参数时以 console 方式运行
            //{
            //    Console.WriteLine("Run as Console");
               // x.OnStart(null);
               //Console.ReadLine();
            //}
            //else
            //intallutil 成服务后
            //即: 无参数时,以 Service 方式运行
            //{
                Console.WriteLine("Run as Service");
                ServiceBase.Run(x);
            //}
        }
        protected override void OnStart(string[] args)
        {
            ThreadStart ts = new ThreadStart(OnStart);//
            Thread t = new Thread(ts);//
            t.Start();//
            // TODO: 在此处添加代码以启动服务。
        }
        void OnStart()
        {
            Program.Init();//
            if (argsLen > 0)
            {
                Program.Start_Analytics();//
            }
            else
            {
                Program.Start_Rss();//
                //Program.Start_Analytics();//
            }
        }

        protected override void OnStop()
        {
            Program.Quit();//
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
        protected override void OnShutdown()
        {
            Program.Quit();//
            base.OnShutdown();
        }
        protected override void OnPause()
        {

            Program.Pause();//

        }

        protected override void OnContinue()
        {

            Program.Continue();//

        }
    }
}
