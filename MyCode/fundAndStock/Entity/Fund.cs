
using System;
using Castle.ActiveRecord;

namespace Entity
{

    /// <summary>
    /// 基金
    /// </summary>
    [ActiveRecord]
    public class Fund : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundId { get; set; }

        [Property]
        public virtual string MorningstarId { get; set; }

        [Property]
        public virtual string FundCode { get; set; }

        [Property]
        public virtual string FundName { get; set; }

        [Property]
        public string 类型 { get; set; }

        [Property]
        public string 开封 { get; set; }

        [Property]
        public string 风格1 { get; set; }

        [Property]
        public string 风格2 { get; set; }

        [Property]
        public string 申购状态 { get; set; }

        [Property]
        public string 赎回状态 { get; set; }

        [Property]
        public DateTime 成立日期 { get; set; }

        [Property]
        public DateTime 开放日期 { get; set; }

        [Property]
        public decimal 资产规模 { get; set; }

        [Property]
        public decimal 份额规模 { get; set; }

        [Property]
        public DateTime FundNavRefreshTime { get; set; }

    }

    /// <summary>
    /// 基金公司
    /// </summary>
    [ActiveRecord]
    public class FundCompany : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundCompanyId { get; set; }

        [Property]
        public virtual string CompanyName { get; set; }

        [Property]
        public virtual string FundCode { get; set; }
    }

    /// <summary>
    /// 基金经理
    /// </summary>
    [ActiveRecord]
    public class FundManager : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundManagerId { get; set; }

        /// <summary>
        /// http://www.howbuy.com/fund/manager/index.htm
        /// </summary>
        [Property]
        public virtual string HowBuyManagerId { get; set; }

        [Property]
        public virtual string ManagerName { get; set; }

        [Property]
        public virtual string 从业时间 { get; set; }

        [Property]
        public virtual string 擅长 { get; set; }

        [Property]
        public virtual decimal? 好买评分 { get; set; }

        [Property]
        public virtual decimal? 从业年均回报 { get; set; }

        [Property]
        public virtual decimal? 最大盈利 { get; set; }

        [Property]
        public virtual decimal? 最大回撤 { get; set; }

    }

    /// <summary>
    ///  基金经理 业绩 成就
    /// </summary>
    [ActiveRecord]
    public class FundManagerAchievement : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundManagerAchievementId { get; set; }

        [Property]
        public virtual string ManagerName { get; set; }

        [Property]
        public virtual string FundCode { get; set; }

        [Property]
        public virtual string 任职时间 { get; set; }

        [Property]
        public virtual decimal? 规模 { get; set; }

        [Property]
        public virtual decimal? 任期总回报 { get; set; }

        [Property]
        public virtual decimal? 近三月回报 { get; set; }

        [Property]
        public virtual decimal? 近六月回报 { get; set; }

        [Property]
        public virtual decimal? 近一年回报 { get; set; }
    }

    /// <summary>
    /// 风险统计
    /// </summary>
    [ActiveRecord]
    public class FundRiskStats : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundRiskStatsId { get; set; }

        [Property]
        public virtual string FundCode { get; set; }

        [Property]
        public DateTime? EffectiveDate { get; set; }

        [Property]
        public string Name { get; set; }

        [Property]
        public decimal? ToInd { get; set; }

        [Property]
        public decimal? ToCat { get; set; }
    }

    /// <summary>
    /// 风险评估
    /// </summary>
    [ActiveRecord]
    public class FundRiskAssessment : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundRiskAssessmentId { get; set; }

        [Property]
        public virtual string FundCode { get; set; }

        [Property]
        public DateTime? EffectiveDate { get; set; }

        [Property]
        public string Name { get; set; }

        [Property]
        public decimal? ToInd { get; set; }

        [Property]
        public decimal? ToCat { get; set; }
    }

    /// <summary>
    /// 基金每日回报
    /// </summary>
    [ActiveRecord]
    public class FundRateDetail : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundDetailId { get; set; }

        [Property]
        public virtual string FundCode { get; set; }

        [Property]
        public virtual DateTime NavDate { get; set; }

        [Property]
        public virtual decimal? Nav { get; set; }

        [Property]
        public virtual decimal? NavTotal { get; set; }

        [Property]
        public virtual decimal? NavRate { get; set; }
    }

    /// <summary>
    /// 基金 回报 统计
    /// </summary>
    [ActiveRecord]
    public class FundRateHistory : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundRateHistoryId { get; set; }

        [Property]
        public virtual string FundCode { get; set; }

        [Property]
        public virtual DateTime NavStartDate { get; set; }

        [Property]
        public virtual DateTime NavEndDate { get; set; }

        /// <summary>
        /// d3,d5,d7,d9 w1 w2
        /// </summary>
        [Property]
        public virtual string NavDateType { get; set; }

        [Property]
        public virtual decimal? NavRate { get; set; }

        [Property]
        public virtual double? StandardDeviation { get; set; }

        [Property]
        public virtual double? SharpeRatio { get; set; }
    }
    /// <summary>
    /// 行业
    /// </summary>
    [ActiveRecord]
    public class FundIndustry : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundIndustryId { get; set; }
        [Property]
        public virtual string IndustryCode { get; set; }
        [Property]
        public virtual string IndustryName { get; set; }

    }
    /// <summary>
    /// 基金行业比重
    /// </summary>
    [ActiveRecord]
    public class FundIndustryWeight : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long FundIndustryWeightId { get; set; }
        [Property]
        public virtual string FundCode { get; set; }
        [Property]
        public virtual string IndustryCode { get; set; }
        [Property]
        public virtual decimal? NetAssetWeight { get; set; }
        [Property]
        public virtual decimal? CatAvgWeight { get; set; }
    }
}
