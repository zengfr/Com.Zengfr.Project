using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using System.Security.Principal;
using System.Security;
namespace Com.Zfrong.Common.Data.AR.Base.Entity
{

    public abstract class Base //: Disposable
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
        public virtual void SetProperties(Hashtable properties)
        {
            props = properties;//
        }
        public virtual void SetProperties(IDictionary<string, string> properties)
        {
            foreach (KeyValuePair<string, string> kv in properties)
            {
                props[kv.Key] = kv.Value;//
            }
        }
        public virtual void SetProperties(IDictionary<string, object> properties)
        {
            foreach (KeyValuePair<string, object> kv in properties)
            {  
                props[kv.Key] = kv.Value;//
            }
        }
        public virtual Hashtable GetProperties()
        {
            return props;//
        }
        protected virtual T Get<T>(string key)
        {
            if (props[key] == null)
                return default(T);//
            return (T)props[key];//
        }
        protected virtual void Set<T>(string key, T value)
        {
            props[key] = value;//
        }



        #region IDisposable Members

        public virtual void Dispose()
        {
            if (this.props != null)
            { 
                this.props.Clear(); this.props = null; 
            }
            //this.Dispose(true);//
        }
        ~Base()
        {
            //if(!IsDisposed)
              //this.Dispose(false);//
        }
        public Base()
        {
           
        }
       public Base(Hashtable properties)
        {
            this.props = properties;//
       }
        #endregion
    }
    public abstract class Base2 : Base { }
    public class DoState:Base
    {
        [Property( NotNull = true)]
        [Display("状态")]
        public virtual byte Status
        {
            get { return Get<byte>(props, "Status"); }

            set { Set<byte>(props, "Status", value); }
        }
        [Property(NotNull = true)]
        [Display("排序")]
        public virtual byte Sort
        {
            get { return Get<byte>(props, "Sort"); }

            set { Set<byte>(props, "Sort", value); }
        }

        [Property( NotNull = true)]
        [Display("回收站")]
        public virtual bool IsDelete
        {
            get { return Get<bool>(props, "IsDelete"); }

            set { Set<bool>(props, "IsDelete", value); }
        }
        [Property( NotNull = true)]
        [Display("启用/禁用")]
        public virtual bool IsActive
        {
            get { return Get<bool>(props, "IsActive"); }

            set { Set<bool>(props, "IsActive", value); }
        }
        [Property( NotNull = true)]
        [Display("审核状态")]
        public virtual byte CheckStatus
        {
            get { return Get<byte>(props, "CheckStatus"); }

            set { Set<byte>(props, "CheckStatus", value); }
        }
    }
    public class DoTime: Base
    {
        public DoTime()
        {
            CreateTime = UpdateTime = DeleteTime =new DateTime(1900,1,1);
        }
        [Property(NotNull = true)]//[Property(SqlType = "smallDateTime", NotNull = true, Default = "getdate()")]
        [Display("创建时间")]
        public virtual DateTime CreateTime
        {
            get { return Get<DateTime>(props, "CreateTime"); }

            set { Set<DateTime>(props, "CreateTime", value); }
        }
        [Property(NotNull = true)]//[Property(SqlType = "smallDateTime", NotNull = true, Default = "getdate()")]
        [Display("最后更新")]
        public virtual DateTime UpdateTime
        {
            get { return Get<DateTime>(props, "UpdateTime"); }

            set { Set<DateTime>(props, "UpdateTime", value); }
        }
        [Property(NotNull = true)]//[Property(SqlType = "smallDateTime", NotNull = true, Default = "getdate()")]
        [Display("删除时间")]
        public virtual DateTime DeleteTime
        {
            get { return Get<DateTime>(props, "DeleteTime"); }

            set { Set<DateTime>(props, "DeleteTime", value); }
        }
       
    }
    public class DoCount: Base
    {
        [Property]
        [Display]
        public virtual int PCount
        {
            get { return Get<int>(props, "PCount"); }

            set { Set<int>(props, "PCount", value); }
        }
        [Property]
        [Display]
        public virtual int VCount
        {
            get { return Get<int>(props, "VCount"); }

            set { Set<int>(props, "VCount", value); }
        }
    }
    /// <summary>
    /// ID/State/Sort/IsDelete/IsActive
    /// </summary>
    public class EntityBase:Base
    {
       public EntityBase() { DoState = new DoState(); }
        [Display]
        [PrimaryKey]
        public virtual int ID
        {
            get { return Get<int>(props,"ID"); }

            set { Set<int>(props,"ID", value); }
        }
        [Nested]
        public virtual DoState DoState
        {
            get { return Get<DoState>(props, "DoState"); }

            set { Set<DoState>(props, "DoState", value); }
        }
    }

    public class TreeEntityBase<T> : EntityBaseWithTime
    {
        
        public virtual string text
        {
            get { return Name; }
            set { Name = value; }
        }
        [Property]
        public virtual bool leaf { get; set; }
        [Property]
        public virtual string cls { get; set; }
        [Property]
        public virtual string iconCls { get; set; }
        [Property]
        public virtual string url { get; set; }
        [Property]
        public virtual string Name{ get; set; }
        [BelongsTo(Column = "ParentId")]
        public virtual T Parent
        { get; set; }
    }
    public class EntityBaseWithName:EntityBase
    {
       
        [Property]
        [Display]
        public virtual string Name
        {
            get { return Get<string>(props, "Name"); }

            set { Set<string>(props, "Name", value); }
        }
    }
    public class EntityBaseWithTitle : EntityBase
    {
        [Property]
        [Display]
        public virtual string Title
        {
            get { return Get<string>(props, "Title"); }

            set { Set<string>(props, "Title", value); }
        }
    }
    public class EntityBaseWithContent : EntityBase
    {
        [Property]
        [Display]
        public virtual string Content
        {
            get { return Get<string>(props, "Content"); }

            set { Set<string>(props, "Content", value); }
        }
    }
   /// <summary>
    ///CreateTime/UpdateTime; EntityBaseExt : EntityBase
   /// </summary>
    public class EntityBaseWithTime : EntityBase
    {
        public EntityBaseWithTime() { DoTime = new DoTime(); }
        [Nested]
        public virtual DoTime DoTime
        {
            get { return Get<DoTime>(props, "DoTime"); }

            set { Set<DoTime>(props, "DoTime", value); }
        }
    }
    /// <summary>
    /// EntityBaseExtA:EntityBaseA
    /// </summary>
    public class EntityBaseWithNameAndTime : EntityBaseWithName
    {
        public EntityBaseWithNameAndTime() { DoTime = new DoTime(); }
        [Nested]
        public virtual DoTime DoTime
        {
            get { return Get<DoTime>(props, "DoTime"); }

            set { Set<DoTime>(props, "DoTime", value); }
        }
    }
    public class EntityBaseWithTitleAndTime : EntityBaseWithTitle
    {

        [Nested]
        public virtual DoTime DoTime
        {
            get { return Get<DoTime>(props, "DoTime"); }

            set { Set<DoTime>(props, "DoTime", value); }
        }
    }
    /// <summary>
    /// EntityBaseExtE: EntityBaseExtA
    /// </summary>
    public class EntityBaseWithInfo : EntityBaseWithTitleAndTime
    {

        [Nested]
        [Display]
        public virtual DoCount DoCount
        {
            get { return Get<DoCount>(props, "DoCount"); }

            set { Set<DoCount>(props, "DoCount", value); }
        }
        [Property]
        [Display]
        public virtual string Content
        {
            get { return Get<string>(props, "Content"); }

            set { Set<string>(props, "Content", value); }
        }
    }
   
}
