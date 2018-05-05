
using System;
using System.Collections;
using System.Text;
using System.Web;
using chinapaysecure;
namespace Com.Zengfr.Proj.Common.Pay.ChinaPay
{
    /// <summary>
    /// 0002企业网银支付
    /// </summary>
    public class ChinaPayGetwayB : ChinaPayGetway
    {
        public ChinaPayGetwayB()
        {
            this.MerchantCode = "481211601130001"; //商户号B2B
            this.TranType = "0002";

            this.SecurityFile = @"/Properties/securityB2B.properties";

            this.PrivateKeyPath = "/certs/ChinaPay/kuaizhuizfr201603121448.pfx";
            this.PrivateKeyPwd = "zengfanrong001";
        }
    }
    /// <summary>
    /// 0001个人网银支付
    /// </summary>
    public class ChinaPayGetwayC : ChinaPayGetway
    {
        public ChinaPayGetwayC()
        {
            this.MerchantCode = "481211601130002"; //商户号B2C
            this.TranType = "0001";

            this.SecurityFile = @"/Properties/securityB2C.properties";

            this.PrivateKeyPath = "/certs/ChinaPay/chinapay481211601130002.pfx";
            this.PrivateKeyPwd = "306581";
        }
    }
    /// <summary>
    /// 0004快捷支付
    /// </summary>
    public class ChinaPayGetwayKJ : ChinaPayGetway
    {
        public ChinaPayGetwayKJ()
        {
            this.MerchantCode = "481211601130003"; //无卡
            this.TranType = "0004";

            this.SecurityFile = @"/Properties/securityKj.properties";

            this.PrivateKeyPath = "/certs/ChinaPay/kuaizhuizfr201603202049.pfx";
            this.PrivateKeyPwd = "zengfanrong001";
        }
    }
    /// <summary>
    /// 0006认证支付
    /// </summary>
    public class ChinaPayGetwayRZ : ChinaPayGetway
    {
        public ChinaPayGetwayRZ()
        {
            this.MerchantCode = "481211601130003"; //无卡
            this.TranType = "0006";

            this.SecurityFile = @"/Properties/securityKj.properties";

            this.PrivateKeyPath = "/certs/ChinaPay/kuaizhuizfr201603202049.pfx";
            this.PrivateKeyPwd = "zengfanrong001";
        }
    }
    public abstract partial class ChinaPayGetway
    {
        public static ChinaPayGetway Create(string tranType)
        {
            ChinaPayGetway chinaPayGetway = null;
            switch (tranType)
            {
                case "0001":
                    chinaPayGetway = new ChinaPayGetwayC();
                    chinaPayGetway.Init();
                    break;
                case "0002":
                    chinaPayGetway = new ChinaPayGetwayB();
                    chinaPayGetway.Init();
                    break;
                case "0004":
                    chinaPayGetway = new ChinaPayGetwayKJ();
                    chinaPayGetway.Init();
                    break;
                case "0006":
                    chinaPayGetway = new ChinaPayGetwayRZ();
                    chinaPayGetway.Init();
                    break;
                default:
                    break;
            }
            return chinaPayGetway;
        }
    }
    public abstract partial class ChinaPayGetway
    {
        protected virtual string SecurityFile { get; set; }
        protected virtual string MerchantCode { get; set; }
        protected virtual string TranType { get; set; }

        protected virtual string PrivateKeyPath { get; set; }
        protected virtual string PrivateKeyPwd { get; set; }

        private SecssUtil secssUtil = new SecssUtil();

