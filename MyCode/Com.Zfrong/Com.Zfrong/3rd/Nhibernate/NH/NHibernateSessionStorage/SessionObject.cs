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
    public class SessionObject
    {
        private ISession _session;
        private bool _isNeedClose;

        public SessionObject(ISession session, bool isNeedClose)
        {
            this._session = session;
            this._isNeedClose = isNeedClose;
        }

        public ISession Session
        {
            get { return _session; }
            set { _session = value; }
        }

        public bool IsNeedClose
        {
            get { return _isNeedClose; }
            set { _isNeedClose = value; }
        }

        public void Close()
        {
            this._session.Close();
        }
    }
}
