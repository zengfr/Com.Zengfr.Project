using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model.Base;
using System.ServiceModel;
using NHibernate.Criterion;
using GeneratedCode.Model;
using Zfrong.Framework.Core.Service;
using Zfrong.Framework.Core.Service.ServiceContract;
namespace GeneratedCode.Contract{
[ServiceContract]
[ServiceKnownType(typeof(Order))]
[ServiceKnownType(typeof(SimpleExpression))]
[ServiceKnownType(typeof(DetachedCriteria))]
public interface IaccountsService : IService<accounts>,IServicePlugin<accounts>
{

}
}
