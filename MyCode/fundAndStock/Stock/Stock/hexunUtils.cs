
using System;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Common.Logging;

using NHibernate.Criterion;
using Entity;
using Spring.Rest.Utils;
using Common.Utils;

namespace  Stock.Utils
{
    /// <summary>
    /// http://zhibiao.hexun.com/DiagnoseStock/Diagnose_Stock_PartialView?stockcode=600125
    /// </summary>
    public class hexunUtils : RestTemplateUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(hexunUtils));
        /// <summary>
        /// 和讯指标云  指标诊股
        /// </summary>
        public static void zhibiao()
        {

        }
        public static void zhibiao(string stockcode)
        {
            var code = stockcode.Replace("sz", "").Replace("sh", "");
            var url = string.Format(@"http://zhibiao.hexun.com/DiagnoseStock/Diagnose_Stock_PartialView?stockcode={0}", code);

            var template = RestTemplateUtils.BuildRestTemplate(url);



            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        try
                        {
                            Parse_zhibiao(stockcode, r.Response.Body);
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("{0}", ex, string.Empty);
                        }
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }

        private static void Parse_zhibiao(string stockcode, string source)
        {
            Match m1 = Regex.Match(source, @"txt_zbzg_stock(.*?)value=""(.*?)""", RegexOptions);
            Match m2 = Regex.Match(source, "数据来源：(.*?)个交易类指标", RegexOptions);
            Match m3 = Regex.Match(source, @"<span class=""zg-font"">(.*?)</span>看多指标(.*?)前一日 (\d+)</p>", RegexOptions);
            Match m4 = Regex.Match(source, @"看多指标(.*?)<span class=""zg-font"">(.*?)</span>看空指标(.*?)前一日 (\d+)</p>", RegexOptions);

            Match m5 = Regex.Match(source, "交易类指标(.*?)数据来源：(.*?)个提示类指标", RegexOptions);

            Match m6 = Regex.Match(source, @"今日发出提示信号指标(.*?)zg-font"">(.*?)</span>", RegexOptions);
            Match m7 = Regex.Match(source, @"未来10个交易日上涨5%的概率(.*?)zg-font"">(.*?)%", RegexOptions);
            if (m1.Success && m2.Success && m3.Success)
            {
                var obj = new Stock和讯指标云();

                obj.stockcode = stockcode;
                obj.stockName = m1.Groups[2].Value;
                obj.effectDate =DateUtils.GetDateTimeNoWeekEnd(0);

                obj.DataChangeLastTime = DateTime.Now;
                obj.交易类看多指标数 = int.Parse(m3.Groups[1].Value);
                obj.交易类看空指标数 = int.Parse(m4.Groups[2].Value);
                obj.交易类指标数 = int.Parse(m2.Groups[1].Value);

                obj.交易类看多指标数变化量 = obj.交易类看多指标数 - int.Parse(m3.Groups[3].Value);
                obj.交易类看空指标数变化量 = obj.交易类看空指标数 - int.Parse(m4.Groups[4].Value);

                if (m5.Success && m6.Success && m7.Success)
                {
                    obj.提示类指标数 = int.Parse(m5.Groups[2].Value);
                    obj.今日发出提示信号指标数 = int.Parse(m6.Groups[2].Value);
                    obj.未来10个交易日上涨百分5的概率 = int.Parse(m7.Groups[2].Value);

                }
                var ff = ActiveRecordMediator<Stock和讯指标云>.FindOne(Restrictions.Eq("stockcode", obj.stockcode),
                        Restrictions.Eq("effectDate", obj.effectDate));

                if (ff != null)
                {
                    return;//
                    obj.Stock和讯指标云Id = ff.Stock和讯指标云Id;
                    ActiveRecordMediator<Stock和讯指标云>.Update(obj);
                }
                else
                {
                    ActiveRecordMediator<Stock和讯指标云>.Create(obj);
                }
                log.InfoFormat("指标诊股：{0}", stockcode);
            }
        }
        
    }
}
