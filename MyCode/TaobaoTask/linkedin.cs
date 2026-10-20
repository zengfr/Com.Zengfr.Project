using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
namespace TaobaoTask
{
    #region MyRegion

    #endregion
    public class linkedin
    {
        public static void relationshipsdiscovery()
        {
            for (int i = 0; i < 22; i++)
            {
                relationshipsdiscovery(i);
            }
        }
        public static void relationshipsdiscovery(int page)
        {
            string baseUrl = "https://www.linkedin.com/";
            string url = string.Format("voyager/api/relationships/discovery?count=10&includeInsights=true&q=pymk&start={0}", page * 10);
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            RestClientUtil.SetCookies(restClient, "visit=\"v=1&G\"; bcookie=\"v=2&fd74071d-0e33-4a79-8afe-a798d4a87e91\"; bscookie=\"v=1&20180321144046945160bb-72b3-49a1-813a-9ae59dadd735AQHVdnShDXuZPM6j5Vjyt13sxvG6sguZ\"; _ga=GA1.2.1721678963.1556280047; liap=true; JSESSIONID=\"ajax:7510070965300390264\"; _guid=d971f5b9-f9db-436b-a174-3986098ffbd1; li_oatml=AQGE6D771bXZrQAAAWpZhud0YAhYE47OplMprDHfstDnMoC2nELZkpkSrVMaGu2FXPbx46-eu6J4SvNiE1GFtvjvog6ve1h0; sl=v=1&wHNEv; li_at=AQEDASBqGbAF2VZNAAABalmIPdwAAAFqfZTB3FEAQF9uPd0dn0eQAFO9_cdTfuaO4nCx3pF1UdzkInp91ceDQm5flMxJ3lIKnbZ1ADADc2-EG6pZ8YJ9XDA8spiCHB7nwI3l7Xe-e9W1bSAmhe1xOltn; li_cc=AQGoCEUOuDOb1AAAAWpZiPNLGdXwcaymzntf5dQQ-Wno8Cbzl5wDf89REPLmcqIDGBIgJintVdOL; _lipt=CwEAAAFqXyQw9U6mbi59QaVvhIO-Z3-lqLT3NrAg-n-CfutoYfxbJM8wmoqv256dqHV8NFJ363fWgJQid2ZYlFgi6NBM1dZ7eXfOTbHRscu4rPeC-lbVY98hn1qOZ_v4ge9ttxRo88sA1xZ_UAImWweYyt0N5315WZKWZkAEXDGz-NQ7m8HCtJQ8_gktIf7Uly8h98X_RTO5jo0RgKNDq_YWxVcyDcleMyDyNF4o1UEopMXXzLQLxBg6ERexzn0Gxbs; lang=v=2&lang=zh-cn; UserMatchHistory=AQJSZj4H1MWKMQAAAWpjwpenyaeDsDAN_tu31LUVHJjqDxZeWzyBUwPv5kAJM48KHhqG-Kg-lofLw1SV6DAgq-uqA8Pb9p_PNRB7STiBQAswm7RuONxbXlvaHyD2SuDWIHL6h9Y2meeVaBRWuGzVPmMsH02ViWMlXv3Zdx3aCSzh8sK_sKpX_8D6NN9SKeJ9f7M; lidc=\"b=OGST08:g=1082:u=1:i=1556452473:t=1556538873:s=AQG-ejLR_77GU0z333R1T6QcVNR1z_SX\"");
            RestClientUtil.SetDefaultHeader(restClient, "csrf-token", "ajax:7510070965300390264");
            RestClientUtil.SetDefaultHeader(restClient, "x-li-lang", "zh_CN");
            RestClientUtil.SetDefaultHeader(restClient, "x-li-proto", "http/2");
            //RestClientUtil.SetDefaultHeader(restClient, "x-li-page-instance", "urn:li:page:d_flagship3_people;MnEqAlTqTHWH6sIXqByQxg==");
            RestClientUtil.SetDefaultHeader(restClient, "x-li-page-instance", "urn:li:page:d_flagship3_search_srp_people;R+dWgcmDRWOthn5e9G3Jag==");
            RestClientUtil.SetDefaultHeader(restClient, "x-restli-protocol-version", "2.0.0");
            RestClientUtil.SetDefaultHeader(restClient, "accept", "application/vnd.linkedin.normalized+json+2.1");
            RestClientUtil.SetDefaultHeader(restClient, "origin", "https://www.linkedin.com");
            RestClientUtil.SetDefaultHeader(restClient, "referer", "https://www.linkedin.com/mynetwork/");
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //RestClientUtil.SetDefaultHeader(restClient,resp.Headers);
                RestClientUtil.SetCookies(restClient, resp.Cookies);
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {
                    var userInfos = RestClientUtil.RegexGetStringsBySplit(resp.Content, "\"summary\"", "member\"(.*?)\"group\"", 1);

                    foreach (var userInfo in userInfos)
                    {
                        var fs_miniProfile = RestClientUtil.RegexGetString("", userInfo, "fs_miniProfile:(.*?)\"", 1);
                        var trackingId = RestClientUtil.RegexGetString("", userInfo, "\"trackingId\"(.*?)\"(.*?)\"", 2);
                        if (!string.IsNullOrEmpty(fs_miniProfile) && !string.IsNullOrEmpty(trackingId))
                        {
                            normInvitationsAddFriend(restClient, fs_miniProfile, trackingId);
                        }
                    }
                }
            }
        }
        protected static void normInvitationsAddFriend(IRestClient restClient, string fs_miniProfile, string trackingId)
        {
            string url = "voyager/api/growth/normInvitations";
            NormInvitationsRoot r = new NormInvitationsRoot();
            r.emberEntityName = "growth/invitation/norm-invitation";
            r.trackingId = trackingId;
            r.invitee = new Invitee();
            r.invitee.InviteeProfile = new InviteeProfile();
            r.invitee.InviteeProfile.profileId = fs_miniProfile;
            r.message = "你好 加个好友 无事不打扰 有机会可以联系认识下哦 谢谢 ";
            String body = Newtonsoft.Json.JsonConvert.SerializeObject(r);
            RestClientUtil.SetDefaultHeader(restClient, "accept", "application/vnd.linkedin.normalized+json+2.1");
            RestClientUtil.SetDefaultHeader(restClient, "content-type", "application/json; charset=UTF-8");
            RestClientUtil.SetDefaultHeader(restClient, "x-li-track", "{\"clientVersion\":\"1.3.56\",\"osName\":\"web\",\"timezoneOffset\":8,\"deviceFormFactor\":\"DESKTOP\",\"mpName\":\"voyager-web\"}");
            var resp = RestClientUtil.Post(restClient, url, body);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(resp.Content))
                {

                }
            }
        }
        protected static void track()
        {
            string msg = @"";
        }

        public class InviteeProfile
        {
            /// <summary>
            /// 
            /// </summary>
            public string profileId { get; set; }
        }

        public class Invitee
        {
            [Newtonsoft.Json.JsonProperty("com.linkedin.voyager.growth.invitation.InviteeProfile")]
            public InviteeProfile InviteeProfile { get; set; }
        }

        public class NormInvitationsRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public string emberEntityName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Invitee invitee { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string trackingId { get; set; }
            public string message { get; set; }
           
        }
    }
}