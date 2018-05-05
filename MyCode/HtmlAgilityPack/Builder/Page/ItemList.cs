using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using DB.DataDB;
using System.Diagnostics;
using CommonPack;//
namespace Builder.Page
{
   public class ItemList
    {
        static int Total = 0;
        static int TotalPage = 0;
        static int PageSize = 60;//
        static StoredProcedure SP;
        public static void Build()
        {
            SP = SPs.ComPagerData("NewsItem", "ID", "Id", 1, PageSize, "*", "", null, Total);//
            SP.Execute();//
            Total = int.Parse(SP.OutputValues[0].ToString());
            TotalPage = ((int)Math.Ceiling((double)Total / PageSize));
            for (int i = 1; i < TotalPage; i++)
            {
                MakePage(i);//
            }
        }
        static void MakePage(int pageIndex)
        {
            Console.WriteLine("开始ItemList-Page:" + pageIndex);//
            int startID = 0;//
            StringBuilder sb = new StringBuilder();//
            sb.Append(FileOperate.ReadFile("template/List.shtml"));
            sb.Replace("{PageIndex}", Convert.ToString(pageIndex%JS.List));//
            sb.Replace("{ListItems}", itemList(pageIndex, out startID));//

            if (System.IO.File.Exists(Environment.CurrentDirectory + "ItemList/" + Program.make(startID) + ".shtml"))
                return;//
            Console.WriteLine("保存ItemList-Page:" + pageIndex+" ID:"+startID);//
            FileOperate.WriteFile("ItemList/" + Program.make(startID) + ".shtml", sb.ToString());//
            sb.Remove(0, sb.Length);//
        }
       static string itemList(int pageIndex,out int startID)
       {
           startID=0;//

           SP = SPs.ComPagerData("NewsItem", "ID", "Id", pageIndex, PageSize, "*", "", null, Total);//
           NewsItemCollection NewsItems = new NewsItemCollection();
           NewsItems.LoadAndCloseReader(SP.GetReader());//

           StringBuilder sb = new StringBuilder();//
           sb.Append("<ul id=\"List\">");//
           for (int i = 0; i < NewsItems.Count; i++)
           {
               if(i==0) startID=NewsItems[i].Id;

               sb.Append("<li><a target=\"_blank\" href=\"/Item/" + Program.make(NewsItems[i].Id) + ".shtml\">");//
               if (NewsItems[i].Title.Length > 80)
                   sb.Append("" + NewsItems[i].Title.Substring(0, 80) + "...</a></li>");//
               else
                   sb.Append("" + NewsItems[i].Title + "</a></li>");//
               //NewsItems[i].State = 1;
               //NewsItems[i].Save();//
           }

           sb.Append("</ul><div class='Pageer' id='Pageer'>");//
           sb.Append("[<a href=\"/ItemList/" + Program.make(startID + PageSize) + ".shtml\">前一页</a>] ");//
           sb.Append("[<a href=\"/ItemList/" + Program.make(startID + PageSize * 100) + ".shtml\">前100页</a>] ");//
           sb.Append("[<a href=\"/ItemList/" + Program.make(startID + PageSize * 50) + ".shtml\">前50页</a>] ");//
           sb.Append("[<a href=\"/ItemList/" + Program.make(startID + PageSize * 10) + ".shtml\">前10页</a>] ");//
           sb.Append("[" + startID + "]");//
            if (startID - PageSize * 10 > 0)
                sb.Append(" [<a href=\"/ItemList/" + Program.make(startID - PageSize * 10) + ".shtml\">后10页</a>]");//
            if (startID - PageSize * 50 > 0)
                sb.Append(" [<a href=\"/ItemList/" + Program.make(startID - PageSize * 50) + ".shtml\">后50页</a>]");//
          if (startID - PageSize * 100 > 0)
              sb.Append(" [<a href=\"/ItemList/" + Program.make(startID - PageSize * 100) + ".shtml\">后100页</a>]");//
          if (startID - PageSize > 0)
               sb.Append(" [<a href=\"/ItemList/" + Program.make(startID - PageSize) + ".shtml\">后一页</a>]");//

           sb.Append("</div>");//
           string str = sb.ToString();//
           sb.Remove(0, sb.Length);//
           sb = null;//
           return str;//

       }
    }
}
