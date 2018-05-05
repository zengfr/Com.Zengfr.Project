using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.DataContract;
using System.ServiceModel;
using Zfrong.Framework.Core.IRepository;
namespace Zfrong.Framework.Core.IService.Plugin
{
    [ServiceContract]
    public interface IExtTreeServicePlugin<T, TId>
    {
        IList<T> NodeFindAll(string parentNode, string checkedValue);
       ExtResponse<T> NodeCreate(TId parentId, string checkedValue, T dataItem);
       ExtResponse<T> NodeRemove(TId id);
       ExtResponse<T> NodeUpdate(TId id, T dataItem);
       ExtResponse<T> NodeMove(TId id, TId parentId);
    }
    [ServiceContract]
    public interface IExtTreeServicePlugin<T> : IExtTreeServicePlugin<T, long>
    {

    }
}
