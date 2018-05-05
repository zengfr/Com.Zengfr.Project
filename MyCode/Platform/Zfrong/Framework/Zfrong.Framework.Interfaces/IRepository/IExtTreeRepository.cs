using System.Collections.Generic;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IDao.Plugin;
using NHibernate;
namespace Zfrong.Framework.Core.IRepository
{
    public interface IExtTreeNHibernateRepository<T, TId> : IExtTreeDaoPlugin<T, TId>
    {
        ISessionFactory SessionFactory { get; set; }
     }
    public interface IExtTreeNHibernateRepository<T> : IExtTreeNHibernateRepository<T, long>, IExtTreeDaoPlugin<T>
      {

      }
}