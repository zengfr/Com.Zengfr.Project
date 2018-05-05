using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    public class GoogleSearcher_News : Searcher
    {
        public GoogleSearcher_News()
            : base()
        {
            this.QueryBaseUrl = @"http://news.google.cn/news?&ie=UTF-8&scoring=d";
            RowTemplate t = new RowTemplate();//
            t.Patterns.Add(new Pattern(@"<div class=lh><font style=""font-size:100%;line-height:130%"">(.*?)<br><font size=-1><font color=#6f6f6f>"));
            t.CellTemplates.Add(new CellTemplate("连接", new Pattern(PatternType.A_Href)));

            XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
            //list.Add(new Replace("<font color=\"#cc0033\">", ""));
            //list.Add(new Replace("</font>", ""));//
            t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern(PatternType.A_InnerText, list)));



            this.AreaTemplates.Add(new AreaTemplate(t));//
        }
        protected override string GetTotalCountRegex()
        {
            return "[约]{0,2}有<b>([0-9,]+)</b>项符合";
        }
        public static XmlSerializableList<Area> Search(string key, int pn, ref int totalCount)
        {
            Searcher s = new GoogleSearcher_News();//
            s.QueryParams["q"] = Searcher.Encode(key, "utf-8");//
            s.QueryParams["start"] = pn.ToString();//
            s.TotalCount = totalCount;//
            s.GetValues();//
            totalCount = s.TotalCount;//

            return s.AreaResults;//
        }
    }
}