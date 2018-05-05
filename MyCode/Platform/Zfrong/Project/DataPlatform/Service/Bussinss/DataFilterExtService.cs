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
    public class DataFilterExtService : ExtServiceBase<DataFilter>, IDataFilterExtService
    {
        public DataFilterExtService(IExtServicePlugin<DataFilter> plugin)
            : base(plugin)
        {

        }
    }
   
}