        protected static Random random = new Random();
        public string CreatePayOrderId()
        {
            var payOrderId = string.Format("{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), random.Next(100000, 999999));
            return payOrderId;
        }
        public bool Init()
        {
            var result = secssUtil.init(System.Web.HttpContext.Current.Server.MapPath(this.SecurityFile));
            return result;
        }
        public string decryptData(string data, bool decrypt)
        {
            if (decrypt)
            {
                secssUtil.decryptData(data);
                data = secssUtil.getDecData();
            }
            return data;
        }
        public string encryptData(string data, bool encrypt)
        {
            if (encrypt)
            {
                secssUtil.encryptData(data);
                data = secssUtil.getEncValue();
            }
            return data;
        }
        public string VerifyRtnErrCode(Hashtable myMap)
        {
            secssUtil.verify(myMap);
            var result = secssUtil.getErrCode();
            return result;
        }
        public string Pay(HttpContextBase httpContext, string merOrderNo, decimal? orderAmt, string commodityMsg, ChinaPayMerResv chinaPayMerResv)
        {
            string payUrl = "https://payment.chinapay.com/CTITS/service/rest/page/nref/000000000017/0/0/0/0/0";//1.	前台交易
            //payUrl = "https://payment.chinapay.com/CTITS/service/rest/forward/sta/000000000017/0/0/0/0/0";//2.	后台交易
            var scheme = httpContext.Request.Url.Scheme;
            var authority = httpContext.Request.Url.Authority;

            Hashtable myMap = new Hashtable();
            myMap.Add("MerId", this.MerchantCode);
            myMap.Add("MerOrderNo", merOrderNo);
            myMap.Add("TranDate", DateTime.Now.ToString("yyyyMMdd"));//交易日期
            myMap.Add("TranTime", DateTime.Now.ToString("HHmmss"));//交易时间
            myMap.Add("OrderAmt", orderAmt.Value.ToString("#0"));
            myMap.Add("TranType", this.TranType);
            myMap.Add("BusiType", "0001");
            myMap.Add("MerPageUrl", scheme + "://" + authority + "/Pay/PayResult");
            myMap.Add("MerBgUrl", scheme + "://" + authority + "/Pay/PayBackRcv");
            myMap.Add("CurryNo", "CNY");

            myMap.Add("RemoteAddr", HttpContext.Current.Request.UserHostAddress ?? string.Empty);
            //myMap.Add("BankInstNo", "700000000000002");

            myMap.Add("PayTimeOut", "145");
            myMap.Add("Version", "20140728");
            myMap.Add("CommodityMsg", HttpUtility.UrlDecode(commodityMsg, Encoding.UTF8));//用来描述购买商品的信息，ChinaPay原样返回

            var merResv = chinaPayMerResv.SerializeObject(true);

            myMap.Add("MerResv", merResv);//商户自定义，ChinaPay原样返回
            //myMap.Add("TranReserved", chinaPayMerResv.SerializeObject());//交易扩展域，JSON格式填写，商户交易请求内容原样返回 
            //myMap.Add("TranReserved", "{\"Referred\":\"www.chinapay.com\",\"BusiId\":\"0001\",\"TimeStamp\":\"1438915150976\",\"Remoteputr\":\"172.16.9.44\"}");  
            //交易扩展域，JSON格式填写，商户交易请求内容原样返回 

            /***********************/
            //坑1：这里注意，如果采用chinpay自带的签名，必须在服务器的浏览器上获取交易私钥，否则由开发环境转到生产环境会出错！！！此坑注意
            //chinapaysecure.SecssUtil su = new chinapaysecure.SecssUtil();
            //string path = Server.MapPath("/cer/security.properties");
            //log.Debug(path);
            //bool flag = su.init(path);
            //su.sign(myMap);
            //if ("00" != su.getErrCode())
            //{
            //    log.Error(su.getErrCode() + "=" + su.getErrMsg());
            //    ViewBag.ChinaPay = "签名过程发生错误，错误信息为-->" + su.getErrMsg();
            //    return View();
            //}
            //myMap.Add("Signature", su.getSign());
            /*******************************************/

            //以下为采用自己扩展的签名方法，无需关心测试还是生产环境的问题！！！
            string signValue = ChinaPayHelper.Sign(myMap, this.PrivateKeyPath, this.PrivateKeyPwd);

            myMap.Add("Signature", signValue);
            string sHtmlText = ChinaPayHelper.BuildRequest(myMap, payUrl);
            httpContext.Response.Write(sHtmlText);
            return sHtmlText;
        }
    }
}
