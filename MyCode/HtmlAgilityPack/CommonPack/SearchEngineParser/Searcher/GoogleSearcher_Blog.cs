using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    public class GoogleSearcher_Blog : Searcher
    {
        public GoogleSearcher_Blog()
            : base()
        {
            this.QueryBaseUrl = @"http://blogsearch.google.cn/blogsearch?&ie=UTF-8&scoring=d";
            RowTemplate t = new RowTemplate();//
            t.Patterns.Add(new Pattern(@"</td></tr></table><p class=g></p>(.*?)<table border=0 cellpadding=0 cellspacing=0><tr><td class=j>",0));
            t.CellTemplates.Add(new CellTemplate("连接", new Pattern(PatternType.A_Href)));

            XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
            list.Add(new Replace("<font color=\"#cc0033\">", ""));
            list.Add(new Replace("</font>", ""));//
            t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern(PatternType.A_InnerText, list)));



            this.AreaTemplates.Add(new AreaTemplate(t));//
        }
        protected override string GetTotalCountRegex()
        {
            return "[约]{0,2}有<b>([0-9,]+)</b>项符合";
        }
        public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
        {
            Searcher s = new GoogleSearcher_Blog();//
            s.QueryParams["q"] = Searcher.Encode(key, "utf-8");//
            s.QueryParams["start"] = pn.ToString();//
            s.TotalCount = totalCount;//
            s.GetValues();//
            totalCount = s.TotalCount;//

            return s.AreaResults;//
        }
    }
}
