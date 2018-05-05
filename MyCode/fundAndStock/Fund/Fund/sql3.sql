declare @topV  int 
set @topV=22
select t.[FundCode],f.[FundName],d1.[NavRate],d2.[NavRate],d3.[NavRate],--sum(d1.[NavRate]+d2.[NavRate]+d3.[NavRate]) as r,
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
where [NavDateType]='w'  and NavStartDate='2015-04-06'
order by [NavRate] desc
union all
SELECT top (@topV) [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='w'  and NavStartDate='2015-03-29'
order by [NavRate] desc
union all
SELECT top (@topV) [FundCode] FROM [zfrDB].[dbo].[FundRateHistory] 
where [NavDateType]='w' and NavStartDate='2015-03-22'
order by [NavRate] desc 

union all
SELECT top (@topV)  [FundCode] FROM [zfrDB]..[FundRateHistory] 
where [NavDateType]='m' and NavStartDate='2015-03-01'
order by [NavRate] desc
) as t 
join [zfrDB]..Fund f on f.[FundCode]=t.[FundCode]
left join [zfrDB]..[FundRateDetail] d1 on d1.[FundCode]=f.[FundCode] and d1.[NavDate]='2015-04-10'
left join [zfrDB]..[FundRateDetail] d2 on d2.[FundCode]=f.[FundCode] and d2.[NavDate]='2015-04-13'
left join [zfrDB]..[FundRateDetail] d3 on d3.[FundCode]=f.[FundCode] and d3.[NavDate]='2015-04-14'
 group by t.[FundCode],f.[FundName],d1.[NavRate],d2.[NavRate],d3.[NavRate]
  order by  c desc ,d1.[NavRate]+d2.[NavRate]+d3.[NavRate] desc

  select FundCode,NavDate from  [zfrDB]..[FundRateDetail] group by FundCode,NavDate having count(1) >1