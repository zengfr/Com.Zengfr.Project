using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zfrong.Amib.Threading
{
    class ThreadPoolManagerFactory : DictionaryManagerBase<string, ThreadPoolManager>
    {
       static ThreadPoolManagerFactory() {
           System.Net.WebRequest.DefaultWebProxy = null;
           System.Net.ServicePointManager.DefaultConnectionLimit = 512;
           System.Net.ServicePointManager.MaxServicePoints = 128;
           System.Net.ServicePointManager.UseNagleAlgorithm = false;
           
       }
       #region Instance
       public static ThreadPoolManagerFactory Instance = new ThreadPoolManagerFactory();

       #endregion
      
        public virtual  ThreadPoolManager Create(string poolName, int maxWorkerThreads)
        {
            ThreadPoolManager t ;
            if (!this.Exist(poolName))
            {
                t = new ThreadPoolManager(poolName, maxWorkerThreads);
                this.Items[poolName] = t;
            }
            return this.Items[poolName];
        }
    }
}
