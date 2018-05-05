using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.DataContract;
using NHibernate.Criterion;
using NHibernate;
namespace Zfrong.Framework.Core.IDao
{
    public interface IExtTreeDao<T, TId>
    {
     
    }
    public interface IExtTreeDao<T> : IExtTreeDao<T, long>
    {

    }
}
