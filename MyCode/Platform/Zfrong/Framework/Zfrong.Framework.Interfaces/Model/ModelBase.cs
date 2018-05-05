using System;
using System.Collections.Generic;
using Zfrong.Framework.Core.IModel;
namespace Zfrong.Framework.Core.Model
{
    public abstract class ModelBase<TId> : IModel<TId>
    {
        public ModelBase()
        {
            this.DoState = new DoState();
            this.DoTime = new DoTime();
        }
        public virtual TId ID
        {
            get;

            set;
        }
        public virtual DoState DoState
        {
            get;

            set;
        }
        public virtual DoTime DoTime
        {
            get;

            set;
        }
    }
    public abstract class ModelBase : ModelBase<long>, IModel.IModel
    {

    }
    public class DoTime
    {
        public DoTime()
        {
            CreateTime = UpdateTime = DeleteTime = DateTime.Now;
        }
        public virtual DateTime CreateTime
        { get; set; }
        public virtual DateTime UpdateTime
        { get; set; }
         public virtual DateTime DeleteTime
        { get; set; }
    }
    public class DoState
    {
        public virtual bool IsActive { get; set; }
        public virtual bool IsDelete { get; set; }
        public virtual byte Sort { get; set; }
        public virtual byte Status{ get; set;}
        public virtual byte CheckStatus{ get; set; }
    }
}
