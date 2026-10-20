using System;
namespace Com.Zengfr.CodeGenerator.Schema
{
    using System.Collections.Generic;


    [Serializable]
    public class TypeSchema
    {
        public virtual string TableName { get; set; }

        public virtual string PrimaryKeyName { get; set; }
        public virtual string PrimaryKeyType { get; set; }
        public virtual string Name { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Namespace { get; set; }

        public virtual string DisplayName { get; set; }
        public virtual string DisplayShortName { get; set; }
        public virtual string Description { get; set; }
        public virtual List<PropertySchema> Properties { get; set; }
        public TypeSchema() { this.Properties = new List<PropertySchema>(); }

    }

}
