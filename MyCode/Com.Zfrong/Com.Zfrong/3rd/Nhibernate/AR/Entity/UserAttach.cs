using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
    [ActiveRecord("ut_UserAttach",
  DiscriminatorColumn = "Type",
  DiscriminatorType = "byte",
  DiscriminatorValue = "0")]
  public  class UserAttach:EntityBaseWithTitleAndTime
    {
      string path;
      int size;
        User user;
        [BelongsTo("UserID")]
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }
      [Property]
      public virtual int Size
      {
          get { return size; }
          set { size = value; }
      }
        [Property]
        public virtual string Path
        {
            get { return path; }
            set { path = value; }
        }
    }
    //[ActiveRecord(DiscriminatorValue = "1")]
    //public class UserFiles : UserFile
    //{
       
    //}
}
