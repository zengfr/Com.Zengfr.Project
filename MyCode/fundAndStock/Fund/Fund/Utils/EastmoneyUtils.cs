

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Common.Logging;
 
using MathNet.Numerics.Statistics;
using NHibernate.Criterion;
using Entity;
namespace Fund
{
    using System;
    using Common.Utils;
    using Spring.Rest.Utils;

    public class EastmoneyUtils : RestTemplateUtils
    {
        //http://fund.eastmoney.com/f10/jjjz_580002.html
        //http://fund.eastmoney.com/f10/F10DataApi.aspx?type=lsjz&code=580002&page=1&per=20&sdate=2015-04-07&edate=2015-04-10&rt=0.07360585900677896
        protected static ILog log = LogManager.GetLogger(typeof(MorningstarUtils));
        public static void F10DataApi(string code)
        {
            F10DataApi(code, DateTime.Today.AddMonths(-2), DateTime.Today, 1);
            //F10DataApi(code, DateTime.Today.AddMonths(-3), DateTime.Today, 2);
            //F10DataApi(code, DateTime.Today.AddMonths(-3), DateTime.Today, 3);
        }

        public static void F10DataApi(string code, DateTime start, DateTime end, int page)
        {

            string url =
                string.Format("http://fund.eastmoney.com/f10/F10DataApi.aspx?type=lsjz&code={0}&page={4}&per=100&sdate={1}&edate={2}&rt={3}",
                   code, start, end, Random.NextDouble(), page);

            var template = RestTemplateUtils.BuildRestTemplate(url);



            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        try
                        {
                            ParseFundRateDetail(code, r.Response.Body);
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

        private static List<FundRateDetail> ParseFundRateDetail(string fundCode, string source)
        {
            var rateDetails = new List<FundRateDetail>();
            var matches = Regex.Matches(source, @"<tr><td>(.*?)</td>(.*?)>(.*?)</td>(.*?)>(.*?)</td>(.*?)>(.*?)</td>(.*?)>(.*?)</td></tr>", RestTemplateUtils.RegexOptions);

            DateTime date;
            decimal v1;
            decimal v2;
            decimal v3;
            var minDate = DateUtils.GetMinDateTime();
            foreach (Match match in matches)
            {
                if (match.Success)
                {

                    DateTime.TryParse(match.Groups[1].Value, out date);
                    decimal.TryParse(match.Groups[3].Value, out v1);
                    decimal.TryParse(match.Groups[5].Value, out v2);
                    decimal.TryParse(match.Groups[7].Value.Trim('%').Trim(), out v3);

                    var f = new FundRateDetail();
                    f.FundCode = fundCode;
                    f.NavDate = date;
                    f.Nav = v1;
                    f.NavTotal = v2;
                    f.NavRate = v3;
                    log.InfoFormat("{0}:{1}\t{2}\t{3}\t{4}", f.FundCode, f.NavDate.ToString("yyyy-MM-dd"), f.Nav,
                        f.NavTotal, f.NavRate);
                    if (date > minDate)
                    {
                        rateDetails.Add(f);
                    }
                }
            }
            foreach (var f in rateDetails)
            {
                lock (lockObj)
                {
                    var ff = ActiveRecordMediator<FundRateDetail>.FindOne(Restrictions.Eq("FundCode", f.FundCode),
                        Restrictions.Eq("NavDate", f.NavDate));

                    if (ff != null)
                    {
                        if (ff.NavRate != f.NavRate)
                        {
                            ff.NavDate = f.NavDate;
                            ff.Nav = f.Nav;
                            ff.NavTotal = f.NavTotal;
                            ff.NavRate = f.NavRate;
                            ff.DataChangeLastTime = DateTime.Now;
                            ActiveRecordMediator<FundRateDetail>.Update(ff);
                        }
                    }
                    else
                    {
                        f.DataChangeLastTime = DateTime.Now;
                        ActiveRecordMediator<FundRateDetail>.Create(f);
                    }
                }
            }
            if (rateDetails.Count > 0)
            {
                BuildFundRateHistory(fundCode, rateDetails);
            }
            return rateDetails;

        }
        protected static void BuildFundRateHistory(string fundCode, List<FundRateDetail> rateDetails)
        {
            var today =  DateUtils.GetDateTimeNoWeekEnd(0);
            var startOfLastWeek = DateUtilities.GetStartOfLastWeek();
            var endLastWeek = DateUtilities.GetEndOfLastWeek().Date;
            var rds = rateDetails.OrderByDescending(t => t.NavDate);
            //本周
            BuildFundRateHistory(fundCode, rds, "w0", DateUtilities.GetStartOfCurrentWeek(), today);
            //本月
            BuildFundRateHistory(fundCode, rds, "m0", DateUtilities.GetStartOfCurrentMonth(), today);

            //最近7天
            BuildFundRateHistory(fundCode, rds.Take(3), "d3", DateTime.Today.AddDays(-33), today);
            BuildFundRateHistory(fundCode, rds.Take(5), "d5", DateTime.Today.AddDays(-33), today);
            BuildFundRateHistory(fundCode, rds.Take(7), "d7", DateTime.Today.AddDays(-33), today);

            //上周
            BuildFundRateHistory(fundCode, rds, "w", startOfLastWeek, endLastWeek);
            BuildFundRateHistory(fundCode, rds, "w", startOfLastWeek.AddDays(-7), endLastWeek.AddDays(-7));
            BuildFundRateHistory(fundCode, rds, "w", startOfLastWeek.AddDays(-7 * 2), endLastWeek.AddDays(-7 * 2));
            BuildFundRateHistory(fundCode, rds, "w", startOfLastWeek.AddDays(-7 * 3), endLastWeek.AddDays(-7 * 3));
            BuildFundRateHistory(fundCode, rds, "w", startOfLastWeek.AddDays(-7 * 4), endLastWeek.AddDays(-7 * 4));
            BuildFundRateHistory(fundCode, rds, "w", startOfLastWeek.AddDays(-7 * 5), endLastWeek.AddDays(-7 * 5));

            //上月
            BuildFundRateHistory(fundCode, rds, "m", DateUtilities.GetStartOfLastMonth(), DateUtilities.GetEndOfLastMonth());


        }

        protected static object lockObj = new object();
        protected static void BuildFundRateHistory(string fundCode, IEnumerable<FundRateDetail> rateDetails, string dateType, DateTime start, DateTime end)
        {
            var rds = rateDetails.Where(r => r.NavDate >= start && r.NavDate <= end);
            if (!rds.Any()) return;

            var t0OrdDateType = dateType.StartsWith("d") || dateType.EndsWith("0");

            FundRateHistory fundRateHistory = new FundRateHistory();
            fundRateHistory.FundCode = fundCode;

            fundRateHistory.NavDateType = dateType;
            fundRateHistory.NavStartDate = t0OrdDateType ? rds.Last().NavDate : start;
            fundRateHistory.NavEndDate = t0OrdDateType ? rds.First().NavDate : end;

            fundRateHistory.NavRate = rds.Sum(t => t.NavRate);

            DescriptiveStatistics stats = new DescriptiveStatistics(rds.Select(t => (double)t.NavRate));

            if (stats.StandardDeviation > 0)
            {
                var riskFreeReturn = 0.059;
                var mean = stats.Mean;
                var sharpeRatio = (mean - (riskFreeReturn / 12)) / stats.StandardDeviation * Math.Sqrt(12);

                fundRateHistory.StandardDeviation = stats.StandardDeviation;
                fundRateHistory.SharpeRatio = sharpeRatio;
            }

            lock (lockObj)
            {
                ICriterion[] exp = new ICriterion[]
                {
                    Restrictions.Eq("FundCode", fundRateHistory.FundCode),
                    Restrictions.Eq("NavDateType", fundRateHistory.NavDateType),
                    Restrictions.Eq("NavStartDate", fundRateHistory.NavStartDate),
                    Restrictions.Eq("NavEndDate", fundRateHistory.NavEndDate)
                };
                if (t0OrdDateType)
                {
                    exp = new ICriterion[]{
                    Restrictions.Eq("FundCode", fundRateHistory.FundCode),
                    Restrictions.Eq("NavDateType", fundRateHistory.NavDateType), };
                }
                var rh = ActiveRecordMediator<FundRateHistory>.FindOne(exp);
                if (rh != null)
                {
                    if (rh.NavRate != fundRateHistory.NavRate)
                    {
                        rh.NavRate = fundRateHistory.NavRate;
                        rh.DataChangeLastTime = DateTime.Now;
                        ActiveRecordMediator<FundRateHistory>.Update(rh);
                    }
                }
                else
                {
                    fundRateHistory.DataChangeLastTime = DateTime.Now;
                    ActiveRecordMediator<FundRateHistory>.Create(fundRateHistory);
                }

            }

        }


    }
}
