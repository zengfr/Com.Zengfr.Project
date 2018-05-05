using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common.NH.Audit
{
    public class AuditMetadataLog
    {
        public virtual long AuditMetadataLogID { get; set; }
        public virtual string AuditType { get; set; }
        public virtual string EntityName { get; set; }
        public virtual long EntityId { get; set; }
        public virtual string FieldName { get; set; }
        public virtual string OldValue { get; set; }
        public virtual string NewValue { get; set; }
        public virtual DateTime Timestamp { get; set; }
    }
}
