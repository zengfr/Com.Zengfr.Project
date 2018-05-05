declare @topV  int 
set @topV=22 
select t.[FundCode],f.[FundName],d.[NavRate],count(1) as c from (
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
SELECT top 22  [FundCode] FROM [zfrDB]..[FundRateHistory] 
where [NavDateType]='m' and NavStartDate='2015-03-01'
order by [NavRate] desc
) as t 
join [zfrDB]..Fund f on f.[FundCode]=t.[FundCode]
 join [zfrDB]..[FundRateDetail] d on d.[FundCode]=f.[FundCode] and d.[NavDate]='2015-04-13'
 group by t.[FundCode],f.[FundName],d.[NavRate] order by  c desc,d.[NavRate] desc

