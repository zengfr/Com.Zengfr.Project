using System;
using Castle.ActiveRecord;
using Common.Logging;

using Newtonsoft.Json;
using NHibernate.Criterion;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using ComLib.CsvParse;
using EmitMapper.Conversion;
using Entity;
using Common.Utils;
using Spring.Rest.Utils;

namespace  Stock.Utils
{
    public class SinaStockUtils : RestTemplateUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(SinaStockUtils));
        protected static object lockObj = new object();

        public static void Get实时(string stockCode)
        {
            var url = string.Format("http://hq.sinajs.cn/rn=1434260840000&list={0},{0}_i", stockCode, Random.Next(0, 10000));
            var template = RestTemplateUtils.BuildRestTemplate(url);

            template.GetForMessageAsync<string>(url, r =>
            {
                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        Parse实时(stockCode, r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }
        private static void Parse实时(string stockCode, string js)
        {
            var match1 = Regex.Match(js, string.Format(@"{0}=""(.*?)""", stockCode), RegexOptions);
            var match2 = Regex.Match(js, string.Format(@"{0}_i=""(.*?)""", stockCode), RegexOptions);

            var stockDatail = new StockDatail();
            stockDatail.StockCode = stockCode;

            var isGetSuccess = false;
            if (match1.Success)
            {//五档盘口
                var array1 = match1.Groups[1].Value.Split(',');
                if (array1.Length > 10)
                {
                    stockDatail.StockName = array1[0];
                    stockDatail.今开 = array1[1].ToDecimal();
                    stockDatail.昨收 = array1[2].ToDecimal();
                    stockDatail.现价 = array1[3].ToDecimal(); //现价
                    stockDatail.最高 = array1[4].ToDecimal();
                    stockDatail.最低 = array1[5].ToDecimal();
                    stockDatail.成交量 = array1[8].ToDecimal();
                    stockDatail.成交额 = array1[9].ToDecimal();
                    stockDatail.EffectDate = array1[array1.Length - 3].ToDateTime();
                }
            }
            isGetSuccess = match1.Success && match2.Success && stockDatail.EffectDate.HasValue;
            if (isGetSuccess)
            {
                var array2 = match2.Groups[1].Value.Split(',');
                if (array2.Length > 16)
                {
                    stockDatail.前一年每股收益和 = array2[2].ToDecimal();
                    stockDatail.最近四个季度每股收益和 = array2[3].ToDecimal();
                    stockDatail.最近报告的每股净资产 = array2[5].ToDecimal();
                    stockDatail.过去5个交易日平均每分钟成交量 = array2[6].ToDecimal();
                    stockDatail.最近年度净利润 = array2[12].ToDecimal();
                    stockDatail.最近四个季度净利润 = array2[13].ToDecimal();

                    stockDatail.每股收益 = array2[4].ToDecimal();
                    stockDatail.每股净资产 = array2[5].ToDecimal();
                    stockDatail.最新总股本 = array2[7].ToDecimal();
                    stockDatail.最新流通股 = array2[8].ToDecimal();
                    stockDatail.发行价格 = array2[14].ToDecimal();
                    //stockDatail.净资产收益率 = array2[16].ToDecimal();

                    if (stockDatail.昨收 > 0) stockDatail.振幅 = (stockDatail.最高 - stockDatail.最低) / stockDatail.昨收;
                    if (stockDatail.最新流通股 > 0) stockDatail.换手率 = stockDatail.成交量 / stockDatail.最新流通股;

                    if (stockDatail.最近四个季度每股收益和 > 0) stockDatail.市盈率TTM = stockDatail.现价 / stockDatail.最近四个季度每股收益和;// (stockDatail.每股收益 * 4);
                    if (stockDatail.每股净资产 > 0) stockDatail.市净率 = stockDatail.现价 / stockDatail.每股净资产;

                }
            }
            if (isGetSuccess)
            {

                var item = ActiveRecordMediator<StockDatail>.FindOne(
                     Restrictions.Eq("StockCode", stockDatail.StockCode),
                               Restrictions.Eq("EffectDate", stockDatail.EffectDate)
                               );
                if (item != null)
                {
                    stockDatail.StockDatailId = item.StockDatailId;
                    stockDatail.DataChangeLastTime = DateTime.Now;
                    ActiveRecordMediator<Stock和讯指标云>.Update(stockDatail);
                }
                else
                {
                    stockDatail.DataChangeLastTime = DateTime.Now;
                    ActiveRecordMediator<Stock和讯指标云>.Create(stockDatail);
                }
            }

        }
        public static void GetFuQuanMarketHistory(string stockCode)
        {
            var day = DateTime.Today;
            for (int i = 0; i < 5; i++)
            {
                GetFuQuanMarketHistory(stockCode, day.Year, i);
                GetFuQuanMarketHistory(stockCode, day.Year - 1, i);
            }
        }
        public static void GetFuQuanMarketHistory(string stockCode, int year, int jidu)
        {
            var code = stockCode.Replace("sh", "").Replace("sz", ""); ;
            var url = string.Format("http://vip.stock.finance.sina.com.cn/corp/go.php/vMS_FuQuanMarketHistory/stockid/{0}.phtml?year={1}&jidu={2}", code, year, jidu);
            var template = RestTemplateUtils.BuildRestTemplate(url, "gb2312");

            template.GetForMessageAsync<string>(url, r =>
            {
                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        ParseFuQuanMarketHistory(stockCode, r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }
        private static void ParseFuQuanMarketHistory(string stockCode, string html)
        {
            var match = Regex.Match(html, @"<table id=""FundHoldSharesTable"">(.*?)</table>", RegexOptions);
            if (match.Success)
            {
                html = match.Groups[0].Value;
                html = Regex.Replace(html, "<thead>(.*?)</thead>", "", RegexOptions);
            }
            else
            {
                return;
            }
            DataTable table = HtmlTableParser.ParseTable(html);
            StockFuQuanMarketHistory stockFuQuanMarketHistory;
            int index = 0;
            foreach (DataRow dr in table.Rows)
            {
                if (index++ > 0)
                {
                    stockFuQuanMarketHistory = ConvertTo(stockCode, dr);

                    var item = ActiveRecordMediator<StockFuQuanMarketHistory>.FindOne(
                         Restrictions.Eq("StockCode", stockFuQuanMarketHistory.StockCode),
                                   Restrictions.Eq("EffectDate", stockFuQuanMarketHistory.EffectDate)
                                   );
                    if (item != null)
                    {
                        return;//
                        stockFuQuanMarketHistory.StockFuQuanMarketHistoryId = item.StockFuQuanMarketHistoryId;
                        stockFuQuanMarketHistory.DataChangeLastTime = DateTime.Now;
                        ActiveRecordMediator<Stock和讯指标云>.Update(stockFuQuanMarketHistory);
                    }
                    else
                    {
                        stockFuQuanMarketHistory.DataChangeLastTime = DateTime.Now;
                        ActiveRecordMediator<Stock和讯指标云>.Create(stockFuQuanMarketHistory);
                    }
                }
            }



        }
        private static StockFuQuanMarketHistory ConvertTo(string stockCode, DataRow dr)
        {
            var stockFuQuanMarketHistory = new StockFuQuanMarketHistory();
            stockFuQuanMarketHistory.DataChangeLastTime = DateTime.Now;
            stockFuQuanMarketHistory.EffectDate = ClearDataRowHtml(dr, 0).ToDateTime();
            stockFuQuanMarketHistory.StockCode = stockCode;
            stockFuQuanMarketHistory.开盘价 = ClearDataRowHtml(dr, 1).ToDecimal();
            stockFuQuanMarketHistory.最高价 = ClearDataRowHtml(dr, 2).ToDecimal();
            stockFuQuanMarketHistory.收盘价 = ClearDataRowHtml(dr, 3).ToDecimal();
            stockFuQuanMarketHistory.最低价 = ClearDataRowHtml(dr, 4).ToDecimal();
            stockFuQuanMarketHistory.交易量 = ClearDataRowHtml(dr, 5).ToDouble();
            stockFuQuanMarketHistory.交易金额 = ClearDataRowHtml(dr, 6).ToDouble();
            stockFuQuanMarketHistory.复权因子 = ClearDataRowHtml(dr, 7).ToDecimal();
            return stockFuQuanMarketHistory;
        }
        private static string ClearDataRowHtml(DataRow dr, int index)
        {
            var html = dr[index].ToString();
            html = html.Replace(@"[\r\n\t]", "");
            html = html.Replace(@"<div align=""center"">", "");
            html = html.Replace(@"</div>", "");
            html = Regex.Replace(html, @"<a(.*?)>(.*?)</a>", "$2", RegexOptions);
            return html;
        }
        public static void Get主力连续净流(int page)
        {
            string url = string.Format("http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/MoneyFlow.ssl_bkzj_lxjlr?page={0}&num=20&sort=cnt_r0x_ratio&asc=0&bankuai=", page);

            var template = RestTemplateUtils.BuildRestTemplate(url);

            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        Parselxjlr(r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }
        private static void Parselxjlr(string json)
        {
            json = json.Replace("e", "E");
            var lxjlrs = JsonConvert.DeserializeObject<SinaStocklxjlr.item[]>(json);

            if (lxjlrs != null)
            {
                foreach (var lxjlr in lxjlrs)
                {
                    lock (lockObj)
                    {
                        var item = ActiveRecordMediator<Stock主力连续净流>.FindOne(
                            Restrictions.Eq("代码", lxjlr.symbol)
                            );
                        if (item == null)
                        {
                            item = new Stock主力连续净流();
                        }
                        item.代码 = lxjlr.symbol;
                        item.阶段换手率 = lxjlr.turnover / 100;
                        item.阶段涨跌幅 = lxjlr.changeratio * 100;
                        item.净流率 = lxjlr.ratioamount * 100;
                        item.净流万 = lxjlr.netamount;
                        item.名称 = lxjlr.name;
                        item.天数 = lxjlr.cnt_r0x_ratio;
                        item.主力净流万 = lxjlr.r0_net;
                        item.主力罗盘 = lxjlr.r0x_ratio * 100;
                        //lxjlr.trade  //lxjlr.amount;
                        item.DataChangeLastTime = DateTime.Now;
                        if (item.Stock主力连续净流Id > 0)
                            ActiveRecordMediator<Stock主力连续净流>.Update(item);
                        else
                            ActiveRecordMediator<Stock主力连续净流>.Create(item);
                    }
                }
            }
        }
        private static void Parsessggzj(string json)
        {
            json = json.Replace("e", "E");
            var ssggzjs = JsonConvert.DeserializeObject<SinaStockssggzj.item[]>(json);
            if (ssggzjs != null)
            {
                foreach (var ssggzj in ssggzjs)
                {
                    lock (lockObj)
                    {
                        var item = ActiveRecordMediator<Stock主力净流>.FindOne(
                            Restrictions.Eq("代码", ssggzj.symbol)
                            );
                        if (item == null)
                        {
                            item = new Stock主力净流();
                        }
                        item.代码 = ssggzj.symbol;
                        item.换手率 = ssggzj.turnover / 100;
                        item.名称 = ssggzj.name;
                        item.涨跌幅 = ssggzj.changeratio * 100;
                        item.主力净流入率 = ssggzj.r0_ratio * 100;
                        item.主力净流入万 = ssggzj.r0_net;
                        item.主力流出万 = ssggzj.r0_out;
                        item.主力流入万 = ssggzj.r0_in;
                        item.主力罗盘 = ssggzj.r0x_ratio;
                        item.最新价 = ssggzj.trade;
                        item.DataChangeLastTime = DateTime.Now;
                        if (item.Stock主力净流Id > 0)
                            ActiveRecordMediator<Stock主力净流>.Update(item);
                        else
                            ActiveRecordMediator<Stock主力净流>.Create(item);
                    }
                }
            }

        }
        public static void Get主力净流(int page)
        {
            string url = string.Format("http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/MoneyFlow.ssl_bkzj_ssggzj?page={0}&num=20&sort=r0_ratio&asc=0&bankuai=&shichang=", page);

            var template = RestTemplateUtils.BuildRestTemplate(url);

            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        Parsessggzj(r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }
        /// <summary>
        /// 股票详情页
        /// </summary>
        /// <param name="daima"></param>
        /// <param name="page"></param>
        public static void Get资金流入趋势(string daima, int page)
        {
            string url = string.Format("http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/MoneyFlow.ssl_qsfx_zjlrqs?page={1}&num=20&sort=opendate&asc=0&daima={0}", daima, page);

            var template = RestTemplateUtils.BuildRestTemplate(url);

            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        Parseqsfx_zjlrqs(daima, r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });
        }

        private static void Parseqsfx_zjlrqs(string daima, string json)
        {
            json = json.Contains("ERROR:") ? string.Empty : json.Replace("e", "E");
            var zjlrqss = JsonConvert.DeserializeObject<SinaStockqsfx_zjlrqs.item[]>(json);
            if (zjlrqss != null)
            {
                foreach (var zjlrqs in zjlrqss)
                {
                    lock (lockObj)
                    {
                        var item = ActiveRecordMediator<Stock资金流入趋势>.FindOne(
                            Restrictions.Eq("代码", daima), Restrictions.Eq("日期", zjlrqs.opendate)
                            );
                        if (item == null)
                        {
                            item = new Stock资金流入趋势();
                        }
                        item.代码 = daima;
                        item.日期 = zjlrqs.opendate;
                        item.换手率 = zjlrqs.turnover / 100;
                        item.涨跌幅 = zjlrqs.changeratio * 100;
                        item.行业净流入 = zjlrqs.cate_na;
                        item.行业净流入率 = zjlrqs.cate_ra * 100;
                        item.净流入率 = zjlrqs.ratioamount;
                        item.净流入万 = zjlrqs.netamount;
                        item.收盘价 = zjlrqs.trade;
                        item.主力净流入率 = zjlrqs.r0_ratio;
                        item.主力净流入万 = zjlrqs.r0_net;
                        item.主力罗盘 = zjlrqs.r0x_ratio;//主力罗盘
                        item.DataChangeLastTime = DateTime.Now;
                        if (item.Stock资金流入趋势Id > 0)
                            return;// ActiveRecordMediator<Stock资金流入趋势>.Update(item);
                        else
                            ActiveRecordMediator<Stock资金流入趋势>.Create(item);
                    }
                }
            }
        }


        public static void GetBillList大单数据(string daima, DateTime date, int page)
        {
            var url =
                string.Format(
                    @"http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/CN_Bill.GetBillList?symbol={0}&num=60&page={1}&sort=ticktime&asc=0&volume=0&amount=500000&type=0&day={2}",
                    daima, page, date.ToString("yyyy-MM-dd"));
            var template = RestTemplateUtils.BuildRestTemplate(url);

            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        Parse_BillList(date, r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });

        }

        private static void Parse_BillList(DateTime date, string json)
        {
            json = json.Contains("ERROR:") ? string.Empty : json.Replace("e", "E");
            var billList = JsonConvert.DeserializeObject<SinaStock_BillList.item[]>(json);
            if (billList != null)
            {
                foreach (var billItem in billList)
                {
                    billItem.date = date;
                    lock (lockObj)
                    {
                        var item = ActiveRecordMediator<SotckBill>.FindOne(
                            Restrictions.Eq("symbol", billItem.symbol),
                            Restrictions.Eq("date", billItem.date),
                            Restrictions.Eq("ticktime", billItem.ticktime)
                            );
                        if (item == null)
                        {
                            item = new SotckBill();
                        }
                        item.date = billItem.date;
                        item.symbol = billItem.symbol;
                        item.ticktime = billItem.ticktime;
                        item.kind = billItem.kind;
                        item.name = billItem.name;
                        item.prev_price = billItem.prev_price;
                        item.price = billItem.price;
                        item.volume = billItem.volume;

                        item.DataChangeLastTime = DateTime.Now;
                        if (item.SotckBillId > 0)
                            return;// ActiveRecordMediator<SotckBill>.Update(item);
                        else
                            ActiveRecordMediator<SotckBill>.Create(item);
                    }
                }
            }
        }

        public static void Get历史成交分布(string daima, int page)
        {
            var url =
                string.Format(
                    @"http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/MoneyFlow.ssl_qsfx_lscjfb?page={1}&num=20&sort=opendate&asc=0&daima={0}",
                    daima, page);
            var template = RestTemplateUtils.BuildRestTemplate(url);

            template.GetForMessageAsync<string>(url, r =>
            {

                if (r.Error == null)
                {
                    if (!string.IsNullOrEmpty(r.Response.Body))
                    {
                        Parse_历史成交分布(daima, r.Response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", r.Error, string.Empty);
                }
            });

        }
        private static void Parse_历史成交分布(string daima, string json)
        {
            json = json.Contains("ERROR:") ? string.Empty : json.Replace("e", "E");
            var billList = JsonConvert.DeserializeObject<SinaStock_lscjfb.item[]>(json);
            if (billList != null)
            {
                foreach (var billItem in billList)
                {
                    lock (lockObj)
                    {
                        var item = ActiveRecordMediator<Stock历史成交分布>.FindOne(
                            Restrictions.Eq("代码", daima),
                            Restrictions.Eq("opendate日期", billItem.opendate)
                            );
                        if (item == null)
                        {
                            item = new Stock历史成交分布();
                        }
                        item.changeratio涨跌幅 = billItem.changeratio;
                        item.netamount净流入 = billItem.netamount;
                        item.opendate日期 = billItem.opendate;
                        item.r0 = billItem.r0;
                        item.r0_net超大单 = billItem.r0_net;
                        item.r1 = billItem.r1;
                        item.r1_net大单 = billItem.r1_net;
                        item.r2 = billItem.r2;
                        item.r2_net小单 = billItem.r2_net;
                        item.r3 = billItem.r3;
                        item.r3_net散单 = billItem.r3_net;
                        item.ratioamount净流入率 = billItem.ratioamount;
                        item.trade收盘价 = billItem.trade;
                        item.turnover换手率 = billItem.turnover;
                        item.代码 = daima;


                        item.DataChangeLastTime = DateTime.Now;
                        if (item.Stock历史成交分布Id > 0)
                            return;// ActiveRecordMediator<Stock历史成交分布>.Update(item);
                        else
                            ActiveRecordMediator<Stock历史成交分布>.Create(item);
                    }
                }
            }
        }
        /// <summary>
        /// 例如 date=2016-08-05&symbol=sh601989
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="openDate"></param>
        public static IEnumerable<StockTradeHistory> Get历史成交明细(string symbol, DateTime openDate)
        {
            var url = string.Format(@"http://market.finance.sina.com.cn/downxls.php?date={1}&symbol={0}",
                   symbol, openDate.ToString("yyyy-MM-dd"));
            var template = RestTemplateUtils.BuildRestTemplate(url, "gb2312");
            System.Threading.Thread.Sleep(500);
            var response = template.GetForMessage<string>(url);
               if (response!= null)
              {
                    if (!string.IsNullOrEmpty(response.Body))
                    {
                       return Parse_历史成交明细(symbol, openDate,response.Body);
                    }
                }
                else
                {
                    log.ErrorFormat("{0}", url, string.Empty);
                }
            return null;
        }
        private static IEnumerable<StockTradeHistory> Parse_历史成交明细(string symbol, DateTime openDate, string csvString)
        {
            var tradehistoryItems = new List<StockTradeHistory>();
            if (string.IsNullOrWhiteSpace(csvString) || csvString.IndexOf("<script") != -1)
            {
                return tradehistoryItems;
            }
            CsvDoc csvDoc = Csv.LoadText(csvString, true, true, '\t');
            var csvData = csvDoc.ToDataTable();
            var rows = csvData.Rows.Count;
            var index = 0;
            log.InfoFormat("历史成交明细:{0}/{1}->{2}:{3}", index, rows, symbol, openDate);
            if (rows > 0)
            {
                //ActiveRecordMediator<Sina_TradeHistory>.DeleteAll(string.Format("OpenDate='{0}'", openDate.ToString("yyyy-MM-dd")));
                foreach (DataRow dr in csvData.Rows)
                {
                    StockTradeHistory tradehistory = new StockTradeHistory();
                    tradehistory.Symbol = symbol;
                    tradehistory.OpenDate = openDate;
                    tradehistory.ItemIndex = rows - (index++);
                    tradehistory.成交时间 = dr["成交时间"].AsString().ToDateTime();
                   
                    if (tradehistory.成交时间.HasValue)
                    {
                        tradehistory.OpenDateTime = openDate.Add(tradehistory.成交时间.Value.TimeOfDay);

                        tradehistory.价格变动 = dr["价格变动"].AsString().ToDecimal();
                        var sz = dr["性质"].AsString();
                        tradehistory.性质 = NullableConverter.ToSByte(sz=="买盘"?1:sz=="卖盘"?-1:0);
                        tradehistory.成交价 = dr["成交价"].AsString().ToDecimal();
                        tradehistory.成交量手 = NullableConverter.ToInt64(dr["成交量(手)"]);
                        tradehistory.成交额元 = dr["成交额(元)"].AsString().ToDecimal();

                        tradehistoryItems.Add(tradehistory);
                    }

                }
            }
            return tradehistoryItems;

        }
        /// <summary>
        /// 主力净流
        /// </summary>
        public class SinaStockssggzj
        {
            public virtual item[] items { get; set; }
            public class item
            {
                public string symbol { get; set; }
                public string name { get; set; }
                public decimal trade { get; set; }
                public double changeratio { get; set; }
                public decimal turnover { get; set; }
                public decimal amount { get; set; }
                public decimal inamount { get; set; }
                public decimal outamount { get; set; }

                public decimal netamount { get; set; }
                public double ratioamount { get; set; }

                public decimal r0_in { get; set; }
                public decimal r0_out { get; set; }
                public decimal r0_net { get; set; }

                public decimal r3_in { get; set; }
                public decimal r3_out { get; set; }
                public decimal r3_net { get; set; }

                public double r0_ratio { get; set; }
                public double r3_ratio { get; set; }
                public double r0x_ratio { get; set; }

            }
        }
        /// <summary>
        /// 主力连续净流
        /// </summary>
        public class SinaStocklxjlr
        {
            public virtual item[] items { get; set; }
            public class item
            {
                public string symbol { get; set; }
                public string name { get; set; }
                public int cnt_r0x_ratio { get; set; }

                public decimal trade { get; set; }
                public double changeratio { get; set; }
                public decimal turnover { get; set; }

                public decimal amount { get; set; }
                public decimal netamount { get; set; }
                public double ratioamount { get; set; }


                public decimal r0_net { get; set; }
                public double r0x_ratio { get; set; }

            }
        }
        /// <summary>
        /// 趋势分析 资金流入趋势
        /// </summary>
        public class SinaStockqsfx_zjlrqs
        {
            public virtual item[] items { get; set; }
            public class item
            {
                public DateTime opendate { get; set; }
                public decimal trade { get; set; }
                public double changeratio { get; set; }
                public decimal turnover { get; set; }
                public decimal netamount { get; set; }
                public double ratioamount { get; set; }

                public decimal r0_net { get; set; }
                public double? r0_ratio { get; set; }
                /// <summary>
                /// 主力罗盘
                /// </summary>
                public double r0x_ratio { get; set; }

                public int cnt_r0x_ratio { get; set; }
                /// <summary>
                /// 行业净流入率
                /// </summary>
                public double cate_ra { get; set; }
                public double cate_na { get; set; }

            }
        }
        /// <summary>
        /// 大单数据
        /// </summary>
        public class SinaStock_BillList
        {
            public virtual item[] items { get; set; }

            public class item
            {
                public string symbol { get; set; }
                public string name { get; set; }
                public DateTime date { get; set; }
                public DateTime ticktime { get; set; }
                /// <summary>
                /// 成交价
                /// </summary>
                public decimal price { get; set; }
                /// <summary>
                /// 成交量(手)　
                /// </summary>
                public decimal volume { get; set; }

                public decimal prev_price { get; set; }
                /// <summary>
                /// 买卖盘性质 E 中性  U 买 D 卖盘
                /// </summary>
                public string kind { get; set; }
            }
        }
        /// <summary>
        /// 历史成交分布
        /// </summary>
        public class SinaStock_lscjfb
        {
            public virtual item[] items { get; set; }

            public class item
            {
                public DateTime opendate { get; set; }
                /// <summary>
                /// 收盘价
                /// </summary>
                public decimal? trade { get; set; }
                /// <summary>
                /// 涨跌幅
                /// </summary>
                public double? changeratio { get; set; }
                /// <summary>
                /// 换手率
                /// </summary>
                public decimal? turnover { get; set; }
                /// <summary>
                /// 净流入/万
                /// </summary>
                public decimal? netamount { get; set; }
                /// <summary>
                /// 净流入率
                /// </summary>
                public double? ratioamount { get; set; }

                public decimal? r0 { get; set; }
                public decimal? r1 { get; set; }
                public decimal? r2 { get; set; }
                public decimal? r3 { get; set; }
                /// <summary>
                /// 超大单	
                /// </summary>
                public decimal? r0_net { get; set; }
                /// <summary>
                /// 大单
                /// </summary>
                public decimal? r1_net { get; set; }
                /// <summary>
                /// 小单	
                /// </summary>
                public decimal? r2_net { get; set; }
                /// <summary>
                /// 散单
                /// </summary>
                public decimal? r3_net { get; set; }


            }

        }

         
    }
}
