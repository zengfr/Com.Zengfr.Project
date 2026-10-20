using System;
namespace Com.Zengfr.CodeGenerator.Schema
{
    [Serializable]
    public class PropertySchema
    {
        public virtual string ColumnName { get; set; }

        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string DisplayShortName { get; set; }

        public virtual Type Type { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string TypeFullName { get; set; }
        public virtual int Length { get; set; }
        public virtual bool AllowNull { get; set; }
        public virtual string DefaultValue { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsUnique { get; set; }
        public virtual bool IsKey { get; set; }
        public virtual bool IsID { get; set; }
        public virtual bool IsIndex { get; set; }
        public virtual bool IsIndexed { get; set; }

        public virtual bool IsArray { get; set; }
        public virtual bool IsGenericType { get; set; }
        public virtual bool IsValueType { get; set; }
        public virtual bool IsClass { get; set; }
        public virtual bool IsComponentClass { get; set; }
    }
}
