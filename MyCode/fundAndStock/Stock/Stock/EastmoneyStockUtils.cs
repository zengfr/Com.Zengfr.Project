using System;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;

using NHibernate.Criterion;
using Spring.Http;
using Common.Logging;
using Entity;
using Spring.Rest.Utils;

namespace Stock.Utils
{
    public class EastmoneyStockUtils : RestTemplateUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(EastmoneyStockUtils));
        public static void GetStockCodes()
        {
            string url = string.Format("http://quote.eastmoney.com/stocklist.html", "");

            var template = RestTemplateUtils.BuildRestTemplate(url, "gb2312");


            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        ParseStockCode(r.Response.Body);

                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }

        private static void ParseStockCode(string html)
        {
            var mColl = Regex.Matches(html, @"href=""http://quote.eastmoney.com/(sh|sz)(.*?).html"">(.*?)</a></li>", RegexOptions);
            var code = string.Empty;
            var name = string.Empty;
            int total = mColl.Count;
            int index = 0;
            foreach (Match m in mColl)
            {
                if (m.Success)
                {
                    code = m.Groups[1].Value + m.Groups[2].Value;
                    name = m.Groups[3].Value;
                    log.InfoFormat("ParseStockCode:{0}/{1}->{2}:{3}", index++, total, code, name);
                    var item = ActiveRecordMediator<Entity.Stock>.FindOne(
                        Restrictions.Eq("StockCode", code)
                        );
                    if (item == null)
                    {
                        item = new Entity.Stock();
                    }
                    item.StockCode = code;
                    item.StockName = name;

                    item.DataChangeLastTime = DateTime.Now;
                    if (item.StockId > 0)
                    {
                        ActiveRecordMediator<Entity.Stock>.Update(item);
                    }
                    else
                    {
                        ActiveRecordMediator<Entity.Stock>.Create(item);
                    }


                }
            }
        }
    }
}
