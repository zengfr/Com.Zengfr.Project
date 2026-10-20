using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.CodeGenerator.Common.Util;
using Common.Logging;
using SchemaExplorer;
namespace Com.Zengfr.CodeGenerator.Common.Meta.Builder
{
    public class ZMeta4DBBuilder : IZMetaBuilder
    {
        private static ILog logger = LogManager.GetLogger(typeof(ZMeta4DBBuilder));
        private DatabaseSchema databaseSchema;
        private string tablePrefix;
        private string namespaces;
        public ZMeta4DBBuilder(DatabaseSchema databaseSchema,string tablePrefix, string namespaces) {
           this. databaseSchema= databaseSchema;
            this.tablePrefix = tablePrefix;
            this.namespaces = namespaces;
    }
        public virtual ZMeta BuildMeta()
        {
            ZMeta meta = new ZMeta();
            meta = BuildMeta(databaseSchema, tablePrefix, namespaces);
            return meta;
        }

        #region

        public static ZMeta BuildMeta(SchemaExplorer.DatabaseSchema databaseSchema, string tablePrefix, string namespaces)
        {
            var obj = new ZMeta();
            if (databaseSchema != null)
            {
                obj.Name = databaseSchema.Name;
                logger.Debug(string.Format("BuildMeta:{0},{1}", tablePrefix, namespaces, databaseSchema.ConnectionString));
                if (databaseSchema.Tables != null)
                {
                    foreach (var t in databaseSchema.Tables)
                    {
                        obj.Models.Add(BuildMetaModel(t, tablePrefix, namespaces));
                    }
                }
                if (databaseSchema.Views != null)
                {
                    foreach (var t in databaseSchema.Views)
                    {
                       obj.Models.Add(BuildMetaModel(t, tablePrefix, namespaces));
                      
                    }
                }
            }
            return obj;
        }
        private static ZMetaModel BuildMetaModel(SchemaExplorer.TableSchema t, string tablePrefix, string namespaces)
        {
            var obj = new ZMetaModel();
            if (t != null)
            {
                logger.Debug(string.Format("BuildMetaModel:{0},{1},{2}", tablePrefix,  namespaces, t.Name));
                obj.Name = StringUtil.Instance.ToPascal(t.Name.Replace(tablePrefix, string.Empty));
                obj.Namespace = namespaces;
                obj.Description = t.Description;
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
                    obj.Attributes.Add(BuildMetaModelAttribute(c, indexColumns));
                }
            }
            return obj;
        }
        private static ZMetaModel BuildMetaModel(SchemaExplorer.ViewSchema t, string tablePrefix, string namespaces)
        {
            var obj = new ZMetaModel();
            if (t != null)
            {
                obj.Name = StringUtil.Instance.ToPascal(t.Name.Replace(tablePrefix, string.Empty));
                obj.Namespace = namespaces;

                obj.Description = t.Description;

                foreach (var c in t.Columns)
                {
                    obj.Attributes.Add(BuildMetaModelAttribute(c));
                }

            }
            return obj;
        }
        private static ZMetaAttribute BuildMetaModelAttribute(SchemaExplorer.ColumnSchema c, IList<string> indexColumns)
        {
            var obj = new ZMetaAttribute();
            if (c != null)
            {
                obj.Name = StringUtil.Instance.ToPascal(c.Name);

                obj.AllowNull = c.AllowDBNull;
                obj.Description = c.Description;

                obj.IsIndex = indexColumns.Contains(obj.Name);
                obj.IsKey = c.IsForeignKeyMember || c.IsPrimaryKeyMember;
                obj.IsID = c.IsPrimaryKeyMember;
                obj.IsUnique = c.IsUnique;
                obj.Length = c.Size;

                var type = c.SystemType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                obj.TypeFullName = type.FullName;
                obj.TypeName = type.Name;
                obj.DbType = c.NativeType;

            }
            return obj;
        }
        private static ZMetaAttribute BuildMetaModelAttribute(SchemaExplorer.ViewColumnSchema c)
        {
            var obj = new ZMetaAttribute();
            if (c != null)
            {
                obj.Name = StringUtil.Instance.ToPascal(c.Name);
                obj.AllowNull = c.AllowDBNull;
                obj.Description = c.Description;

                obj.Length = c.Size;

                var type = c.SystemType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                obj.TypeFullName = type.FullName;
                obj.TypeName = type.Name;
                obj.DbType = c.NativeType;
            }
            return obj;
        }
        #endregion
    }
}
