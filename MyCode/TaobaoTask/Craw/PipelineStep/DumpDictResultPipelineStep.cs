using HtmlAgilityPack;
using NCrawler;
using NCrawler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrawler.HtmlProcessor.Extensions;
using System.Text.RegularExpressions;
using TaobaoTask;

namespace cc
{
    public class DumpDictResult : IPipelineStep
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(DumpDictResult));
         
        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            log.DebugFormat("FIND URL:{0}", propertyBag.Step.Uri);
            HtmlDocument htmlDoc = propertyBag["HtmlDoc"].Value as HtmlDocument;
            if (htmlDoc != null)
            {
                try
                {
                    var text = htmlDoc.ExtractText();
                    ProcessTextToDict(text);

                    var titles = htmlDoc.DocumentNode.SelectNodes("//*[@title]//@title");
                    if (titles != null)
                    {
                        foreach (var title in titles)
                        {
                            var titleText=  title.GetAttributeValue("title", "");
                            log.DebugFormat("FIND Title:{0}", titleText);
                            ProcessTextToDict(titleText);
                            titleText = null;
                        }
                    }
                    text = null; titles = null;
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception:{0}", ex.ToString());
                }
            }
        }
        static DateTime LastStoreTime = DateTime.Now;
        static DateTime LastGCTime = DateTime.Now;
        
        static object lockobj = new object();
        private static void ProcessTextToDict(string text)
        {
            var addCount = 0;

            var r = DumpDictResult.Split(text);
            addCount+= DictUtil.AddItem(r);

            var maxLen = 5;
            for (int len = 1; len <= maxLen; len++)
            {
                var tt = DumpDictResult.SplitCutChs(r, len);
                var count = DictUtil.AddItem(tt);
                addCount += count;
                log.DebugFormat("Dict->AddItemCount:{0},{1]",len, count);
                tt = null;
            }
            if (DateTime.Now - LastStoreTime > TimeSpan.FromMinutes(30))
            {
                lock (lockobj)
                {
                    if (DateTime.Now - LastStoreTime > TimeSpan.FromMinutes(30))
                    {
                        DictUtil.Store();
                        LastStoreTime = DateTime.Now;
                    }
                }
            }
            if (DateTime.Now - LastGCTime > TimeSpan.FromMinutes(5))
            {
                lock (lockobj)
                {
                    if (DateTime.Now - LastGCTime > TimeSpan.FromMinutes(5))
                    {
                        AppGCUtils.ClearMemory();
                        LastGCTime = DateTime.Now;
                    }
                }
            }
           text = null; r = null;
        }
        private static Regex regex = new Regex("[^\u4e00-\u9fa5_a-zA-Z0-9]{1,}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static Regex regexChs = new Regex("[\u4e00-\u9fa5]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static string[] Split(string str)
        { 
             var mColl=regex.Split(str).Where(t=>!string.IsNullOrWhiteSpace(t)).ToArray();
           
            return mColl;
        }
        public static IList<string> SplitCut(string[] str, int len)
        {
            var list = new List<string>();
            foreach (var s in str)
            {
                list.AddRange(SplitCut(s, len));
            }
            return list;
        }
        public static IList<string> SplitCut(string str, int len)
        {
            var list = new List<string>();
            if (len > 0 && !string.IsNullOrWhiteSpace(str))
            {
                for (int i = 0; i <= str.Length - 1 - (len - 1); i++)
                {
                    list.Add(str.Substring(i, len));
                }
            }
            return list;
        }
        public static IList<string> SplitCutChs(string[] str, int len)
        {
            var list = new List<string>();
            foreach (var s in str)
            {
                list.AddRange(SplitCutChs(s, len));
            }
            return list;
        }
        public static IList<string> SplitCutChs(string str, int len)
        {
            var list = new List<string>();
            if (regexChs.IsMatch(str))//&&str.Length>len)
            {
                if (len > 0 && !string.IsNullOrWhiteSpace(str))
                {
                    for (int i = 0; i <= str.Length - 1 - (len - 1); i++)
                    {
                        list.Add(str.Substring(i, len));
                    }
                }
            }
            else {
                list.Add(str);
            }
            return list;
        }
    }
}
