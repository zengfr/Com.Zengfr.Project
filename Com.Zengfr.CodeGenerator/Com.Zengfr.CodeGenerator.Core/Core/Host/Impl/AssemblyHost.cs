using System;


namespace Com.Zengfr.CodeGenerator.Host
{
    using System.Collections.Generic;
    using System.Linq;
    using Com.Zengfr.CodeGenerator.Schema;
    [Serializable]
    public class AssemblyHost : FileBaseHost
    {
        public virtual AssemblySchema AssemblySchema
        {
            get;
            set;
        }
        public virtual TypeSchema TypeSchema
        {
            get;
            set;
        }
        public virtual IList<string> GetTypesName()
        {
            return GetTypesName(this.AssemblySchema);
        }
        public virtual IList<string> GetTypesName(AssemblySchema schema)
        {
            return GetTypesName(schema.Types);
        }
        public virtual IList<string> GetTypesName(IList<TypeSchema> types)
        {
            var objs = types.Select<TypeSchema, string>
                (t => t.Name).ToList();
            return objs;
        }
        public virtual IList<string> GetPropertiesName()
        {
            return GetPropertiesName(this.TypeSchema);
        }
        public virtual IList<string> GetPropertiesName(TypeSchema schema)
        {
            var objs = schema.Properties.Select<PropertySchema, string>
               (t => t.Name).ToList();
            return objs;
        }
       
       
    }
}
