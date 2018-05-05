
using System;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Common.Logging;

using NHibernate.Criterion;
using Entity;
using Spring.Rest.Utils;
using Common.Utils;

namespace Stock.Utils
{
    /// <summary>
    /// doctor.10jqka.com.cn//score/top/cateTop
    /// doctor.10jqka.com.cn//score/top/
    /// </summary>
    public class doctor10jqkaUtils : RestTemplateUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(doctor10jqkaUtils));

        /// <summary>
        /// 牛叉诊股-个股分析利器_同花顺金融网
        /// </summary>
        public static void GetStockScores(string stockCode)
        {
            var ocde = stockCode.Replace("sh", "").Replace("sz", "");
            string url = string.Format("http://doctor.10jqka.com.cn/{0}/", ocde);

            var template = RestTemplateUtils.BuildRestTemplate(url);


            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        ParseStock(stockCode, r.Response.Body);

                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });

        }

        private static object lockobj = new object();
        private static void ParseStock(string stockCode, string html)
        {
            var effectDate = GetDateValue(html);
            if (effectDate <= DateUtils.GetMinDateTime())
            {
                return;
            }
            lock (lockobj)
            {
                var item = ActiveRecordMediator<doctor10jqkascore>.FindOne(
                    Restrictions.Eq("stockCode", stockCode),
                    Restrictions.Eq("effectDate", effectDate)
                    );
                if (item == null)
                {
                    item = new doctor10jqkascore();
                }

                item.stockCode = stockCode;
                item.effectDate = effectDate;
                item.stockName = GetValue(html, @"<title>(.*?)\(", 1, "");
                item.zhScore = GetValue(GetValue(html, "综合诊断：(.*?)分 打败了(.*?)%的", 1, ""));
                item.zhScoreRate = GetValue(GetValue(html, "综合诊断：(.*?)分 打败了(.*?)%的", 2, ""));
                item.avgAmount = GetValue(GetValue(html, "近期的平均成本为<strong>(.*?)元</strong>", 1, ""));
                item.type = GetValue(html, "股价在成本(.*?)运行", 1, "");
                item.ylAmount = GetValue(GetValue(html, "股压力位为(.*?)元", 1, ""));
                item.zcAmount = GetValue(GetValue(html, "支撑位为(.*?)元", 1, ""));

                item.technicalScore = GetValue(GetValue(html, "<a>技术面诊股</a>(.*?)诊断结果(.*?)<em>(.*?)</em>", 3, ""));
                item.fundsScore = GetValue(GetValue(html, "<a>资金面诊股</a>(.*?)诊断结果(.*?)<em>(.*?)</em>", 3, ""));
                item.messageScore = GetValue(GetValue(html, "<a>消息面诊股</a>(.*?)诊断结果(.*?)<em>(.*?)</em>", 3, ""));
                item.tradeScore = GetValue(GetValue(html, "<a>行业面诊股</a>(.*?)诊断结果(.*?)<em>(.*?)</em>", 3, ""));
                item.basicScore = GetValue(GetValue(html, "<a>基本面诊股</a>(.*?)诊断结果(.*?)<em>(.*?)</em>", 3, ""));

                if (item.doctor10jqkascoreId > 0)
                   return;// ActiveRecordMediator<doctor10jqkascore>.Update(item);
                else
                    ActiveRecordMediator<doctor10jqkascore>.Create(item);
                log.InfoFormat("{0},{1}", item.fundsScore, item.effectDate);
            }
        }

        private static decimal GetValue(string vlaue)
        {
            decimal v = 0;
            decimal.TryParse(vlaue, out v);
            return v;
        }

        private static string GetValue(string source, string patern, int index, string defValue)
        {
            var m = Regex.Match(source, patern, RegexOptions);
            if (m.Success)
            {
                return m.Groups[index].Value;
            }
            return defValue;
        }

        private static DateTime GetDateValue(string source)
        {
            var m = Regex.Match(source, @"\[诊断日期:(.*?)年(.*?)月(.*?)日(.*?)\]", RegexOptions);
            if (m.Success)
            {
                return DateTime.Parse(string.Format("{0}-{1}-{2}", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value));
            }
            return DateTime.MinValue;
        }

    }
}
