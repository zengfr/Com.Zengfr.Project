using System;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model;
namespace Zfrong.Framework.Core.IModel
{
    public interface IModel<TId>
    {
        TId ID { get; set; }
        DoState DoState { get; set; }
    }
    public interface IModel:IModel<long>
    {

    }
}
