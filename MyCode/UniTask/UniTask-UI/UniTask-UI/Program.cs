using DeviceManager;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using TaobaoTask;
using UniTask_UI;
using UniTask_UI.common;
using UniTask_UI.p1;

namespace DockSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Program2.Main2();
            //PhantomjsUtil.test(null);
            //test();    
             return;
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyMainForm());
        }

        public static void test()
        {
            var pool = MyThreadPool.getInstance("test");
            int thread = 5;
            pool.init(thread);
            pool.start();
            while (true)
            {
                var smart = pool.smart;
                if (smart.InUseThreads < thread || smart.CurrentWorkItemsCount < thread)
                {
                   // pool.addTask(,runTask, 1, 2, 3);
                }
                Thread.Sleep(100);
            }
        }

        private static void runTask(int arg1, int arg2, int arg3)
        {
            var mre = RestClientUtil.GetMre("mode-" + arg1);
            LoggerUtil.Info("wait ....{0}", arg1);
            mre.Wait();
            LoggerUtil.Info("enter....{0}", arg1);
            LoggerUtil.Info("doing....{0}", arg1);
            dotask(arg1);
            LoggerUtil.Info("doing....{0}", arg1);
            mre.Release();

        }
        static Random r = new Random();
        private static void dotask(int arg1)
        {
            Thread.Sleep(r.Next(3000, 6000));
        }
    }
}