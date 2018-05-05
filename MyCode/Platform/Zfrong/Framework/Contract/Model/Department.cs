using System;
using System.Collections.Generic;
using Zfrong.Framework.CoreBase.Model;
namespace Zfrong.Framework.CoreBase.Model
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department
    {
        Company Company{ get; set; }
       
        IList<PermissionItem> PermissionItems { get; set; }
    }
}
