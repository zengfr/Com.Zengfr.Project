using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
namespace Zfrong.Amib.Threading
{
    public class SmartThreadPoolFactory : DictionaryManagerBase<string, SmartThreadPool>
    {
        #region Instance
        public static SmartThreadPoolFactory Instance = new SmartThreadPoolFactory();

        #endregion
        public virtual SmartThreadPool Create(string poolName, int maxWorkerThreads)
        {
            SmartThreadPool stp;
            if (!this.Exist(poolName))
            {
                stp = new SmartThreadPool(new STPStartInfo() { StartSuspended = true, MaxWorkerThreads = maxWorkerThreads, EnableLocalPerformanceCounters=true });
                stp.Name = poolName;
                stp.OnThreadTermination += EventManagerFactory.Instance.Create(poolName).FireThreadTermination;
                this.Items[stp.Name] = stp;
            }
            return this.Items[poolName];
        }
    }
}
