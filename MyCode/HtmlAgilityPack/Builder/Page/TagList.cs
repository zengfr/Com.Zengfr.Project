using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using DB.DataDB;
using System.Diagnostics;
namespace Builder.Page
{
   public class TagList
    {
       static int Total = 0;
       static int TotalPage = 0;
       static int PageSize = 250;//
       static StoredProcedure SP;
      public static void Build()
       {
           SP = SPs.ComPagerData("Dict", "ID", "Id Desc", 1, PageSize, "*", "", null, Total);//
           SP.Execute();//
           Total = int.Parse(SP.OutputValues[0].ToString());
           TotalPage = ((int)Math.Ceiling((double)Total / PageSize));
           for (int i = 1; i < TotalPage; i++)
           { 
               MakePage(i);//
               Trace.WriteLine("TagList:" + "Page:" + i);//
           }
       }
       static void MakePage(int pageIndex)
       {
           StringBuilder sb = new StringBuilder();//
           sb.Append(FileOperate.ReadFile("template/taglist.shtml"));
           sb.Replace("{PageIndex}", pageIndex.ToString());//
           sb.Replace("{TagItems}", tagList(pageIndex));//
           FileOperate.WriteFile("TagList/" + Program.make(pageIndex) + ".shtml", sb.ToString());//
           sb.Remove(0, sb.Length);//
       }
       static string tagList(int pageIndex)
       {
           SP = SPs.ComPagerData("Dict", "ID", "Id Desc", pageIndex, PageSize, "*", "", null, Total);//
           DictCollection dicts = new DictCollection();
           dicts.LoadAndCloseReader(SP.GetReader());//
           
           StringBuilder sb = new StringBuilder();//
           sb.Append("<ul class='taglist'>");//
           foreach (Dict dict in dicts)
           {
               sb.Append("<li><a href=\"/TagItemList/" + Program.make(dict.Id) + "/" + Program.make(1)+ ".shtml\">");//
               sb.Append("" + dict.Word + "</a> </li>");//
           }
           sb.Append("</ul><div class='pageer'");//
           int startPage = pageIndex - 5;
           if (startPage < 1) startPage = 1;
           int endPage = pageIndex + 5;
           if (endPage > TotalPage) endPage = TotalPage;

           if(pageIndex>10)
               sb.Append(" <a href=\"/TagList/" + Program.make(pageIndex-10) + ".shtml\">[Ç°10Ò³]</a> ");//
           for (int i = startPage; i < endPage; i++)
           {
               if(i!=pageIndex)
               sb.Append(" <a href=\"/TagList/" + Program.make(i) + ".shtml\">[" + i + "]</a> ");//
               else
               sb.Append(" [" + i + "] ");//
           }
           if (pageIndex < TotalPage-10)
               sb.Append(" <a href=\"/TagList/" + Program.make(pageIndex + 10) + ".shtml\">[ºó10Ò³]</a> ");//
           
           sb.Append("</div>");//
           string str = sb.ToString();//
           sb.Remove(0, sb.Length);//
           sb = null;//
           return str;//
       }
    }
}
