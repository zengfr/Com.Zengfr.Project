using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Zengfr.CodeGenerator.Web.Models
{
    public class ModelBase
    {
        public virtual Boolean DataItemIsDeleted { get; set; }
        public virtual byte DataItemStatus { get; set; }
        public virtual DateTime DataCreateLastTime { get; set; }
        public virtual DateTime DataChangeLastTime { get; set; }
        
    }
    public class Company: ModelBase
    {
        public virtual long CompanyId { get; set; }
        public virtual string CompanyName { get; set; }
    }
    public class Database : ModelBase
    {
        public virtual long DatabaseId { get; set; }
        public virtual Company Company { get; set; }
        public virtual string DatabaseName { get; set; }
        public virtual string DatabaseDescription { get; set; }
        public virtual string DatabaseType { get; set; }
        public virtual string ConnctionString { get; set; }
    }
    public class Model : ModelBase
    {
        public virtual long ModelId { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public virtual Database Database { get; set; }
        /// <summary>
        /// 1=DBModel 2=DomainModel
        /// </summary>
        public virtual int ModelType { get; set; }
        public virtual string ModelName { get; set; }
        public virtual string ModelDescription { get; set; }
    }
    public class ModelAttr : ModelBase
    {
        public virtual long ModelAttrId { get; set; }
        public virtual Model Model { get; set; }
        public virtual string AttrName { set; get; }
        public virtual string AttDescription { get; set; }
        public virtual string TypeName { get; set; }

        public virtual int Length { get; set; }
        public virtual string DefaultValue { get; set; }
        public virtual bool IsUnique { get; set; }
        public virtual bool IsKey { get; set; }
        public virtual bool IsID { get; set; }
        public virtual bool IsIndex { get; set; }
    }
    public class GenTask : ModelBase
    {
        public virtual long GenTaskId { get; set; }
        public virtual string Result { get; set; }
        public virtual int ResultStatus { get; set; }
        public virtual string Description { get; set; }
       
    }
}