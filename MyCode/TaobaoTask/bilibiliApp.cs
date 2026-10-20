using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using TaobaoTask;

namespace ConsoleTask
{
   public class bilibiliApp
    {
        static string adduser_url = "x/relation/tags/addUsers?cross_domain=true";
        static string baseUrl = "https://api.bilibili.com/";
        public static void process() {
            var consoleApp = ConsoleApp.Instance;
            consoleApp.InitConsole(true, true);
            consoleApp.InitConfig();

            var cookie = "_uuid=504CA463-27DB-0ACB-8B16-CB6FD78065C448106infoc; buvid3=E96C99BE-B525-4CDC-A652-4E26DCAA2AAF53933infoc; sid=b8z8oz6a; im_notify_type_492484080=0; CURRENT_FNVAL=16; LIVE_BUVID=AUTO2315808918313687; rpdid=|(J|lm|m)m)Y0J'ul)|ml)lmJ; DedeUserID=492484080; DedeUserID__ckMd5=51845b8fe61b3f5a; SESSDATA=24f21b51%2C1599270013%2Cfca32*31; bili_jct=2c6a52fa5eb44596e0f35c6393da48ef; PVID=1; bp_article_offset_492484080=397691693617952656; bp_t_offset_492484080=397691693617952656; bp_video_offset_492484080=400591956182355040; bfe_id=7b6a677b97d4786cd8b2f807c787d88b";
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            RestClientUtil.SetCookies(restClient, cookie);
            var csrf = "2c6a52fa5eb44596e0f35c6393da48ef";
            int index = 0;
            while (true) {
                var i =10000000+ r.Next(100,500000000);
                bool rtn = false;
                info (consoleApp, restClient, i, csrf,ref rtn);
                if (rtn)
                {
                   var b1= modify(consoleApp, restClient, i, csrf);
                    if (b1)
                    {
                        Thread.Sleep(3000 + r.Next(2000, 4000));
                        addusers(consoleApp, restClient, i, csrf);
                        Thread.Sleep(3000 + r.Next(3000, 6000));
                        if (index++ == 300)
                        {
                            Thread.Sleep(1000 * 60);
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    Thread.Sleep(3030);
                }
            }
        }
        static Random r = new Random();
        public static void info(ConsoleApp consoleApp, IRestClient restClient, long id, string csrf
           ,ref bool rtn )
        {
            var url =string.Format("x/space/acc/info?mid={0}&jsonp=jsonp",id);

            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {

                rtn= !level(resp.Content);
            }
        }
        private static bool level(string content) {
            return content.Contains("\"level\":0,") || content.Contains("\"level\":1,") || content.Contains("\"level\":2,");
        }
        public static bool modify(ConsoleApp consoleApp, IRestClient restClient, long id,string csrf)
        {
           var url = "x/relation/modify";
      
                 var data = new Dictionary<string, object>();
            data["fid"] = id;
            data["act"] = 1;
            data["re_src"] = 11;
            data["jsonp"] = "jsonp";
            data["csrf"] = csrf;

            var resp = RestClientUtil.PostForm(restClient, url, data, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                consoleApp.LeftWriter.WriteLine("modify:{0},{1}",id, String.Format("resp {0}", resp.Content.Replace("{", "").Replace("}", "")));//{"code":0,"message":"0","ttl":1}
            }
            return !(resp.Content.Contains("用户不存在"));
        }
        public static void addusers(ConsoleApp consoleApp, IRestClient restClient, long id,string csrf)
        {
            var url = string.Format("" + adduser_url);
            var data = new Dictionary<string, object>();
            data["fids"] = id;
            data["tagids"] = 263671;
            data["csrf"] = csrf;

            var resp = RestClientUtil.PostForm(restClient, url, data, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                consoleApp.LeftWriter.WriteLine("addusers:{0},{1}",id, String.Format("resp {0}",resp.Content.Replace("{","").Replace("}","")));//{"code":0,"message":"0","ttl":1}
            }
        }
    }
}
