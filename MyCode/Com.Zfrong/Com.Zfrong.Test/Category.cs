// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.ActiveRecord.Tests.Model.GenericModel
{
	using System;
	using System.Collections;
    using System.Collections.Generic;

    [ActiveRecord("Category", Lazy = true)]
	public class Category : ActiveRecordBase<Category>
	{
		private int _id;
		private String _name;
        private IList<Post> _posts;
        private Category _parent;//

        public Category()
        {
            _posts = new List<Post>();
		}
        public Category(string name)
        {
            _name = name;//
            _posts = new List<Post>();
        }
		[PrimaryKey]//(PrimaryKeyType.Identity)]//Assigned)]
        public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[Property]
        public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}
        [BelongsTo("ParentId")]
        public virtual Category Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        [HasAndBelongsToMany(typeof(Post), Lazy = true,RelationType=RelationType.Bag,  Table = "Post_Category", ColumnRef = "PostID", ColumnKey = "CategoryID")]
        public virtual IList<Post> Posts
		{
            get { return _posts; }
            set { _posts = value; }
		}
        public virtual void SaveWithException()
        {
            Save();

            throw new ApplicationException("Fake Exception");
        }
	}
}
