using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using DB.DataDB;
using System.Diagnostics;
namespace Builder.Page
{
    class TagItemList
    {
        static int Total = 0;
        static int TotalPage = 0;
        static int PageSize = 50;//
        static StoredProcedure SP;
        public static void Build()
        {
            int total = 0;
            StoredProcedure sp = SPs.ComPagerData("Dict", "ID", "Id Desc", 1, PageSize, "*", "", null, total);//
            sp.Execute();//
            total = int.Parse(sp.OutputValues[0].ToString());
            int totalPage = ((int)Math.Ceiling((double)total / PageSize));

            for (int i = 1; i < totalPage; i++)
            {
                sp= SPs.ComPagerData("Dict", "ID", "ID Desc", i, PageSize, "*", "", null, Total);//
                DictCollection dicts = new DictCollection();
                dicts.LoadAndCloseReader(sp.GetReader());//
                foreach (Dict dict in dicts)
                {
                    MakeKey(dict);//
                    Trace.WriteLine("TagItemList:" + dict.Word);//
                }
            }
        }
        static void MakeKey(Dict dict)
        {
            SP = SPs.ComPagerData("ArticleNews", "ID", "Id Desc", 1, PageSize, "*", " text like '%" + dict.Word + "%'", null, Total);//
            ArticleNewsCollection ArticleNewss = new ArticleNewsCollection();
            ArticleNewss.LoadAndCloseReader(SP.GetReader());//
            SP.Execute();//
            Total = int.Parse(SP.OutputValues[1].ToString());
            TotalPage = ((int)Math.Ceiling((double)Total / PageSize));
            for (int i = 1; i < TotalPage; i++)
            {
                MakePage(dict, i);//
                Trace.WriteLine("TagItemList:" + dict.Word+ "Page:"+i);//
            }
        }
        static void MakePage(Dict dict, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();//
            sb.Append(FileOperate.ReadFile("template/TagItemlist.shtml"));
            sb.Replace("{Title}", dict.Word);//
            sb.Replace("{PageIndex}", pageIndex.ToString());//
            sb.Replace("{Tag.Text}", dict.Word);//
            sb.Replace("{ListItems}", itemList(dict, pageIndex));//

            //sb.Replace("{TagTitle}", key);//
            //sb.Replace("{TagItems}", tagList(pageIndex));//
            FileOperate.WriteFile("TagItemList/" + Program.make(dict.Id) + "/" + Program.make(pageIndex) + ".shtml", sb.ToString());//
            sb.Remove(0, sb.Length);//
        }
        static string itemList(Dict dict, int pageIndex)
        {
            SP = SPs.ComPagerData("ArticleNews", "ID", "Id Desc", pageIndex, PageSize, "*", " text like '%" + dict.Word + "%'", null, Total);//
            ArticleNewsCollection ArticleNewss = new ArticleNewsCollection();
            ArticleNewss.LoadAndCloseReader(SP.GetReader());//

            StringBuilder sb = new StringBuilder();//
            sb.Append("<ul>");//
            foreach (ArticleNews ArticleNews in ArticleNewss)
            {
                sb.Append("<li><a href=\"/Item/" + Program.make(ArticleNews.Id) + ".shtml\">");//
                if (ArticleNews.TextX.Length > 70)
                    sb.Append("" + ArticleNews.TextX.Substring(0, 70) + "...</a></li>");//
                else
                    sb.Append("" + ArticleNews.TextX + "</a></li>");//
            }
            sb.Append("</ul><div class='pageer'>");//
            int startPage = pageIndex - 5;
            if (startPage < 1) startPage = 1;
            int endPage = pageIndex + 5;
            if (endPage > TotalPage) endPage = TotalPage;

            if (pageIndex > 10)
                sb.Append(" <a href=\"/TagItemList/" + Program.make(dict.Id) + "/" + Program.make(pageIndex - 10) + ".shtml\">[Ç°10Ò³]</a> ");//
         
            for (int i = startPage; i < endPage; i++)
            {
                if (i != pageIndex)
                    sb.Append("<a href=\"/TagItemList/" + Program.make(dict.Id) + "/" + Program.make(i) + ".shtml\">[" + i + "]</a> ");//
            else
                sb.Append(" [" + i + "] ");//
            }
            if (pageIndex < TotalPage - 10)
                sb.Append(" <a href=\"/TagItemList/" + Program.make(dict.Id) + "/" + Program.make(pageIndex + 10) + ".shtml\">[ºó10Ò³]</a> ");//
           
            sb.Append("</div>");//
            string str = sb.ToString();//
            sb.Remove(0, sb.Length);//
            sb = null;//
            return str;//

        }
    }
}
