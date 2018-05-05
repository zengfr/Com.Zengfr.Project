using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
   public class BaiduSearcher_Blog:Searcher
    {
       public BaiduSearcher_Blog()
               : base()
           {
               this.QueryBaseUrl = @"http://blogsearch.baidu.com/s?tn=baidublog";
               this.QueryParams["wd"] = "baidu";//
               this.QueryParams["pn"] = "0";//
               //this.KeyParamName = @"&wd=";
               //this.PageParamName = @"&pn=";

               //this.KeyParamValue = @"baidu";
               //this.PageParamValue = @"0";

               RowTemplate t = new RowTemplate();//
               t.Patterns.Add(new Pattern(@"<table border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class=f>(.*?)<br></font></td></tr></table><br>"));
               t.CellTemplates.Add(new CellTemplate("连接", new Pattern(PatternType.A_Href)));

               XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
                list.Add(new Replace("<font size=\"3\">",""));//
               list.Add(new Replace("<font color=\"#C60A00\">", ""));
               list.Add(new Replace("</font>", ""));//
               t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern(PatternType.A_InnerText, list)));

               

               this.AreaTemplates.Add(new AreaTemplate(t));//
           }
       protected override string GetTotalCountRegex()
       {
           return "找到相关文章[约]{0,2}([0-9,]+)篇";
       }
       public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
       {
           Searcher s = new BaiduSearcher_Blog();//
           s.QueryParams["bsm"] = "1";//
           s.QueryParams["wd"] = Searcher.Encode(key, "gb2312");//
           s.QueryParams["pn"] = pn.ToString();//
           s.QueryParams["zfrong"] = Guid.NewGuid().ToString();//
           s.TotalCount = totalCount;//
           s.GetValues();//
           totalCount = s.TotalCount;//
           
           return s.AreaResults;//
       }
    }
}
