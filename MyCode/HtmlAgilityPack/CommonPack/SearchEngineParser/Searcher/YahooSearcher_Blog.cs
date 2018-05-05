using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// &x=t_wz%3Ao_t&pid=hp&v=blog&
    /// </summary>
    public class YahooSearcher_Blog : Searcher
    {
        public YahooSearcher_Blog()
               : base()
           {
               this.QueryBaseUrl = @"http://one.cn.yahoo.com/s?&x=t_wz%3Ao_t&pid=hp&v=blog";
               this.QueryParams["p"] = "yahoo";//
               this.QueryParams["b"] = "0";//
               
               RowTemplate t = new RowTemplate();//
               t.Patterns.Add(new Pattern(@"<div class=""desc"">(.*?)</li><"));
               t.CellTemplates.Add(new CellTemplate("连接", new Pattern(PatternType.A_Href,0)));

               XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
               list.Add(new Replace("<b>", ""));
               list.Add(new Replace("</b>", ""));//
               t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern(PatternType.A_InnerText,0, list)));

               

               this.AreaTemplates.Add(new AreaTemplate(t));//
           }
       protected override string GetTotalCountRegex()
       {
           return "找到相关博客约([0-9,]+)条";
       }
       public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
       {
           //pn = pn / 10 + 1;
           Searcher s = new YahooSearcher_Blog();//
            s.QueryParams["p"] = Searcher.Encode(key, "gb2312");//
           s.QueryParams["b"] = pn.ToString();//
           //s.QueryParams["zfrong"] = Guid.NewGuid().ToString();//
           s.TotalCount = totalCount;//
           s.GetValues();//
           totalCount = s.TotalCount;//
           
           return s.AreaResults;//
       }
    }
}
