using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model.Mapping
{
    public class DataFilterMapping : MyClassMapping<DataFilter>
    {
        public DataFilterMapping()
      {
          this.Table("ut_DataFilter");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.Content, map => map.Length(128));
        this.Property(c => c.Note);
        this.Property(c => c.IsContent);
        this.Property(c => c.IsTitle);
        this.Property(c => c.IsURL);

        this.Property(c => c.UserID);
        this.Component(x => x.DoState, MapperForDoState());
        this.Component(x => x.DoTime, MapperForDoTime());
    }
    }
}
