using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Entity;
using ConsoleApplication3.Stock;

using NHibernate.Criterion;

using System.Text.RegularExpressions;

using Common.Logging;

using Spring.Http;
using Stock.Utils;

namespace ConsoleApplication3.Stock
{
    public class StockUtils
    {
        protected static ILog log = LogManager.GetLogger(typeof(StockUtils));
        public static void process()
        {
            EastmoneyStockUtils.GetStockCodes();
            //for (var page = 0; page < 165; page++)
            //{
            //    SinaStockUtils.Get主力连续净流(page);
            //    SinaStockUtils.Get主力净流(page);
            //}
        }
        //public static void process2()
        //{
           
        //    var stocks = ActiveRecordMediator<Stock>.FindAll();//new Order[] { Order.Desc("DataChangeLastTime") });
        //    var code = string.Empty;
        //    var index = 0; var total = stocks.Length;
        //    stocks = stocks.OrderBy(t=>t.StockCode).ToArray();
        //    foreach (var stock in stocks)
        //    {
        //        code = stock.StockCode;
        //        log.InfoFormat("Process->StockCode:{0}/{1}->{2}:{3}", index++, total, code, stock.StockName);
        //        if ((!code.Contains("sh300")) && (!code.Contains("sz300")))
        //        {
        //            if ((code.Contains("sh6")) || (code.Contains("sz0")))
        //            {
        //                doctor10jqkaUtils.GetStockScores(code);
        //                hexunUtils.zhibiao(code);

        //                SinaStockUtils.Get实时(code);
        //                for (int i = 0; i < 1; i++) {
        //                    SinaStockUtils.Get历史成交明细(code,DateTime.Today.AddDays(-i));
        //                }

        //                SinaStockUtils.Get资金流入趋势(code, 1);
        //                //SinaStockUtils.Get资金流入趋势(code, 2);
        //                //SinaStockUtils.Get资金流入趋势(code, 3);
        //                SinaStockUtils.GetBillList大单数据(code, StockAndFundDateUtils.GetDateTime(0), 1);
        //                SinaStockUtils.Get历史成交分布(code, 1);
                        
        //                SinaStockUtils.GetFuQuanMarketHistory(code);
        //            }
        //        }
        //    }
        //}
    }
}
