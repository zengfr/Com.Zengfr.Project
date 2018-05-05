using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model.Mapping
{
    public class StatisticsMapping : MyClassMapping<Statistics>
    {
        public StatisticsMapping()
      {
          this.Table("ut_Statistics");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.DateTime);
        this.Property(c => c.Adversary);
        this.Property(c => c.Type);
        this.Property(c => c.D1);
        this.Property(c => c.D2);
        this.Property(c => c.D3);
        this.Property(c => c.V1);
        this.Property(c => c.V2);
        this.Property(c => c.V3);

        this.Property(c => c.UserID);
        this.Component(x => x.DoState, MapperForDoState());
        this.Component(x => x.DoTime, MapperForDoTime());
    }
    }
}
