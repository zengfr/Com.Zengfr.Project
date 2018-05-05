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
using Com.Zfrong.Common.Data.AR.Base;
using Com.Zfrong.Common.Data.AR.Base.Entity;
using NHibernate;
using NHibernate.Criterion;
namespace Test
{
    public partial class Form1 : Form, Test.IForm
    {
        protected static IConfigurationSource GetConfigSource()
        {
            return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
        }
        public Form1()
        {
            InitializeComponent();
            sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            Console.SetOut(sw);
            Test.MAIN.f = this;//
            log4net.Config.XmlConfigurator.Configure();
            ActiveRecordStarter.ResetInitializationFlag();

            ActiveRecordStarter.Initialize(GetConfigSource(),typeof(Base2), typeof(Test.KeyWord), typeof(Test.QQGroup), typeof(Test.ZipCode));
            //ActiveRecordStarter..1213CreateSchema();//
         }
       static StringBuilder sb;
        public void ShowLog(object str)
        {
            ShowLog(str.ToString());
        }
        public  void ShowLog(string str)
        {
            Console.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString("yyMMdd HH:mm:ss fff"), str));
            sb.Remove(0, sb.ToString().IndexOf("\r") + 2);
            this.textBox2.Text = sb.ToString();//
            this.textBox2.Select(this.textBox2.Text.Length, 0);
            this.textBox2.ScrollToCaret();//

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length != 0)
            {
                Test.KeyWord k = DB<Test.KeyWord>.FindFirst(Expression.Eq("Name", this.textBox1.Text.Trim()));//
                if (k==null||k.IDID == 0)
                {
                    k = new Test.KeyWord();//
                    k.Name = this.textBox1.Text.Trim();//
                    DB<Test.KeyWord>.CreateAndFlush(k);
                    refesh();//
                    this.textBox1.Text = "";//
                }     
            }
        }
        private void refesh()
        {
            Test.KeyWord[] ks = DB<Test.KeyWord>.FindAll(new Order[] { Order.Asc("PCount"), Order.Desc("IDID") });//
            this.listBox1.Items.Clear();//
            foreach (Test.KeyWord k in ks)
            {
                this.listBox1.Items.Add(k.Name+" "+k.PCount);//
            }
        }
        private void Run()
        {
           Test.KeyWord k = DB<Test.KeyWord>.FindFirst(new Order[] { Order.Asc("PCount"), Order.Desc("IDID") });
           if (k != null)
           {
               k.PCount += 1; DB<Test.KeyWord>.UpdateAndFlush(k);//
               refesh();
               Test.MAIN.TestQQGroup2(k.Name);//
           }
        }
        public void RunT()
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(Run));//
            t.IsBackground = true;//
            t.Start();//
        }
        

       
    }
}