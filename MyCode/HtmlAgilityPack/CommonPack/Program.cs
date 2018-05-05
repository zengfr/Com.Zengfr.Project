using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Com.Zfrong.Algorithms.LCS;
using HtmlAgilityPack;
using CommonPack.HtmlAgilityPack;
using CommonPack.LCS;
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main2(string[] args)
        {
          // Com.Zfrong.Algorithms.TFIDF.Class1.Main2(args);//
           // ParserEngine.Class1.Main(args);//
           // HtmlToText.Test();//
           //GetT();
           //Get();//
           // GetHH();//
            Console.ReadKey();//
        }
        static void GetT()
        {
            TemplateBuilder tb = new TemplateBuilder();
            tb.TemplateFile="item.html";
            tb.Urls.Add("http://www.newhua.com/soft/34536.htm");
            tb.Urls.Add("http://www.newhua.com/soft/3616.htm");
            tb.Urls.Add("http://www.newhua.com/soft/55911.htm");
            tb.Urls.Add("http://www.newhua.com/soft/55447.htm");
            tb.Urls.Add("http://www.newhua.com/soft/5636.htm");
            tb.Start();//

        }
        static void GetHH()
        {
            ItemBuilder tb = new ItemBuilder();
            tb.SetText("http://www.newhua.com/soft/53496.htm");
            tb.SetTemplate("item.html");

            IDictionary<int, string> diff = tb.GetAllItemValue();//
            foreach (KeyValuePair<int,string> v in diff)
            {
                Console.WriteLine("***************");
                Console.WriteLine(v.Value);//
            }
        }
        static void Get()
        {
            ItemBuilder tb = new ItemBuilder();
            tb.SetText("http://www.newhua.com/soft/34536.htm");//
            tb.SetTemplate("soft.html");

            //Console.WriteLine(tb.GetItemValue("class1", 3,3));//
            Console.WriteLine(tb.GetItemValue("item", 8, 8));//
            //Console.WriteLine(tb.GetItemStart("class", 3));//
          // Console.WriteLine(tb.GetItemEnd("class", 3));//
        }
    }