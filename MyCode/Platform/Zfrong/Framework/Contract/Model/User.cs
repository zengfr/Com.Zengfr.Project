using System;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model;
namespace Zfrong.Framework.CoreBase.Model
{
    public class User : ModelBase
    {
        public User() { }
        public virtual string Password { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpiredDateTime { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string RealName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public virtual string IDCode { get; set; }

        public virtual string Email { get; set; }
        public virtual string TEL { get; set; }
        public virtual string Phone { get; set; }
        public virtual string MSN { get; set; }
        public virtual string QQ { get; set; }

        public virtual string Province { get; set; }
        public virtual string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public virtual string Area { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        public virtual string Address { get; set; }

        IList<PermissionItem> PermissionItems { get; set; }
    }
}
