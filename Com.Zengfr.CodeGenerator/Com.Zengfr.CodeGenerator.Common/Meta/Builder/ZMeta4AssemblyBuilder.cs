using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.CodeGenerator.Common.Util;
using Common.Logging;

namespace Com.Zengfr.CodeGenerator.Common.Meta.Builder
{
   public class ZMeta4AssemblyBuilder : IZMetaBuilder
    {
        private static ILog logger = LogManager.GetLogger(typeof(ZMeta4AssemblyBuilder));
        private string file;
        private bool onlyModelTypes;
        public ZMeta4AssemblyBuilder(string file, bool onlyModelTypes) {
            this.file = file;
            this.onlyModelTypes = onlyModelTypes;
        }
        public virtual ZMeta BuildMeta() {
            ZMeta meta = new ZMeta();
            meta = BuildMeta(file, onlyModelTypes);
            return meta;
        }


        #region

        public static ZMeta BuildMeta(string file, bool onlyModelTypes)
        {
            logger.Debug(string.Format("BuildMeta:{0},{1}", onlyModelTypes, file));
            var obj = new ZMeta();
            var assembly = Assembly.Load(AssemblyName.GetAssemblyName(file));
            if (assembly != null)
            {
                var ts = assembly.GetTypes();
                if (ts != null)
                {
                    foreach (Type t in ts)
                    {
                        if (onlyModelTypes)
                        {
                            if (ZMetaUtil.Instance.IsModelType(t))
                            {
                               var s = BuildMetaModel(t);
                               obj.Models.Add(s);
                            }
                        }
                        else
                        {
                            var s = BuildMetaModel(t);
                            obj.Models.Add(s);
                        }
                    }
                }
            }
            return obj;
        }

        private static ZMetaModel BuildMetaModel(Type type)
        {
            var obj = new ZMetaModel();
            if (type != null)
            {
                logger.Debug(string.Format("BuildMetaModel:{0},{1}", type.Namespace, type.Name));
                obj.Description = string.Empty;
                obj.Name = type.Name;
                obj.Namespace = type.Namespace;
                var ps = ReflectionUtil.Instance.GetProperties(type);
                foreach (PropertyInfo p in ps)
                {
                    if (!p.IsSpecialName && p.Name != "Item")
                    {
                        var bps = BuildMetaModelAttr(p);
                        obj.Attributes.Add(bps);
                    }
                }

            }
            return obj;
        }
        public static IList<ZMetaAttribute> BuildMetaModelAttrs(PropertyInfo[] ps)
        {
            List<ZMetaAttribute> objs = new List<ZMetaAttribute>();
            if (ps != null)
            {
                foreach (PropertyInfo p in ps)
                {
                    if (!p.IsSpecialName && p.Name != "Item")
                    {
                        var bps = BuildMetaModelAttr(p);
                        objs.Add(bps);
                    }
                }
            }
            return objs;
        }
        public static ZMetaAttribute BuildMetaModelAttr(PropertyInfo p)
        {
            var obj = new ZMetaAttribute();
            if (p != null)
            {
                obj.Name = p.Name;
                obj.Description = p.Name;
                var type = p.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                obj.TypeFullName = type.FullName;
                obj.TypeName = type.Name;
                obj.DbType = type.Name;
                ParseAttributes(obj, p);
            }
            return obj;
        }
       
        private static void ParseAttributes(ZMetaAttribute obj, PropertyInfo p)
        {
            var atributes = p.GetCustomAttributes(true);
            if (atributes != null && atributes.Length > 0)
            {
                foreach (var atribute in atributes)
                {
                    if (atribute is NHibernate.Mapping.Attributes.PropertyAttribute)
                    {
                        var attr = atribute as NHibernate.Mapping.Attributes.PropertyAttribute;
                        obj.AllowNull = attr.NotNull;
                        obj.Length = attr.Length;
                        obj.IsID = false;
                        obj.IsIndex = !string.IsNullOrEmpty(attr.Index);
                        obj.IsUnique = attr.Unique;

                    }
                    else if (atribute is NHibernate.Mapping.Attributes.IdAttribute)
                    {
                        var attr = atribute as NHibernate.Mapping.Attributes.IdAttribute;
                        obj.AllowNull = false;
                        obj.DefaultValue = attr.UnsavedValue;
                        obj.Length = attr.Length;
                        obj.IsID = true;
                        obj.IsIndex = true;
                        obj.IsKey = true;
                        obj.IsUnique = true;
                    }else if(atribute.GetType().Name.IndexOf("PrimaryKey")!=-1
                        || atribute.GetType().Name.IndexOf("Key") != -1
                        || atribute.GetType().Name.IndexOf("ID") != -1)
                    {
                        obj.IsID = true;
                        obj.IsIndex = true;
                        obj.IsKey = true;
                        obj.IsUnique = true;
                    }
                }
            }

            if (p.Name.Equals(p.ReflectedType.Name + "ID",StringComparison.CurrentCultureIgnoreCase))
            {
                obj.IsID = true;
                obj.IsIndex = true;
                obj.IsKey = true;
                obj.IsUnique = true;
            }
        }
        #endregion
    }
}
