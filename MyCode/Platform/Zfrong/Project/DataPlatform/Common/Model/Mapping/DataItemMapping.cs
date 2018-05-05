using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model.Mapping
{
   public class DataItemMapping : MyClassMapping<DataItem>
    {
        public DataItemMapping()
      {
          this.Table("ut_SearchTerm");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.Content, map => map.Length(128));
        this.Property(c => c.Adversary);
        this.Property(c => c.GoodOrBad);
        this.Property(c => c.MediaType);
        this.Property(c => c.SearcherType);
        this.Property(c => c.Title);
        this.Property(c => c.URL);
        this.Property(c => c.UserID);
        this.OneToOne(c => c.SearchTerm, map => { });
        
        this.Component(x => x.DoState, MapperForDoState());
        this.Component(x => x.DoTime, MapperForDoTime());
    }
    }
}
