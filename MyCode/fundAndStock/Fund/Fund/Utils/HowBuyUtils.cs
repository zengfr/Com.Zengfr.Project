
using System;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Common.Logging;

using NHibernate.Criterion;
using Entity;
using Spring.Rest.Utils;

namespace Fund
{
    public class HowBuyUtils : RestTemplateUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(HowBuyUtils));
        public static void GetHowBuyManagerIds(int pageIndex)
        {
            pageIndex = 1;
            string url = string.Format("http://www.howbuy.com/fund/manager/index.htm?&perPage=3000&page={0}", pageIndex);

            var template = RestTemplateUtils.BuildRestTemplate(url);



            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        try
                        {
                            ParseHowBuyManagerIds(r.Response.Body);
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

        private static void ParseHowBuyManagerIds(string source)
        {
            var matches = Regex.Matches(source, @"/fund/manager/(\d+?)/"" target=""_blank""", RestTemplateUtils.RegexOptions);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    UpdateHowBuyManagerIdsToDB(match.Groups[1].Value.Trim());
                }
            }

        }

        private static void UpdateHowBuyManagerIdsToDB(string howBuyManagerId)
        {
            ICriterion[] exp = new ICriterion[]
                {
                    Restrictions.Eq("HowBuyManagerId", howBuyManagerId), 
                };

            var item = ActiveRecordMediator<FundManager>.FindOne(exp);
            if (item == null)
            {
                item = new FundManager();
                item.HowBuyManagerId = howBuyManagerId;
                item.DataChangeLastTime = DateTime.Now;
                ActiveRecordMediator<FundManager>.Create(item);
            }
        }

        public static void UpdateFundManagerAchievements()
        {
            var managers = ActiveRecordMediator<FundManager>.FindAll(new Order[] { Order.Desc("DataChangeLastTime") });

            foreach (var m in managers)
            {
                UpdateFundManagerAchievements(m.HowBuyManagerId);
                System.Threading.Thread.Sleep(1000 * 3);
            }
        }

        public static void UpdateFundManagerAchievements(string howBuyManagerId)
        {
            string url = string.Format("/fund/manager/{0}/", howBuyManagerId);

            var template = RestTemplateUtils.BuildRestTemplate(url);



            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        try
                        {
                            var name = ParseHowBuyManager(howBuyManagerId, r.Response.Body);
                            if (!string.IsNullOrEmpty(name))
                            {
                                ParseHowBuyManagerAchievement(howBuyManagerId, r.Response.Body);
                            }
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

        private static void ParseHowBuyManagerAchievement(string managerName, string source)
        {
            var mColl = Regex.Matches(source, @"<tr>(.*?)<td class=""tc"">(.*?)</td>(.*?)<td class=""tc"">(.*?)</td>(.*?)<td class=""tc"">(.*?)</td>(.*?)<td class=""time"">(.*?)</td>(.*?)<td class=""rate"">(.*?)</td>(.*?)<td class=""rate"">(.*?)</td>(.*?)<td class=""rate"">(.*?)</td>(.*?)<td class=""rate"">(.*?)</td>(.*?)</tr>", RestTemplateUtils.RegexOptions);

            foreach (Match m in mColl)
            {
                if (m.Success)
                {
                    decimal 规模;
                    decimal 近六月回报 = 0;
                    decimal 近三月回报 = 0;
                    decimal 近一年回报 = 0;
                    decimal 任期总回报 = 0;


                    decimal.TryParse(m.Groups[6].Value, out 规模);
                    decimal.TryParse(m.Groups[10].Value.Replace(@"<span class=""cRed"">", "").Replace("%</span>", ""), out 任期总回报);
                    decimal.TryParse(m.Groups[12].Value.Replace(@"<span class=""cRed"">", "").Replace("%</span>", ""), out 近三月回报);
                    decimal.TryParse(m.Groups[14].Value.Replace(@"<span class=""cRed"">", "").Replace("%</span>", ""), out 近六月回报);
                    decimal.TryParse(m.Groups[16].Value.Replace(@"<span class=""cRed"">", "").Replace("%</span>", ""), out 近一年回报);

                    var managerAchievement = new FundManagerAchievement();
                    managerAchievement.ManagerName = managerName;
                    managerAchievement.FundCode = m.Groups[2].Value;
                    managerAchievement.规模 = 规模;
                    managerAchievement.近六月回报 = 近六月回报;
                    managerAchievement.近三月回报 = 近三月回报;
                    managerAchievement.近一年回报 = 近一年回报;
                    managerAchievement.任期总回报 = 任期总回报;
                    managerAchievement.任职时间 = m.Groups[8].Value.Trim();
                    managerAchievement.DataChangeLastTime = DateTime.Now;

                    ICriterion[] exp = new ICriterion[]
                {
                    Restrictions.Eq("ManagerName", managerAchievement.ManagerName), 
                     Restrictions.Eq("FundCode", managerAchievement.FundCode), 
                };
                    var item = ActiveRecordMediator<FundManagerAchievement>.FindOne(exp);
                    if (item != null)
                    {
                        item.DataChangeLastTime = managerAchievement.DataChangeLastTime;
                        item.规模 = managerAchievement.规模;
                        item.近六月回报 = managerAchievement.近六月回报;
                        item.近三月回报 = managerAchievement.近三月回报;
                        item.近一年回报 = managerAchievement.近一年回报;
                        item.任期总回报 = managerAchievement.任期总回报;
                        item.任职时间 = managerAchievement.任职时间;

                        ActiveRecordMediator<FundManagerAchievement>.Update(item);
                    }
                    else
                    {
                        ActiveRecordMediator<FundManagerAchievement>.Create(managerAchievement);
                    }

                }
            }
        }

        private static string ParseHowBuyManager(string howBuyManagerId, string source)
        {
            ICriterion[] exp = new ICriterion[]
                {
                    Restrictions.Eq("HowBuyManagerId", howBuyManagerId), 
                };
            var item = ActiveRecordMediator<FundManager>.FindOne(exp);
            if (item != null)
            {
                decimal 好买评分;
                decimal 从业年均回报 = 0;
                decimal 最大回撤 = 0;
                decimal 最大盈利 = 0;

                decimal.TryParse(Regex.Match(source, @"grade(.*?)class=""num"">(.*?)</span></li>", RestTemplateUtils.RegexOptions).Groups[2].Value.Replace("<span>", ""), out 好买评分);
                decimal.TryParse(Regex.Match(source, @"从业年均回报(.*?)<span class=""cRed"">(.*?)%</span>", RestTemplateUtils.RegexOptions).Groups[2].Value, out 从业年均回报);
                decimal.TryParse(Regex.Match(source, @"最大回撤(.*?)<span class=""cGreen"">(.*?)%</span>", RestTemplateUtils.RegexOptions).Groups[2].Value, out 最大回撤);
                decimal.TryParse(Regex.Match(source, @"最大盈利(.*?)<span class=""cRed"">(.*?)%</span>", RestTemplateUtils.RegexOptions).Groups[2].Value, out 最大盈利);

                item.ManagerName = Regex.Match(source, @"<p class=""name"">(.*?)</p>", RestTemplateUtils.RegexOptions).Groups[1].Value;
                item.从业年均回报 = 从业年均回报;
                item.从业时间 = Regex.Match(source, @"<li>从业时间：(.*?)</li>", RestTemplateUtils.RegexOptions).Groups[1].Value;
                item.好买评分 = 好买评分;
                item.擅长 = Regex.Match(source, @"<li>最擅长的基金类型：(.*?)</li>", RestTemplateUtils.RegexOptions).Groups[1].Value;
                item.最大回撤 = 最大回撤;
                item.最大盈利 = 最大盈利;


                item.DataChangeLastTime = DateTime.Now;
                ActiveRecordMediator<FundManager>.Update(item);
                return item.ManagerName;
            }
            return string.Empty;
        }
    }
}
