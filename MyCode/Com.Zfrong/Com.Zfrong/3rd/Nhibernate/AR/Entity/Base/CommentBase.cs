using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
   public class CommentBase<T>:InfoBase
    {
       [BelongsTo("TargetID")][Display]
       public virtual T Target
       {
           get { return Get<T>(props, "Target"); }
           set { Set<T>(props, "Target", value); }
       }
    }
    public class CustomCommentBase<T> : CustomInfoBase
    {
        [BelongsTo("TargetID")]
        [Display]
        public virtual T Target
        {
            get { return Get<T>(props, "Target"); }
            set { Set<T>(props, "Target", value); }
        }
    }
   
}
