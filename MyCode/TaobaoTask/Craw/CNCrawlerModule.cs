using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NCrawler;
using NCrawler.Interfaces;
using NCrawler.Services;

namespace  Craw
{
    public class CNCrawlerModule : NCrawlerModule
    {
      
        public CNCrawlerModule()
        {
             
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new InMemoryViaBloomFilterCrawlerHistoryService())
                   .As<ICrawlerHistory>()
                   .SingleInstance()
                   .ExternallyOwned();

            builder.Register((IComponentContext comp) => new WebDownloaderV2() { ReadTimeout = TimeSpan.FromMinutes(2) })
                // .As<IWebDownloader>().SingleInstance().ExternallyOwned();
                .As<IWebDownloader>().InstancePerLifetimeScope().OwnedByLifetimeScope();
        }

       
    }
}
