using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using Com.Zfrong.Common.Data.AR.Base.Entity;
using Com.Zfrong.Common.Data.AR.Base;
using NHibernate;
using NHibernate.Criterion;
namespace Test
{
    public partial class Form2 : Test.IForm
    {
        protected static IConfigurationSource GetConfigSource()
        {
            return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
        }
        public Form2()
        {
           //sb = new StringBuilder();
           // StringWriter sw = new StringWriter(sb);
            //Console.SetOut(sw);
            Test.MAIN.f = this;//
            log4net.Config.XmlConfigurator.Configure();
            ActiveRecordStarter.ResetInitializationFlag();

            ActiveRecordStarter.Initialize(GetConfigSource(),typeof(Base2), typeof(Test.KeyWord), typeof(Test.QQGroup), typeof(Test.ZipCode));
            //ActiveRecordStarter..1213CreateSchema();//
               }
       //static StringBuilder sb;
        public void ShowLog(object str)
        {
            ShowLog(str.ToString());
        }
        public  void ShowLog(string str)
        {
            Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss fff"), str));
              
        }
        
       
        //private void Run()
        //{
        //  Test.ZipCode k;// = DB<Test.ZipCode>.FindFirst(new Order[] { Order.Asc("State"), Order.Desc("IDID") });
        //  while(!Com.Zfrong.Common.Http.Common.IsConnected) 
        //  {
        //      ShowLog("网络未连接!!!"); System.Threading.Thread.Sleep(10*1000);
        //  }
        //   SimpleQuery query = new SimpleQuery(typeof(Test.ZipCode), @"from ZipCode Order By State,ID DESC");
        //   query.SetQueryRange(1);//
        //   k = (ActiveRecordBase<Test.ZipCode>.ExecuteQuery(query) as Test.ZipCode[])[0];
        //    if (k != null)
        //   {
        //       k.State += 1;
        //       //DB<Test.ZipCode>.UpdateAndFlush(k);//
        //       Logic.ExecuteNonQuery<Test.ZipCode>("update zipcode set state=state+1 where id between " + (k.IDID - 2000) + " and " + (k.IDID + 2000) + " and city='" + k.City + "'");//
        //        string str = k.City;
        //       str = System.Text.RegularExpressions.Regex.Replace(str, "(自治|省|市|自治州|城|地区|区|县|旗|镇|乡|村|路|街|巷)", "");//
        //       Test.MAIN.TestQQGroup2(str);//
        //   }
        //}
        private void Run2()
        {
            if (MAIN.Sleep > 25) return;
            //int c=System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
           // if (c >100) return;//

            // Test.ZipCode k;// = DB<Test.ZipCode>.FindFirst(new Order[] { Order.Asc("State"), Order.Desc("IDID") });
            while (!Com.Zfrong.Common.Http.Common.IsConnected)
            {
                ShowLog("网络未连接!!!"); System.Threading.Thread.Sleep(10 * 1000);
            }
            GC.Collect();//
           // SimpleQuery query = new SimpleQuery(typeof(Test.ZipCode), @"from ZipCode Order By State,ID DESC");
           // query.SetQueryRange(1);//
            //k = (ActiveRecordBase<Test.ZipCode>.ExecuteQuery(query) as Test.ZipCode[])[0];
            Test.ZipCode k = Get();
            if (k != null)
            {
               // k.State += 1;
                //DB<Test.ZipCode>.UpdateAndFlush(k);//
               // Logic.ExecuteNonQuery<Test.ZipCode>("update zipcode set state=state+1 where id =" + k.IDID);//
                 string str = "";
                 string[] vv = k.Address.Split('、', '(', ')', '～'); k.Dispose(); k = null;
                foreach (string v in vv)
                {
                    str = System.Text.RegularExpressions.Regex.Replace(v, "(自治|省|市|自治州|城|地区|区|县|旗|镇|乡|村|路|街|巷|嘎查|胡同|单元)", "");//
                    if (str != null && str.Length != 0)
                    {
                        try
                        {
                            Test.MAIN.TestQQGroup2(str);//
                        }
                        catch { continue; }
                    }
                }
                vv = null;
            }
        }
        static object Lock=new object();
        static Test.ZipCode Get()
        {
            Test.ZipCode obj = null;
            lock (Lock)
            {
                obj = DB<Test.ZipCode>.FindFirst(new Order[] { Order.Asc("State"), Order.Desc("IDID") });
                System.Threading.Monitor.PulseAll(Lock);//
                if (obj!= null)
                {
                    Logic.ExecuteNonQuery<Test.ZipCode>("update zipcode set state=state+1 where id =" + obj.IDID);//
                }
            }
            return obj;
        }
        
            
        public void RunT()
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(Run2));//
            t.IsBackground = true;//
            t.Start();//
        }
       
       public void  Start()
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(qwerty));//
            t.IsBackground = true;//
            t.Start();//
            Console.ReadLine();
        }
        private void qwerty()
        {
            while (1 == 1)
            {
                 int c = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
                for (int i = c; i <60; i++)
                {
                    RunT();//
                    System.Threading.Thread.Sleep(1000);//
                }
                System.Threading.Thread.Sleep(15*60*1000);//
            }
            
        }

       
    }
}