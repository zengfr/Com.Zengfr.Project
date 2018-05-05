using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace TaobaoTask
{
   public class youzanApp
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(youzanApp));
        public static void process()
        {
            Console.WriteLine("输入ID->saveOKOnly,start,end:");
            var ids= Console.ReadLine().Split(';',',');
            bool saveOKOnly = true;
            long start = 0;
            long end = long.MaxValue;
            if (ids.Length > 0)
                bool.TryParse(ids[0], out saveOKOnly);
            if (ids.Length > 1)
                long.TryParse(ids[1],out start);
            if (ids.Length > 2)
                long.TryParse(ids[2], out end);
            var results = new List<youzanItem>();
            var saveTime = DateTime.Now;
           // for (long i = start; i < end; i++) {
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(process), i);
           // }
            var minute = 10;
            do {
                var item=process(start++);
                //if (saveOKOnly)
                {
                    if (item.StatusCode == "OK"&&item.Type== "店铺主页")
                    {
                        results.Add(item);
                    }
                }
                //else {
                   // results.Add(item);
                //}
                CSVUtil.SaveToFile(ref saveTime, minute, results,"0");
            } while (start<end);
            saveTime = saveTime.AddMinutes(-minute-5);
            CSVUtil.SaveToFile(ref saveTime, minute, results,"0");
        }
        public static youzanItem process(object id)
        {
           return process((long)id);
        }
        public static youzanItem process(long id){

            var baseUrl = string.Format("https://h5.youzan.com/","");
            var request = new RestRequest(string.Format("v2/showcase/homepage?kdt_id={0}", id), Method.GET);
            var client = RestClientUtil.GetRestClient(baseUrl);
            var response = client.Execute(request);

            var item = new youzanItem();
            item.Id = id;
            item.Url = baseUrl+request.Resource;
            item.StatusCode = ""+response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                item.Title = GetStringRegex("-1", response.Content, @"<title>(.*?)</title>", 1);
                item.Name = GetStringRegex("",response.Content, @"tpl-shop-title"">(.*?)</div>",1);
                item.AllCount = GetStringRegex("-1", response.Content, @"js-all-goods(.*?)count"">(.*?)</span>", 2);
                item.NewCount = GetStringRegex("-1", response.Content, @"js-new-goods(.*?)count"">(.*?)</span>", 2);
                if (response.Content != null && response.Content.IndexOf("店铺歇业中") <= 0)
                {
                    item.Type = GetStringRegex("-1", response.Content, @"class=""ft-links(.*?)_blank"">(.*?)</a>", 2);
                }
                else {
                    item.Type = "店铺歇业";
                }
            }
            log.InfoFormat("{0},{1},{2},{3},{4},{5},{6}", item.StatusCode,id, item.AllCount, item.NewCount, item.Name, item.Type,item.Title);
            return item;
        }
        static RegexOptions RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
        private static string GetStringRegex(string defaultV, string source, string pattern, int index) {
          var m=  Regex.Match(source, pattern, RegexOptions);
            if (m.Success) {
                return m.Groups[index].Value;
            }
            return defaultV;
        }
        public class youzanItem {
            public long Id { get; set; }
            public string StatusCode { get; set; }

            public string Url { get; set; }
            public string Name { get; set; }
            public string AllCount { get; set; }
            public string NewCount { get; set; }
            public string Title { get; set; }

            public string Type  { get; set; }
        }
    }
}
