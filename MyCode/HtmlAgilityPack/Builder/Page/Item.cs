using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using DB.DataDB;
using System.Diagnostics;
using CommonPack;
namespace Builder.Page
{
   public class Item
    {
        static int Total = 0;
        static int TotalPage = 0;
        static int PageSize = 250;//
        static StoredProcedure SP;
        public static void Build()
        {
            SP = SPs.ComPagerData("NewsItem", "ID", "ID Desc", 1, PageSize, "*", "State=0", null, Total);//
            SP.Execute();//
            Total = int.Parse(SP.OutputValues[0].ToString());
            TotalPage = ((int)Math.Ceiling((double)Total / PageSize));

            for (int i = 1; i < TotalPage; i++)
            {
                SP = SPs.ComPagerData("NewsItem", "ID", "ID Desc", i, PageSize, "*", "State=0", null, Total);//
                NewsItemCollection NewsItems = new NewsItemCollection();
                NewsItems.LoadAndCloseReader(SP.GetReader());//
                foreach (NewsItem NewsItem in NewsItems)
                {
                    MakePage(NewsItem);//
                    NewsItem.State = 1;
                    NewsItem.Save();//
                }
            }
        }
       static void MakePage(NewsItem NewsItem)
       {
           Console.WriteLine("¿ªÊ¼Item:" + NewsItem.Id);//
           if (System.IO.File.Exists(Environment.CurrentDirectory + "Item/" + Program.make(NewsItem.Id) + ".shtml"))
               return;//
            StringBuilder sb = new StringBuilder();//
            sb.Append(FileOperate.ReadFile("template/Item.shtml"));
            sb.Replace("{Title}", NewsItem.Title);//
            sb.Replace("{Description}", NewsItem.Content);//
            sb.Replace("{Keywords}", NewsItem.Content);//

            sb.Replace("{Item.ID}", Convert.ToString( NewsItem.Id%JS.Item));//
            sb.Replace("{Item.Title}", NewsItem.Title);//
            Console.WriteLine("±£´æItem:" + NewsItem.Id);//
            FileOperate.WriteFile("Item/" + Program.make(NewsItem.Id) + ".shtml", sb.ToString());//
            sb.Remove(0, sb.Length);//
        }
        
    }
}
