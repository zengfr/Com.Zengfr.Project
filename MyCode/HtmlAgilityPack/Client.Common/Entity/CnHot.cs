using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace DB 
{
    [ActiveRecord(Lazy = true, Table = "CnHot")]
	public partial class CnHot : ActiveRecordBase<CnHot>
	{
		private int _doCount;
        private int _count;
        private byte _state;
		private string _name;
        private int _nameHash;
		private int _id;

        public CnHot()
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
        [Property(NotNull = true, Column = "Count")]
        public virtual int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        [Property(NotNull = true, Column = "State")]
        public virtual byte State
        {
            get { return _state; }
            set { _state = value; }
        }
        [Property(NotNull = true, Length = 50, Column = "Name")]
		public virtual string Name
		{
            get { return _name; }
            set { _name = value; }
		}

        [Property(NotNull = true, Column = "NameHash")]
        public virtual int NameHash
		{
            get { return _nameHash; }
            set { _nameHash = value; }
		}
	}
}
