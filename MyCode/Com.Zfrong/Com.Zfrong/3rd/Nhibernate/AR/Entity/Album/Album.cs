using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

namespace Com.Zfrong.Common.Data.AR.Entity
{
    [ActiveRecord("ut_Album")]
    public class Album : CustomInfoBase
    {
        IList<AlbumTag> tags;
        IList<AlbumCategory> categorys;
        IList<AlbumComment> comments;
        IList<UserAttach> attachs;

        [HasAndBelongsToMany(typeof(AlbumTag), Index = "Album_Tag_CommentID", IndexType = "int",
           Lazy = true, RelationType = RelationType.Bag, Table = "ut_Album_Tag", ColumnRef = "AlbumID", ColumnKey = "TagID")]
        IList<AlbumTag> Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        [HasAndBelongsToMany(typeof(AlbumCategory), Index = "Album_Category_CommentID", IndexType = "int",
          Lazy = true, RelationType = RelationType.Bag, Table = "ut_Album_Category", ColumnRef = "AlbumID", ColumnKey = "CategoryID")]
        IList<AlbumCategory> Categorys
        {
            get { return categorys; }
            set { categorys = value; }
        }

        [HasMany(typeof(AlbumComment), Lazy = true, RelationType = RelationType.Bag, ColumnKey = "TargetID")]
        IList<AlbumComment> Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        [HasAndBelongsToMany(typeof(UserAttach), Index = "Album_UserAttach_AttachID", IndexType = "int",
      Lazy = true, RelationType = RelationType.Bag, Table = "ut_Album_UserAttach", ColumnRef = "AlbumID", ColumnKey = "AttachID")]
        IList<UserAttach> Attachs
        {
            get { return attachs; }
            set { attachs = value; }
        }
    }

    [ActiveRecord("ut_AlbumComment")]
    public class AlbumComment : CommentBase<Album>
    {
       
    }

    [ActiveRecord("ut_AlbumCategory",
  DiscriminatorColumn = "Type",
  DiscriminatorType = "byte",
  DiscriminatorValue = "1")]
    public class AlbumCategory : CategoryBase<AlbumCategory>
    {
        IList<User> users;
        [HasAndBelongsToMany(typeof(User), Index = "User_AlbumCategory_UserID", IndexType = "int",
        Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_AlbumCategory", ColumnRef = "CategoryID", ColumnKey = "UserID")]
        public IList<User> Users
        {
            get { return users; }
            set { users = value; }
        }
    }

    [ActiveRecord("ut_AlbumTag",
  DiscriminatorColumn = "Type",
  DiscriminatorType = "byte",
  DiscriminatorValue = "1")]
    public class AlbumTag : TagBase<AlbumTag>
    {
        IList<User> users;
        [HasAndBelongsToMany(typeof(User), Index = "User_AlbumTag_UserID", IndexType = "int",
        Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_AlbumTag", ColumnRef = "TagID", ColumnKey = "UserID")]
        public IList<User> Users
        {
            get { return users; }
            set { users = value; }
        }
    }
    [ActiveRecord("ut_AlbumFavorite")]
    public class AlbumFavorite : FavoriteBase<AlbumFavorite>
    {
        
    }
}
