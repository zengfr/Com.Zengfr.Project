using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model
{
  public  class DataFilter : MyModelBase
    {
        public virtual string Content { get; set; }
        public virtual string Note { get; set; }
        public virtual bool IsTitle { get; set; }
        public virtual bool IsContent { get; set; }
        public virtual bool IsURL { get; set; }
    }
}
