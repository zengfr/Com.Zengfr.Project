using System;

namespace ConsoleApplication3.Stock.Indicators
{

    public class Indicator3 : AbstractIndicator
    {
        protected override string Name
        {
            get { return "指标上涨概率"; }
        }
        public override string[] Process(DateTime day)
        {
            var sql =
               string.Format(@"SELECT top 11 [stockcode]
  FROM [Stock和讯指标云] h (nolock)
  where effectdate='{0}' and 
[交易类看多指标数变化量]-[交易类看空指标数变化量]>=0 and
[交易类看多指标数变化量]>=0 and 
[交易类看空指标数变化量]<=0 and 
[未来10个交易日上涨百分5的概率]>0 and 
stockcode not like '%sh300%' and 
stockcode not like '%sz300%' and 
stockName not like '%ST%'", day.ToString("yyyy-MM-dd"));
            sql += " order by [未来10个交易日上涨百分5的概率] desc, [今日发出提示信号指标数] desc";
            var results = ProcessQuery(sql);
            return results;
        }

        public override double[] Process(string code, DateTime day)
        {
            throw new NotImplementedException();
        }
    }
}
