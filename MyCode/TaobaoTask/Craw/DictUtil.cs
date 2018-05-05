using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cc
{
    public class DictUtil
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(DictUtil));
        static IDictionary<int, IDictionary<string, long>> Dicts = new Dictionary<int, IDictionary<string, long>>();
        public static int MaxLen = 9;
        public static int MinValue = 50;
        public static ManualResetEvent lockEvent = new ManualResetEvent(true);
        public static int AddItem(IEnumerable<string> keys)
        {
            var addCount = 0;
            foreach (var key in keys)
            {
                addCount += AddItem(key);
            }
            if (addCount > 0)
            {
                log.DebugFormat("Dict->AddItemCount:{0}", addCount);
            }
            return addCount;
        }
        private static int AddItem(string key)
        {
            var addCount = 0;
            var len = key.Length;
            if (len <= MaxLen)
            {
                if (!Dicts.ContainsKey(len))
                {
                    lock (Dicts)
                    {
                        if (!Dicts.ContainsKey(len))
                        {
                            lockEvent.WaitOne();
                            Dicts.Add(len, new Dictionary<string, long>());
                        }
                    }
                }
                if (!Dicts[len].ContainsKey(key))
                {
                    lock (Dicts[len])
                    {
                        if (!Dicts[len].ContainsKey(key))
                        {
                            lockEvent.WaitOne();
                            Dicts[len].Add(key, 0);
                            log.DebugFormat("Dict->AddItem:{0}", key);
                            addCount++;
                        }
                    }

                }
                Dicts[len][key] += 1;
            }
            return addCount;
        }
        public static void Load()
        {
            lock (Dicts)
            {
                Dicts.Clear();
                for (int i = 1; i <= MaxLen; i++)
                {
                    Dicts.Add(i, FileUtil.Retrieve(i + ".dict"));
                }
            }
        }
        public static void Store()
        {
            Store(MinValue);
        }
        public static void Store(int minValue)
        {
            lock (Dicts)
            {
                lockEvent.Reset();
                foreach (var kv in Dicts)
                {
                    FileUtil.Store(kv.Value, kv.Key + ".dict", minValue);
                }
                lockEvent.Set();
            }
        }
    }
}
