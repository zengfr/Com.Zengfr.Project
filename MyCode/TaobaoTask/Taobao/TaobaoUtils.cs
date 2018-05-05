using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
namespace TaobaoTask.Taobao
{
    public class TaobaoUtils
    {
        static RegexOptions RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
        static DateTime lastTime = DateTime.Now.AddMinutes(-1);
        protected static RestClient GetRestClient(string baseUrl)
        {
            var restClient = new RestClient(baseUrl);
            restClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
            restClient.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            restClient.AddDefaultHeader("Accept-Language", "en-US,en;q=0.5");
            restClient.AddDefaultHeader("Accept-Encoding", "gzip,deflate,br");
            restClient.AddDefaultHeader("Cache-Control", "max-age=0");

            var timeSpan = (lastTime.AddMilliseconds(511 * 1)-DateTime.Now);
            if (timeSpan.TotalMilliseconds > 0)
            {
                System.Threading.Thread.Sleep(timeSpan);
            }
            lastTime = DateTime.Now;
            return restClient;
        }
        protected static RestClient GetRestClient()
        {
            string baseUrl = "http://www.taobao.com";
            return GetRestClient(baseUrl);
        }
        /// <summary>
        /// 主题市场 产品列表 https://www.taobao.com/markets/tbhome/market-list
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string[] GetmarketlistWords()
        {
            var words = new List<string>();

            var request = new RestRequest("/markets/tbhome/market-list?", Method.GET);
            string baseUrl = "https://www.taobao.com/";
            var client = GetRestClient(baseUrl);
            var response = client.Execute(request);
            var resultContent = response.Content;
            if (!string.IsNullOrWhiteSpace(resultContent))
            {
               var matchs= Regex.Matches(resultContent, "<a(.*?)class=\"category(.*?)>(.*?)</a>", RegexOptions);
                foreach (Match m in matchs)
                {
                    words.Add(m.Groups[3].Value);
                }
            }
            return words.ToArray();
        }
        /// <summary>
        /// 搜索关键字下拉联想
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static suggestResult suggest(string word)
        {
            var request = new RestRequest("/sug?code=utf-8&q={q}&area=c2c", Method.GET);
            request.AddParameter("q", word, ParameterType.UrlSegment);
            string baseUrl = "https://suggest.taobao.com/";
            var client = GetRestClient(baseUrl);
            var response = client.Execute(request);
            suggestResult resultData = null;
            try
            {
                resultData = JsonConvert.DeserializeObject<suggestResult>(response.Content);
            }
            catch { };
            return resultData;
        }
        /// <summary>
        /// 搜索查询结果
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static searchResult search(string word)
        {
            var request = new RestRequest("/search?ajax=true&ie=utf-8&q={q}&sort=sale-desc", Method.GET);
            request.AddParameter("q", word, ParameterType.UrlSegment);
            string baseUrl = "https://s.taobao.com/";
            var client = GetRestClient(baseUrl);
            var response = client.Execute(request);
            searchResult resultData = null;
            try
            {
                resultData = JsonConvert.DeserializeObject<searchResult>(response.Content);
            }
            catch { };
            return resultData;
        }
        public class suggestResult
        {

            public class magicItem
            {
                public string type { get; set; }
                public int index { get; set; }

                public magicDataItem[][] data { get; set; }

            }

            public class magicDataItem
            {
                public string type { get; set; }
                public string title { get; set; }
            }
            public string[][] result { get; set; }
            public magicItem[] magic { get; set; }
            public string[][] cat { get; set; }
            public string tmall { get; set; }
        }
        public class searchResult
        {
            public class searchmods
            {
                public searchitemList itemlist { get; set; }
            }
                public class searchitemList
            {
                
                public itemListData data { get; set; }
            }
            public class itemListData
            {
                public string query { get; set; }
                public auctionItem[] auctions { get; set; }
            }
            public class auctionItem
            {

                public long? nid { get; set; }
                public long? category { get; set; }
                public long? pid { get; set; }
                public string raw_title { get; set; }
                public decimal? view_price { get; set; }
                public decimal? view_fee { get; set; }
                public string item_loc { get; set; }
                public decimal? reserve_price { get; set; }
                public long? user_id { get; set; }
                public string view_sales { get; set; }
                public long? comment_count { get; set; }
                public string nick { get; set; }
                public auctionShopcard shopcard { get; set; }
            }
            public class auctionShopcard
            {
                public bool? isTmall { get; set; }
                public long?[] delivery { get; set; }
                public long?[] description { get; set; }
                public long?[] service { get; set; }
                public long? totalRate { get; set; }

            }
            public searchmods mods { get; set; }
        }
    }
}