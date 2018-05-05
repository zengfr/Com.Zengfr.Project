using System.Collections;
using System.Collections.Generic;
using Zfrong.Framework.Core.DataContract;
using System.ServiceModel;
namespace Zfrong.Framework.Core.IService.Plugin
{
    [ServiceContract]
    [ServiceKnownType(typeof(ExtDictionary<string, object>))]
    [ServiceKnownType(typeof(ExtKVItem<int>))]
    [ServiceKnownType(typeof(ExtKVItem))]
    [ServiceKnownType(typeof(Dictionary<string, object>))]
    public interface IExtServicePlugin<T, TId>
    {
        [OperationContract]
         ExtPagingResponse<T> Delete(ExtRequest<IList<T>> Data);
        [OperationContract(Name = "DeleteByDict")]
        ExtPagingResponse<T> Delete(ExtRequest<IList<ExtDictionary<string, object>>> Data);
        [OperationContract]
        ExtPagingResponse<T> LogicDel(ExtRequest<IList<T>> Data);
        [OperationContract(Name = "LogicDeleteByDict")]
        ExtPagingResponse<T> LogicDel(ExtRequest<IList<ExtDictionary<string, object>>> Data);

        #region
        [OperationContract]
        ExtPagingResponse<T> Create(ExtRequest<IList<T>> Data);
        [OperationContract(Name = "CreateByDict")]
        ExtPagingResponse<T> Create(ExtRequest<IList<ExtDictionary<string, object>>> Data);
        [OperationContract]
        ExtPagingResponse<T> Update(ExtRequest<IList<T>> Data);
        [OperationContract(Name = "UpdateByDict")]
        ExtPagingResponse<T> Update(ExtRequest<IList<ExtDictionary<string, object>>> Data);
       
        #endregion
        #region
        [OperationContract]
        ExtPagingResponse<T> SlicedFindAll(string start, string limit, string sort, string dir, ExtFilterItem[] filter);
        [OperationContract]
        ExtPagingResponse<ExtKVItem<long>> SlicedFindAllPV(string start, string limit, string property, ExtFilterItem[] filter);
        [OperationContract]
        ExtPagingResponse<ExtKVItem> SlicedFindAllPVDistinct(string start, string limit, string property, ExtFilterItem[] filter);
        #endregion 
        #region
        #endregion
    }
    [ServiceContract]
    public interface IExtServicePlugin<T> : IExtServicePlugin<T, long>
    {

    }
}