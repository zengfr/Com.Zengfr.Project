
namespace Com.Zengfr.CodeGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Com.Zengfr.CodeGenerator.Schema;
    public class SchemaBuilder
    {
        static Dictionary<string, string> IgnoreTypes = new Dictionary<string, string>();
        public SchemaBuilder()
        {
            IgnoreTypes.Clear();
            IgnoreTypes.Add("Entity", string.Empty);
            IgnoreTypes.Add("Model", string.Empty);
            IgnoreTypes.Add("Contract", string.Empty);
            IgnoreTypes.Add("Schema", string.Empty);
            IgnoreTypes.Add("Base", string.Empty);
            IgnoreTypes.Add("Mapping", string.Empty);
            IgnoreTypes.Add("Properties", string.Empty);
        }
        private static bool Ignore(Type t)
        {
            var ignore = false;
            var dict = new Dictionary<string, string>();
            foreach (var key in IgnoreTypes)
            {
                ignore = t.FullName.IndexOf(key.Key) > 0;
            }
            return ignore;
        }
        #region

        public static AssemblySchema BuildAssemblySchema(string file, bool onlyModelTypes)
        {
            var obj = new AssemblySchema();
            var assembly = Assembly.Load(AssemblyName.GetAssemblyName(file));
            if (assembly != null)
            {
                obj.Location = assembly.Location;
                obj.FullName = assembly.FullName;
                var ts = assembly.GetTypes();
                if (ts != null)
                {
                    foreach (Type t in ts)
                    {
                        if (onlyModelTypes)
                        {
                            if ((t.FullName.IndexOf("Entity") != -1 ||
                                t.FullName.IndexOf("Model") != -1 ||
                                // t.FullName.IndexOf("Domain") != -1 ||
                         t.FullName.IndexOf("Contract") != -1) && t.FullName.IndexOf(".Schema") == -1 && t.Name.StartsWith("Base") == false
                                && t.FullName.IndexOf("Mapping") == -1 && t.Name != "Properties")
                            {
                                //if (!IsComponentClass(t))
                                {
                                    var s = BuildTypeSchema(t);
                                    obj.Types.Add(s);
                                }
                            }
                        }
                        else
                        {
                            obj.Types.Add(BuildTypeSchema(t));
                        }
                    }
                }
            }
            return obj;
        }

        private static TypeSchema BuildTypeSchema(Type type)
        {
            var obj = new TypeSchema();
            if (type != null)
            {
                obj.Description = string.Empty;

                obj.TableName = type.Name;
                obj.Name = type.Name;
                obj.DisplayName = obj.Name;
                obj.DisplayShortName = obj.Name;


                obj.FullName = type.FullName;
                obj.Namespace = type.Namespace;
                obj.PrimaryKeyName = obj.Name + "ID";
                obj.PrimaryKeyType = "long";
                var ps = type.GetProperties(BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in ps)
                {
                    if (!p.IsSpecialName && p.Name != "Item")
                    {
                        var bps = BuildPropertySchema(p);
                        if (bps != null && bps.IsID)
                        {
                            obj.PrimaryKeyName = bps.Name;
                            obj.PrimaryKeyType = bps.TypeName;

                            var displayName = bps.DisplayName.Replace("标识", "");
                            if (!string.IsNullOrEmpty(displayName)) { obj.DisplayName = displayName; }

                            var displayShortName = bps.DisplayShortName.Replace("标识", "");
                            if (!string.IsNullOrEmpty(displayShortName)) { obj.DisplayShortName = displayShortName; }
                            displayName = null; displayShortName = null;
                        }
                        obj.Properties.Add(bps);
                    }
                }

            }
            return obj;
        }
        public static IList<PropertySchema> BuildPropertySchema(PropertyInfo[] ps)
        {
            List<Schema.PropertySchema> objs = new List<Schema.PropertySchema>();
            if (ps != null)
            {
                foreach (PropertyInfo p in ps)
                {
                    if (!p.IsSpecialName && p.Name != "Item")
                    {
                        var bps = BuildPropertySchema(p);
                        objs.Add(bps);
                    }
                }
            }
            return objs;
        }
        public static PropertySchema BuildPropertySchema(PropertyInfo p)
        {
            var obj = new PropertySchema();
            if (p != null)
            {
                obj.Description = string.Empty;
                obj.ColumnName = p.Name;
                obj.Name = p.Name;
                obj.DisplayName = obj.Name;
                obj.DisplayShortName = obj.Name;

                obj.Type = p.PropertyType;
                if (obj.Type.IsGenericType && obj.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    obj.Type = obj.Type.GetGenericArguments()[0];
                }

                obj.TypeName = obj.Type.Name;
                obj.TypeFullName = obj.Type.FullName;
                obj.IsArray = obj.Type.IsArray;
                obj.IsGenericType = obj.Type.IsGenericType;
                obj.IsValueType = obj.Type.IsValueType;
                obj.IsClass = obj.Type.IsClass;
                obj.IsComponentClass = IsComponentClass(obj.Type);

                
                ParseAttributes(obj, p);
            }
            return obj;
        }
        private static bool IsComponentClass(Type type)
        {
            if (type != null && type.IsClass)
            {
                var t = type.BaseType;
                if (t != null && (t.Name.IndexOf("Entity") >= 0 || t.Name.IndexOf("Model") >= 0))
                {
                    return false;
                }
                if (t == null)
                {
                    return true;
                }
                return IsComponentClass(t);
            }
            return false;
        }
        private static void ParseAttributes(PropertySchema obj, PropertyInfo p)
        {
            var atributes = p.GetCustomAttributes(true);
            if (atributes != null && atributes.Length > 0)
            {
                foreach (var atribute in atributes)
                {
                    if (atribute is NHibernate.Mapping.Attributes.PropertyAttribute)
                    {
                        var attr = atribute as NHibernate.Mapping.Attributes.PropertyAttribute;
                        obj.ColumnName = attr.Column;
                        obj.AllowNull = attr.NotNull;
                        obj.Length = attr.Length;
                        obj.IsID = false;
                        obj.IsIndex = !string.IsNullOrEmpty(attr.Index);
                        obj.IsUnique = attr.Unique;

                    }
                    else if (atribute is NHibernate.Mapping.Attributes.IdAttribute)
                    {
                        var attr = atribute as NHibernate.Mapping.Attributes.IdAttribute;
                        obj.ColumnName = attr.Column;
                        obj.AllowNull = false;
                        obj.DefaultValue = attr.UnsavedValue;
                        obj.Length = attr.Length;
                        obj.IsID = true;
                        obj.IsIndex = true;
                        obj.IsIndexed = true;
                        obj.IsKey = true;
                        obj.IsUnique = true;
                    }
                    else if (atribute is System.ComponentModel.DataAnnotations.DisplayAttribute)
                    {
                        var attr = atribute as System.ComponentModel.DataAnnotations.DisplayAttribute;
                        obj.Description = attr.Description != null ? attr.Description : string.Empty;
                        obj.DisplayName = attr.Name;
                        obj.DisplayShortName = attr.ShortName != null ? attr.ShortName : obj.DisplayName;
                    }
                }
            }

            if (p.Name == "" + p.ReflectedType.Name + "ID")
            {
                obj.IsID = true;
                obj.IsIndex = true;
                obj.IsIndexed = true;
                obj.IsKey = true;
                obj.IsUnique = true;
            }
        }
        #endregion

        #region

        public static AssemblySchema BuildAssemblySchema(SchemaExplorer.DatabaseSchema databaseSchema, string tablePrefix, string namespaces, IList<string> tables, IList<string> views)
        {
            var obj = new AssemblySchema();
            if (databaseSchema != null)
            {
                obj.FullName = databaseSchema.Name;

                if (databaseSchema.Tables != null)
                {
                    foreach (var t in databaseSchema.Tables)
                    {
                        if (tables.Contains(t.Name))
                        {
                            obj.Types.Add(BuildTypeSchema(t, tablePrefix, namespaces));
                        }
                    }
                }
                if (databaseSchema.Views != null)
                {
                    foreach (var t in databaseSchema.Views)
                    {
                        if (views.Contains(t.Name))
                        {
                            obj.Types.Add(BuildTypeSchema(t, tablePrefix, namespaces));
                        }
                    }
                }
            }
            return obj;
        }
        private static TypeSchema BuildTypeSchema(SchemaExplorer.TableSchema t, string tablePrefix, string namespaces)
        {
            var obj = new TypeSchema();
            if (t != null)
            {
                obj.TableName = tablePrefix + t.Name;
                obj.Name = Common.ConvertUtils.ToPascal(t.Name.Replace(tablePrefix, string.Empty));
                obj.FullName = t.FullName.Replace(tablePrefix, string.Empty);
                obj.Namespace = namespaces;

                obj.Description = t.Description;
                obj.DisplayName = obj.Name;
                obj.DisplayShortName = obj.Name;

                var indexColumns = new List<string>();
                foreach (var index in t.Indexes)
                {
                    foreach (var mc in index.MemberColumns)
                    {
                        indexColumns.Add(mc.Name);
                    }
                }


                foreach (var c in t.Columns)
                {
                    obj.Properties.Add(BuildPropertySchema(c, indexColumns));
                }
                if (t.PrimaryKey != null)
                {
                    obj.PrimaryKeyName = t.PrimaryKey.Name;
                    if (t.PrimaryKey.MemberColumns.Count > 0)
                    {
                        obj.PrimaryKeyName = t.PrimaryKey.MemberColumns[0].Name;
                        obj.PrimaryKeyType = t.PrimaryKey.MemberColumns[0].SystemType.Name;
                    }
                }
            }
            return obj;
        }
        private static TypeSchema BuildTypeSchema(SchemaExplorer.ViewSchema t, string tablePrefix, string namespaces)
        {
            var obj = new TypeSchema();
            if (t != null)
            {
                obj.TableName = tablePrefix + t.Name;
                obj.Name = Common.ConvertUtils.ToPascal(t.Name.Replace(tablePrefix, string.Empty));
                obj.FullName = t.FullName.Replace(tablePrefix, string.Empty);
                obj.Namespace = namespaces;

                obj.Description = t.Description;

                foreach (var c in t.Columns)
                {
                    obj.Properties.Add(BuildPropertySchema(c));
                }

            }
            return obj;
        }
        private static PropertySchema BuildPropertySchema(SchemaExplorer.ColumnSchema c, IList<string> indexColumns)
        {
            var obj = new PropertySchema();
            if (c != null)
            {
                obj.ColumnName = c.Name;
                obj.Name = Common.ConvertUtils.ToPascal(c.Name);
                obj.DisplayName = obj.Name;
                obj.DisplayShortName = obj.Name;

                obj.AllowNull = c.AllowDBNull;
                obj.Description = c.Description;

                obj.IsIndex = indexColumns.Contains(obj.Name);
                obj.IsIndexed = c.IsForeignKeyMember || c.IsPrimaryKeyMember || c.IsUnique;
                obj.IsIndexed = obj.IsIndexed || obj.IsIndex;

                obj.IsKey = c.IsForeignKeyMember || c.IsPrimaryKeyMember;
                obj.IsID = c.IsPrimaryKeyMember;
                obj.IsUnique = c.IsUnique;
                obj.Length = c.Size;

                obj.Type = c.SystemType;
                if (obj.Type.IsGenericType && obj.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    obj.Type = obj.Type.GetGenericArguments()[0];
                }
                obj.TypeFullName = obj.Type.FullName;
                obj.TypeName = obj.Type.Name;

                obj.IsClass = obj.Type.IsClass;
                obj.IsValueType = obj.Type.IsValueType;
                obj.IsComponentClass = IsComponentClass(obj.Type);
            }
            return obj;
        }
        private static PropertySchema BuildPropertySchema(SchemaExplorer.ViewColumnSchema c)
        {
            var obj = new PropertySchema();
            if (c != null)
            {
                obj.ColumnName = c.Name;
                obj.Name = Common.ConvertUtils.ToPascal(c.Name);
                obj.AllowNull = c.AllowDBNull;
                obj.Description = c.Description;

                obj.Length = c.Size;

                obj.Type = c.SystemType;
                if (obj.Type.IsGenericType && obj.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    obj.Type = obj.Type.GetGenericArguments()[0];
                }
                obj.TypeFullName = obj.Type.FullName;
                obj.TypeName = obj.Type.Name;

                obj.IsClass = obj.Type.IsClass;
                obj.IsValueType = obj.Type.IsValueType;
                obj.IsComponentClass = IsComponentClass(obj.Type);
            }
            return obj;
        }
        #endregion
    }
}
