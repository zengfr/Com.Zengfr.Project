using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.Core.Model;
namespace DataPlatform.Model
{
   public class Statistics : MyModelBase
    {
        /// <summary>
        /// 统计时间
        /// </summary>
        public virtual DateTime DateTime { get; set; }
        /// <summary>
        /// 统计类型：按XX类型
        /// </summary>
        public virtual string  Type { get; set; }
        public virtual string V1 { get; set; }
        public virtual string V2 { get; set; }
        public virtual string V3 { get; set; }
        public virtual string D1 { get; set; }
        public virtual string D2 { get; set; }
        public virtual string D3 { get; set; }
    }
}
