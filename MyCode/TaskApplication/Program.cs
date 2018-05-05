using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
//using Castle.ActiveRecord;
//using Castle.ActiveRecord.Framework;
//using Castle.ActiveRecord.Framework.Config;
//using TaskApplication.pro;
namespace TaskApplication
{
    class Program
    {
        static void Main(string[] args)
        {
             Console.WindowWidth = 140;
            Console.WindowHeight =40;
            Console.BufferWidth = Console.WindowWidth * 2;
          // TaskApplication.Task.SpiderTest.Test();
            SmartThreadPoolTest.Test();//
            Console.ReadLine();

             
            Console.ReadLine();
            return;
            //log4net.Config.XmlConfigurator.Configure();
            //ActiveRecordStarter.ResetInitializationFlag();
            //IConfigurationSource source2;
           // source2 = InPlaceConfigurationSource.Build(DatabaseType.MySql, "Server=127.0.0.1;database=weiboku;uid=root;pwd=123456;Max Pool Size = 512;");
            //ActiveRecordStarter.Initialize(source2, typeof(oooapp));
            ///ActiveRecordStarter.DropSchema();
            ///ActiveRecordStarter.CreateSchema();
            //TaskStart.Start();
            

        }
       //static void Test()
       // {
       //     for(int i=0;i<99999;i++)
       //     {
       //         QueueWorkItemFactory.Instance.QueueWorkItem(new workItem());
       //     }
       //     //ThreadPoolUtils.Instance.DefaultGroup.
       // }
       //public class workItem:IWorkItem
       //{
       //    public void DoWork()
       //    {
       //        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff"));
       //    }
       //}
    }
}
