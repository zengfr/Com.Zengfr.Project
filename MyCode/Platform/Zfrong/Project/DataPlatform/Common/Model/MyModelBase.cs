using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.Core.Model;
namespace DataPlatform.Model
{
    public class MyModelBase:ModelBase
    {
        public virtual long UserID { get; set; }
        /// <summary>
        /// 对手 0,1,2
        /// </summary>
        public virtual bool Adversary { get; set; }
    }
}
