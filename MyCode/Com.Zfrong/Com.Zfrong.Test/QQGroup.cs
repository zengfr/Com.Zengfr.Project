using System;
using System.Net;
using System.IO;//
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using  NetServ.Net.Json;
using NHibernate;
using NHibernate.Criterion;
using Com.Zfrong.Common.Data.AR.Base;
using Com.Zfrong.Common.Data.AR.Base.Entity;
using Com.Zfrong.Common.Http;
using Com.Zfrong.Common.Http.Async;
using Com.Zfrong.Common.Common;
namespace Test
{
    public interface IForm
    { 
       void RunT();
       void ShowLog(string str); 
    }
    public class MAIN
{
     public static void TestICXO()
        {
            Encoding _encoding = Encoding.GetEncoding("gb2312");//
            string _url,url = "http://bbs.icxo.com/my.php?item=buddylist&newbuddyid={ID}&buddysubmit=yes&inajax=1&ajaxmenuid=ajax_buddy_0_menu";
            string _data = "cdb_sid=f4yhkZ; depot_sid=SKImQ1; cdb_cookietime=2592000; cdb_auth=L2QXByHy8jC7ebO1NYaR00bymginBvVz7oUwSO5JbCEwDCw8Bz%2BP3XSfvwbQQjyW; cdb_smile=1D1; __utma=157472290.2630143752148071400.1242734694.1243401617.1243408368.13; __utmz=157472290.1242909151.5.2.utmcsr=bbs.icxo.com|utmccn=(referral)|utmcmd=referral|utmcct=/forum-1747-5.html; cdb_frameon=yes; discuz_leftmenu_openids=_2020__1609__1606__1895__1610__1971__1614__1963__1843__1842__1960__120__91_; __utmc=157472290; depot_oldtopics=D184688D; depot_fid2020=1243407821; cdb_oldtopics=D252981D251588D235177D254192D253933D193395D253905D253938D246286D254168D254442D254439D254441D254481D253414D254461D; cdb_fid133=1243385531; cdb_supe_refresh_items=0_500179_490608_497018_495801_457322_495264; cdb_fid51=1243410580";
            for (int id = 25359; id < 150000; id++)// 128400
            {
                Thread.Sleep(1000);//
                _url = url.Replace("{ID}", id.ToString());
                Common.Show(SyncHttpRequest.Get(_url, _data.Replace('$', GetRandomChar()), _encoding, _url,null,null, null));
            }
     }
        public static void TestCSDN()
        {
            string url = "http://remark2.csdn.net/CSDN_RemarkView.aspx?s=f5acec92-3be6-4494-961c-86c46a335615&c={ID}&style=http%3a%2f%2fremark2.csdn.net%2fdemo%2fcsdnnews%2frview.css&t=Google%u5c06%u51fa%u5e2dGDC+%u8fdb%u519b%u6e38%u620f%u4e1a%uff1f&Tags=+%2c+gdc%2c%u6e38%u620f%u4e1a%2c%u6e38%u620f%2c%u53d1%u5e03%u4f1a%2cgoogle%2c+%2c+&Charset=utf-8&RandomImageWidth=90&RandomImageHeight=25&RandomImageFontSize=15&RemarkCountElemId=remark_count1%7cremark_count2&p_iframe_id=REMARK_IFRAME_ID_0&UrlReferrer=http%3a%2f%2fnews.csdn.net%2fn%2f20090318%2f124226.html&Anthem_CallBack=true";
            string _cookies = "ASP.NET_SessionId=sw0m0045fn5m5a550j5swqya";
            string _data = "Anthem_UpdatePage=true" +
                //"&__VIEWSTATE=/wEPDwUJNDk4NTU1NTE5D2QWAgIBD2QWAgIBD2QWBmYPFgIeCWlubmVyaHRtbAUBMWQCAQ8WAh4EaHJlZgWTBS9yL2Y1YWNlYzkyM2JlNjQ0OTQ5NjFjODZjNDZhMzM1NjE1MTI0MjI2LmFzaHg/aWFucGFybT1Oa016T1RBeFF6VXdNelUzUkRZNU1qZzVNRGczT1RZd05URkZNRUl4UmpRNU56a3dRVU0xTjBJelJrUXlNemc1UmpneVF6a3pRMEUzT0RCRE5qQTJPVU14T1RRNU56azBNREUzTTBRd1JqSkVSVGhHTkRSQlJVVTFNRFpGUmtRd04wVTFOVFpHUmpnMlJURTBRak5EUkVZMk5UTTJRVU5HTlVKRU1Ua3lRamhGTmtZMU1VTkJNVVJHUmpFM1F6WXdNemd6Umprd09FTTJRVUkzT1RnME1UUkJPRGswT1RCR1JUSTFORGRHTkVReE5VWTJRekF3T0RVd05UWXpNall6TlVWRlJqZzJRalV3UVVSRVJqVkZNekF6TkRVd1FVVTNOemhHUlVNeE0wSXdPVFl4UVVWQk1ERkdRVEV6T0VaR05FRTFNVUV3TTBNM09VVXlRVEV4T0RJd1F6QkdSRUl5TkRVMlFVSTFRVGc1TlVKQk9UZzBNRVV5T1VVME5EZEVPRUpHTWprM05VRXhRVEkyTXpkRlFrSXpPVVEzUWtVME5VRkJOVUV3UXpRM01UazJNa1JHTWpkRk5EUTNRelk0UmpZelFUZ3pORFk0UkVJeVFqTkRPVGxDUkRFM05FVkJSVU01UWpCQlJFTXpNREF5TlRFeVFqVkZNRVkxUVVJMVFqRTRNVVE1T0VRME4wSTNPRGhETXpaR1FqSkJNamxDUkRsR1JVSkdSRUUxTWpFNVJFVkZOVGcxTWpnMFJVWTNOREExUkRnMFEwSTBSRUV6TWtWRVJnPT09PT09ZAIEDw8WBB4ISW1hZ2VVcmwFKC9BcHBfVGhlbWVzL0RlZmF1bHQvSW1hZ2VzL3IvYnRuUG9zdC5naWYeFkltYWdlVXJsRHVyaW5nQ2FsbEJhY2sFKS9BcHBfVGhlbWVzL0RlZmF1bHQvSW1hZ2VzL3IvYnRuUG9zdF8uZ2lmZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFDElEX0J0blN1Ym1pdM7C1WENP8gksR 3Dwvb041/LpTY" +
            "&ID_Title={T}&ID_Content={C}" +
            "&ID_BtnSubmit.x=34&ID_BtnSubmit.y=21" +
                "&__EVENTTARGET=ID_BtnSubmit&__EVENTARGUMENT=";
            string t = "上$海$【IT$技$术$行$业】$联$盟";//
            string c = "好！！！\r\n上$海【IT$技$术$行$业】联$盟 \r\n技$术$开$发$M$SN$群 group$58$12$9@m$sn$zone.$cn \r\n创$业$融$资$群 group$80$80$4@m$sn$zone.$cn \r\n单$身$白$领$V$IP$俱$乐$部/夜$生$活$群 group$47$67$6@m$sn$zone.$cn \r\n 搜$索$引$擎$数$据$挖$掘$语$义$分$析group$45$32$6@m$sn$zone.$cn\r\n【请$将$群$号$中$的$符号$去$掉！$！】";
            _data = _data.Replace("{T}", t).Replace("{C}", c);
            Encoding _encoding = Encoding.GetEncoding("utf-8");//
            string _url;
            for (int id = 124712; id < 150000; id++)// 128400
            {
                Thread.Sleep(1000);//
                _url = url.Replace("{ID}", id.ToString());
                SyncHttpRequest.Post(_url, _data.Replace('$', GetRandomChar()), false, _cookies, _encoding, _url, null,null, null);
            }
        }
        private static string s = "`~!#$^&*-;:',.";
        private static char GetRandomChar()
        {
            return s[new Random().Next(0, s.Length)];//
        }
        public static void TestTianya()
        {
            string url = "http://post.tianya.cn/new/Publicforum/content_submit.asp?idwriter=13368091&key=147646719&flag=1#Bottom";
            string _cookies = "vjuids=-13bce1b9.11da8cedce8.0.33dc4f6abd2078; vjlast=1226897809,1228197160,11; user=w=%u4F18%u4F18%u5148&id=13368091; isCategoryNav=false; __cid=2; temp=k=147646719&s=&t=1228225411&b=0C4E52B251CC83CF499DE1ECB5DC0BF3&ct=1228197207";
            //string data = "q=qq&strItem=no11&strTitle=%5B%C3%C0%C8%DD%C3%C0%B7%A2%5D%CF%EB%C2%F2%D2%BB%BF%EE%BA%C3%D3%C3%B5%C4%B7%DB%B5%D7%D5%DA%B8%C7%BA%DA%D1%DB%C8%A6%A3%AC%D3%D0%C3%BB%D3%D0%BA%C3%B5%C4%CD%C6%BC%F6%C4%D8%A3%BF%A3%BF&idArticle={ID}&flag=1&idSign=1&tAuthor=%D3%C5%D3%C5%CF%C8&chrAuthor=%D3%C5%D3%C5%CF%C8&ckey=147646719&cidWriter=13368091&Submit=Response&strContent={C}&PicDesc=&strAlbumPicURL=http%3A%2F%2F&MyBlog=";
            string data = "strContent={C}&strItem=no11&strTitle={T}&idArticle={ID}&flag=1&idSign=1&tAuthor=%C7%A7%E7%FA%E7%EA%B4%A8&chrAuthor=%C7%A7%E7%FA%E7%EA%B4%A8&Submit=Response&PicDesc=&strAlbumPicURL=http%3A%2F%2F&ckey=147646719&cidWriter=13368091&MyBlog=";
            string t = "上海【IT技术】联盟";//
            string c = "好！！！\r\n上海【IT技术】联盟-Castle/Nhibernate/IBatisNet技术开发MSN群 group58129#msnzone.cn \r\n创业融资群 group80804@msnzone.cn \r\n单身白领VIP俱乐部/夜生活群 group47676@msnzone.cn";
            data = data.Replace("{T}", t).Replace("{C}", c);
            Encoding _encoding = Encoding.GetEncoding("gb2312");//
            string _url;
            string _data;
            string refer = "http://cache.tianya.cn/publicforum/content/no11/1/644298.shtml";
            for (int id = 644298; id < 644299; id++)
            {
                _url = url.Replace("{ID}", id.ToString());
                _data = data.Replace("{ID}", id.ToString());
                SyncHttpRequest.Post(_url, _data, false, _cookies, _encoding, refer,null,null, null);
            }
        }
        public static void TestZhidao()
        {
            string url = "http://zhidao.baidu.com/q";
            string _cookies = "BAIDUID=ACC16D4838DB3EEE4265807BEDB824FB:FG=1; BD_UTK_DVT=1; BDSTAT=1ac2fb9a0f822296d394b95aed33a3d5f703918fb1ec08fa553d26975bee242a; BDTIPTYPE=none; BDTIPID=32-42-52-; BDUSS=Hd1YUNFZElZc1l5Z3FIYTgxdnFYVldrTWZCdDQyekUxbUhyY2ZFUUQ0bDZCVnRKQXdBQUFBJCQAAAAAAAAAAAEAAABdKL0DuuzUxvC9zvMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHp4M0l6eDNJb; IM_old=0|fo8pxpy4";
            string data = "ct=22&cm=100009&tn=ikreplysubmit&qid={ID}&cid=147&lu=http%3A%2F%2Fzhidao.baidu.com%2Fquestion%2F{ID}.html%3Ffr%3Didnw&co={C}&sn=&md=1";
            string t = "上海【IT技术】联盟";//
            string c = "\r\n\r\n上海IT技术联盟-技术开发MSN群 group58129@msnzone.cn \r\n 创业融资MSN群 group80804@msnzone.cn\r\n  单身白领VIP俱乐部夜生活MSN群 group47676@msnzone.cn \r\n\r\n";
            data = data.Replace("{T}", t);//.Replace("{C}", c);
            Encoding _encoding = Encoding.GetEncoding("gb2312");//
            string _url;
            string _data;
            for (int id = 77727397; id < 79727307; id++)
            {
                _url = url.Replace("{ID}", id.ToString());
                _data = data.Replace("{ID}", id.ToString());
                _data = _data.Replace("{C}", ChineseCode.GetRegionChineseString(40) + c + ChineseCode.GetRegionChineseString(40));
                SyncHttpRequest.Post(_url, _data, false, _cookies, _encoding, _url, null,null, null);
                Console.WriteLine("--------------------->ID:" + id);//
                Thread.Sleep(5000);//
            }
        }
        public static void TestMop()
        {
            string url = "http://bbs.news.mop.com/post.php?action=newthread&fid=161&extra=page%3D1&topicsubmit=yes";
            string _cookies = "mop_uniq_ckid=116.232.22.237_1230175053_1352693636; _utc=sFtWSt; mop_auto_login=forever1980%7C92bdcf2e108d38d3279d757fb0f290e1%7C0%7C7764ACF9A723995D1AB7ACEF69572C5E; mop_logon=43996703%7C%C3%BD%BD%E9%B4%EF%C8%CB%7Cforever1980%7C20081225132334%7C94A18670995C620FD052C92F9FD2B683; mop_status=54O50O124O55O51O50O46O50O50O46O50O51O50O46O54O49O49O124O20982O20013O124O24120O27491O124O48O124O54O56O124O48O56O57O49O114O101O118O101O114O111O102O124O20154O36798O20171O23186; cnzz_a1159759=0; vw1159759=%3A18952465%3A; sin1159759=http%3A//www.mop.com/%3Fticket%3DST-44631-ceX4gxEf3mRmwJcmRBczRuf99UIJe2C4idI-20; rtime=0; ltime=1230174966199; cnzz_eid=44080830-1230175081-http%3A//www.mop.com/%3Fticket%3DST-44631-ceX4gxEf3mRmwJcmRBczRuf99UIJe2C4idI-20; cdb_sid=9PnGPN; cdb_auth=emtcIRhmkf83iR6BO3PuErUs; _oft=108-12-25-11-16-36-866-31214178; _ort=108-12-25-15-39-6-838; _orccc=40; __utma=247464916.731487476.1230174998.1230184904.1230190565.3; __utmc=247464916; __utmz=247464916.1230174998.1.1.utmccn=(direct)|utmcsr=(direct)|utmcmd=(none); user_status=43996703|86|0|0|0|0|0|1|0|0|0|0|0|0|0|1|0|; dzh_bg_color=%23DBEAFF; __utma=47770683.1885833562992171300.1230175261.1230175261.1230182500.2; __utmc=47770683; __utmz=47770683.1230182500.2.2.utmccn=(referral)|utmcsr=dzh.mop.com|utmcct=/static/listSubLeft_0_0.html|utmcmd=referral; col_ad=9133539_43996703; rest_ad_collect9133539_49_43996703=1230182637826; __utmb=247464916";
            string t = "上海$【IT$技术$行业】$联盟";//
            string c = "好！！！\r\n上海【IT$技术$行业】联盟-Castle/Nhibernate/IBatisNet技术$开发$MSN群 group$58129@msn$zone.cn \r\n创业$融资$群 group$80804@msn$zone.cn \r\n单身$白领$VIP$俱乐部/夜生活$群 group$47676@msn$zone.cn 请将群号中的美元符号$去掉！！";

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("formhash", "fe97887e");//
            data.Add("fid", "{FID}");//
            data.Add("subject", t);//
            data.Add("message", c);//
            data.Add("attachdesc[]", "");//
            data.Add("attach[]", "");//
            data.Add("wysiwyg", "1");//
            Encoding _encoding = Encoding.GetEncoding("gb2312");//
            string _url;
            string _data;
            string refer = "http://bbs.news.mop.com/post.php?action=newthread&fid=161&extra=page%3D1";
            for (int id = 161; id < 162; id++)
            {
                _url = url.Replace("{FID}", id.ToString());
                _data = Common.KVToString(data).Replace("{FID}", id.ToString());
                // _data = Build_MultiPartFormData(data).Replace("{FID}", id.ToString());
                SyncHttpRequest.Post(_url, _data, true, _cookies, _encoding, refer, null,null, null);
            }
        }
        public static void TestQQGroupEmail()
        {
            string url = "http://m326.mail.qq.com/cgi-bin/groupmail_send?sid=ODMzNjYyMjU1MDE1MTczMDI3362505707,zh_CN";
            string _cookies = "sid=284f54df3f45d3168ce03d5feff893ad; username=362505707; qqmail_alias=362505707@qq.com; qqmail_sid=284f54df3f45d3168ce03d5feff893ad; qqmail_username=362505707; qqmail_domain=http://m326.mail.qq.com; CCSHOW=0000; edition=4m326.mail.qq.com; exstype=2; UM5K=5b36bdfa0c9783033b9223284dfaac24";//
            string t = "上海$【IT$技术$行业】$联盟";//
            string c = "好！！！\r\n上海【IT$技术$行业】联盟-Castle/Nhibernate/IBatisNet技术$开发$MSN群 group$58129@msn$zone.cn \r\n创业$融资$群 group$80804@msn$zone.cn \r\n单身$白领$VIP$俱乐部/夜生活$群 group$47676@msn$zone.cn 请将群号中的美元符号$去掉！！";

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("sid", "ODMzNjYyMjU1MDE1MTczMDI3362505707,zh_CN");//
            data.Add("actiontype", "send");//
            data.Add("priority", "");//
            data.Add("contenttype", "html");//
            data.Add("savesendbox", "");//
            data.Add("sendname", "alias");//
            data.Add("qqshow", "");//
            data.Add("fmailid", "");//
            data.Add("fattachlist", "");//
            data.Add("rsturl", "");//
            data.Add("fileidlist", "");//
            data.Add("verifycode", "");//
            data.Add("t", "");//
            data.Add("s", "group");//
            data.Add("hitaddrbook", "");//
            data.Add("backurl", "");//
            data.Add("mailtype", "");//
            data.Add("b83e98f717a0bbc0cfbdc11c6aa6463e", "04ca43933aa12da7cd6ea18e2d90bc04");//
            data.Add("groupname", "QQ群");//
            data.Add("qqgroupid", "29294557@groupmail.qq.com");//
            data.Add("subject", t);//
            data.Add("votesubject", "");//
            data.Add("option", "");//
            data.Add("Uploader0", "");//
            data.Add("content", c);//
            Encoding _encoding = Encoding.GetEncoding("gb2312");//
            string _url;
            string _data;
            string refer = "http://m326.mail.qq.com/cgi-bin/grouplist?sid=ODMzNjYyMjU1MDE1MTczMDI3362505707,zh_CN&t=compose_group&backurl=";
            for (int id = 161; id < 162; id++)
            {
                _url = url.Replace("{FID}", id.ToString());
                _data = Common.KVToString(data).Replace("{FID}", id.ToString());
                // _data = Build_MultiPartFormData(data).Replace("{FID}", id.ToString());
                SyncHttpRequest.Post(_url, _data, true, _cookies, _encoding, refer, null,null, null);
            }
        }
        public static void TestQQGroup()
        {
            string url = "http://qun.qq.com/1.1/sresult/?cl=&cnum=0&tx=%25E7%25BE%25A4&pg={P}&st=0&c1=0&c2=0&c3=0&_=1231319241962";
            JsonParser parser;
            StringReader rdr;
            JsonObject obj;
            IJsonArray kv;//
            for (int p = 1; p < 20000; p++)
            {
                rdr = new StringReader(SyncHttpRequest.Get(url.Replace("{P}", p.ToString()),null, Encoding.UTF8,null,null,null,null));//
                parser = new NetServ.Net.Json.JsonParser(rdr, true);

                obj = parser.ParseObject();
                //obj2 = obj.Values as NetServ.Net.Json.JsonArray; //
                kv = (obj["response"] as IDictionary<string, IJsonType>)["results"] as IJsonArray;//
                //Show(kv);
            }
        }
        
