using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    public class InfoBase : EntityBaseWithInfo
    {

    }
    public class CustomInfoBase : InfoBase
    {

        [BelongsTo("UserID")]
        public virtual User User
        {
            get { return Get<User>(props, "User"); }
            set { Set<User>(props, "User", value); }
        }

        
    }
}
