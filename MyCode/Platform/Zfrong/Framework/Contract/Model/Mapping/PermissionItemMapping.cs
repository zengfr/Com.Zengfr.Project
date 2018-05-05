using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
namespace Zfrong.Framework.CoreBase.Model.Mapping
{
   public class PermissionItemMapping:MyClassMapping<PermissionItem>
    {
       public PermissionItemMapping()
      {
         this.Table("ut_permissionItem");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.GroupID);
        this.Property(c => c.PID);
        this.Property(c => c.Name, map => map.Length(24));
    }
    }
}
