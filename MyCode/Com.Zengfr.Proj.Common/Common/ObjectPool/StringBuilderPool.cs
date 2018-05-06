using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.ObjectPool
{
    public static class StringBuilderPool
    {
        public static StringBuilder Allocate()
        {
            return SharedPools.Default<StringBuilder>().AllocateAndClear();
        }

        public static void Free(StringBuilder builder)
        {
            SharedPools.Default<StringBuilder>().ClearAndFree(builder);
        }

        public static string ReturnAndFree(StringBuilder builder)
        {
            SharedPools.Default<StringBuilder>().ForgetTrackedObject(builder);
            return builder.ToString();
        }
    }
}
