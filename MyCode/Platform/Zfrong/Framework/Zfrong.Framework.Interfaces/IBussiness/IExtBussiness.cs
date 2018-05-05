using System;
using System.Collections.Generic;
using System.ServiceModel;
using NHibernate.Criterion;
using NHibernate;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IBussiness
{
    public interface IExtBussiness<T, TId> :
       IExtServicePlugin<T, TId>
    {
        IExtNHibernateRepository<T, TId> CurrentRepository { get; set; }
    }
    public interface IExtBussiness<T> : IExtBussiness<T, long>,IExtServicePlugin<T>
    {

    }
   
}
