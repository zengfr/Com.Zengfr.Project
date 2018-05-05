using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Threading;

namespace Com.Zengfr.Proj.Common
{
    public class WebJobExecutor
    {
        static AutoResetEvent wait = new AutoResetEvent(false);
        public static void AddTask(string name, int seconds, Action<object, bool> call, object state)
        {
            ThreadPool.RegisterWaitForSingleObject(wait, new WaitOrTimerCallback(call), state, seconds * 1000, false);
        }
    }
}
