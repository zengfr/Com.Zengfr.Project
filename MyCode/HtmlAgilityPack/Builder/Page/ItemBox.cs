using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using DB.DataDB;
using CommonPack;
namespace Builder.Page
{
  public  class ItemBox
    {
        public static void Build()
        {
            FileOperate.WriteFile("Template/Item_New.shtml", New());//
            FileOperate.WriteFile("Template/Item_Hot.shtml", Hot());//
        }
        static string New()
        {
            Query q = NewsItem.Query();
            q.Top = "20";
            q.OrderBy = OrderBy.Desc(NewsItem.Columns.Id);//
            NewsItemCollection NewsItems = new NewsItemCollection();//
            NewsItems.LoadAndCloseReader(q.ExecuteReader());//

            StringBuilder sb = Make(NewsItems);
            sb.Insert(0, "<div class=\"t3 bcg bgg\">最新</div><div class=\"b3 bcg mb6\">");//
            sb.Append("</div>");//
            string str = sb.ToString();//
            sb.Remove(0, sb.Length);//
            sb = null;//
            return str;//
        }
        static string Hot()
        {
            Query q = NewsItem.Query();
            q.Top = "20";
            q.OrderBy = OrderBy.Desc(NewsItem.Columns.Num);//
            NewsItemCollection NewsItems = new NewsItemCollection();//
            NewsItems.LoadAndCloseReader(q.ExecuteReader());//

            StringBuilder sb = Make(NewsItems);
            sb.Insert(0, "<div class=\"t3 bcg bgg\">热门</div><div class=\"b3 bcg mb6\">");//
            sb.Append("</div>");//
            string str = sb.ToString();//
            sb.Remove(0, sb.Length);//
            sb = null;//
            return str;//
        }
      static StringBuilder Make(NewsItemCollection NewsItems)
        {
            StringBuilder sb = new StringBuilder();//

            sb.Append("<ul class='ItemList'>");//
            foreach (NewsItem NewsItem in NewsItems)
            {
                sb.Append("<li><a target=\"item\" href=\"/Item/" + Program.make(NewsItem.Id) + ".shtml\">");//
                if (NewsItem.Title.Length > 25)
                    sb.Append("" + NewsItem.Title.Substring(0, 25) + "...</a></li>");//
                else
                    sb.Append("" + NewsItem.Title + "</a></li>");//
            }
            sb.Append("<li><a target=\"item\" href=\"/ItemList/" + Program.make(1) + ".shtml\">更多...</a></li></ul>");//

            return sb;//
        }
    }
}