      public static AsyncHttpRequest xx = new AsyncHttpRequest();
       static MAIN()
        {
            xx.DataArrival += new DataArrivalEventHandler(xx_DataArrival);
            xx.DataComplete += new DataCompleteEventHandler(xx_DataComplete);
            //xx.Beginning += new RequestEventHandler(xx_Beginning);
        }

       static void xx_Beginning(object sender, HttpWebRequest req)
       {
           
       }
       public static void TestQQGroup2(string[] keys)
       {
           foreach (string key in keys) { TestQQGroup2(key); }
       }
       public static void TestQQGroup2(string key)
        {
       // http://qun.qq.com/?w=a&c=index&a=sresult&cl=&cnum=0&tx=ip&pg=2&st=0&c1=0&c2=0&c3=0&_=1238680604173
            string url = "w=a&c=index&a=sresult&cl=&cnum=0&tx={KEY}&pg={P}&st={ST}&c1=0&c2=0&c3=0&_=1231319241962";//http://qun.qq.com/1.1/sresult/?
            url = url.Replace("{KEY}", System.Web.HttpUtility.UrlEncode(key));//
            string _url;//
            TimeSpan ts;
            //string str;
            ShowLog(string.Format("----------------开始 {0}----------------------", key)); 
            for (int j = 0; j < 4; j++)
            {
                 _url = url.Replace("{ST}", j.ToString());//
                 for (int p = 1; p < 101; p++)
                {
                   //Show(string.Format("----------------{0} {1} {2} {3}----------------------", key, j, 101, p));
                    //str = Com.Zfrong.Common.Common.Http.HttpRequest.Get(_url.Replace("{P}", p.ToString()), Encoding.UTF8);//
                    //DO(str);//
                   
                    Sleep += 1;
                    while (xx.Working > 200)
                    {
                        ts = DateTime.Now - xx.StartTime;
                        ShowLog(string.Format("\t\t\tSleep:{0} Work:{1} Finish:{2} Err:{3} Len:{4} SP:{5} {6} C:{7} E:{8} SP2:{9} {10}", Sleep, xx.Working, xx.Finished, xx.Error, xx.Length,
                            xx.Length / (1024 * ts.TotalSeconds), (xx.Finished + xx.Error) / ts.TotalSeconds, Inserted, Existed,
                           0,0// Common.NetworkSpeed[0],Common.NetworkSpeed[1]
                            ));
                       
                        Thread.Sleep(500);
                    }
                    Sleep -= 1;
                   xx.Get("http://qun.qq.com/", _url.Replace("{P}", p.ToString()), Encoding.UTF8,
                         null,null,null,null, null,new string[]{  _url,p.ToString(),_url.Replace("{P}", p.ToString())});//
                }
            }
            ShowLog(string.Format("----------------完成 {0}----------------------", key));
            url = null; _url = null;
           f.RunT();//
        }
       public  static int Sleep = 0;
       static int Inserted = 0;
       static long Existed = 0;
       static void xx_DataComplete(object sender, DataCompleteEventArgs e)
       {
           DO(e);//
          // f.RunT();//
       }
       private static void xx_DataArrival(object sender, DataArrivalEventArgs e)
       {
           //f.RunT();//
       }
       private static void DO(DataCompleteEventArgs e)
       {
           ShowLog(string.Format("完成-->耗时(ms):{0}", e.TimeSpan));
          DO(e.RecievedData);//
       }
       private static void DO(string str)
       {
           //GC.Collect();
           JsonParser parser;
           StringReader rdr;
           JsonObject obj;
           IJsonArray kv;//%25E7%25BE%25A4
           //if (str == null || str.Length < 100)
           //{
           //    p--; continue;//
           //}
           //ShowLog("解析...开始" + str.Length);
           rdr = new StringReader(str);//
           try
           {
               parser = new NetServ.Net.Json.JsonParser(rdr, true);
               obj = parser.ParseObject();
               rdr.Dispose(); rdr = null;
               parser.Close(); parser = null;

           }
           catch (Exception ex)
           {
               ShowLog(ex.Message);
               return; //continue; 
           }
           //ShowLog("解析...完成" + str.Length);
           //int p = int.Parse(f[1]);//
           //if (p == 1)
           //{
           //    dict[f[0].GetHashCode()] = int.Parse(((obj["response"] as IDictionary<string, IJsonType>)["numFound"] as IJsonString).Value) / 10 + 1;//
           //    if (dict[f[0].GetHashCode()] > 101) dict[f[0].GetHashCode()] = 101;//
           //}
           //if (dict.ContainsKey(f[0].GetHashCode()))
           //{
           //    if (p == dict[f[0].GetHashCode()])
           //        dict.Remove(f[0].GetHashCode());//
           //}
          // ShowLog("DB...开始");
           if (obj.ContainsKey("response"))
           {
               if ((obj["response"] as IDictionary<string, IJsonType>).ContainsKey("results"))
               {
                   kv = (obj["response"] as IDictionary<string, IJsonType>)["results"] as IJsonArray;//
                   Show(kv);
                   //data.Remove(f[2].GetHashCode());//
               }
           }
           //ShowLog("DB...结束"); 
       }
    private static void Show( IJsonArray obj)
        {
            IDictionary<string, string> data; Thread thread;
            foreach (IJsonType t in obj)
            {
                data=new Dictionary<string,string>();
                //Show("Foreach Data...");
                foreach (KeyValuePair<string, IJsonType> kv in t as IDictionary<string, IJsonType>)
                {
                     data.Add(kv.Key, (kv.Value as JsonString).Value);//
                }
                data["TI"] = System.Text.RegularExpressions.Regex.Replace(data["TI"], "<font color=#C60A00>|</font>", "");//
                data["TX"] = System.Text.RegularExpressions.Regex.Replace(data["TX"], "<font color=#C60A00>|</font>", "");//
                
                thread = new Thread(new ParameterizedThreadStart(Save));
                thread.IsBackground = true;
                thread.Start(data);//
            }
            obj.Clear(); obj = null; 
        }
    private static void Save(object data)
    {
        Save(data as Dictionary<string, string>);
    }
    private static void Save(Dictionary<string, string> data)
    {
        QQGroup qq; object v;
        //Show("DB...开始");
        v =Logic.ExecuteScalar<QQGroup>("select count(IDID) from QQGroup WHERE MD='" + data["MD"] + "'");
        //Show("DB...结束"); 
        if ((int)v == 0)// !DB<QQGroup>.Exists("MD="+data["MD"]))
        {
            qq = new QQGroup();
            ShowLog("Create:" + data["TI"]);
            qq.SetProperties(data);//
            qq.UR = "";//
            DB<QQGroup>.CreateAndFlush(qq);//
            Inserted++;//
            qq.Dispose(); qq = null;//
            data.Clear();data = null;
        }
        else
        {
            Existed++;
           // ShowLog("Exist:" + data["TI"]);
        }
    }
    
