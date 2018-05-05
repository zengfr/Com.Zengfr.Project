using System;
using System.Collections.Generic;
using System.ServiceModel;
using NHibernate.Criterion;
using NHibernate;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IBussiness
{
    public interface IExtTreeBussiness<T, TId> :
       IExtTreeServicePlugin<T, TId>
    {
      
    }
    public interface IExtTreeBussiness<T> : IExtTreeBussiness<T, long>, IExtTreeServicePlugin<T>
    {

    }
   
}
