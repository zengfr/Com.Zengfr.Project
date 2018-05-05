using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BloomFilter;
using log4net;
using NCrawler.Interfaces;
using NCrawler.Services;
using NCrawler.Utils;

namespace  Craw
{
    public class InMemoryViaBloomFilterCrawlerHistoryService : DisposableBase ,ICrawlerHistory
    {
        protected static log4net.ILog logger = LogManager.GetLogger(typeof(InMemoryViaBloomFilterCrawlerHistoryService));
        protected  Filter<string> filter = new Filter<string>(1024*1024*10);
        protected   long totalCount = 0;
        public long RegisteredCount
        {
            get
            {
                return totalCount;
            }
        }

        public bool Register(string key)
        {
           var isExist=  filter.Contains(key);
            if (!isExist)
            {
                Interlocked.Increment(ref totalCount);
                filter.Add(key);
            }
            //logger.DebugFormat("Register:{0,5},{1},{2},{3}", isExist, totalCount,"", key);
            return !isExist;
        }

        protected override void Cleanup()
        {
            ;//
        }
    }
}