    public static IForm f;
    private static void ShowLog(string str)
    {
        f.ShowLog(str); 
    }
}
    [ActiveRecord]
    public class KeyWord : Base
    {
        [PrimaryKey]
        public virtual int IDID
        {
            get { return Get<int>("IDID"); }

            set { Set<int>("IDID", value); }
        }
        [Property(Length = 25)]
        public virtual string Name
        {
            set { Set<string>("Name", value); }
            get { return Get<string>("Name"); }
        }
        [Property]
        public virtual int PCount
        {
            set { Set<int>("PCount", value); }
            get { return Get<int>("PCount"); }
        }
    }
    [ActiveRecord]
    public class QQGroup : Base
    {
        [PrimaryKey]
        public virtual int IDID
        {
            get { return Get<int>("IDID"); }

            set { Set<int>("IDID", value); }
        }
        [Property]
        public virtual string RQ
        {
            set { Set<string>("RQ", value); }
            get { return Get<string>("RQ"); }
        }
        [Property]
        public virtual string MD
        {
            set { Set<string>("MD", value); }
            get { return Get<string>("MD"); }
        }
        [Property]
        public virtual string ID
        {
            set { Set<string>("ID", value); }
            get { return Get<string>("ID"); }
        }
        [Property]
        public virtual string XT
        {
            set { Set<string>("XT", value); }
            get { return Get<string>("XT"); }
        }
        [Property]
        public virtual string DT
        {
            set { Set<string>("DT", value); }
            get { return Get<string>("DT"); }
        }
        [Property]
        public virtual  string TI
        {
            set { Set<string>("TI", value); }
            get { return Get<string>("TI"); }
        }
        [Property]
        public virtual  string UR
        {
            set { Set<string>("UR", value); }
            get { return Get<string>("UR"); }
        }
        [Property]
        public virtual  string QQ
        {
            set { Set<string>("QQ", value); }
            get { return Get<string>("QQ"); }
        }
        [Property]
        public virtual  string TA
        {
            set { Set<string>("TA", value); }
            get { return Get<string>("TA"); }
        }
        [Property]
        public virtual  string CL
        {
            set { Set<string>("CL", value); }
            get { return Get<string>("CL"); }
        }
        [Property]
        public virtual  string GA
        {
            set { Set<string>("GA", value); }
            get { return Get<string>("GA"); }
        }
        [Property]
        public virtual  string GB
        {
            set { Set<string>("GB", value); }
            get { return Get<string>("GB"); }
        }
        [Property]
        public virtual  string GC
        {
            set { Set<string>("GC", value); }
            get { return Get<string>("GC"); }
        }
        [Property]
        public virtual  string GD
        {
            set { Set<string>("GD", value); }
            get { return Get<string>("GD"); }
        }
        [Property]
        public virtual  string GE
        {
            set { Set<string>("GE", value); }
            get { return Get<string>("GE"); }
        }
        [Property]
        public virtual  string GF
        {
            set { Set<string>("GF", value); }
            get { return Get<string>("GF"); }
        }
        [Property]
        public virtual  string GH
        {
            set { Set<string>("GH", value); }
            get { return Get<string>("GH"); }
        }
        [Property]
        public virtual  string GI
        {
            set { Set<string>("GI", value); }
            get { return Get<string>("GI"); }
        }
        [Property]
        public virtual  string GJ
        {
            set { Set<string>("GJ", value); }
            get { return Get<string>("GJ"); }
        }
        [Property]
        public virtual  string EX
        {
            set { Set<string>("EX", value); }
            get { return Get<string>("EX"); }
        }
        [Property]
        public virtual  string DX
        {
            set { Set<string>("DX", value); }
            get { return Get<string>("DX"); }
        }
        [Property(Length=600)]
        public virtual  string TX
        {
            set { Set<string>("TX", value); }
            get { return Get<string>("TX"); }
        }
    }

   
    [ActiveRecord]
    public class ZipCode : Base2
    {
        [PrimaryKey("ID")]
        public virtual int IDID
        {
            get;

            set;
        }
        [Property("City")]
        public virtual string City
        {
            set;
            get;
        }
        [Property("Address")]
        public virtual string Address
        {
            set;
            get;
        }
        [Property]
        [Display("状态")]
        public virtual byte State
        {
            set;
            get;
        }
    }
}
