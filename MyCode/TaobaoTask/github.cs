using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace TaobaoTask
{
    public class github
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(github));
        public static void SearchProject()
        {
            var keys = gitee.getKeys();
            string[] sorts = new string[] { "", "stars", "forks", "updated" };
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
        public static void SearchProject(string key, string sort, int page)
        {
            string baseUrl = "https://github.com/";
            string url = string.Format("search?search?l=Java&type=Repositories&o=desc&q={0}&p={1}&s={2}", RestClientUtil.Unicode2String(key), page, sort);
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            string cookie = File.ReadAllText("github.cookie");
            RestClientUtil.SetCookies(restClient, cookie);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    logger.Info(string.Format("resp->{0} {1}", resp.ContentLength, resp.Content.Length));
                    var projUrls = RestClientUtil.RegexGetStrings(resp.Content, "class=\"repo-list-item(.*?)<a(.*?)href=\"(.*?)\">", 3).ToList();

                    foreach (var projUrl in projUrls)
                    {
                        logger.Info(string.Format("Project->{0}", projUrl));
                        //WatchProject(restClient, projUrl);
                        //StarProject(restClient, projUrl);

                        GetWatch(restClient, projUrl, page);
                        //GetStar(restClient, projUrl, page);
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
            string url = string.Format("{0}/{1}?page={2}", projUrl, type, page);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var usertextlist = RestClientUtil.RegexGetStrings(resp.Content, "<form class=\"js-form-toggle-target(.*?)</form>", 0).ToList();

                    foreach (var usertext in usertextlist)
                    {
                        if (!usertext.Contains("value=\"Follow\""))
                            continue;
                        var user = RestClientUtil.RegexGetString("", usertext, "target=(.*?)\\\"", 1);
                        var authenticity_token = RestClientUtil.RegexGetString("", usertext, "authenticity_token\\\" value=\\\"(.*?)\\\"", 1);
                        //var results = GetUserInfo(restClient, user, out authenticity_token);
                        //logger.Info(string.Format("UserInfo->{0}:{1}", user, string.Join(",", results)));
                        //if (IsUserInfoCheckOK(results))
                        {
                            FollowUser(restClient, user, authenticity_token);
                        }

                    }
                }
            }

        }
        protected static bool IsUserInfoCheckOK(List<int> nums)
        {
            if (nums.Count >= 5)
            {
                int repositories = nums[0];
                int projects = nums[1];
                int stars = nums[2];
                int followers = nums[3];
                int following = nums[4];
                int n = 5;
                if (followers > 22 || repositories > 88 || stars > 88 || following > 88)
                {
                    return true;
                }
                if (nums.Sum() > 111)
                {
                    return true;
                }
                if ((followers <= n || stars <= n || repositories <= n || following <= n || stars < repositories))
                {
                    return false;
                }

                return true;
            }
            return false;
        }
        protected static List<int> GetUserInfo(IRestClient restClient, string user, out string authenticity_token)
        {
            authenticity_token = string.Empty;

            string url = string.Format("{0}", user);
            var resp = RestClientUtil.Get(restClient, url, null);
            var results = new List<int>();
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var nums = RestClientUtil.RegexGetStrings(resp.Content, "<span class=\"Counter hide-lg hide-md hide-sm\">(.*?)</span>", 1).ToList();
                    int n;
                    foreach (var num in nums)
                    {
                        if (!string.IsNullOrEmpty(num))
                        {
                            if (num.IndexOf('k') != -1)
                            {
                                var num2 = num.Replace("k", ""); float n2;
                                if (float.TryParse(num2, out n2))
                                {
                                    results.Add(Convert.ToInt32(n2 * 1000));
                                }
                            }
                            else if (int.TryParse(num, out n))
                            {
                                results.Add(n);
                            }
                        }

                    }

                    authenticity_token = RestClientUtil.RegexGetString(authenticity_token, resp.Content, "<span class=\"follow\">(.*?)<input(.*?)name=\"authenticity_token\"(.*?)value=\"(.*?)\"(.*?)</span>", 4);
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

            var resp2 = RestClientUtil.PostForm(restClient, projUrl + "/star", null, null);
            logger.Info(string.Format("StarProject->{0}:{1}", projUrl, resp2.StatusCode));
            Sleep();
        }
        static int followCount = 0;
        public static void FollowUser(IRestClient restClient, string user, string authenticity_token)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["_method"] = "post";
            data["authenticity_token"] = authenticity_token;
            data["commit"] = "Follow";
            data["utf8"] = "&#x2713;";
            var resp2 = RestClientUtil.PostForm(restClient, "/users/follow?target=" + user.Trim('/'), data, null);
            logger.Info(string.Format("Follow->{0},{1}:{2},{3},{4}", followCount, user, authenticity_token, resp2.StatusCode, resp2.Content));
            if (resp2.Content != null && resp2.Content.Length > 5 && resp2.Content.Length < 100)
            {
                followCount++;
                Sleep();
            }
        }
        static Random rnd = new Random();
       
        protected static void Sleep()
        {
            var s = rnd.Next(60 * 1-20, 60 * 2-30);
            logger.Info(string.Format("Sleep->{0}s {1}", s, DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff")));
            System.Threading.Thread.Sleep(1000 * s);
        }
    }
}
