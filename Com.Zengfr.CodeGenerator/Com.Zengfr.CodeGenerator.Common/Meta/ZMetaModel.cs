using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Meta
{
    public class ZMetaModel
    {
        public virtual String Name { get; set; }
        public virtual string Namespace { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<ZMetaAttribute> Attributes { get; set; }
        public ZMetaModel()
        {
            Attributes = new List<ZMetaAttribute>();
            Name = string.Empty;
        }
    }
}
