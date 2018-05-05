using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    [ActiveRecord("ut_UserMessage")]
    public class UserMessage : EntityBaseWithContent
    {
       [BelongsTo("SenderID")]
       public virtual User Sender
       {
           get { return Get<User>(props, "Sender"); }
           set { Set<User>(props, "Sender", value); }
       }
        [HasAndBelongsToMany(typeof(User), Index = "User_Message_UserID", IndexType = "int", Lazy = true,
            RelationType = RelationType.Bag, Table = "ut_User_Message", ColumnRef = "MessageID", ColumnKey = "UserID")]
       public virtual IList<User> Recipients
       {
           get { return Get<IList<User>>(props, "Recipients"); }
           set { Set<IList<User>>(props, "Recipients", value); }
       }
    }
}
