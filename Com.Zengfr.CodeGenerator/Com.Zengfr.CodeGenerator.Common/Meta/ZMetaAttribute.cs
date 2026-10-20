using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Meta
{
    public class ZMetaAttribute
    {
        public virtual String Name { get; set; }
        public virtual string Description { get; set; }

        public virtual string TypeFullName { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string DbType { get; set; }

        public virtual int Length { get; set; }
        public virtual bool AllowNull { get; set; }
        public virtual string DefaultValue { get; set; }
        public virtual bool IsUnique { get; set; }
        public virtual bool IsKey { get; set; }
        public virtual bool IsID { get; set; }
        public virtual bool IsIndex { get; set; }
        public ZMetaAttribute()
        {
            Name = string.Empty;
        }
    }
}
