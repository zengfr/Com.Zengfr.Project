using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.Core.Model;
namespace Zfrong.Framework.Core.IModel
{
    public interface IExtModel<TId> : IModel<TId>
    {
       
    }
    public interface IExtModel: IExtModel<long>,IModel
    {
       
    }
}
