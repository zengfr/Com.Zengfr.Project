using System;
using System.Collections.Generic;
using System.Text;
using Zfrong.Framework.CoreBase.Model;
namespace Zfrong.Framework.CoreBase.Model{
    /// <summary>
    /// 公司/组织机构
    /// </summary>
    public class Company
    {
        Group Group { get; set; }
        IList<PermissionItem> PermissionItems { get; set; }
    }
}
