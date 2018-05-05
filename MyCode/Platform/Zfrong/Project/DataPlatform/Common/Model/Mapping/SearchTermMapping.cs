using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model.Mapping
{
    public class SearchTermMapping : MyClassMapping<SearchTerm>
    {
        public SearchTermMapping()
      {
          this.Table("ut_SearchTerm");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.Name, map => map.Length(128));
        this.Property(c => c.Adversary);
        this.ManyToOne(c => c.Product, map => { map.Lazy(LazyRelation.NoLazy); });
        this.Property(c => c.UserID);
        this.Component(x => x.DoState, MapperForDoState());
        this.Component(x => x.DoTime, MapperForDoTime());
    }
    }
}
