using System;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model;
using Zfrong.Framework.Core.IModel;
namespace Zfrong.Framework.CoreBase.Model
{
    public abstract class ExtModelBase<TId> : ModelBase<TId> ,IExtModel<TId>
    {
      
    }
    public abstract class ExtModelBase : ExtModelBase<long>,IExtModel
    {

    }
   
}
