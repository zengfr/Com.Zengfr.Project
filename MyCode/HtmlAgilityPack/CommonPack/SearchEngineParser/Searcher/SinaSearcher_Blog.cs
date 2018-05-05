using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    public class SinaSearcher_Blog : Searcher
    {
        public SinaSearcher_Blog()
               : base()
           {
               this.QueryBaseUrl = @"http://search.blog.sina.com.cn/blog/search?&s=1&t=keyword";
               this.QueryParams["q"] = "Sina";//
               this.QueryParams["p"] = "1";//
               
               RowTemplate t = new RowTemplate();//
               t.Patterns.Add(new Pattern(@"<div id=""itemTitle"">(.*?)<div id=""itemContent"">"));
               t.CellTemplates.Add(new CellTemplate("连接", new Pattern(PatternType.A_Href,0)));

               XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
               list.Add(new Replace("<font color=#cc0033>", ""));
               list.Add(new Replace("</font>", ""));//
               t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern(PatternType.A_InnerText,0, list)));

               

               this.AreaTemplates.Add(new AreaTemplate(t));//
           }
       protected override string GetTotalCountRegex()
       {
           return "找到相关博客([0-9,]+)条";
       }
       public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
       {
           pn = pn / 10 + 1;
           Searcher s = new SinaSearcher_Blog();//
            s.QueryParams["q"] = Searcher.Encode(key, "gb2312");//
           s.QueryParams["p"] = pn.ToString();//
           s.QueryParams["zfrong"] = Guid.NewGuid().ToString();//
           s.TotalCount = totalCount;//
           s.GetValues();//
           totalCount = s.TotalCount;//
           
           return s.AreaResults;//
       }
    }
}
