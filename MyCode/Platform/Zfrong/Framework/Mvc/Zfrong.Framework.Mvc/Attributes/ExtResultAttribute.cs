using System;
using System.Collections.Generic;
namespace Zfrong.Framework.Mvc.Attributes
{
    /// <summary>
    /// 表示Action的返回是ExtResult类型的Json
    /// <remarks>因为非ExtResult类型的Json,在我们的EXT中未进行处理,所以需要标识出来(</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExtResultAttribute : Attribute
    {

    }
}
