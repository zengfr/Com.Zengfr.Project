
namespace ConsoleApplication3.Model
{
    using System;
    /// <summary>
    ///基金持股明细 http://jingzhi.funds.hexun.com/database/cgmx.aspx?fundcode=510180
    /// </summary>
    public class FundStockRelation
    {
        public virtual Fund Fund { get; set; }
        public virtual Stock Stock { get; set; }

        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        /// <summary>
        ///股票数量（万股）
        /// </summary>
        public virtual decimal StockCount { get; set; }
        /// <summary>
        //持股市值 股票市值
        /// </summary>
        public virtual decimal StockAmount { get; set; }
        /// <summary>
        /// 占流通市值比例%
        /// </summary>
        public virtual decimal StockValueRatio { get; set; }
        /// <summary>
        /// 占净值比例%
        /// </summary>
        public virtual decimal EquityFundRatio { get; set; }
    }
}
