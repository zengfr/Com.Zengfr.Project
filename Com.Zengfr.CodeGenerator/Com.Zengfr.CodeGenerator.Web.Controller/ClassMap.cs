using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Zengfr.CodeGenerator.Web.Models;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace Com.Zengfr.CodeGenerator.Web
{
    public class MyClassConvention : IClassConvention
    {
        public virtual void Apply(IClassInstance instance)
        {
            var name = instance.EntityType.Name;
            instance.Table("g_"+name);
            instance.DynamicUpdate();
           // instance.
        }
    }
  
    public class PrimaryKeyConvention : IIdConvention
    {
        public virtual void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.Identity();
        }
    }
    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(t => t.CompanyId);
            Map(t => t.CompanyName);
        }
    }
    public class DatabaseMap : ClassMap<Database>
    {
        public DatabaseMap() {
            Id(t => t.DatabaseId);
            Map(t => t.DatabaseName);
            References<Company>(p => p.Company);
        }
    }
    public class ModelMap : ClassMap<Model>
    {
        public ModelMap()
        {
            Id(t => t.ModelId);
            Map(t => t.ModelName);
            References<Database>(p=>p.Database);
        }
    }
    public class ModelAttrMap : ClassMap<ModelAttr>
    {
        public ModelAttrMap()
        {
            Id(t => t.ModelAttrId);
            Map(t => t.AttrName);
            References<Model>(p => p.Model);
        }
    }
}