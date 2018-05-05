//-----------------------------------------------------------------------
// <copyright file="DispatcherActor.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using Akka.Actor;
using SymbolLookup.Actors.Messages;

using System.Threading.Tasks;
using ConsoleApplication3.Stock;
using Castle.ActiveRecord;
using Common.Logging;
using Entity;
namespace SymbolLookup.Actors
{
    /// <summary>
    /// Root actor used by the application
    /// </summary>
    public class SymbolDispatcherActor : TypedActor, 
        IHandle<DownloadAllSymbolDataCommand>, IHandle<DownloadSymbolDataCommand>, IHandle<StockTradeHistory>
    {
        protected static ILog log = LogManager.GetLogger(typeof(SymbolDispatcherActor));

        ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 };
        private readonly EventHandler<DownloadSymbolDataCommand> _commandHandler;
        private readonly EventHandler<string> _statusHandler;
        private IActorRef symbolDownloadActor = Context.ActorOf(Props.Create(() => new SymbolDownloadActor()), "symbolDownload");
        private IActorRef symbolStorageActor = Context.ActorOf(Props.Create(() => new SymbolStorageActor()).WithDispatcher("my-dispatcher"), "symbolStorage");

        public SymbolDispatcherActor(EventHandler<DownloadSymbolDataCommand> dataHandler, EventHandler<string> statusHandler)
        {
            _commandHandler = dataHandler;
            _statusHandler = statusHandler;
        }
        public void Handle(DownloadAllSymbolDataCommand message)
        {
            _statusHandler(this, string.Format("DownloadAllSymbolData->{0},{1}", string.Empty, string.Empty));

 
            var stocks = ActiveRecordMediator<Entity.Stock>.FindAll();//new Order[] { Order.Desc("DataChangeLastTime") });
            var code = string.Empty;
            var index = 0; var total = stocks.Length;
            stocks = stocks.OrderBy(t => t.StockCode).ToArray();
            foreach (var stock in stocks)
            {
                code = stock.StockCode;
                log.InfoFormat("Process->StockCode:{0}/{1}->{2}:{3}", index++, total, code, stock.StockName);
                if ((!code.Contains("sh300")) && (!code.Contains("sz300")))
                {
                    if ((code.Contains("sh6")) || (code.Contains("sz0")))
                    {
                        Handle(new DownloadSymbolDataCommand() { Symbol=code, OpenDate= message.OpenDate });
                    }
                }
            }

            _statusHandler(this, string.Empty);
        }

        public void Handle(DownloadSymbolDataCommand message)
        {
            _statusHandler(this, string.Format("DownloadSymbolData->{0},{1}", message.Symbol, message.OpenDate));

            //var stockActor = Context.ActorOf(Props.Create<SymbolDownloadActor>(),
            //   string.Format("symbol-{0}-{1}", message.Symbol, message.OpenDate.Value.ToString("yyyyMMdd")));
            symbolDownloadActor.Tell(message);
                 
            _statusHandler(this, string.Empty);
        }
       

        public void Handle(StockTradeHistory message)
        {
            symbolStorageActor.Tell(message);
        }
        public void Handle(StockTradeHistoryStatistic60 message)
        {
            symbolStorageActor.Tell(message);
        }
    }
}

