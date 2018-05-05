using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
namespace DataBuilder
{
    partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        //public static void Main(string[] args)
        //{
        //    Run(new string[] {});//
        //}
         static void Run(string[] args)
        {
            Service1 x = new Service1();
            int l = 0;
            if (args != null)
            {
                l = args.Length;
            }

            if (l > 0) //有参数时以 console 方式运行
            {
                Console.WriteLine("Run as Console");
                x.OnStart(null);
                Console.ReadLine();
            }
            else
            //intallutil 成服务后
            //即: 无参数时,以 Service 方式运行
            {
                Console.WriteLine("Run as Service");
                ServiceBase.Run(x);
            }
            ;//
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
            DataBuilder.Init();

            DataBuilder.Start();//
            //DataBuilder.DoWhile();
        }

        protected override void OnStop()
        {
            DataBuilder.SaveTask();//
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
        protected override void OnShutdown()
        {
            DataBuilder.SaveTask();//
            base.OnShutdown();
        }

    }
}
