using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
namespace Zfrong.Framework.CoreBase.Model.Mapping
{
    public class UserMapping : MyClassMapping<User>
    {
       public UserMapping()
      {
        this.Table("ut_user");
        this.Id(c => c.ID, map =>
        {
            map.Column("ID");
            map.Generator(Generators.Identity);
        });
        this.Property(c => c.Password, map => map.Length(128));
        this.Property(c => c.Email, map => map.Length(32));
        this.Property(c => c.Phone, map => map.Length(16));
        this.Property(c => c.TEL, map => map.Length(16));
        this.Property(c => c.QQ, map => map.Length(18));
        this.Property(c => c.RealName, map => map.Length(12));
        this.Property(c => c.IDCode, map => map.Length(20));
        this.Component(x => x.DoState, MapperForDoState());
        this.Component(x => x.DoTime, MapperForDoTime());
    }
    
    }
}
