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
    /// 储存一个ISession <see cref="HttpContext.Items" /> 集合.
    /// </summary>
    public class HttpSessionSource : ISessionStorage
    {
        /// <summary>
        /// 获得ISession 
        /// </summary>
        /// <returns>获得的ISession</returns>
        public SessionObject Get()
        {
            return (SessionObject)HttpContext.Current.Items[Config.HttpSessionSourceItemName];
        }

        /// <summary>
        /// 保存ISession
        /// </summary>
        /// <param name="value">需要保存的ISession</param>
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
