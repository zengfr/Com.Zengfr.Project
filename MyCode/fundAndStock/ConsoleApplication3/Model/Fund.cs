
namespace ConsoleApplication3.Model
{
    /// <summary>
    /// 基金
    /// </summary>
    public class Fund : FundStockBase
    {
        /// <summary>
        /// 开放式 \封闭式
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 股票型
        /// </summary>
        public string InvestType { get; set; }
        /// <summary>
        /// 平衡型
        /// </summary>
        public string InvestStyle { get; set; }
        public string Company { get; set; }
    }

    public class FundEx : Fund
    {

    }
}
