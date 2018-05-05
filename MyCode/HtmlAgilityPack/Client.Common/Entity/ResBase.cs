using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Castle.ActiveRecord;

namespace DB
{
    public abstract class ResBase : ActiveRecordBase
    {
        Hashtable props = new Hashtable();
        #region Props
        [PrimaryKey]
        public virtual int ID
        {
            get { return Get<int>("ID"); }

            set { Set<int>("ID", value); }

        }
        [Property(Length=50)]
        public virtual string Content
        {
            get { return Get<string>("Content"); }

            set { Set<string>("Content", value); }

        }
        [Property]
        public virtual int Num
        {
            get { return  Get<int>("Num"); }

            set { Set<int>("Num", value); }

        }
        [Property]
        public virtual int Success
        {
            get { return  Get<int>("Success"); }

            set { Set<int>("Success", value); }

        }
        [Property]
        public virtual int Fail
        {
            get { return  Get<int>("Fail"); }

            set { Set<int>("Fail", value); }

        }

        private T Get<T>(string key)
        {
            if (props[key] == null)
                return default(T);//
            return (T)props[key];//
        }
        private void Set<T>(string key,T value)
        {
           props[key]=value;//
        }
        #endregion

    }
}
