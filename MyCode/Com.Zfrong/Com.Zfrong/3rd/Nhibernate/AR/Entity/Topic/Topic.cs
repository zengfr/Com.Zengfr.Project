using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

namespace Com.Zfrong.Common.Data.AR.Entity
{
    [ActiveRecord("ut_Topic")]
    public class Topic : CustomInfoBase
    {
        IList<TopicTag> tags;
        IList<TopicCategory> categorys;
        IList<TopicComment> comments;
        [HasAndBelongsToMany(typeof(TopicTag), Index = "Topic_Tag_TagID", IndexType = "int",
           Lazy = true, RelationType = RelationType.Bag, Table = "ut_Topic_Tag", ColumnRef = "TopicID", ColumnKey = "TagID")]
        IList<TopicTag> Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        [HasAndBelongsToMany(typeof(TopicCategory), Index = "Topic_Category_CategoryID", IndexType = "int",
         Lazy = true, RelationType = RelationType.Bag, Table = "ut_Topic_Category", ColumnRef = "TopicID", ColumnKey = "CategoryID")]
        IList<TopicCategory> Categorys
        {
            get { return categorys; }
            set { categorys = value; }
        }

        [HasMany(typeof(TopicComment), Lazy = true, RelationType = RelationType.Bag, ColumnKey = "TargetID")]
        IList<TopicComment> Comments
        {
            get { return comments; }
            set { comments = value; }
        }
    }
    [ActiveRecord("ut_TopicComment")]
    public class TopicComment : CustomCommentBase<Topic>
    {
       
    }

    [ActiveRecord("ut_TopicCategory",
DiscriminatorColumn = "Type",
DiscriminatorType = "byte",
DiscriminatorValue = "1")]
    public class TopicCategory : CategoryBase<TopicCategory>
    {
        IList<User> users;
        [HasAndBelongsToMany(typeof(User), Index = "User_TopicCategory_UserID", IndexType = "int",
       Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_TopicCategory", ColumnRef = "CategoryID", ColumnKey = "UserID")]
        public IList<User> Users
        {
            get { return users; }
            set { users = value; }
        }
    }
    [ActiveRecord("ut_TopicTag",
 DiscriminatorColumn = "Type",
 DiscriminatorType = "byte",
 DiscriminatorValue = "1")]
    public class TopicTag : TagBase<TopicTag>
    {
        IList<User> users;
        [HasAndBelongsToMany(typeof(User), Index = "User_TopicTag_UserID", IndexType = "int",
       Lazy = true, RelationType = RelationType.Bag, Table = "ut_User_TopicTag", ColumnRef = "TagID", ColumnKey = "UserID")]
        public IList<User> Users
        {
            get { return users; }
            set { users = value; }
        }
    }
    [ActiveRecord("ut_TopicFavorite")]
    public class TopicFavorite : FavoriteBase<TopicFavorite>
    {

    }
}
