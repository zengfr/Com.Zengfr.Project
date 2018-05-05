using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zfrong.Framework.CoreBase.Model
{
    public interface IHasPermissionItems
    {
        IList<PermissionItem> PermissionItems { get; set; }
    }
    public class PermissionItem
    {
        /// <summary>
        /// 主键
        /// </summary>
      public virtual  long ID { get; set; }
        /// <summary>
        /// 权限ID标识
        /// </summary>
      public virtual long PID { get; set; }
        /// <summary>
        /// 权限说明
        /// </summary>
      public virtual string Name { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
      public virtual int GroupID { get; set; }
    }
}
