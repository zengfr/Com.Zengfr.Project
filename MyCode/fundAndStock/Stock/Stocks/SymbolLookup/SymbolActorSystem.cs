//-----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using Akka.Actor;
using QDFeedParser;
using SymbolLookup.Actors;
using SymbolLookup.Actors.Messages;
using Akka.Configuration;

namespace SymbolLookup
{
    public partial class SymbolActorSystem
    {
        private readonly object m_lock = new object();
        private ActorSystem ActorSystem;
        public IActorRef SymbolDispatcherActor;

        public event EventHandler<DownloadSymbolDataCommand> DataAvailable;
        public event EventHandler<string> StatusChange;
        public SymbolActorSystem()
        {
            DataAvailable += SymbolActorSystem_DataAvailable;
            StatusChange += SymbolActorSystem_StatusChange;

            var config = ConfigurationFactory.ParseString(@"my-dispatcher{
  # Dispatcher is the name of the event-based dispatcher
  type = Dispatcher
  # What kind of ExecutionService to use
  executor = ""fork-join-executor""
  # Configuration for the fork join pool
  fork-join-executor {
# Min number of threads to cap factor-based parallelism number to
   parallelism-min =50
        # Parallelism (threads) . . . ceil(available processors * factor)
    parallelism-factor =16.0
        # Max number of threads to cap factor-based parallelism number to
    parallelism-max = 250
      }
# Throughput defines the maximum number of messages to be
# processed per actor before the thread jumps to the next actor.
# Set to 1 for as fair as possible.
            throughput = 100
}");

               ActorSystem = ActorSystem.Create("Symbol", config);
            SymbolDispatcherActor = ActorSystem.ActorOf(
                   Props.Create(() => new SymbolDispatcherActor(DataAvailable, StatusChange))
                   //.WithDispatcher("my-dispatcher")
               //.WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
               , "dispatcher");
        }

        private void SymbolActorSystem_StatusChange(object sender, string e)
        {
             
        }

        private void SymbolActorSystem_DataAvailable(object sender, DownloadSymbolDataCommand e)
        {
             
        }

        public void Terminate()
        {
            ActorSystem.Terminate();
        }

        public void Tell(DateTime openDate)
        {

            SymbolDispatcherActor.Tell(new DownloadAllSymbolDataCommand() { OpenDate=openDate });
        }
        public void Tell(string symbol,DateTime openDate)
        {

            SymbolDispatcherActor.Tell(new DownloadSymbolDataCommand() { OpenDate=openDate,Symbol=symbol });
        }
    }
}

