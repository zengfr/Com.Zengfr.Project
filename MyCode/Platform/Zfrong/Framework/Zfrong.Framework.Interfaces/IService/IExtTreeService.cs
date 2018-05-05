using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.DataContract;
using System.ServiceModel;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IService
{
    [ServiceContract]
    public interface IExtTreeService<T, TId>
    {
       
    }
    [ServiceContract]
    public interface IExtTreeService<T> : IExtTreeService<T, long>
    {

    }
}
