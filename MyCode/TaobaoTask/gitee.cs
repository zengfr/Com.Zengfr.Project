using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;

namespace TaobaoTask
{
    public class gitee
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(gitee));
        static string baseUrl = "https://gitee.com/";
        public static List<String> getKeys()
        {
            string[] keys = new string[] { "框架","并发","高并发","分布式","高性能","大型","服务框架","插件","开发包","跨平台","开发工具","生成器",
                "企业级","工业级","管理","后台","服务器","智能","分销","物联网","商城","全栈式","快速开发","爬虫","大数据","微服务","工具","常用",
                "阿里","阿里巴巴", "腾讯", "百度", "京东", "美团", "点评", "支付宝", "蚂蚁", "平安","低代码","直播","广告",
                "头条", "淘宝", "微信", "库存", "抖音","拼多多", "小米", "手机","支付","保险","android","安卓","团购","活动","同城",
                "华为","区块链", "优酷", "金融", "财富","银行", "基金", "股票","量化交易","机器学习","小程序","汇编","游戏","社交","交友"

            ,"spring cloud","dubbo","apache","Spring","Springboot","hibernate","mybatis","netty","guava","lucene",
                "redis","elasticsearch","common","commons","aop","rpc","ioc","vue","flutter","angular","docker","k8s"};
            var rs = keys.ToList();
            string url = "https://www.oschina.net/project/tags";
            var restClient = RestClientUtil.GetRestClient(url, true);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var tags = RestClientUtil.RegexGetStrings(resp.Content, " title=\"(.*?)\"", 1);
                rs.AddRange(tags);
            }
            return rs;
        }
        public static void SearchProject()
        {
            List<String> keys = getKeys();

            string[] sorts = new string[] { "default", "last_push_at", "forks_count", "watches_count", "stars_count" };
            var keys2 = RestClientUtil.GetRandomCollection(keys);
            var sorts2 = RestClientUtil.GetRandomCollection(sorts);
            for (int i = 1; i < 5; i++)
            {
               
                    foreach (var key in keys2)
                {
                    foreach (var sort in sorts2)
                    {
                        logger.Info(string.Format("Search->{0},{1},{2}", key, sort, i));
                        SearchProject(key, sort, i);
                    }
                }
            }
        }
        static Queue<String> queue = new Queue<string>();
        public static void SearchProject(string key, string sort, int page)
        {
            string url = string.Format("search?q={0}&page={1}&sort={2}", RestClientUtil.Unicode2String(key), page, sort);
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            string cookie = File.ReadAllText("gitee.cookie");
            RestClientUtil.SetCookies(restClient, cookie);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    RestClientUtil.SetCookies(restClient, resp.Cookies);
                    var projUrls = RestClientUtil.RegexGetStrings(resp.Content, "<div class=\"title\"(.*?)<a href=\"(.*?)\" class=\"ns\" target=\"_blank\"", 2);
                    var ms = RestClientUtil.RegexGetMatchs(resp.Content, "<div class=\"title\"(.*?)<a href=\"(.*?)\" class=\"ns\" target=\"_blank\"(.*?)<em>(.*?)</em>(.*?)<em>(.*?)</em>(.*?)<em>(.*?)</em>");
                    logger.Info(string.Format("Match->{0}", ms.Count));
                    foreach (Match m in ms)
                    {
                        if (m.Success)
                        {
                            if ((m.Groups[4].Value + m.Groups[6].Value + m.Groups[8].Value).ToUpper(). Contains("K")
                                || int.Parse(m.Groups[4].Value.Trim('K')) > 222 || int.Parse(m.Groups[6].Value.Trim('K')) > 222 || m.Groups[8].Value.Length < 11 && int.Parse(m.Groups[8].Value.Trim('K')) > 222)
                            {
                                var projUrl = m.Groups[2].Value;
                                if (!queue.Contains(projUrl)){
                                    queue.Enqueue(projUrl);
                                   // StarProject(restClient, m.Groups[2].Value);
                                }

                            }
                        }
                    }
                    while (queue.Count > 44)
                    {
                        queue.Dequeue();
                    }
                    //return;
                    foreach (var projUrl in projUrls)
                    {
                        logger.Info(string.Format("Project->{0}", projUrl));
                        //WatchProject(restClient, projUrl);
                        //StarProject(restClient, projUrl);

                        GetWatch(restClient, projUrl, page);
                        GetStar(restClient, projUrl, page);
                        //GetFork(restClient, projUrl, page);
                    }
                }
            }
        }
        protected static void GetWatch(IRestClient restClient, string projUrl, int page)
        {
            var type = "watchers";
            GetProjectInfo(restClient, projUrl, type, page);

        }
        protected static void GetStar(IRestClient restClient, string projUrl, int page)
        {
            var type = "stargazers";
            GetProjectInfo(restClient, projUrl, type, page);
        }
        protected static void GetFork(IRestClient restClient, string projUrl, int page)
        {
            var type = "members";
            GetProjectInfo(restClient, projUrl, type, page);
        }
        protected static void GetProjectInfo(IRestClient restClient, string projUrl, string type, int page)
        {
            if (projUrl.Contains("?"))
                projUrl = projUrl.Substring(0, projUrl.IndexOf("?"));
            string url = string.Format("{0}/{1}?page={2}", projUrl.Replace(""+baseUrl,""), type, page);
            restClient.RemoveDefaultParameter("referer");
            restClient.AddDefaultHeader("referer", projUrl);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    RestClientUtil.SetCookies(restClient, resp.Cookies);
                    var users = RestClientUtil.RegexGetStringsBySplit(resp.Content, "<div class='target-item'", "follow-button(.*?)<a(.*?)rel=\"nofollow\" data-method=\"post\" href=\"(.*?)/follow\"", 3).ToList();

                    string csrfToken = RestClientUtil.RegexGetString("", resp.Content, "csrf-token\" content=\"(.*?)\"", 1);
                    var authenticity_token = csrfToken;

                    logger.Info(string.Format("users->{0},{1},{2},{3}", projUrl,  type,  page,users.Count()));
                    foreach (var user in users)
                    {
                        var results = GetUserInfo(restClient, user,ref authenticity_token);
                        logger.Info(string.Format("UserInfo->{0}:{1}", user, string.Join(",", results)));
                        if (IsUserInfoCheckOK(results))
                        {
                            FollowUser(restClient, user, authenticity_token);
                        }

                    }
                }
            }

        }
        protected static bool IsUserInfoCheckOK(List<int> nums)
        {
            if (nums.Count >= 4)
            {
                int stars = nums[0];
                int watches = nums[1];
                int followers = nums[2];
                int following = nums[3];
                
                int n = 3;
                if (followers > 11 || stars > 88 || following > 88 || watches > 88)
                {
                    return true;
                }
                if (nums.Sum() > 222)
                {
                    return true;
                }
                if ((followers <= n || stars <= n || watches <= n))
                {
                    return false;
                }

                return false;
            }
            return false;
        }
        protected static List<int> GetUserInfo(IRestClient restClient, string user,ref string csrfToken)
        {
            string url = string.Format("{0}", user);
            var resp = RestClientUtil.Get(restClient, url, null);
            var results = new List<int>();
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var nums = RestClientUtil.RegexGetStringsBySplit(resp.Content, "<div class='four wide column'>",
                        "<div class='social-count'(.*?)>(.*?)</div>", 2);
                    int n;
                    foreach (var num in nums)
                    {
                        if (!string.IsNullOrEmpty(num))
                        {
                            if (int.TryParse(num, out n))
                            {
                                results.Add(n);
                            }
                        }

                    }
                      csrfToken = RestClientUtil.RegexGetString("", resp.Content, "csrf-token\" content=\"(.*?)\"", 1);
                    
                }
            }
            return results;
        }
        public static void WatchProject(IRestClient restClient, string projUrl)
        {

            var resp2 = RestClientUtil.PostForm(restClient, projUrl + "/watch", null, null);
            logger.Info(string.Format("WatchProject->{0}:{1}", projUrl, resp2.StatusCode));
            Sleep();
        }
        public static void StarProject(IRestClient restClient, string projUrl)
        {
            var resp = RestClientUtil.Get(restClient, projUrl, null);
            string csrfToken = RestClientUtil.RegexGetString("", resp.Content, "csrf-param(.*?)content=\"(.*?)\" name=\"csrf-token", 2);
            RestClientUtil.SetDefaultHeader(restClient, "Referer", baseUrl.TrimEnd('/') + projUrl);
            var resp2 = RestClientUtil.PostForm(restClient, projUrl + "/star", csrfToken, null, null);
            logger.Info(string.Format("StarProject->{0}:{1},{2},{3}", projUrl, resp2.StatusCode, csrfToken, resp2.Content.Length));
            Sleep();
        }
        public static void FollowUser(IRestClient restClient, string user, string authenticity_token)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["_method"] = "post";
            data["authenticity_token"] = authenticity_token;
            //RestClientUtil.SetDefaultHeader(restClient, "X-CSRF-Token", authenticity_token);
            var resp2 = RestClientUtil.PostForm(restClient, user + "/follow", authenticity_token, data, null);
            logger.Info(string.Format("FollowUser->{0}:{1} {2}", user, resp2.StatusCode,authenticity_token));
            Sleep();
        }
        static Random rnd = new Random();
        protected static void Sleep()
        {
            var s = rnd.Next(111, 211);
            logger.Info(string.Format("Sleep->{0}s {1}", s, DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff")));
            System.Threading.Thread.Sleep(1000 * s);
        }
    }
}
