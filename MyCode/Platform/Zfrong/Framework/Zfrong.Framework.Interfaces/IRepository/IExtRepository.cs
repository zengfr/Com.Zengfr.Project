using System.Collections.Generic;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IDao.Plugin;
using NHibernate;
namespace Zfrong.Framework.Core.IRepository
{
    public interface IExtNHibernateRepository<T, TId> : IExtDaoPlugin<T, TId>
    {
        ISessionFactory SessionFactory { get; set; }
     }
    public interface IExtNHibernateRepository<T> : IExtNHibernateRepository<T, long>, IExtDaoPlugin<T>
      {

      }
}