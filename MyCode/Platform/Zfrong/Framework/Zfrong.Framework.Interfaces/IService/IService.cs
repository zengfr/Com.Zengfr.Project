using System;
using System.Collections.Generic;
using System.ServiceModel;
using NHibernate.Criterion;
using NHibernate;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IService
{
    [ServiceContract]
    public interface IService<T,TId>
        
    {
         }
    [ServiceContract]
     public interface IService<T>:IService<T,long>
    {

    }
   
}
