/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/


using NHibernate;
using System.Web;
using Com.Zfrong.Common.Data.NH.SessionStorage.Cfg;

namespace Com.Zfrong.Common.Data.NH.SessionStorage
{
    /// <summary>
    /// ����һ��ISession <see cref="HttpContext.Items" /> ����.
    /// </summary>
    public class HttpSessionSource : ISessionStorage
    {
        /// <summary>
        /// ���ISession 
        /// </summary>
        /// <returns>��õ�ISession</returns>
        public SessionObject Get()
        {
            return (SessionObject)HttpContext.Current.Items[Config.HttpSessionSourceItemName];
        }

        /// <summary>
        /// ����ISession
        /// </summary>
        /// <param name="value">��Ҫ�����ISession</param>
        public void Set(SessionObject value)
        {
            if (value != null)
            {
                if (System.Web.HttpContext.Current.Items.Contains(Config.HttpSessionSourceItemName))
                {
                    System.Web.HttpContext.Current.Items[Config.HttpSessionSourceItemName] =  value;
                }
                else
                {
                    System.Web.HttpContext.Current.Items.Add(Config.HttpSessionSourceItemName, value);
                }
            }
            else
            {
                System.Web.HttpContext.Current.Items.Remove(Config.HttpSessionSourceItemName);
            }
        }
    }
}
