using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using DB.DataDB;
namespace Builder.Page
{
   public class TagBox
    {
       public static void Build()
       {
           FileOperate.WriteFile("Template/Tag_New.shtml", New());//
           FileOperate.WriteFile("Template/Tag_Hot.shtml", Hot());//
       }
       static string New()
       {
           Query q = Dict.Query();
           q.Top = "20";
           q.OrderBy = OrderBy.Desc(Dict.Columns.Id);//
           DictCollection dicts = new DictCollection();//
           dicts.LoadAndCloseReader(q.ExecuteReader());//

           StringBuilder sb = Make(dicts);
           sb.Insert(0,"<div class=\"t3 bcg bgg\">最新标签</div><div class=\"b3 bcg mb6\">");//
           sb.Append("</div>");//
           string str = sb.ToString();//
           sb.Remove(0, sb.Length);//
           sb = null;//
           return str;//
       }
       static string  Hot()
       {
           Query q = Dict.Query();
           q.Top = "20";
           q.OrderBy = OrderBy.Desc(Dict.Columns.Num);//
           DictCollection dicts = new DictCollection();//
           dicts.LoadAndCloseReader(q.ExecuteReader());//

           StringBuilder sb = Make(dicts);
           sb.Insert(0, "<div class=\"t3 bcg bgg\">热门标签</div><div class=\"b3 bcg mb6\">");//
           sb.Append("</div>");//
           string str = sb.ToString();//
           sb.Remove(0, sb.Length);//
           sb = null;//
           return str;//
       }
       static StringBuilder Make(DictCollection dicts)
       {
           StringBuilder sb = new StringBuilder();//
           
           sb.Append("<ul class='tag'>");//
           foreach (Dict dict in dicts)
           {
               sb.Append("<li><a href=\"/TagItemList/" + Program.make(dict.Id) +"/"+Program.make(1)+ ".shtml\">");//
               sb.Append("" + dict.Word + "</a></li>");//
           }
           sb.Append("<li><a href=\"/TagList/" + Program.make(1) + ".shtml\">更多...</a></li></ul>");//

           return sb;//
       }
    }
}
