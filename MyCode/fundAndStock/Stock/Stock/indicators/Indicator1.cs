using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;

using NHibernate;
namespace ConsoleApplication3.Stock.Indicators
{
    
    public class Indicator1 : AbstractIndicator
    {
        protected override string Name
        {
            get { return "基本"; }
        }

        public override string[] Process(DateTime day)
        {
            var sql =
               string.Format(@"select distinct t.[代码]  
from   [dbo].[doctor10jqkascore] a
 join 
 (
	 select  [代码] from [dbo].[Stock主力连续净流] where [天数]>0 and [净流万]>0  
	  and [净流率]>0  
   and [主力罗盘]>0.90  
    intersect
    select [stockCode] as [代码] from [dbo].[doctor10jqkascore] where  [effectDate]>='{1}' and
     [technicalScore] >=0.65 and
	 [basicScore]>=0.65 and
	 [messageScore] >=0.65 and
	 [tradeScore] >=0.65 

   intersect
   select [代码] from [dbo].[Stock资金流入趋势] where 日期>='{1}' and  [主力罗盘]>=90  and  [涨跌幅]>-4 and  [涨跌幅]<9 
   
intersect
   select [代码] from  [dbo].[Stock历史成交分布]  where [opendate日期]>='{1}' 
   and  [ratioamount净流入率]>0
   and [r0_net超大单]>0 and [r1_net大单]>0 and [r2_net小单]>0 and [r3_net散单]>0
except
  select [代码] from [dbo].[Stock资金流入趋势] where 日期='{0}' and [主力罗盘]<0 and [主力罗盘]>=-90
except
  select [代码] from [dbo].[Stock资金流入趋势] where 日期='{2}' and [涨跌幅]>8.5 
except
  select [代码] from [dbo].[Stock资金流入趋势] where 日期='{3}' and [涨跌幅]>8.5 
except
  select [代码] from [dbo].[Stock资金流入趋势] where 日期='{4}' and [涨跌幅]>8.5
)as t   on t.[代码]=a.[stockCode] 
where a.effectDate='{0}' and
a.stockcode not like '%sh300%' and 
a.stockcode not like '%sz300%' and 
a.stockName not like '%ST%' ",
                   day.ToString("yyyy-MM-dd"),
                   day.AddDays(-3).ToString("yyyy-MM-dd"),
                   day.ToString("yyyy-MM-dd"),
                   day.AddDays(-1).ToString("yyyy-MM-dd"),
                   day.AddDays(-2).ToString("yyyy-MM-dd"));

            var results = ProcessQuery(sql);
            return results ;
        }

        public override double[] Process(string code, DateTime day)
        {
            throw new NotImplementedException();
        }
    }
}
