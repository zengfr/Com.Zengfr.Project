using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;

using NHibernate;
namespace ConsoleApplication3.Stock.Indicators
{
    
    public class Indicator6 : AbstractIndicator
    {
        protected override string Name
        {
            get { return "净流入,跌"; }
        }
        public override string[] Process(DateTime day)
        {
            var sql =
               string.Format(@" select a.[代码]  
FROM Stock主力净流 a (nolock)
WHERE (a.主力罗盘 > - 90) AND (a.主力罗盘 < 0) AND (a.涨跌幅 < 0)
and
a.代码 not like '%sh300%' and 
a.代码 not like '%sz300%' and 
a.代码 not like '%ST%' 
ORDER BY 主力净流入率 DESC,涨跌幅 ASC",
                   day.ToString("yyyy-MM-dd"),
                   day.AddDays(-2).ToString("yyyy-MM-dd") 
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
