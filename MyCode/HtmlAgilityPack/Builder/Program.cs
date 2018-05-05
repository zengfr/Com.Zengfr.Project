using System;
using System.Collections.Generic;
using System.Text;
using Rss;//
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;//
namespace Builder
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
          //HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();//
           //string str =ThreadWorker.UrlReWritor.RemoveForbidChar(ThreadWorker.UrlReWritor.ConvertToFilePath("e:\\",ThreadWorker.UrlReWritor.ReWriteUrl("http://bbs.qqzhi.com/my.php?item=threads&srchfid=2")));//
           //Console.Write(str); //string str="http://military.club.china.com/data/thread/1011/2369/66/10/7_1.html";
            //str = "http://www.qqzhi.com";
          // Console.Write(CommonPack.HtmlAgilityPack.HtmlToSubject.Analytics(hw.Load(str)));//
           ////CommonPack.LCS.SampleBuilder.Test();//
          //return;
            //ServiceRanking.Class1.Main2(null);//
            //return;//

            Init();//
            //ParserEngine.BaiduSearcher_Blog blog = new ParserEngine.BaiduSearcher_Blog();//
            //blog.QueryParams["bsm"] = "1";//
            //blog.QueryParams["wd"] = "QQ";//

            //blog.QueryParams["pn"] = "0";//
            //blog.GetValues();//
            //blog.Print();//
            //for (int i = 10; i < blog.TotalCount; )
            //{
            //    blog.QueryParams["pn"] = i.ToString();//
            //    blog.GetValues();//
            //    blog.Print();//
            //    i += 10;
            //}
            //string url = "http://soft.ccw.com.cn/news/htm2008/20080423_413578.shtml";//
            ////url = "http://money.business.sohu.com/20080606/n257337711.shtml";//
            ////HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();//
            ////HtmlAgilityPack.HtmlDocument doc = hw.Load(url);//
            //System.IO.StreamReader sr = new System.IO.StreamReader(@"e:\1.html", System.Text.Encoding.UTF8);
            //url = CommonPack.HtmlAgilityPack.HtmlToSubject.Analytics2(sr);//
            //Console.Write(url);//
            //Console.ReadKey();//
            //return;//
            //System.Windows.Forms.Application.Run(new DockSample.MainForm());
            VACUUMDataBase();//
           Builder.Data.CMDHelper.Start(null);//
           // ParserEngine.Class1.Main(null);//
        }
        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            ActiveRecordStarter.ResetInitializationFlag();
            ActiveRecordStarter.Initialize(System.Reflection.Assembly.GetAssembly(typeof(DB.AdvData)), GetConfigSource());
            //ActiveRecordStarter.Initialize( GetConfigSource(),typeof(DB.ResBase),typeof(DB.QQ),typeof(DB.Mobile),typeof(DB.Email));
            //ActiveRecordStarter.DropSchema();//
            //ActiveRecordStarter.CreateSchema();//
        }
        static IConfigurationSource GetConfigSource()
        {
            return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
        }
        public static void VACUUMDataBase()
        {
            try
            {
                ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
                NHibernate.ISession session = sessionHolder.GetSessionFactory(typeof(DB.Base)).OpenSession();
                Finisar.SQLite.SQLiteConnection sQLiteConnection = session.Connection as Finisar.SQLite.SQLiteConnection;
                Finisar.SQLite.SQLiteCommand cmd = new Finisar.SQLite.SQLiteCommand("VACUUM", sQLiteConnection);
                cmd.ExecuteNonQuery();
               
            }
            catch { }
        }
    }
}
