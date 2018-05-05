

declare @topV  int 
declare @z1 datetime
set @topV=99
set @z1=DATEADD(wk, DATEDIFF(wk,0,getdate()), -1)
select tt.c, f.[FundCode],f.[FundName],f.[���1],f.[����] ,fm.ManagerName,fm.��������,fm.��ҵ����ر�,fm.���ӯ��,fm.���س�,
d1.[NavRate] as d1,d3.[NavRate] as d3,d5.[NavRate] as d5,d7.[NavRate] as d7,
w0.[NavRate] as w0,m0.[NavRate] as m0,w1.[NavRate] as w1,w2.[NavRate] as w2 ,w3.[NavRate] as w3,w4.[NavRate] as w4
,m.[NavRate] as m
from[zfrDB]..Fund f    
 join 
 (select  
t.[FundCode],
count(1) as c from (
SELECT top (@topV)  [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='d7'  
order by [NavRate] desc
union all
SELECT top (@topV)  [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='w0'  
order by [NavRate] desc
union all
SELECT top (@topV)  [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='m0'  
order by [NavRate] desc
union all
SELECT top (@topV)  [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='w'  and NavStartDate=DATEADD(wk,-1,@z1)
order by [NavRate] desc
union all
SELECT top (@topV) [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='w'  and NavStartDate=DATEADD(wk,-2,@z1)
order by [NavRate] desc
union all
SELECT top (@topV) [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
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

left join [zfrDB]..[FundRateDetail] d1 on d1.[FundCode]=f.[FundCode] and d1.[NavDate]='2015-04-17'
left join [zfrDB]..[FundRateHistory] d3 on d3.[FundCode]=f.[FundCode] and d3.[NavDateType]='d3'
left join [zfrDB]..[FundRateHistory] d5 on d5.[FundCode]=f.[FundCode] and d5.[NavDateType]='d5'
left join [zfrDB]..[FundRateHistory] d7 on d7.[FundCode]=f.[FundCode] and d7.[NavDateType] ='d7'
left join [zfrDB]..[FundRateHistory] w0 on w0.[FundCode]=f.[FundCode] and w0.[NavDateType] ='w0'
left join [zfrDB]..[FundRateHistory] m0 on m0.[FundCode]=f.[FundCode] and m0.[NavDateType] ='m0'
left join [zfrDB]..[FundRateHistory] w1 on w1.[FundCode]=f.[FundCode] and w1.[NavDateType] ='w' and w1.NavStartDate=DATEADD(wk,-1,@z1)
left join [zfrDB]..[FundRateHistory] w2 on w2.[FundCode]=f.[FundCode] and w2.[NavDateType] ='w' and w2.NavStartDate=DATEADD(wk,-2,@z1)
left join [zfrDB]..[FundRateHistory] w3 on w3.[FundCode]=f.[FundCode] and w3.[NavDateType] ='w' and w3.NavStartDate=DATEADD(wk,-3,@z1)
left join [zfrDB]..[FundRateHistory] w4 on w4.[FundCode]=f.[FundCode] and w4.[NavDateType] ='w' and w4.NavStartDate=DATEADD(wk,-3,@z1)
left join [zfrDB]..[FundRateHistory] m on m.[FundCode]=f.[FundCode] and m.[NavDateType] ='m'   --and m.NavStartDate=DATEADD(wk,-4,@z1)

where f.[����]<>'��ծ����' and f.[����]<>'��תծ����'


   order by --m  desc,  
   c desc,w0 desc 
   