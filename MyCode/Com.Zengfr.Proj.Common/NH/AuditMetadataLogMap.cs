using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FluentNHibernate.Mapping;

namespace Com.Zengfr.Proj.Common.NH.Audit
{
    public class AuditMetadataLogMap : ClassMap<AuditMetadataLog>
    {
        public AuditMetadataLogMap()
        {
            Id(x => x.AuditMetadataLogID).GeneratedBy.Identity();

            Map(x => x.AuditType).Length(3);
            Map(x => x.EntityId);
            Map(x => x.EntityName).Length(20);

            Map(x => x.FieldName).Length(20);
            Map(x => x.OldValue);
            Map(x => x.NewValue).Length(1000);
            Map(x => x.Timestamp);


        }
    }

}