using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    public class TagBase<T> : EntityBaseWithName
    {
         [BelongsTo(Column = "ParentId")]
        public virtual T Parent
        {
            get { return Get<T>(props, "Parent"); }
            set { Set<T>(props, "Parent", value); }
        }
    }

    public class CustomTagBase<T> : EntityBaseWithName
    {
        [BelongsTo(Column = "ParentId")]
        public virtual T Parent
        {
            get { return Get<T>(props, "Parent"); }
            set { Set<T>(props, "Parent", value); }
        }
        [BelongsTo("UserID")]
        public virtual User User
        {
            get { return Get<User>(props, "User"); }
            set { Set<User>(props, "User", value); }
        }

    }
   
}
