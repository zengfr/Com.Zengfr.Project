using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate;
namespace Zfrong.Framework.Core.IDao
{
    public interface IExtDao<T, TId> 
    {
         
    }
    public interface IExtDao<T> : IExtDao<T, long>
    {

    }
}
