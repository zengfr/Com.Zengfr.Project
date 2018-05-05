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
    /// 保存一个Session在一个thread-static的类成员中。
    /// </summary>
    public class ThreadSessionSource : ISessionStorage
    {
        [ThreadStatic]
        private static SessionObject m_Session;

        /// <summary>
        ///获得Session 
        /// </summary>
        /// <returns></returns>
        public SessionObject Get()
        {
            if (m_Session != null && m_Session.Session != null)
            {
                if (!m_Session.Session.IsConnected)
                {
                    m_Session.Session.Reconnect();
                }
            }
            return m_Session;
        }

        /// <summary>
        /// 保存Session
        /// </summary>
        /// <param name="value"></param>
        public void Set(SessionObject value)
        {
            if (value != null && value.Session != null && value.Session.IsConnected)
            {
                value.Session.Disconnect();
            }
            m_Session = value;
        }
    }

}