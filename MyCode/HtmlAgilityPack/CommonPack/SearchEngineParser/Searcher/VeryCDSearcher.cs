using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    public class VeryCDSearcher : Searcher
    {
        public VeryCDSearcher()
            : base()
        {
            this.QueryBaseUrl = @"http://lib.verycd.com/2006/10/19/0000124580.html?";
            //this.KeyParamName = @"&wd=";
            //this.PageParamName = @"&pn=";

            //this.KeyParamValue = @"baidu";
            //this.PageParamValue = @"0";

            RowTemplate t = new RowTemplate();//

            t.Patterns.Add(new Pattern("<div id=\"position\">您的位置：<a href=\"http://www.verycd.com/\">首页</a>(.*?)<div id=\"pingsList\">"));

            XmlSerializableList<Pattern> list;

            list = new XmlSerializableList<Pattern>();
           // list.Add(new Pattern("</a> → (.*?)</a> →", PatternType.A_InnerText));//
            t.CellTemplates.Add(new CellTemplate("分类", list));

            list = new XmlSerializableList<Pattern>();
            //list.Add(new Pattern("<a href=(.*?)</a>", PatternType.A_InnerText, 2));//
            t.CellTemplates.Add(new CellTemplate("分类2", list));

            list = new XmlSerializableList<Pattern>();
            list.Add(new Pattern("<h1>(.*?)</h1>", PatternType.Text));//
            t.CellTemplates.Add(new CellTemplate("标题", list));

            //list = new List<Pattern>();
            //list.Add(new Pattern("<!--eMule-->(.*?)<!--End eMule-->"));//
            //this.RowTemplate.CellTemplates.Add(new CellTemplate("连接", PatternType.Input_Value, list));

            list = new XmlSerializableList<Pattern>();
            list.Add(new Pattern("<!--End eMule-->(.*?)<div id=\"pingsList\">", PatternType.Text));//
            t.CellTemplates.Add(new CellTemplate("描述", list));

            this.AreaTemplates.Add(new AreaTemplate(t));//
            // list = new List<Pattern>();
            //list.Add(new Pattern("<!--End eMule-->(.*?)<div id=\"pingsList\">"));//
            //this.RowTemplate.CellTemplates.Add(new CellTemplate("图片", PatternType.Img_Src, list));

            t = new RowTemplate();//
            t.Patterns.Add(new Pattern("<input type=\"checkbox\" class=(.*?)</td></tr>"));

            t.CellTemplates.Add(new CellTemplate("连接", new Pattern("<a>(.*?)</a>", PatternType.A_Href)));
            t.CellTemplates.Add(new CellTemplate("连接Text", new Pattern("<a>(.*?)</a>", PatternType.A_InnerText)));

            this.AreaTemplates.Add(new AreaTemplate(t));//
        }

    }
}
