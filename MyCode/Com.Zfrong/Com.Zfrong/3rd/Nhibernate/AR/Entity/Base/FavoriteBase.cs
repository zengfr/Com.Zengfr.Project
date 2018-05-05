using System;
using System.Collections.Generic;
using System.Text;


using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
   public class FavoriteBase<T>:EntityBaseWithTime
    {
       [BelongsTo("UserID")]
       public virtual User User
       {
           get { return Get<User>(props, "User"); }
           set { Set<User>(props, "User", value); }
       }
       [BelongsTo("TargetID")]
       public virtual T Target
       {
           get { return Get<T>(props, "Target"); }
           set { Set<T>(props, "Target", value); }
       }
    }
}
