using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base.Entity;
namespace Com.Zfrong.Common.Data.AR.Entity
{
  //  [ActiveRecord("ut_CategoryBase",
  //DiscriminatorColumn = "Type",
  //DiscriminatorType = "byte",
  //DiscriminatorValue = "1")]
    public class CategoryBase<T> : EntityBaseWithNameAndTime
    {
         [BelongsTo(Column = "ParentId")]
        public virtual T Parent
        {
            get { return Get<T>(props, "Parent"); }
            set { Set<T>(props, "Parent", value); }
        }
        [Property]
        public virtual string Url
        {
            get { return Get<string>(props, "Url"); }
            set { Set<string>(props, "Url", value); }
        }
        //IList<User> users;
        //[HasManyToAny(typeof(User), "ID", "User_Category", typeof(int),
        // "Type", "Id", MetaType = typeof(int))]
        //[Any.MetaValue("1", typeof(AlbumCategory))]
        //[Any.MetaValue("2", typeof(ArticleCategory))]
        //public IList<User> Users
        //{
        //    get { return users; }
        //    set { users = value; }
        //}
    }
    public class CustomCategoryBase<T> : EntityBaseWithName
    {
        [BelongsTo(Column = "ParentId")]
        public virtual T Parent
        {
            get { return Get<T>(props, "Parent"); }
            set { Set<T>(props, "Parent", value); }
        }
        [Property]
        public virtual string Url
        {
            get { return Get<string>(props, "Url"); }
            set { Set<string>(props, "Url", value); }
        }
        [BelongsTo("UserID")]
        public virtual User User
        {
            get { return Get<User>(props, "User"); }
            set { Set<User>(props, "User", value); }
        }
    }
   
    
}
