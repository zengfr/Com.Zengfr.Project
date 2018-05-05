//-----------------------------------------------------------------------
// <copyright file="TickerActors.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Net.Http;
using Akka.Actor;
using QDFeedParser;
using SymbolLookup.Actors.Messages;

using Common.Logging;
using ConsoleApplication3.Stock;
using System.Threading.Tasks;
using System.Threading;
using Entity;
using Stock.Utils;

namespace SymbolLookup.Actors
{
    public class SymbolDownloadActor : TypedActor, 
        IHandle<DownloadSymbolDataCommand>
    {
        protected static ILog log = LogManager.GetLogger(typeof(SymbolDownloadActor));

        ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 };
        public static long Count = 0;
        public void Handle(DownloadSymbolDataCommand message)
        {
            var symbol = message.Symbol??string.Empty;
            var openDate = message.OpenDate;
            log.InfoFormat("SymbolDownloadActor->{0},{1}", symbol, openDate);
            if (!string.IsNullOrWhiteSpace(symbol))
            {
                if (openDate.HasValue)
                {

                    var stockTradeHistoryItems=SinaStockUtils.Get历史成交明细(symbol, openDate.Value);
                    if (stockTradeHistoryItems != null)
                    {
                        var sender = Context.Sender;
                        Parallel.ForEach(stockTradeHistoryItems,parallelOptions,stockTradeHistoryItem =>
                        {
                            sender.Tell(stockTradeHistoryItem);
                            Interlocked.Increment(ref Count);
                        });

                        var tradehistoryStatistic60Items1 = StockTradeConvert.Convert<StockTradeHistoryStatistic60>(stockTradeHistoryItems,60,1);
                        Parallel.ForEach(tradehistoryStatistic60Items1, parallelOptions, tradehistoryStatistic60Item1 =>
                        {
                            sender.Tell(tradehistoryStatistic60Item1);
                            Interlocked.Increment(ref Count);
                        });

                        var tradehistoryStatistic60Items2 = StockTradeConvert.Convert<StockTradeHistoryStatistic60>(stockTradeHistoryItems, 60, -1);
                        Parallel.ForEach(tradehistoryStatistic60Items2, parallelOptions, tradehistoryStatistic60Item2 =>
                        {
                            sender.Tell(tradehistoryStatistic60Item2);
                            Interlocked.Increment(ref Count);
                        });

                        var tradehistoryStatistic60Items3 = StockTradeConvert.Convert<StockTradeHistoryStatistic60>(stockTradeHistoryItems, 60, 0);
                        Parallel.ForEach(tradehistoryStatistic60Items3, parallelOptions, tradehistoryStatistic60Item3 =>
                        {
                            sender.Tell(tradehistoryStatistic60Item3);
                            Interlocked.Increment(ref Count);
                        });
                    }
                }
                else
                {
                    //doctor10jqkaUtils.GetStockScores(symbol);
                    //hexunUtils.zhibiao(symbol);

                    //SinaStockUtils.Get实时(symbol);


                    //SinaStockUtils.Get资金流入趋势(symbol, 1);
                    //SinaStockUtils.Get资金流入趋势(symbol, 2);
                    //SinaStockUtils.Get资金流入趋势(symbol, 3);
                    //SinaStockUtils.GetBillList大单数据(symbol, StockAndFundDateUtils.GetDateTime(0), 1);
                    //SinaStockUtils.Get历史成交分布(symbol, 1);

                    //SinaStockUtils.GetFuQuanMarketHistory(symbol);
                }
            }
        }


    }
 
}

