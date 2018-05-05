using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IService
{
    [ServiceContract]
    public interface IExtService<T, TId> 
    {
       
    }
    [ServiceContract]
    public interface IExtService<T> : IExtService<T, long>
    {

    }
}
