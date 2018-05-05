
namespace ConsoleApplication3.Model
{

    /// <summary>
    /// 基金重仓股统计 http://paiming.funds.hexun.com/cc/zcgtj.htm
    /// </summary>
    public class zcgtj
    {
        public string fund_class { get; set; }
        public string invest_type { get; set; }
        public string invest_custom { get; set; }
        public string fund_com_open { get; set; }
        public string enddate_open { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string gpdm { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string gpmc { get; set; }
        /// <summary>
        /// 基金覆盖面(只)
        /// </summary>
        public decimal jjjbm { get; set; }
        /// <summary>
        /// 持股总数(万股)
        /// </summary>
        public decimal cgzs { get; set; }
        /// <summary>
        /// 持股总市值(万元)
        /// </summary>
        public decimal cgzsz { get; set; }
        /// <summary>
        /// 占该股流通市值比例
        /// </summary>
        public decimal szbl { get; set; }
    }
}
