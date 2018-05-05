using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CommonPack;
using HLSSplit;
using System.Threading;
//using DB.DataDB;
//using DB.DataWeb;
//using SubSonic;
using System.Data;
using HtmlAgilityPack;
using CommonPack.HtmlAgilityPack;
using ParserEngine;
namespace WordBuilder
{
//    class Program
//    {
//        //public static void VeryCD(string url)
//        //{
//        //    Console.WriteLine("处理:"+url);//
//        //    Searcher s;
            
//        //    VeryCDItem obj = new VeryCDItem();
//        //    obj.LoadByParam(VeryCDItem.Columns.Url, url);
//        //    if (obj.Id==0||obj.Class1==null)
//        //    {
//        //        s = new VeryCDSearcher();//
//        //        s.GetItems(url);//
//        //        obj.Url = url;
//        //        foreach (Item item in s.Items)
//        //        {
//        //            //Console.WriteLine("----------------------------");//
//        //            foreach (Cell cell in item.Cells)
//        //            {
//        //                switch (cell.Name)
//        //                {
//        //                    case "分类": obj.Class1 = cell.Value; break;
//        //                    case "分类2":obj.Class2 = cell.Value; break;
//        //                    case "描述": obj.Content = cell.Value; break;
//        //                    case "标题": obj.Title = cell.Value; break;
//        //                    case "连接": obj.Url = cell.Value; break;
//        //                }
//        //                //Console.WriteLine(cell.Name + ":" + cell.Value);//
//        //            }
//        //        }
//        //        obj.Save();
//        //    }
            
//        //    VeryCDED2K obj2;
//        //    s = new VeryCDSearcher2();//
//        //    s.GetItems(url);//
//        //    foreach (Item item in s.Items)
//        //    {
//        //        obj2 = new VeryCDED2K();
//        //        obj2.ItemID = obj.Id;
//        //        foreach (Cell cell in item.Cells)
//        //        {
//        //            switch (cell.Name)
//        //            {
//        //                case "连接": obj2.Link = cell.Value; break;
//        //                //case "分类2": obj2.Title = cell.Value; break;
//        //                case "连接Text": obj2.Name = cell.Value; break;
//        //            }
//        //            //Console.WriteLine(cell.Name + ":" + cell.Value);//
//        //        }
//        //        Query q = VeryCDED2K.Query().AddWhere(VeryCDED2K.Columns.ItemID, obj2.ItemID).AND(VeryCDED2K.Columns.Name, obj2.Name);//
//        //        IDataReader read = VeryCDED2K.FetchByQuery(q);
//        //        bool b = read.Read();//
//        //        if (!b)
//        //        { obj2.Save(); }
//        //        //Console.WriteLine("----------------------------");//
//        //    }

//        //}
//        static void Main2(string[] args)
//        {
//            //VeryCDItemCollection coll = new VeryCDItemCollection();
//            //Query q =VeryCDItem.Query();
//            //q.AddWhere("class1",Comparison.Is,null);
//            //IDataReader read = q.ExecuteReader();//
//            //coll.LoadAndCloseReader(read);//
//            //foreach (VeryCDItem item in coll)
//            //{
//            //    VeryCD(item.Url);//
//            //}
//           // text();//
//           // CommWordBuilder();//
//           // Res();
           
//           //form();
//            //hexun();//
//            //Do();//
//            //word();//
//            //Test();//
//            Console.ReadKey();//
//        }
//        //static void text()
//        //{
//        //    string url = "http://www.ggtang.com/ggtang_article/d8/1299.html";//
//        //    url = "http://haoting.com/musiclist/ht_e3d3551d2cb440a2.htm";//
//        //    url = "http://lib.verycd.com/2007/09/14/0000163410.html";//
//        //    //url = "http://news.163.com/07/0914/19/3OCH6JO00001124J.html";//
//        //    url="http://video.baidu.com/v?word=%B2%CC%D2%C0%C1%D6";//
//        //    url = "http://video.baidu.com/v?ct=301989888&word=%B2%CC%D2%C0%C1%D6";
//        //    HtmlWeb hw = new HtmlWeb();
//        //    HtmlDocument doc = hw.Load(url);
//        //    HtmlToText htt = new HtmlToText();
//        //    string s = htt.ConvertHtml(doc.Text);
//        //    Console.Write(s);//
//        //}
//        //static void Res()
//        //{
//        //    ThreadWorker.ThreadWorker<Email> w = new ResBuilder<Email>();//
//        //    w.UrlPattern = "114.com.cn";//
//        //    w.Start(new Uri("http://www.114.com.cn"), 20);
//        //    w.Done.WaitBegin();
//        //    w.Done.WaitDone();//
//        //}
//        //static void Do()
//        //{
//        //    ThreadStart starter;
//        //    //starter = new ThreadStart(Baidu);
//        //    //starter = new ThreadStart(GoogleCn);
//        //    //starter = new ThreadStart(GoogleEn);
//        //    //starter = new ThreadStart(ZhiDao);
//        //    //starter = new ThreadStart(Post);
//        //    starter = new ThreadStart(Res);
//        //    Thread spider = new Thread(starter);
//        //    spider.Start();
//        //}
//static void Test()
//{
//  //  SubSonic.Query q = Link.Query();
//  //  q.Top = "1000"; q.QueryType = QueryType.Select;//
//  // //q.OrderBy = OrderBy.PassedValue("newid()");//
//  //IDataReader dr=Link.FetchByQuery(q);
//  //LinkCollection links = new LinkCollection();
//  //links.LoadAndCloseReader(dr);//
//  // string[] doc = new string[1000];
//  // for (int i = 0; i < links.Count;i++ )
//  // {
//  //     doc[i] = links[i].Url;
//  // }
//   ServiceRanking.StopWordsHandler stopword = new ServiceRanking.StopWordsHandler();
//   ServiceRanking.URLTFIDFMeasure tf = new ServiceRanking.URLTFIDFMeasure(doc);
//   tf.DocIndex = 0;//
//   tf.Init();//

//   //tf.GetSimilarityDoc(9);//
//   //tf.GetSimilarityDoc(220);//

//  //  q = Link.Query();
//  //  q.Top = "2000"; q.QueryType = QueryType.Select;//
//  //  //q.OrderBy = OrderBy.PassedValue("id desc");//
//  //  dr=Link.FetchByQuery(q);
//  //links = new LinkCollection();
//  //links.LoadAndCloseReader(dr);//
//  //string[] doc2 = new string[2000];
//  //for (int i = 0; i < links.Count; i++)
//  //{
//  //    doc2[i] = links[i].Url;
//  //}
//  //tf.GetIsSimilarity(220,doc2);//

//}
////        //static void CnWordBuilder()
//        //{
//        //    Config.StartRead();
//        //    ThreadWorker.ThreadWorker<CnHot> w = new CnHotBuilder();
//        //    w.UrlPattern = Config.UrlPattern;//
//        //    w.Start(new Uri(Config.StartUrl), Config.Threads);
//        //    w.Done.WaitBegin();
//        //    w.Done.WaitDone();//
//        //}
//        //static void CommWordBuilder()
//        //{
//        //    Config.StartRead();
//        //    CommBuilder<VeryCDQueue> w = new CommBuilder<VeryCDQueue>();
//        //    w.BuilderType = enumBuilderType.VeryCD;
//        //    w.UrlPattern = Config.UrlPattern;//
//        //   // Uri u;
//        //    ////for(int i=324;i>0;i--)
//        //    ////{
//        //    ////    u = new Uri("http://lib.verycd.com/archive/" + string.Format("{0:D5}",i)+".html");//
//        //    ////     w.addURI(null,u);//
//        //    ////}
//        //    w.Start(new Uri(Config.StartUrl), Config.Threads);
//        //    w.Done.WaitBegin();
//        //    w.Done.WaitDone();//
//        //}
//        //static void EnWordBuilder()
//        //{
//        //    ThreadWorker.ThreadWorker<EnHot> w = new EnHotBuilder();
//        //    w.UrlPattern = "google.com/trends/hottrends?sa=X&date=";//
//        //    int s = 180;
//        //    int e = 0;
//        //    for (int i = e; i <s; i++)
//        //    {
//        //        w.addURI(null,new Uri("http://www.google.com/trends/hottrends?sa=X&date="+DateTime.Today.AddDays(-i).ToString("yyyy-M-dd")));//
//        //    }
//        //    w.Start(new Uri("http://www.google.com/trends?hl=en"), 1);
//        //    w.Done.WaitBegin();
//        //    w.Done.WaitDone();//
//        //}
//        //static void word()
//        //{
//        //    HLParse p = new HLParse();
//        //    //p.ParseAll("海量中文智能分词基础件具有灵活定制的特点支持多平台支持多码制");

//        //    foreach (KeyValuePair<string, float> e in p.KeyWords)
//        //        Console.WriteLine(e.Key + ":" + e.Value);//

//        //    foreach (KeyValuePair<string, HLSSplit.POS> e in p.Words)
//        //        Console.WriteLine(e.Key + ":" + e.Value);//

//        //    foreach (KeyValuePair<string, byte> e in p.Finger)
//        //        Console.WriteLine(e.Key + ":" + e.Value);//

//        //}
//        static void form()
//        {
            
//            string s = "小说游戏mp3手机汽车qq笑话壁纸迅雷周笔畅基金电影nba跑跑卡丁车快乐男声刘亦菲pplivedj视频非主流劲舞团李宇春电驴美女星苹果乐园梦幻西游图片";//
//            s += "[URL=http://www.94sh.com]http://www.94sh.com[/URL]";//
//            s += "<a href='http://www.94sh.com'>周笔畅李宇春魏晨井柏然付辛博乔任孙燕姿阿穆张杰苏醒蔡依林俞灏明</a>";//
//            s = "123456";
//            FormPost p = new FormPost();
//            p.EncodingType = EncodingType.UTF8;
//            p.Referer = "http://zhidao.baidu.com/question/12038274.html";

//            p.GetParams(2);//
//            p.AutoSetParamValue("co", s);//
//            p.SetParam("co", s);//
//            p.SetParam("lu", "http://zhidao.baidu.com/question/12038274.html");//
//            for (int i = 3100; i < 4000; i++)
//            {
//                p.PostDate();//
//            }
//            p = null;//
//        }
//        static void hexun()
//        {
//            string s = "小说游戏mp3手机汽车qq笑话壁纸迅雷周笔畅基金电影nba跑跑卡丁车快乐男声刘亦菲pplivedj视频非主流劲舞团李宇春电驴美女星苹果乐园梦幻西游图片";//
//            s += "[URL=http://www.94sh.com]http://www.94sh.com[/URL]";//
//            s += "<a href='http://www.94sh.com'>周笔畅李宇春魏晨井柏然付辛博乔任孙燕姿阿穆张杰苏醒蔡依林俞灏明</a>";//
//            FormPost p = new FormPost();
//            p.EncodingType = EncodingType.UTF8;
//            p.Referer = "http://group.hexun.com/WXY/Discussion.aspx?articleid=2080214&index=1";

//            p.GetParams(0);//
//            p.AutoSetParamValue("94sh",s);//
//           // p.SetParam("content", s);
//           //p.SetParam("color", "Black");
//           // p.SetParam("SelectFontSize", "12");
//            for (int i = 3100; i < 4000; i++)
//            {
//               p.PostDate();//
//            }
//            p = null;//
//        }
//    }
}
