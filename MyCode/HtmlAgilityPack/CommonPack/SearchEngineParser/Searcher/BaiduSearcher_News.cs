using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    public class BaiduSearcher_News : Searcher
    {
        public BaiduSearcher_News()
               : base()
           {
               this.QueryBaseUrl = @"http://news.baidu.com/ns?tn=news&from=news&ie=gb2312";
               //this.KeyParamName = @"&word=";
               //this.PageParamName = @"&pn=";

               //this.KeyParamValue = @"baidu";
               //this.PageParamValue = @"0";

               RowTemplate t = new RowTemplate();//
               t.Patterns.Add(new Pattern(@"<table cellspacing=0 cellpadding=2>(.*?)<a class=""snapshoot"""));
               t.CellTemplates.Add(new CellTemplate("连接", new Pattern(PatternType.A_Href)));

               XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
               list.Add(new Replace("<font color=\"#C60A00\">", ""));
            list.Add(new Replace("<font color=\"#CC0000\">", ""));
               list.Add(new Replace("</font>", ""));//
            
               t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern(PatternType.A_InnerText, list)));

               

               this.AreaTemplates.Add(new AreaTemplate(t));//
           }
        protected override string GetTotalCountRegex()
        {
            return "找到相关新闻[约]{0,2}([0-9,]+)篇";
        }
        public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
        {
            Searcher s = new BaiduSearcher_News();//
            //s.QueryParams["zfrong"] = Guid.NewGuid().ToString();//
            s.QueryParams["word"] = Searcher.Encode(key, "gb2312");//
            s.QueryParams["pn"] = pn.ToString();//

            s.QueryParams["bs"]=s.QueryParams["word"];
            s.QueryParams["sr"]="0";
            s.QueryParams["cl"]="2";
            s.QueryParams["rn"]="20";
            s.QueryParams["ct"] ="0";
            s.QueryParams["from"] = "news";//
            s.TotalCount = totalCount;//
            s.GetValues();//
            totalCount = s.TotalCount;//

            return s.AreaResults;//
        }
    }
}
