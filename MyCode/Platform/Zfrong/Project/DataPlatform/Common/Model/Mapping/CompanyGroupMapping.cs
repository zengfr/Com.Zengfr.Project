using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model.Mapping
{
    public  class CompanyGroupMapping : MyClassMapping<CompanyGroup>
    {
        public CompanyGroupMapping()
      {
          this.Table("ut_CompanyGroup");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.Name, map => map.Length(128));
        this.Property(c => c.Adversary);
        this.Property(c => c.UserID);
        this.Component(x => x.DoState, MapperForDoState());
        this.Component(x => x.DoTime, MapperForDoTime());
    }
    }
}
