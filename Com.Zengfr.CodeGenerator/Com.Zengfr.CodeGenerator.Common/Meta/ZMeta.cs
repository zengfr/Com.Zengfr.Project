using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Meta
{
    public class ZMeta
    {
        public virtual String Name { get; set; }
        public virtual string Namespace { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<ZMetaModel> Models { get; set; }

        public ZMeta() {
            Models = new List<ZMetaModel>();
            Name = string.Empty;
        }
    }
}
