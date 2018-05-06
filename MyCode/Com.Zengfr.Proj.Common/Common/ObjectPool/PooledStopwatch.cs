using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.ObjectPool
{
    public class PooledStopwatch : Stopwatch
    {
        private static readonly ObjectPool<PooledStopwatch> s_poolInstance = CreatePool();

        private readonly ObjectPool<PooledStopwatch> _pool;

        private PooledStopwatch(ObjectPool<PooledStopwatch> pool)
        {
            _pool = pool;
            UpdateValueFactory = (_, accumulated) => accumulated + Elapsed;
        }

        public Func<object, TimeSpan, TimeSpan> UpdateValueFactory
        {
            get;
        }

        public void Free()
        {
            Reset();
            _pool?.Free(this);
        }

        public static ObjectPool<PooledStopwatch> CreatePool()
        {
            ObjectPool<PooledStopwatch> pool = null;
            pool = new ObjectPool<PooledStopwatch>(() => new PooledStopwatch(pool), 128);
            return pool;
        }

        public static PooledStopwatch StartInstance()
        {
            var instance = s_poolInstance.Allocate();
            instance.Restart();
            return instance;
        }
    }
}
