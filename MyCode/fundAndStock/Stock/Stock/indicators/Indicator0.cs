using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;

using NHibernate;
namespace ConsoleApplication3.Stock.Indicators
{
    
    public class Indicator0 : AbstractIndicator
    {
        protected override string Name
        {
            get { return "牛叉诊股"; }
        }

        public override string[] Process(DateTime day)
        {
            var sql =
               string.Format(@"select a.stockcode
from   [dbo].[doctor10jqkascore] a (nolock)
join  [dbo].[doctor10jqkascore] b (nolock) on a.stockcode=b.[stockCode] 
where a.effectDate='{0}' and a.effectDate='{1}' and 
a.zhscore>b.zhscore and 
a.technicalScore>b.technicalScore and   
a.fundsScore>b.fundsScore  and
a.messageScore>b.messageScore and
a.tradeScore>b.tradeScore and
a.basicScore>=b.basicScore and
a.stockcode not like '%sh300%' and 
a.stockcode not like '%sz300%' and 
a.stockName not like '%ST%' ",
                   day.ToString("yyyy-MM-dd"),
                   day.AddDays(-1).ToString("yyyy-MM-dd")
                    );

            var results = ProcessQuery(sql);
            return results ;
        }

        public override double[] Process(string code, DateTime day)
        {
            throw new NotImplementedException();
        }
    }
}
