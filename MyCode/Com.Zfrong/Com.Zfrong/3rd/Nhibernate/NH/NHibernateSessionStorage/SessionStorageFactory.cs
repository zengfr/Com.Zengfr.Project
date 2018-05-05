/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/

using System;
using Com.Zfrong.Common.Data.NH.SessionStorage.Cfg;

namespace Com.Zfrong.Common.Data.NH.SessionStorage
{
    /// <summary>
    /// 产生ISessionStorage的工厂
    /// </summary>
    public class SessionStorageFactory
    {
        /// <summary>
        /// 获得ISessionStorage
        /// </summary>
        /// <returns></returns>
        public static ISessionStorage GetSessionStorage()
        {
            if (Config.SessionSourceType == "http")  //使用    
            {
                return new HttpSessionSource();
            }
            else if (Config.SessionSourceType == "threadStatic")
            {
                return new ThreadSessionSource();
            }
            else
            {
                throw new NotSupportedException("不支持的SessionSourceType！" + Config.SessionSourceType);
            }
        }
    }
}
