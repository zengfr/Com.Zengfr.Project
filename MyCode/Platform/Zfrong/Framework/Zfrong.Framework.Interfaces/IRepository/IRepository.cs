using System.Collections.Generic;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IDao.Plugin;
using NHibernate;
namespace Zfrong.Framework.Core.IRepository
{
    public interface INHibernateRepository<T, TId> : IDaoPlugin<T, TId>
    {
        ISessionFactory SessionFactory { get; set; }
     }
    public interface INHibernateRepository<T> : INHibernateRepository<T, long>, IDaoPlugin<T>
      {

      }
}