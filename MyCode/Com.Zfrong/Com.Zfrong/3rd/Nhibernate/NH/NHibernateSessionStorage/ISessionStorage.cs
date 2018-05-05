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
    ///����һ��ISession
    /// </summary>
    public interface ISessionStorage
    {
        /// <summary>
        ///���ISession 
        /// </summary>
        /// <returns></returns>
        SessionObject Get();

        /// <summary>
        /// ����ISession
        /// </summary>
        /// <param name="value"></param>
        void Set(SessionObject value);
    }
}
