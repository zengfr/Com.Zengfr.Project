using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine
{
   public class BaiduSearcher_D:Searcher
    {
        public BaiduSearcher_D()
               : base()
           {
               this.QueryBaseUrl = @"http://d.baidu.com/rs.php?tn=baidu";
               //this.KeyParamName = @"&q=";
               //this.PageParamName = @"&pn=";

               //this.KeyParamValue = @"baidu";
               //this.PageParamValue = @"0";

               RowTemplate t = new RowTemplate();//
               t.Patterns.Add(new Pattern(@"<ul><li class=ls>(.*?)></li></ul>"));
               t.CellTemplates.Add(new CellTemplate("Á¬½ÓText", new Pattern(PatternType.A_InnerText)));
               this.AreaTemplates.Add(new AreaTemplate(t));//
           }
    }
}
