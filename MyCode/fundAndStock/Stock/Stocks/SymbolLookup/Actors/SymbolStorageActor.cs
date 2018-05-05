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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using System.Threading;
using Entity;
namespace SymbolLookup.Actors
{
    public class SymbolStorageActor : TypedActor, 
        IHandle<StockTradeHistory>
    {
        protected static ILog log = LogManager.GetLogger(typeof(SymbolStorageActor));

        ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount*2 };

        public static long Count = 0;
        public void Handle(StockTradeHistory tradehistory)
        {
            log.InfoFormat("SymbolStorageActor->StockTradeHistory:{0},{1},{2},{3}",Count, SymbolDownloadActor.Count, tradehistory.Symbol, tradehistory.OpenDateTime);
            ICriterion[] criterions = new ICriterion[] {
                             Expression.Eq("Symbol", tradehistory.Symbol),
                             Expression.Eq("OpenDate", tradehistory.OpenDate),

                             Expression.Eq("ItemIndex", tradehistory.ItemIndex)
                        };
            DetachedCriteria detachedCriteria = DetachedCriteria.For<StockTradeHistory>();
            criterions.ForEach(t => detachedCriteria.Add(t));
            detachedCriteria.SetProjection(Property.ForName("StockTradeHistoryId"));
            var item = ActiveRecordMediator<StockTradeHistory>.FindFirst(criterions);
            if (item != null && item.StockTradeHistoryId > 0)
            {
                tradehistory.StockTradeHistoryId = item.StockTradeHistoryId;
                if (tradehistory.OpenDate >= DateTime.Today.AddDays(-1))
                {
                    ActiveRecordMediator<StockTradeHistory>.Update(tradehistory);
                }
            }
            else
            {
                ActiveRecordMediator<StockTradeHistory>.Create(tradehistory);
            }
            Interlocked.Increment(ref Count);
        }
        public void Handle(StockTradeHistoryStatistic60 tradehistoryStatistic)
        {
            log.InfoFormat("SymbolStorageActor->StockTradeHistoryStatistic60:{0},{1},{2},{3}", Count, SymbolDownloadActor.Count, tradehistoryStatistic.Symbol, tradehistoryStatistic.OpenDateTime);
            ICriterion[] criterions = new ICriterion[] {
                             Expression.Eq("Symbol", tradehistoryStatistic.Symbol),
                             Expression.Eq("OpenDate", tradehistoryStatistic.OpenDate),
                             Expression.Eq("OpenDateTime", tradehistoryStatistic.OpenDateTime),
                             Expression.Eq("kind", tradehistoryStatistic.kind)
                        };
            DetachedCriteria detachedCriteria = DetachedCriteria.For<StockTradeHistoryStatistic60>();
            criterions.ForEach(t => detachedCriteria.Add(t));
            detachedCriteria.SetProjection(Property.ForName("StockTradeHistoryId"));
            var item = ActiveRecordMediator<StockTradeHistoryStatistic60>.FindFirst(criterions);
            if (item != null && item.StockTradeHistoryStatistic60Id > 0)
            {
                tradehistoryStatistic.StockTradeHistoryStatistic60Id = item.StockTradeHistoryStatistic60Id;
                if (tradehistoryStatistic.OpenDate >= DateTime.Today.AddDays(-1))
                {
                    ActiveRecordMediator<StockTradeHistory>.Update(tradehistoryStatistic);
                }
            }
            else
            {
                ActiveRecordMediator<StockTradeHistory>.Create(tradehistoryStatistic);
            }
            Interlocked.Increment(ref Count);
        }
    }
 
}

