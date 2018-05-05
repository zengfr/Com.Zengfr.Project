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

    public class RssFeedWorkItem : WorkItem
    {
        private string Url = null;//
        private string Key = null;//
        public RssFeedWorkItem(string key, string url)
        {
            Url = url;//
            Key = key;
        }
        public override void Perform()
        {
            if (!RssManager.IsQuit)
                GetItems(RssFeed.Read(Url));//
        }
        private void GetItems(RssFeed rf)
        {
            if (rf == null)
                return;//
            foreach (RssChannel rc in rf.Channels)
            {
                foreach (RssItem item in rc.Items)
                {
                    item.Link = new Uri(rc.Link, item.Link);//
                    if (!RssManager.BloomFilter.IsRepeat(item.Link.ToString()))
                    {
                        RssManager.RssItemWorkQueue.Add(new RssItemWorkItem(this.Key, item));//
                        RssManager.ShowMessge("New Item:" + item.Link);//
                    }
                    else
                    {
                        RssManager.ShowMessge("Exist Item:" + item.Link);//
                    }
                }
            }
        }

    }
}