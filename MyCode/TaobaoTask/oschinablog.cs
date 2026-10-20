using MySql.Data.MySqlClient.Memcached;
using MySqlX.XDevAPI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using TaobaoTask;

namespace ConsoleTask
{
    public class oschinablog
    {
        static Random rnd = new Random();
        protected static void Sleep()
        {
            var s = rnd.Next(111, 211);
            logger.Info(string.Format("Sleep->{0}s {1}", s, DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff")));
            System.Threading.Thread.Sleep(1000 * s);
        }
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(gitee));
        static string baseUrl = "https://www.oschina.net/";

        static string recommendurl = "blog/widgets/_blog_index_recommend_list?classification=0&p={0}&type=ajax";
        public static void getLISTs()
        {
            string[] ids = File.ReadAllLines("oschinablog.id");
            var ids2 = new Dictionary<string, string>();
            foreach(string id in ids)
                ids2[id]= id;

            string cookie = File.ReadAllText("oschinablog.cookie");
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            RestClientUtil.SetCookies(restClient, cookie);
            while (true)
            {
                logger.Info(string.Format("start->{0} {1}", cookie.Length, ids2.Count));
                for (int i = 0; i < 8; i++)
                {
                    getLISTs(restClient,  cookie, i,ids2);
                    File.WriteAllLines("oschinablog.id",ids2.Keys.ToArray());
                }
                Sleep();
            }
        }

         static void getLISTs(IRestClient restClient, string cookie, int page, Dictionary<string, string> ids)
        {
            var url = string.Format(recommendurl, page);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var ms = RestClientUtil.RegexGetMatchs(resp.Content, "content(.*?)<a class(.*?)header(.*?)https://my.oschina.net/(.*?)/blog/");
                    logger.Info(string.Format("blog->{0} {1}", page, ms.Count));
                    foreach (Match m in ms)
                    {
                        if (m.Success)
                        {
                            var blogid = m.Groups[4].Value;
                            if (blogid.Length>0&&blogid.Length<32&&!ids.ContainsKey(blogid))
                            {
                                ids[blogid] = blogid;
                                getbolg(restClient,  cookie, blogid);
                                System.Threading.Thread.Sleep(1000 * rnd.Next(6,12));
                            }
                            else
                            {
                                logger.Info(string.Format("blog ign->{0}", blogid));
                            }
                        }
                    }

                }
            }
            else
            {
                logger.Info(string.Format("lst resp->{0}", resp.StatusCode));
            }

        }
         static void getbolg(IRestClient restClient2, string cookie, string blogid)
        {
           
            var restClient = RestClientUtil.GetRestClient("https://my.oschina.net/", "*/*", true);
            RestClientUtil.SetCookies(restClient, cookie,false);
            //restClient.CookieContainer.SetCookies(restClient.BaseUrl, cookie);

            var url2 = string.Format("{0}", blogid);
            var resp = RestClientUtil.Get(restClient, url2, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var ms = RestClientUtil.RegexGetMatchs(resp.Content, "follow-btn(.*?)data-user-id=\"(.*?)\"(.*?)data-follow-status(.*?)>关注</span></a>");
                    logger.Info(string.Format("follow Match->{0} {1} {2}", blogid, ms.Count, resp.Content.Length));
                    foreach (Match m in ms)
                    {
                        if (m.Success)
                        {
                            var uid = m.Groups[2].Value;
                            follow(restClient, blogid, uid);

                        }
                    }
                }

            }
            else
            {
                logger.Info(string.Format("blog resp->{0} {1}", url2, resp.StatusCode));
            }
        }
         static void follow(IRestClient restClient, string blogid,string uid)
        {
            //restClient.CookieContainer.Add(restClient.BaseUrl, new Cookie("Hm_lpvt_a411c4d1664dd70048ee98afe7b28f0b", "" + ConvertDateTimeInt(), "/"));

            var url = string.Format("{0}/user/switch_follow", blogid);
            var data = new Dictionary<string, object>();
            data["friendid"] = uid;
            //data["Hm_lvt_a411c4d1664dd70048ee98afe7b28f0b"] = "1675573193,1675574244,1675742357,1675828446";
            var resp = RestClientUtil.PostForm(restClient, url, data);


            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    logger.Info(string.Format("Content->{0}", resp.Content));
                    Sleep();
                }
            }
        }
        public static int ConvertDateTimeInt( )
        {
            return ConvertDateTimeInt(System.DateTime.Now.AddSeconds(-10)  );

        }
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
