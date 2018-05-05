using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace DB 
{
    [ActiveRecord(Lazy = true, Table = "AdvData")]
	public partial class AdvData : ActiveRecordBase<AdvData>
	{
		private int _id;
        private int _linkHash;
        private int _keyHash;
        private byte _state;

        private string _key;
        private string _link;
        private string _title;
        private string _content;

        public AdvData()
		{
		}

        [PrimaryKey(PrimaryKeyType.Native)]
		public virtual int ID 
		{
			get { return _id; }
			set { _id = value; }
		}

        
        [Property(NotNull = true, Column = "[KeyHash]")]
        public virtual int KeyHash
        {
            get { return _keyHash; }
            set { _keyHash = value; }
        }
        [Property(NotNull = true, Column = "LinkHash")]
        public virtual int LinkHash
        {
            get { return _linkHash; }
            set { _linkHash = value; }
        }
        [Property(NotNull = true, Column = "[State]")]
        public virtual byte State
        {
            get { return _state; }
            set { _state = value; }
        }
        [Property(NotNull = true, Length = 50, Column = "[Key]")]
        public virtual string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        [Property(NotNull = true, Length = 800, Column = "Link")]
        public virtual string Link
        {
            get { return _link; }
            set { _link = value; }
        }
        [Property(NotNull = true, Length = 500, Column = "Title")]
        public virtual string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        [Property(NotNull = true, SqlType = "VARCHAR(MAX)", Column = "[Content]")]
        public virtual string Content
        {
            get { return _content; }
            set { _content = value; }
        }
       
	}
}
