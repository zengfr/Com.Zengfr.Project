using System;
using System.Collections.Generic;
using System.ServiceModel;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IService;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.Core.Model;
using DataPlatform.Model;
using Zfrong.Framework.Core.DataContract;
namespace DataPlatform.Contract.ServiceContract
{
    [ServiceContract]
    public interface ISearchTermExtService : IExtService<SearchTerm>, IExtServicePlugin<SearchTerm>
    {

    }
    
}
