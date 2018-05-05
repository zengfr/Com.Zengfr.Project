using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Castle.ActiveRecord;
namespace DB
{
    public abstract class Base : ActiveRecordBase
    {
        protected static T Get<T>(Hashtable props, string key)
        {
            if (props[key] == null)
                return default(T);//
            return (T)props[key];//
        }
        protected static void Set<T>(Hashtable props, string key, T value)
        {
            props[key] = value;//
        }
        protected Hashtable props = new Hashtable();//
        [PrimaryKey]
        public virtual int ID
        {
            get { return Get<int>(props, "ID"); }

            set { Set<int>(props, "ID", value); }
        }
        [Property]
        public virtual byte State
        {
            get { return Get<byte>(props, "State"); }

            set { Set<byte>(props, "State", value); }
        }
        [Property]
        public virtual byte Sort
        {
            get { return Get<byte>(props, "Sort"); }

            set { Set<byte>(props, "Sort", value); }
        }
    }
    [ActiveRecord(Lazy = true, Table = "AdvURL")]
    public class AdvURL : Base
    {
        [Property(Length=2000)]
        public virtual string Content
        {
            get { return Get<string>(props, "Content"); }

            set { Set<string>(props, "Content", value); }
        }
        [Property]
        public virtual int Hash
        {
            get { return Get<int>(props, "Hash"); }

            set { Set<int>(props, "Hash", value); }
        }
        
       
    }
    [ActiveRecord(Lazy = true, Table = "AdvURLRelation")]
    public class AdvURLRelation : Base
    {
        [Property]
        public virtual int Hash
        {
            get { return Get<int>(props, "Hash"); }

            set { Set<int>(props, "Hash", value); }
        }
        [Property]
        public virtual int ParentHash
        {
            get { return Get<int>(props, "ParentHash"); }

            set { Set<int>(props, "ParentHash", value); }
        }
    }
}
