using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.ObjectPool
{
    public static class ListPool<T>
    {
        public static List<T> Allocate()
        {
            return SharedPools.Default<List<T>>().AllocateAndClear();
        }

        public static void Free(List<T> list)
        {
            SharedPools.Default<List<T>>().ClearAndFree(list);
        }

        public static List<T> ReturnAndFree(List<T> list)
        {
            SharedPools.Default<List<T>>().ForgetTrackedObject(list);
            return list;
        }
    }
}
