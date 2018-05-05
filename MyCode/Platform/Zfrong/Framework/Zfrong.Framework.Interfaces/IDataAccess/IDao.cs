using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate;
namespace Zfrong.Framework.Core.IDao
{
    public interface IDao<T, TID>
    {
        
     }
      public interface IDao<T> : IDao<T,long>
      {

      }
}