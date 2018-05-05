
namespace ConsoleApplication3.Model
{
    /// <summary>
    /// 基金 
    /// </summary>
    public class FundRequest
    {
        public string fund_class { get; set; }
        public string invest_type { get; set; }
        public string invest_custom { get; set; }
        public string fund_com_open { get; set; }
        public string fund_com_close { get; set; }
        public string startdate_open { get; set; }
        public string startdate_close { get; set; }
        public string enddate_open { get; set; }
        public string enddate_close { get; set; }

    }

}
