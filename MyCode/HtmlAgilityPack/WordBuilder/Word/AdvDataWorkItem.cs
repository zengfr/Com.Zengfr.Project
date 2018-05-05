using System;
using System.Collections.Generic;
using System.Text;

using BlackHen.Threading;//
using ParserEngine;//
using DB;//
using System.Threading;//
using NHibernate.Expression;
namespace WordBuilder
{
    public class AdvKeyWorkItem : WorkItem
    {
        public override void Perform()
        {
            if (!Manager.IsQuit)
                 Start();
        }
        private static string GetDBKey()
        {
            CnHot obj;
            do
            {
                Thread.Sleep(1000 * 3);//
                obj = CnHot.FindFirst(new Order[] { Order.Asc("DoCount"), Order.Desc("Count") }, Expression.Eq("State", byte.Parse("1")));//
            } while (obj == null);
            obj.DoCount += 1; obj.Update();//

            return obj.Name;//
        }

        private static void Start()
        {
            string key = GetDBKey();//
            int totalCount = 0;
           AdvDataManager.Search(key, 0, ref totalCount);
            Thread.Sleep(1000 * 10);//
            Manager.ShowMessge("K:" + key + " T:" + totalCount);// 
            if (totalCount == 0)
            {
                Start();//
                return;//
            }
            if (totalCount > 800)
                totalCount = 800;//
            for (int i = Math.Min(10, totalCount) - 1; i < totalCount; )
            {
               AdvDataManager.WorkQueue.Add(new AdvDataWorkItem(key, i, ref totalCount));//
                i += 10;
            }
        }
    }
   public class AdvDataWorkItem:WorkItem
   {
       HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();//
       string Key;
       int Pn;//
       int TotalCount;
       public AdvDataWorkItem(string key, int pn,ref int totalCount)
       {
           this.Key = key;
           this.Pn = pn;//
           this.TotalCount = totalCount;//
       }
       public override void Perform()
       {
           if (!Manager.IsQuit)
               this.Get();//
       }
       private void Get()
       {
           AdvData obj; Manager.ShowMessge("K:"+this.Key + " P/T:" + this.Pn + "/" + this.TotalCount);//
           CommonPack.XmlSerializableList<Area> list = AdvDataManager.Search(this.Key, this.Pn, ref this.TotalCount);//
           int h = 0;//
           foreach (Area box in list)
           {
               Manager.ShowMessge("Get:" + box.Rows.Count + " K:" + this.Key + " P/T:" + this.Pn + "/" + this.TotalCount);//
               foreach (Row Row in box.Rows)
               {
                   Thread.Sleep(100);//
                   h = Row.Cells[0].Value.GetHashCode();//
                   if (AdvDataManager.BloomFilter.IsRepeat(Row.Cells[0].Value))
                   {
                       Manager.ShowMessge("BF Exists:" + Row.Cells[0].Value);//
                       continue;
                   }
                   if (AdvData.Exists("LinkHash=" + h))
                   {
                       Manager.ShowMessge("DB Exists:" + Row.Cells[0].Value);//
                       continue;
                   }
                   obj = new AdvData();//
                   obj.Key = this.Key;//
                   obj.KeyHash = 0;//
                   obj.Link = Row.Cells[0].Value;//
                   obj.LinkHash = obj.Link.GetHashCode();//
                   obj.Title = Row.Cells[1].Value;//
                   try
                   {
                       DateTime t = DateTime.Now;//
                       obj.Content = CommonPack.HtmlAgilityPack.HtmlToSubject.Analytics2(hw.Load(obj.Link));//
                       Manager.ShowMessge(t, "析:" + obj.Link);//
                   }
                   catch (Exception e)
                   {
                       Manager.ShowMessge(e.Message);//
                       continue;
                   }
                   if (obj.Content == null)
                   {
                       Manager.ShowMessge("Null:" + "".PadRight(8, ' ') + "\t" + obj.Link);//
                       continue;
                   }
                   if (obj.Content.Length > 159)
                   {
                       obj.Create();// Save();//
                       Manager.ShowMessge("存:" + obj.ID.ToString().PadRight(8, ' ') + "\t" + obj.Link);//
                   }
                   else
                   {
                       Manager.ShowMessge("<160:" + "".PadRight(8, ' ') + "\t" + obj.Link);//
                   }

               }
           }
           
       }
    }
}
