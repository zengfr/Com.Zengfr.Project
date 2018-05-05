using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace DB 
{
	[ActiveRecord(Lazy=true,Table="RSSItem")]
	public partial class RSSItem : ActiveRecordBase<RSSItem>
	{
		private string _author;
		private int _authorHash;
		private string _category;
		private int _categoryHash;
		private string _content;
		private int _contentHash;
		private string _description;
		private int _descriptionHash;
		private string _key;
		private int _keyHash;
		private string _link;
		private int _linkHash;
		private DateTime _pubDate;
		private byte _state;
		private string _title;
		private int _titleHash;
		private DateTime _updateTime;
		private int _id;

		public RSSItem()
		{
		}

        [PrimaryKey(PrimaryKeyType.Native)]
		public virtual int ID 
		{
			get { return _id; }
			set { _id = value; }
		}

		[Property(NotNull = false, Length = 50, Column = "Author")]
		public virtual string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		[Property(NotNull = true, Column = "AuthorHash")]
		public virtual int AuthorHash
		{
			get { return _authorHash; }
			set { _authorHash = value; }
		}

		[Property(NotNull = false, Length = 100, Column = "Category")]
		public virtual string Category
		{
			get { return _category; }
			set { _category = value; }
		}

		[Property(NotNull = true, Column = "CategoryHash")]
		public virtual int CategoryHash
		{
			get { return _categoryHash; }
			set { _categoryHash = value; }
		}

		[Property(NotNull = false, SqlType = "VARCHAR(MAX)", Column = "[Content]")]
		public virtual string Content
		{
			get { return _content; }
			set { _content = value; }
		}

		[Property(NotNull = true, Column = "ContentHash")]
		public virtual int ContentHash
		{
			get { return _contentHash; }
			set { _contentHash = value; }
		}

		[Property(NotNull = false, SqlType = "VARCHAR(MAX)", Column = "Description")]
		public virtual string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		[Property(NotNull = true, Column = "DescriptionHash")]
		public virtual int DescriptionHash
		{
			get { return _descriptionHash; }
			set { _descriptionHash = value; }
		}

		[Property(NotNull = false, Length = 50, Column = "[Key]")]
		public virtual string Key
		{
			get { return _key; }
			set { _key = value; }
		}

		[Property(NotNull = true, Column = "KeyHash")]
		public virtual int KeyHash
		{
			get { return _keyHash; }
			set { _keyHash = value; }
		}

		[Property(NotNull = false, Length = 800, Column = "Link")]
		public virtual string Link
		{
			get { return _link; }
			set { _link = value; }
		}

		[Property(NotNull = true, Column = "LinkHash")]
		public virtual int LinkHash
		{
			get { return _linkHash; }
			set { _linkHash = value; }
		}

		[Property(NotNull = true, Column = "PubDate")]
		public virtual DateTime PubDate
		{
			get { return _pubDate; }
			set { _pubDate = value; }
		}

		[Property(NotNull = true, Column = "State")]
		public virtual byte State
		{
			get { return _state; }
			set { _state = value; }
		}

		[Property(NotNull = false, Length = 500, Column = "Title")]
		public virtual string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		[Property(NotNull = true, Column = "TitleHash")]
		public virtual int TitleHash
		{
			get { return _titleHash; }
			set { _titleHash = value; }
		}

		[Property(NotNull = true, Column = "UpdateTime")]
		public virtual DateTime UpdateTime
		{
			get { return _updateTime; }
			set { _updateTime = value; }
		}

	}
}
