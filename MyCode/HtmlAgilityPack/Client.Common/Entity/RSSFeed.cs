using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace DB 
{
	[ActiveRecord(Lazy=true,Table="RSSFeed")]
	public partial class RSSFeed : ActiveRecordBase<RSSFeed>
	{
		private int _doCount;
		private string _link;
		private int _linkHash;
		private string _linkText;
		private byte _state;
		private string _title;
		private int _id;

		public RSSFeed()
		{
		}

        [PrimaryKey(PrimaryKeyType.Native)]
		public virtual int ID 
		{
			get { return _id; }
			set { _id = value; }
		}

		[Property(NotNull = true, Column = "DoCount")]
		public virtual int DoCount
		{
			get { return _doCount; }
			set { _doCount = value; }
		}

		[Property(NotNull = true, Length = 800, Column = "Link")]
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

		[Property(NotNull = false, Length = 500, Column = "LinkText")]
		public virtual string LinkText
		{
			get { return _linkText; }
			set { _linkText = value; }
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

	}
}
