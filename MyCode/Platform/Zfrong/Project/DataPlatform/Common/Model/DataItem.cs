using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model
{
   public class DataItem : MyModelBase
    {
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual string URL { get; set; }
        /// <summary>
        /// 搜索来源 百度 google
        /// </summary>
        public virtual int SearcherType { get; set; }
        /// <summary>
        /// BBS/Blog/WIKI
        /// </summary>
        public virtual int MediaType { get; set; }
        /// <summary>
        /// 0 普通 1-5好评 >6差评
        /// </summary>
        public virtual byte GoodOrBad  { get; set; }
        public virtual SearchTerm SearchTerm { get; set; }
    }
}
