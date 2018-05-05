using System;
using System.Collections.Generic;
using System.ServiceModel;
using NHibernate.Criterion;
using NHibernate;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IBussiness
{
    public interface IBussiness<T, TId> :
        IServicePlugin<T, TId>
    {
        INHibernateRepository<T, TId> CurrentRepository { get; set; }
    }
    public interface IBussiness<T> : IBussiness<T, long>,IServicePlugin<T>
    {

    }
   
}
