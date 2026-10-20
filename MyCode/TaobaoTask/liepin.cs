using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask
{
    public class liepin
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(liepin));
        static string baseUrl = "https://c.liepin.com/";
        public static void search()
        {
            string[] keys = new string[] { "框架","并发","分布式","高性能","大型","服务框架","插件","开发包","跨平台","开发工具","生成器",
                "企业级","工业级","管理","后台","服务器","智能","分销","物联网","商城","全栈式","快速开发","爬虫","大数据",
                "阿里","阿里巴巴", "腾讯", "百度", "京东", "美团", "点评", "支付宝", "蚂蚁", "平安",
                "头条", "淘宝", "微信", "抖音","拼多多", "小米", "手机","支付","保险",
                "华为","区块链", "优酷", "金融", "财富","银行", "基金", "股票","量化交易"
            ,"apache","Spring","Springboot","hibernate","mybatis","netty","guava","lucene","redis","elasticsearch","common","commons","aop","rpc","ioc"};
            var keys2 = RestClientUtil.GetRandomCollection(keys);
            foreach (var key in keys2)
            {

                for (int i = 1; i < 11; i++)
                {
                    logger.Info(string.Format("Search->{0},{1}", key, i));
                    search(key, i);
                }
            }
        }
        protected static void search(string key, int page)
        {
            string url = string.Format("/sns/sosnscontactsmore?keyword={0}&curPage={1}&_=1560246747994&traceId=24674799482", System.Web.HttpUtility.UrlEncode(key), page);
            if (page <= 1)
                url = string.Format("sns/sosnscontacts?keyword=%E7%89%A9%E8%81%94%E7%BD%91", System.Web.HttpUtility.UrlEncode(key), page);
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            //RestClientUtil.SetDefaultHeader(restClient,"X-Requested-With", "XMLHttpRequest");
            var ck = "__uuid=1560062799375.25; _uuid=3AFD2E99FFA8441965FA922D1CCBDE99; abtest=0; _fecdn_=1; city_site=sh; access_system=C; need_bind_tel=false; c_flag=adce341e0b3dbcba17bde0cb6824d696; gr_user_id=79fa98ea-b7c9-4b0f-9689-8d9e71dfd7c1; imClientId=c44cbafc41d437491015fe21999bb118; imId=c44cbafc41d437491b9c78675c59b955; imClientId_0=c44cbafc41d437491015fe21999bb118; imId_0=c44cbafc41d437491b9c78675c59b955; bad1b2d9162fab1f80dde1897f7a2972_gr_last_sent_cs1=0b4a091dfbc29aa71a667e00ad0d9493; grwng_uid=fc23114f-94cf-4437-9ce0-bb68190c20ad; gr_session_id_bad1b2d9162fab1f80dde1897f7a2972=81a17001-5fd5-4415-b7b3-079d0ac49f4b; gr_cs1_81a17001-5fd5-4415-b7b3-079d0ac49f4b=UniqueKey%3A0b4a091dfbc29aa71a667e00ad0d9493; __tlog=1560296006452.07%7C00000000%7C00000000%7C00000000%7C00000000; _mscid=00000000; Hm_lvt_a2647413544f5a04f00da7eee0d5e200=1560225081,1560225097,1560225457,1560296007; char_captcha=48E48A7DC0A844059B42AE414D5AC95F; UniqueKey=0b4a091dfbc29aa71a667e00ad0d9493; lt_auth=v%2BsJbyAMy1v%2F5njZiDNZsKsZj9r8ATnN9XwJhh8A0IC7XfK04PjiQwmHqbYGxBMhm0t3dMULNLb%2B%0D%0AN%2BH4yHNN7UoSwG6niZ%2B3v%2BW72GEDT90zc%2F2ujPmok8zSS513nHpQySAwonJKyR6l4R1za4fjmGPh%0D%0Ajoju1bSl%2B%2Fv0iyyH%0D%0A; user_kind=0; user_name=%E6%9B%BE%E7%B9%81%E8%8D%A3; user_photo=5cff1d4b501544662c955a0406u.jpg; user_roles=0; new_user=false; bad1b2d9162fab1f80dde1897f7a2972_gr_session_id=989b92dc-ac5f-4857-971f-adcd0c150ee7; bad1b2d9162fab1f80dde1897f7a2972_gr_last_sent_sid_with_cs1=989b92dc-ac5f-4857-971f-adcd0c150ee7; fe_ck=-1560296108526; bad1b2d9162fab1f80dde1897f7a2972_gr_session_id_989b92dc-ac5f-4857-971f-adcd0c150ee7=true; JSESSIONID=E096F3786B04AE792BDFB423CAFDE5EF; bad1b2d9162fab1f80dde1897f7a2972_gr_cs1=0b4a091dfbc29aa71a667e00ad0d9493; __session_seq=5; __uv_seq=5; Hm_lpvt_a2647413544f5a04f00da7eee0d5e200=1560296115; fe_im_socketSequence=3_3_3";
            RestClientUtil.SetCookies(restClient, ck);
            var resp = RestClientUtil.Get(baseUrl, url, null, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                RestClientUtil.SetCookies(restClient, resp.Cookies);
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var userIds = RestClientUtil.RegexGetStrings(resp.Content, "data-uid=\"(.*?)\"", 1).ToList();

                    foreach (var userId in userIds)
                    {
                        adduser(userId);
                    }
                    if (!userIds.Any()) {
                        logger.Info(string.Format("{0},{1},{2}", key,page,  resp.ResponseUri));
                    }
                }
            }
            else {

            }
        }
        protected static void adduser(string userId)
        {
            var data = new Dictionary<string, object>();
            data["follower_id"] = userId;
            data["tag"] = 4;
            data["desc"] = "我是资深软件工程师曾繁荣,老家江西赣州目前上海,很高兴认识您，希望加您为好友。";
            string url = "/connection/addattention2.json?traceId=24649527581";
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            var resp = RestClientUtil.PostForm(restClient, url, data, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    logger.Info(string.Format("{0}:{1}", userId, resp.Content));
                    if (resp.Content.IndexOf("申请次数已达上限") != -1)
                    {
                        Sleep(60 * 10, 60 * 30);
                    }
                }
            }
            Sleep();
        }
        static Random rnd = new Random();
        protected static void Sleep()
        {
            Sleep(333, 444);

        }
        protected static void Sleep(int min, int max)
        {
            var s = rnd.Next(min, max);
            logger.Info(string.Format("Sleep->{0}s {1}", s, DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff")));
            System.Threading.Thread.Sleep(1000 * s);
        }
    }
}
