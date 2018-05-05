using System;

 
using Castle.ActiveRecord;

namespace Entity
{
    [ActiveRecord]
    public class Stock : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long StockId { get; set; }
        [Property]
        public virtual string StockCode { get; set; }
        [Property]
        public virtual string StockName { get; set; }

    }
    [ActiveRecord]
    public class StockFuQuanMarketHistory : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long StockFuQuanMarketHistoryId { get; set; }
        [Property]
        public virtual string StockCode { get; set; }
        [Property]
        public virtual DateTime? EffectDate { get; set; }
        [Property]
        public virtual decimal? 开盘价 { get; set; }
        [Property]
        public virtual decimal? 最高价 { get; set; }
        [Property]
        public virtual decimal? 收盘价 { get; set; }
        [Property]
        public virtual decimal? 最低价 { get; set; }
        [Property]
        public virtual double? 交易量 { get; set; }
        [Property]
        public virtual double? 交易金额 { get; set; }
        [Property]
        public virtual decimal? 复权因子 { get; set; }

    }
    [ActiveRecord]
    public class StockDatail : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long StockDatailId { get; set; }
        [Property]
        public virtual string StockCode { get; set; }
        [Property]
        public virtual string StockName { get; set; }
        [Property]
        public virtual DateTime? EffectDate { get; set; }
        [Property]
        public virtual decimal? 现价 { get; set; }
        [Property]
        public virtual decimal? 今开 { get; set; }
        [Property]
        public virtual decimal? 最高 { get; set; }
        [Property]
        public virtual decimal? 最低 { get; set; }
        [Property]
        public virtual decimal? 昨收 { get; set; }
        [Property]
        public virtual decimal? 成交量 { get; set; }
        [Property]
        public virtual decimal? 成交额 { get; set; }
        [Property]
        public virtual decimal? 总市值 { get; set; }
        [Property]
        public virtual decimal? 流通市值 { get; set; }
        [Property]
        public virtual decimal? 振幅 { get; set; }
        [Property]
        public virtual decimal? 换手率 { get; set; }
        [Property]
        public virtual decimal? 市净率 { get; set; }
        [Property]
        public virtual decimal? 市盈率TTM { get; set; }

        [Property]
        public virtual decimal? 每股收益 { get; set; }
        [Property]
        public virtual decimal? 每股净资产 { get; set; }
        [Property]
        public virtual decimal? 每股经营现金流净额 { get; set; }
        [Property]
        public virtual decimal? 净资产收益率 { get; set; }
        [Property]
        public virtual decimal? 每股未分配利润 { get; set; }
        [Property]
        public virtual decimal? 每股资本公积金 { get; set; }

        [Property]
        public virtual decimal? 最新总股本 { get; set; }
        [Property]
        public virtual decimal? 最新流通股 { get; set; }
        [Property]
        public virtual decimal? 注册资本 { get; set; }
        [Property]
        public virtual decimal? 发行价格 { get; set; }

        [Property]
        public virtual decimal? 最近四个季度每股收益和 { get; set; }
        [Property]
        public virtual decimal? 前一年每股收益和 { get; set; }

        [Property]
        public virtual decimal? 最近报告的每股净资产 { get; set; }

        [Property]
        public virtual decimal? 过去5个交易日平均每分钟成交量 { get; set; }
        [Property]
        public virtual decimal? 最近年度净利润 { get; set; }
        [Property]
        public virtual decimal? 最近四个季度净利润 { get; set; }

    }
    /// <summary>
    /// http://vip.stock.finance.sina.com.cn/moneyflow/#lxjlrg
    /// </summary>
    [ActiveRecord]
    public class Stock主力连续净流 : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long Stock主力连续净流Id { get; set; }
        [Property]
        public virtual string 代码 { get; set; }
        [Property]
        public virtual string 名称 { get; set; }
        [Property]
        public virtual int 天数 { get; set; }
        [Property]
        public virtual double 阶段涨跌幅 { get; set; }
        [Property]
        public virtual decimal 阶段换手率 { get; set; }
        [Property]
        public virtual decimal 净流万 { get; set; }
        [Property]
        public virtual double 净流率 { get; set; }
        [Property]
        public virtual decimal 主力净流万 { get; set; }
        [Property]
        public virtual double 主力罗盘 { get; set; }
    }
    /// <summary>
    /// http://vip.stock.finance.sina.com.cn/moneyflow/#zljlrepm
    /// </summary>
    [ActiveRecord]
    public class Stock主力净流 : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long Stock主力净流Id { get; set; }
        [Property]
        public virtual string 代码 { get; set; }
        [Property]
        public virtual string 名称 { get; set; }
        [Property]
        public virtual decimal 最新价 { get; set; }

        [Property]
        public virtual double 涨跌幅 { get; set; }
        [Property]
        public virtual decimal 换手率 { get; set; }
        [Property]
        public virtual decimal 主力流出万 { get; set; }
        [Property]
        public virtual decimal 主力流入万 { get; set; }
        [Property]
        public virtual decimal 主力净流入万 { get; set; }
        [Property]
        public virtual double 主力净流入率 { get; set; }
        [Property]
        public virtual double 主力罗盘 { get; set; }
    }
    /// <summary>
    /// http://vip.stock.finance.sina.com.cn/moneyflow/#!ssfx!sz000063
    /// </summary>
    [ActiveRecord]
    public class Stock资金流入趋势 : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long Stock资金流入趋势Id { get; set; }
        [Property]
        public virtual string 代码 { get; set; }
        [Property]
        public virtual DateTime 日期 { get; set; }
        [Property]
        public virtual decimal 收盘价 { get; set; }

        [Property]
        public virtual double 涨跌幅 { get; set; }
        [Property]
        public virtual decimal 换手率 { get; set; }
        [Property]
        public virtual decimal 净流入万 { get; set; }
        [Property]
        public virtual double 净流入率 { get; set; }

        [Property]
        public virtual decimal 主力净流入万 { get; set; }
        [Property]
        public virtual double? 主力净流入率 { get; set; }
        [Property]
        public virtual double 行业净流入 { get; set; }
        [Property]
        public virtual double 行业净流入率 { get; set; }
        /// <summary>
        /// -180～180
        /// </summary>
        [Property]
        public double 主力罗盘 { get; set; }
    }
    [ActiveRecord]
    public class doctor10jqkascore
    {
        [PrimaryKey]
        public virtual long doctor10jqkascoreId { get; set; }
        [Property]
        public virtual string stockCode { get; set; }
        [Property]
        public virtual string stockName { get; set; }
        [Property]
        public virtual DateTime? effectDate { get; set; }
        [Property]
        public virtual decimal? technicalScore { get; set; }
        [Property]
        public virtual decimal? basicScore { get; set; }
        [Property]
        public virtual decimal? fundsScore { get; set; }
        [Property]
        public virtual decimal? messageScore { get; set; }
        [Property]
        public virtual decimal? tradeScore { get; set; }
        /// <summary>
        /// 综合诊断 分
        /// </summary>
        [Property]
        public virtual decimal? zhScore { get; set; }
        /// <summary>
        /// 综合诊断 率
        /// </summary>
        [Property]
        public virtual decimal? zhScoreRate { get; set; }
        /// <summary>
        /// 平均成本
        /// </summary>
        [Property]
        public virtual decimal? avgAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Property]
        public virtual string type { get; set; }

        [Property]
        public virtual decimal? amount { get; set; }
        /// <summary>
        /// 支撑位
        /// </summary>
        [Property]
        public virtual decimal? zcAmount { get; set; }
        /// <summary>
        /// 压力位
        /// </summary>
        [Property]
        public virtual decimal? ylAmount { get; set; }

    }
    [ActiveRecord]
    public class SotckBill : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long SotckBillId { get; set; }
        [Property]
        public string symbol { get; set; }
        [Property]
        public string name { get; set; }
        [Property]
        public DateTime date { get; set; }
        [Property]
        public DateTime ticktime { get; set; }
        /// <summary>
        /// 成交价
        /// </summary>
        [Property]
        public decimal price { get; set; }
        /// <summary>
        /// 成交量(手)　
        /// </summary>
        [Property]
        public decimal volume { get; set; }

        [Property]
        public decimal? prev_price { get; set; }
        /// <summary>
        /// 买卖盘性质 E 中性  U 买 D 卖盘
        /// </summary>
        [Property]
        public string kind { get; set; }

    }
    [ActiveRecord]
    public class Stock历史成交分布 : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long Stock历史成交分布Id { get; set; }
        [Property]
        public virtual string 代码 { get; set; }
        [Property]
        public DateTime opendate日期 { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        [Property]
        public decimal? trade收盘价 { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        [Property]
        public double? changeratio涨跌幅 { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        [Property]
        public decimal? turnover换手率 { get; set; }
        /// <summary>
        /// 净流入/万
        /// </summary>
        [Property]
        public decimal? netamount净流入 { get; set; }
        /// <summary>
        /// 净流入率
        /// </summary>
        [Property]
        public double? ratioamount净流入率 { get; set; }

        [Property]
        public decimal? r0 { get; set; }
        [Property]
        public decimal? r1 { get; set; }
        [Property]
        public decimal? r2 { get; set; }
        [Property]
        public decimal? r3 { get; set; }
        /// <summary>
        /// 超大单	
        /// </summary>
        [Property]
        public decimal? r0_net超大单 { get; set; }
        /// <summary>
        /// 大单
        /// </summary>
        [Property]
        public decimal? r1_net大单 { get; set; }
        /// <summary>
        /// 小单	
        /// </summary>
        [Property]
        public decimal? r2_net小单 { get; set; }
        /// <summary>
        /// 散单
        /// </summary>
        [Property]
        public decimal? r3_net散单 { get; set; }
    }
    /// <summary>
    /// 历史成交明细
    /// </summary>
    [ActiveRecord]
    public class StockTradeHistory : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryId { get; set; }
        [Property]
        public string Symbol { get; set; }
        [Property]
        public DateTime OpenDate { get; set; }
        [Property]
        public DateTime OpenDateTime { get; set; }
        [Property]
        public DateTime? 成交时间 { get; set; }
        [Property]
        public decimal? 成交价 { get; set; }
        [Property]
        public decimal? 价格变动 { get; set; }
        [Property]
        public long? 成交量手 { get; set; }
        [Property]
        public decimal? 成交额元 { get; set; }
        /// <summary>
        /// 0 中性 -1卖盘 1买盘/ E 中性  U 买 D 卖盘
        /// </summary>
        [Property]
        public int? 性质 { get; set; }
        [Property]
        public int ItemIndex { get; set; }
    }
    /// <summary>
    /// 历史成交 统计 按分钟/5分钟/15分钟/30分钟/60分钟/
    /// </summary>
    public abstract class StockTradeHistoryStatistic
    {
        [Property]
        public virtual string Symbol { get; set; }
        [Property]
        public virtual DateTime OpenDate { get; set; }
        [Property]
        public virtual DateTime OpenDateTime { get; set; }
        [Property(Column = "[Open]")]
        public decimal? Open { get; set; }
        [Property(Column = "[Close]")]
        public virtual decimal? Close { get; set; }
        [Property]
        public virtual decimal? High { get; set; }
        [Property]
        public virtual decimal? Low { get; set; }
        [Property]
        public virtual decimal? Volume { get; set; }
        [Property]
        public virtual decimal? 均价 { get; set; }
        /// <summary>
        /// 买卖盘性质 E 中性  U 买 D 卖盘
        /// </summary>
        [Property]
        public int? kind { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryStatistic60 : StockTradeHistoryStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryStatistic60Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryStatistic30 : StockTradeHistoryStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryStatistic30Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryStatistic15 : StockTradeHistoryStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryStatistic15Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryStatistic05 : StockTradeHistoryStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryStatistic05Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryStatistic03 : StockTradeHistoryStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryStatistic03Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryStatistic01 : StockTradeHistoryStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryStatistic01Id { get; set; }
    }
    /// <summary>
    /// 历史成交 分价表 统计 按分钟/5分钟/15分钟/30分钟/60分钟/
    /// </summary>
    public abstract class StockTradeHistoryPriceStatistic
    {
        [Property]
        public virtual string Symbol { get; set; }
        [Property]
        public virtual DateTime OpenDate { get; set; }
        [Property]
        public virtual DateTime OpenDateTime { get; set; }
        [Property]
        public virtual decimal? 成交价元 { get; set; }
        [Property]
        public virtual decimal? 成交量股 { get; set; }
        [Property]
        public virtual decimal? 占比 { get; set; }
        /// <summary>
        /// 买卖盘性质 E 中性  U 买 D 卖盘
        /// </summary>
        [Property]
        public virtual int? kind { get; set; }

    }
    [ActiveRecord]
    public class StockTradeHistoryPriceStatistic60 : StockTradeHistoryPriceStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryPriceStatistic60Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryPriceStatistic30 : StockTradeHistoryPriceStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryPriceStatistic30Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryPriceStatistic15 : StockTradeHistoryPriceStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryPriceStatistic15Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryPriceStatistic05 : StockTradeHistoryPriceStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryPriceStatistic05Id { get; set; }
    }
    [ActiveRecord]
    public class StockTradeHistoryPriceStatistic03 : StockTradeHistoryPriceStatistic
    {
        [PrimaryKey]
        public virtual long StockTradeHistoryPriceStatistic03Id { get; set; }
    }
    [ActiveRecord]
    public class Stock和讯指标云 : AbstractModelBase
    {
        [PrimaryKey]
        public virtual long Stock和讯指标云Id { get; set; }
        [Property]
        public virtual string stockcode { get; set; }
        [Property]
        public virtual string stockName { get; set; }
        [Property]
        public virtual DateTime effectDate { get; set; }
        [Property]
        public virtual int 交易类指标数 { get; set; }
        [Property]
        public virtual int 交易类看多指标数 { get; set; }
        [Property]
        public virtual int 交易类看空指标数 { get; set; }
        [Property]
        public virtual int 交易类看多指标数变化量 { get; set; }
        [Property]
        public virtual int 交易类看空指标数变化量 { get; set; }
        [Property]
        public virtual int? 提示类指标数 { get; set; }
        [Property]
        public virtual int? 今日发出提示信号指标数 { get; set; }
        [Property]
        public virtual int? 未来10个交易日上涨百分5的概率 { get; set; }
    }
}
