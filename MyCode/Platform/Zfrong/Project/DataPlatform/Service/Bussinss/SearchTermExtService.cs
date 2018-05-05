using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.IService.Plugin;
using Zfrong.Framework.CoreBase.Model;
using Zfrong.Framework.CoreBase.Service;
using DataPlatform.Contract.ServiceContract;
using DataPlatform.Model;
using DataPlatform.Dao;
namespace DataPlatform.Bussiness
{
    public class SearchTermExtService : ExtServiceBase<SearchTerm>, ISearchTermExtService
    {
        public SearchTermExtService(IExtServicePlugin<SearchTerm> plugin)
            : base(plugin)
        {

        }
    }

}
