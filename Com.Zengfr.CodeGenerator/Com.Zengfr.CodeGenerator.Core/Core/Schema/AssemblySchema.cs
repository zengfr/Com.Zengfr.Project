using System;
namespace Com.Zengfr.CodeGenerator.Schema
{
    using System.Collections.Generic;


    [Serializable]
    public class AssemblySchema
    {
        public virtual string FullName { get; set; }
        public virtual string Location { get; set; }
        public virtual List<TypeSchema> Types { get; set; }
        public AssemblySchema() { this.Types = new List<TypeSchema>(); }
    }

}
