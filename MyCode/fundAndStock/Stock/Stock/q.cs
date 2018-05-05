using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;

using NHibernate;
using ConsoleApplication3.Stock.Indicators;
using Common.Utils;

namespace ConsoleApplication3.Stock
{
    public class q
    {


        public static void execFund()
        {
            var sql = @"declare @topV  int 
declare @z1 datetime
set @topV=99
set @z1=DATEADD(wk, DATEDIFF(wk,0,getdate()), -1)

select DATEADD(wk,-1,@z1)

select tt.c, f.[FundCode],f.[FundName],f.[风格1],f.[类型] ,fm.ManagerName,fm.好买评分,fm.从业年均回报,fm.最大盈利,fm.最大回撤,
d1.[NavRate] as d1,--d3.[NavRate] as d3,d5.[NavRate] as d5,
d7.[NavRate] as d7,
w0.[NavRate] as w0,m0.[NavRate] as m0,w1.[NavRate] as w1,w2.[NavRate] as w2 ,w3.[NavRate] as w3,w4.[NavRate] as w4
,m.[NavRate] as m
from[zfrDB]..Fund f    
 join 
 (select  
t.[FundCode],
count(1) as c from (
SELECT top (@topV)  [FundCode] FROM [FundRateHistory] 
where [NavDateType]='d7'  
order by [NavRate] desc
union all
SELECT top (@topV)  [FundCode] FROM [FundRateHistory] 
where [NavDateType]='w0'  
order by [NavRate] desc
union all
SELECT top (@topV)  [FundCode] FROM [FundRateHistory] 
where [NavDateType]='m0'  
order by [NavRate] desc
union all
SELECT top (@topV)  [FundCode] FROM [FundRateHistory] 
where [NavDateType]='w'  and NavStartDate=DATEADD(wk,-1,@z1)
order by [NavRate] desc
union all
SELECT top (@topV) [FundCode] FROM [FundRateHistory] 
where [NavDateType]='w'  and NavStartDate=DATEADD(wk,-2,@z1)
order by [NavRate] desc
union all
SELECT top (@topV) [FundCode] FROM [FundRateHistory] 
where [NavDateType]='w' and NavStartDate=DATEADD(wk,-3,@z1)
order by [NavRate] desc 

union all
SELECT top (@topV)  [FundCode] FROM [zfrDB]..[FundRateHistory] 
where [NavDateType]='m' and NavStartDate=DATEADD(wk,-4,@z1)
order by [NavRate] desc
) as t 
 
 group by t.[FundCode] 
 
   ) as tt  on tt.[FundCode]=f.[FundCode]
left join [dbo].[FundManagerAchievement] ma on ma.FundCode=f.[FundCode]
left join [dbo].[FundManager] fm on fm.HowBuyManagerId=ma.ManagerName

left join [zfrDB]..[FundRateDetail] d1 on d1.[FundCode]=f.[FundCode] and d1.[NavDate]='2015-05-22'
--left join [zfrDB]..[FundRateHistory] d3 on d3.[FundCode]=f.[FundCode] and d3.[NavDateType]='d3'
--left join [zfrDB]..[FundRateHistory] d5 on d5.[FundCode]=f.[FundCode] and d5.[NavDateType]='d5'
left join [zfrDB]..[FundRateHistory] d7 on d7.[FundCode]=f.[FundCode] and d7.[NavDateType] ='d7'
left join [zfrDB]..[FundRateHistory] w0 on w0.[FundCode]=f.[FundCode] and w0.[NavDateType] ='w0'
left join [zfrDB]..[FundRateHistory] m0 on m0.[FundCode]=f.[FundCode] and m0.[NavDateType] ='m0'
left join [zfrDB]..[FundRateHistory] w1 on w1.[FundCode]=f.[FundCode] and w1.[NavDateType] ='w' and w1.NavStartDate=DATEADD(wk,-1,@z1)
left join [zfrDB]..[FundRateHistory] w2 on w2.[FundCode]=f.[FundCode] and w2.[NavDateType] ='w' and w2.NavStartDate=DATEADD(wk,-2,@z1)
left join [zfrDB]..[FundRateHistory] w3 on w3.[FundCode]=f.[FundCode] and w3.[NavDateType] ='w' and w3.NavStartDate=DATEADD(wk,-3,@z1)
left join [zfrDB]..[FundRateHistory] w4 on w4.[FundCode]=f.[FundCode] and w4.[NavDateType] ='w' and w4.NavStartDate=DATEADD(wk,-4,@z1)
left join [zfrDB]..[FundRateHistory] m on m.[FundCode]=f.[FundCode] and m.[NavDateType] ='m'   --and m.NavStartDate=DATEADD(wk,-4,@z1)

where f.[类型]<>'短债基金' and f.[类型]<>'可转债基金' and fm.好买评分>=7.5 and  f.[类型] not like '%债券%'
and w1.[NavRate]>0 and w2.[NavRate]>0 and w3.[NavRate]>0 and w4.[NavRate]>0




   order by --m  desc,  
   c desc,d7 desc,w0 desc 
   ";
            var results = QueryUtils.Query<object[]>(sql);
            foreach (var result in results)
            {
                Console.WriteLine("{0},{1},{2}", result);
            }
            Console.ReadLine();
        }
        public static void execStock0(int timeSpanDay)
        {
            IIndicator indicator = new Indicator0();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));
        }
        public static void execStock1(int timeSpanDay)
        {
            IIndicator indicator = new Indicator1();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 1));
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 2));
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 3));
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 4));
           
        }


        public static void execStock2(int timeSpanDay)
        {
            IIndicator indicator = new Indicator2();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 1));
        }


        public static void execStock3(int timeSpanDay)
        {
            IIndicator indicator = new Indicator3();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 1));
        }
        public static void execStock4(int timeSpanDay)
        {
            IIndicator indicator = new Indicator4();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));
            
        }
        public static void execStock5(int timeSpanDay)
        {
            IIndicator indicator = new Indicator5();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));

        }
        public static void execStock6(int timeSpanDay)
        {
            IIndicator indicator = new Indicator6();
            indicator.Test(DateUtils.GetDateTimeNoWeekEnd(timeSpanDay - 0));

        }

         

        




    }

   

}

