using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Soap;
using CommonPack;
namespace ParserEngine
{
    public class Class1
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Searcher s;

           // s = new BaiduSearcher_Blog();// D();// News();// new GoogleSearcher();
            //s.QueryParams["wd"]= Searcher.Encode("娱乐","gb2312");//

            s = new BaiduSearcher_WB();//
            s.QueryParams["word"] = Searcher.Encode("欧莱雅");//

            //s = new VeryCDSearcher();//
            //for (int i = 0; i < 10; i += 10)
            //{
            //    s.PageParamValue = i.ToString();//
            //    s.GetValues("http://lib.verycd.com/2007/09/27/0000164718.html");//
            //}
            //Console.WriteLine(s.DoTime.ToString());//
            s.GetValues();//
           // s.SaveConfig();//
           // s.LoadConfig();
            foreach (Table box in s.Tables)
            {
                foreach (Row Row in box.Rows)
                {
                    //Console.WriteLine(Row.Value);//
                    foreach (Cell cell in Row.Cells)
                    {
                        Console.WriteLine(cell.Name + "-----");//

                        Console.WriteLine(RegexHelper.StripHTML(cell.Value));//
                    }
                    Console.WriteLine("----------------------------");//
                }
            }
           // Main();//
            Console.ReadKey();//
        } 
    }
}
