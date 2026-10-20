using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SchemaExplorer;
using Com.Zengfr.CodeGenerator.Common;
namespace Com.Zengfr.CodeGenerator.Host
{
    [Serializable]
    public class BaseHost
    {
        public virtual string TemplateFile
        {
            get;
            set;
        }
        public virtual Encoding OutputEncoding
        {
            get;
            set;
        }
        private IDictionary<string, object> ExtendProperties
        {
            get;
            set;
        }

        public virtual IList<string> StandardAssemblyReferences
        {
            get;
            set;
        }

        public virtual IList<string> StandardImports
        {
            get;
            set;
        }

        public BaseHost()
        {
            ExtendProperties = new Dictionary<string, object>();
            StandardImports = new List<string>();
            StandardAssemblyReferences = new List<string>();

            this.AddStandardAssemblyReferences(typeof(BaseHost).Assembly.Location);
            this.AddStandardAssemblyReferences(typeof(DatabaseSchema).Assembly.Location);
            this.AddStandardAssemblyReferences(typeof(AppDomain).Assembly.Location);
            this.AddStandardAssemblyReferences(typeof(Uri).Assembly.Location);
            this.AddStandardAssemblyReferences(typeof(DbType).Assembly.Location);
            this.AddStandardAssemblyReferences(typeof(Queryable).Assembly.Location);
            this.AddStandardImports("System");
            this.AddStandardImports("System.IO");
            this.AddStandardImports("System.Xml");
            this.AddStandardImports("System.Linq");
            this.AddStandardImports("System.Text");
            this.AddStandardImports("System.Data");
            this.AddStandardImports("System.Data.Common");
            this.AddStandardImports("System.Collections");
            this.AddStandardImports("System.Collections.Generic");
            this.AddStandardImports("System.Collections.Specialized");

            this.AddStandardImports("SchemaExplorer");
            this.AddStandardImports("Com.Zengfr.CodeGenerator.Common");
            this.AddStandardImports("Com.Zengfr.CodeGenerator.Host");
            this.AddStandardImports("Com.Zengfr.CodeGenerator.Schema");
        }

        public virtual void AddStandardAssemblyReferences(string assemblyLocation)
        {
            if (this.StandardAssemblyReferences.Contains(assemblyLocation))
            {
                return;
            }
            this.StandardAssemblyReferences.Add(assemblyLocation);
        }

        public virtual void AddStandardImports(string namespaceName)
        {
            if (this.StandardImports.Contains(namespaceName))
            {
                return;
            }
            this.StandardImports.Add(namespaceName);
        }

        public virtual void SetValue(string key, object value)
        {
            if (this.ExtendProperties.ContainsKey(key))
            {
                this.ExtendProperties[key] = value;
            }
            else
            {
                this.ExtendProperties.Add(key, value);
            }
        }

        public virtual object GetValue(string key)
        {
            if (this.ExtendProperties.ContainsKey(key))
            {
                return this.ExtendProperties[key];
            }
            return null;
        }

        public virtual T GetValue<T>(string key)
        {
            return (T)GetValue(key);
        }

        public virtual string ToPascal(string phrase)
        {
            return Common.ConvertUtils.ToPascal(phrase);
        }
        public virtual string ToCamel(string phrase)
        {
            return Common.ConvertUtils.ToCamel(phrase);
        }
        public virtual IList<Schema.PropertySchema> GetExtendProperties(IList<Schema.PropertySchema> properties)
        {
            List<Schema.PropertySchema> objs = new List<Schema.PropertySchema>();

            foreach (var p in properties)
            {
                if (p.Type.IsClass && p.Type != typeof(string))
                {
                    GetExtendProperties(objs, p);
                }
                else
                {
                    objs.Add(p);
                }
            }
            return objs;
        }
        protected virtual void GetExtendProperties(IList<Schema.PropertySchema> result, Schema.PropertySchema p)
        {
            if (p.Type.IsClass && p.Type != typeof(string))
            {
                var ps = p.Type.GetProperties(BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance);

                var objs = SchemaBuilder.BuildPropertySchema(ps);
                foreach (var obj in objs)
                {
                    obj.Name = string.Format("{0}_{1}", p.Name, obj.Name);
                    if (obj.Type.IsClass && obj.Type != typeof(string))
                    {
                        GetExtendProperties(result, obj);
                    }
                    else
                    {
                        result.Add(obj);
                    }
                }
            }

        }
    }
}
