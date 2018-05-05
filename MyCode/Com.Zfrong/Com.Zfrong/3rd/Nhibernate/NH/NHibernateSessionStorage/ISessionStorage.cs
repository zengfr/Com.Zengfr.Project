/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/


using System;
using NHibernate;

namespace Com.Zfrong.Common.Data.NH.SessionStorage
{
    /// <summary>
    ///储存一个ISession
    /// </summary>
    public interface ISessionStorage
    {
        /// <summary>
        ///获得ISession 
        /// </summary>
        /// <returns></returns>
        SessionObject Get();

        /// <summary>
        /// 保存ISession
        /// </summary>
        /// <param name="value"></param>
        void Set(SessionObject value);
    }
}
