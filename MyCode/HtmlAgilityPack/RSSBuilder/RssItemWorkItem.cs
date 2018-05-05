using System;
using System.Collections.Generic;
using System.Text;
using Rss;//
using System.Xml;//
using ThreadWorker;
using System.Threading;//
using CommonPack;
using BlackHen.Threading;//
using System.Collections;
using HtmlAgilityPack;
namespace RSSBuilder
{
    public class RssItemWorkItem : WorkItem
    {
        private RssItem RssItem = null;//
        private string Key = null;//
        public RssItemWorkItem(string key, RssItem rssItem)
        {
            this.RssItem = rssItem;//
            this.Key = key;//
        }
        public override void Perform()
        {
            if (!RssManager.IsQuit)
                this.GetItems(this.RssItem);//
        }
        private void GetItems(RssItem item1)
        {
            if (item1 == null)
                return;//

            MyRssItem item = new MyRssItem();//

            foreach (RssCategory rc in item1.Categories)
                item.Category += rc.Name + ";";
            item.CategoryHash = item.Category.GetHashCode();//

            item.Author = item1.Author;//
            item.Description = item1.Description;//
            item.Link = item1.Link;//
            item.Title = item1.Title;//
            item.Key = this.Key;//
            //item.Enclosure = item1.Enclosure;//
            //item.Guid = item1.Guid;//
            //item.PubDate = item1.PubDate;//
            //item.Source = item1.Source;//
            //item.Comments = item1.Comments;//
            
            HtmlDocument doc;
            RssManager.HtmlWeb.UseCookies = true;//

            DateTime t1 = DateTime.Now;
            doc = RssManager.HtmlWeb.Load(item.Link.ToString());//
            RssManager.ShowMessge(t1, DateTime.Now, "Item Downloaded:" + item.Link.ToString());//
            //item.Content = doc.Text;//

            item.Content = CommonPack.HtmlAgilityPack.HtmlToSubject.Analytics2(doc);//
            if (item.Content == null || item.Content.Length < 200)
                return;//
            RssManager.AddRssItem(item);//
            //RssManager.AnalyticsWorkQueue.Add(new RssItemAnalyticsWorkItem(item));//
        }
       
    }

    
}