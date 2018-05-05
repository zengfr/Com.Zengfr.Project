using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;

using NHibernate;
namespace ConsoleApplication3.Stock.Indicators
{
    
    public class Indicator4 : AbstractIndicator
    {
        protected override string Name
        {
            get { return "2日跌,流入"; }
        }
        public override string[] Process(DateTime day)
        {
            var sql =
               string.Format(@" 
   select a.[代码] from [dbo].[Stock资金流入趋势] a (nolock) 
join [dbo].[Stock资金流入趋势] b(nolock) on a.代码=b.代码
where a.日期>='{0}' and  a.[主力罗盘]>=90  and  a.[涨跌幅]>-5 and  a.[涨跌幅]<9 and
b.日期<'{0}' and b.日期>='{1}' and  b.[主力罗盘]>=90  and  b.[涨跌幅]>-5 and  b.[涨跌幅]<9 and
a.代码 not like '%sh300%' and 
a.代码 not like '%sz300%' and 
a.代码 not like '%ST%' ",
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
