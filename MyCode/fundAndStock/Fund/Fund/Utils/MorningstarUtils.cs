
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common.Logging;
 
using Newtonsoft.Json;
using Entity;
namespace Fund
{
    using Castle.ActiveRecord;
    using Common.Utils;
    using NHibernate.Criterion;
    using Spring.Rest.Utils;

    public class MorningstarUtils : RestTemplateUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(MorningstarUtils));

        private static string GetVIEWSTATE(string source)
        {
            var match = Regex.Match(source, @"__VIEWSTATE(.*?)value=""(.*?)""", RestTemplateUtils.RegexOptions);
            if (match.Success)
            {
                return match.Groups[2].Value;
            }
            return string.Empty;
        }

        public static void GetMorningstarIds(int pagesize)
        {
            string url = "http://cn.morningstar.com/fundselect/default.aspx";

            var template = RestTemplateUtils.BuildRestTemplate(url);

            IDictionary<string, object> form = new Dictionary<string, object>(1);
            form.Add("__EVENTTARGET", "ctl00$cphMain$ddlPageSite");
            form.Add("__EVENTARGUMENT", "");
            form.Add("__LASTFOCUS", "");
            form.Add("__VIEWSTATE", "");
            form.Add("ctl00$cphMain$ddlPageSite", pagesize.ToString());

            var source = template.GetForObject<string>(url);
            form["__VIEWSTATE"] = GetVIEWSTATE(source);

            template.PostForMessageAsync<string>(url, form, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        ParseFundsAndMorningstarId(r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });


        }

        protected static List<Entity.Fund> ParseFundsAndMorningstarId(string source)
        {
            var list = new List<Entity.Fund>();
            var matches = Regex.Matches(source, @"<tr(.*?)span tag=""(.*?)""(.*?)<a href=""/quicktake/(.*?)""(.*?)>(.*?)</a></td>", RestTemplateUtils.RegexOptions);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var f = new Entity.Fund();
                    f.MorningstarId = match.Groups[4].Value;
                    f.FundCode = match.Groups[6].Value;
                    f.FundName = match.Groups[2].Value.Replace(f.MorningstarId + "|", "");
                    f.DataChangeLastTime = DateTime.Now;
                    log.InfoFormat("{0}:{1},{2}", f.FundCode, f.MorningstarId, f.FundName);

                    var ff = ActiveRecordMediator<Entity.Fund>.FindOne(Restrictions.Eq("FundCode", f.FundCode));

                    if (ff != null)
                    {
                        if (ff.MorningstarId != f.MorningstarId)
                        {
                            ff.MorningstarId = f.MorningstarId;
                            ff.DataChangeLastTime = DateTime.Now;
                            ActiveRecordMediator<Entity.Fund>.Update(ff);
                        }
                    }
                    else
                    {
                        f.成立日期 =  DateUtils.GetMinDateTime();
                        f.开放日期 =  DateUtils.GetMinDateTime();
                        f.FundNavRefreshTime = DateUtils.GetMinDateTime();
                        ActiveRecordMediator<Entity.Fund>.Create(f);
                    }

                }
            }
            return list;

        }

        public static void quicktake(string morningstarId)
        {
            var url = string.Format("http://cn.morningstar.com/quicktake/{0}", morningstarId); ;
            var template = RestTemplateUtils.BuildRestTemplate(url);
            template.GetForMessageAsync<string>(url, r =>
            {

                var ff = ActiveRecordMediator<Entity.Fund >.FindOne(Restrictions.Eq("MorningstarId", morningstarId));
                DateTime d1, d2;
                var minDate = DateUtils.GetMinDateTime();
                if (ff != null)
                {
                    DateTime.TryParse(
                        Regex.Match(r.Response.Body, @"class=""inception"">(.*?)</span>").Groups[1].Value, out d1);
                    DateTime.TryParse(Regex.Match(r.Response.Body, @"class=""start"">(.*?)</span>").Groups[1].Value, out d2);

                    ff.类型 = Regex.Match(r.Response.Body, @"class=""category"">(.*?)</span>").Groups[1].Value;
                    ff.成立日期 = d1 < minDate ? minDate : d1;
                    ff.开放日期 = d2 < minDate ? minDate : d2;
                    ff.申购状态 = Regex.Match(r.Response.Body, @"class=""subscribe"">(.*?)</span>").Groups[1].Value;
                    ff.赎回状态 = Regex.Match(r.Response.Body, @"class=""redeem"">(.*?)</span>").Groups[1].Value;
                    ff.风格1 = Regex.Match(r.Response.Body, @"class=""sbdesc"">(.*?)</span>").Groups[1].Value;
                    ff.DataChangeLastTime = DateTime.Now;
                    ActiveRecordMediator<Entity.Fund>.Update(ff);

                }
            });
        }

        public static void quicktake(MorningstarCommand command, string fcid, string fundCode)
        {
            string commandStr = command.ToString();
            string url =
                string.Format("http://cn.morningstar.com/handler/quicktake.ashx?command={0}&fcid={1}&randomid={2}",
                   commandStr, fcid, Random.NextDouble());

            var template = RestTemplateUtils.BuildRestTemplate(url);

            IDictionary<string, object> vars = new Dictionary<string, object>(1);
            vars.Add("command", commandStr);
            vars.Add("fcid", fcid);
            vars.Add("randomid", Random.NextDouble());

            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        ParseFundsCommand(command, r.Response.Body, fundCode);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }

        private static void ParseFundsCommand(MorningstarCommand command, string source, string fundCode)
        {
            switch (command)
            {
                case MorningstarCommand.@return:
                    ParseFundsCommand_Return(source, fundCode);
                    break;
                case MorningstarCommand.rating:
                    ParseFundsCommand_Rating(source, fundCode);
                    break;
                case MorningstarCommand.portfolio:
                    ParseFundsCommand_Portfolio(source, fundCode);
                    break;
            }
            // JsonValue json = JsonValue.Parse(source);
            //var r = json.GetValue<MorningstarReturn>();
        }

        private static void ParseFundsCommand_Portfolio(string source, string fundCode)
        {
            var portfolio = JsonConvert.DeserializeObject<MorningstarPortfolio>(source);

            if (portfolio != null && portfolio.Sector != null)
            {
                foreach (var industryWeight in portfolio.Sector)
                {
                    if (!string.IsNullOrEmpty(industryWeight.IndustryCode))
                    {
                        var item = ActiveRecordMediator<FundIndustryWeight>.FindOne(
                            Restrictions.Eq("FundCode", fundCode),
                            Restrictions.Eq("IndustryCode", industryWeight.IndustryCode));
                        if (item != null)
                        {
                            if (industryWeight.NetAssetWeight > 0)
                            {
                                item.CatAvgWeight = industryWeight.CatAvgWeight;
                                item.NetAssetWeight = industryWeight.NetAssetWeight;
                                item.DataChangeLastTime = DateTime.Now;
                                ActiveRecordMediator<FundIndustryWeight>.Update(item);
                            }
                        }
                        else
                        {
                            item = new FundIndustryWeight();
                            item.FundCode = fundCode;
                            item.IndustryCode = industryWeight.IndustryCode;

                            item.CatAvgWeight = industryWeight.CatAvgWeight;
                            item.NetAssetWeight = industryWeight.NetAssetWeight;
                            item.DataChangeLastTime = DateTime.Now;
                            ActiveRecordMediator<FundIndustryWeight>.Create(item);
                        }
                    }
                }
            }
        }

        private static void ParseFundsCommand_Return(string source, string fundCode)
        {
            var r = JsonConvert.DeserializeObject<MorningstarReturn>(source);

            //var rh = ActiveRecordMediator<FundRateHistory>.FindOne(
            //       Restrictions.Eq("FundCode", r.),
            //       Restrictions.Eq("NavDateType", fundRateHistory.NavDateType),
            //       Restrictions.Eq("NavStartDate", fundRateHistory.NavStartDate),
            //       Restrictions.Eq("NavEndDate", fundRateHistory.NavEndDate));
            //if (rh != null)
            //{
            //    if (rh.NavRate != fundRateHistory.NavRate)
            //    {
            //        rh.NavRate = fundRateHistory.NavRate;
            //        rh.DataChangeLastTime = DateTime.Now;
            //        ActiveRecordMediator<FundRateHistory>.Update(rh);
            //    }
            //}
            //else
            //{
            //    fundRateHistory.DataChangeLastTime = DateTime.Now;
            //    ActiveRecordMediator<FundRateHistory>.Create(fundRateHistory);
            //}

        }
        private static void ParseFundsCommand_Rating(string source, string fundCode)
        {

            var rr = JsonConvert.DeserializeObject<MorningstarRating>(source);
            if (rr != null)
            {
                foreach (var rs in rr.RiskStats)
                {
                    ParseRiskStat(rr.EffectiveDate, rs, fundCode);
                }
            }
            //foreach (var rs in rr.)
            //{
            //    ParseRiskStat(rr.EffectiveDate, rs, fundCode);
            //}
        }

        private static void ParseRiskStat(DateTime? effectiveDate, MorningstarRating.MorningstarRiskStat riskStat, string fundCode)
        {
            var r = ActiveRecordMediator<FundRiskStats>.FindOne(
                    Restrictions.Eq("FundCode", fundCode),
                   Restrictions.Eq("Name", riskStat.Name)
                  );
            if (r != null)
            {
                r.EffectiveDate = effectiveDate;
                r.ToInd = riskStat.ToInd;
                r.ToCat = riskStat.ToCat;
                r.DataChangeLastTime = DateTime.Now;
                ActiveRecordMediator<FundRiskStats>.Update(r);

            }
            else
            {
                r = new FundRiskStats();
                r.FundCode = fundCode;
                r.Name = riskStat.Name;
                r.EffectiveDate = effectiveDate;
                r.ToInd = riskStat.ToInd;
                r.ToCat = riskStat.ToCat;
                r.DataChangeLastTime = DateTime.Now;
                ActiveRecordMediator<FundRiskStats>.Create(r);
            }
        }

        public enum MorningstarCommand
        {
            /// <summary>
            /// 历史业绩 季度回报
            /// </summary>
            performance,
            /// <summary>
            /// 历史回报
            /// </summary>
            @return,
            /// <summary>
            /// 风险 评估/统计
            /// </summary>
            rating,
            /// <summary>
            /// 基金管理
            /// </summary>
            manage,
            /// <summary>
            /// 持仓分析 行业分布
            /// </summary>
            portfolio,


        }
        /// <summary>
        /// 历史回报
        /// </summary>
        public class MorningstarReturn
        {
            public CurrentOrMonthEndReturn CurrentReturn { get; set; }
            public CurrentOrMonthEndReturn MonthEndReturn { get; set; }


            public class CurrentOrMonthEndReturn
            {
                public ReturnItem[] Return { get; set; }
            }

            public class ReturnItem
            {
                public string Name { get; set; }
                public decimal? Return { get; set; }
                public decimal? ReturnToInd { get; set; }
                public decimal? ReturnToCat { get; set; }
            }
        }

        public class MorningstarRating
        {
            public virtual DateTime? EffectiveDate { get; set; }
            public virtual DateTime? RatingDate { get; set; }
            public MorningstarRiskStat[] RiskStats { get; set; }
            public class MorningstarRiskStat
            {
                public string Name { get; set; }
                public decimal? ToInd { get; set; }
                public decimal? ToCat { get; set; }
            }
        }

        public class MorningstarPortfolio
        {
            /// <summary>
            /// 行业分布
            /// </summary>
            public MorningstarIndustryWeight[] Sector { get; set; }
        }

        public class MorningstarIndustryWeight
        {
            public string IndustryCode { get; set; }
            public decimal? NetAssetWeight { get; set; }
            public decimal? CatAvgWeight { get; set; }
        }
    }
}
