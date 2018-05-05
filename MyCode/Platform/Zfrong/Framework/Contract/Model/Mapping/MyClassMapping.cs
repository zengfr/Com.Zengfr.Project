using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Zfrong.Framework.Core.Model;
namespace Zfrong.Framework.CoreBase.Model.Mapping
{
    public class MyClassMapping<T> : ClassMapping<T> where T:class
    {
        public static Action<IComponentMapper<DoState>> MapperForDoState()
        {
            return c =>
            {
                c.Property(p => p.CheckStatus, map => map.Column(x => x.Default((byte)0)));
                c.Property(p => p.IsActive, map => map.Column(x => x.Default(0)));
                c.Property(p => p.IsDelete, map => map.Column(x => x.Default(0)));
                c.Property(p => p.Sort, map => map.Column(x => x.Default((byte)0)));
                c.Property(p => p.Status, map => map.Column(x => x.Default((byte)0)));
            };
        }
        public static Action<IComponentMapper<DoTime>> MapperForDoTime()
        {
            DateTime now = DateTime.Now;
            return c =>
            {
                c.Property(p => p.CreateTime, map => map.Column(x => {  x.SqlType("DATETIME"); }));
                c.Property(p => p.DeleteTime, map => map.Column(x => {  x.SqlType("DATETIME"); }));
                c.Property(p => p.UpdateTime, map => map.Column(x => {  x.SqlType("DATETIME"); }));
            };
        }
    }
}
