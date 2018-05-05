using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
namespace Zfrong.Framework.Repository.Utils
{
    public class NHibernateConfiguration
    {
        private static object locker = new object();
        static NHibernateConfiguration instance;
        public static NHibernateConfiguration Instance
        {
            get
            {
                if (instance== null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new NHibernateConfiguration();
                        }
                    }
                }
                return instance;
            }
        }

        private IDictionary<string, Configuration> Configurations = new Dictionary<string, Configuration>();

        public Configuration GetConfiguration(string key)
        {
            if (!Configurations.ContainsKey(key))
            {
                Configurations[key] = InitConfigure(key);
            }
            return Configurations[key];
        }
        private Configuration InitConfigure(string key)
        {
            var configuration = new Configuration();
            // 資料庫設定
            // 這裡的東西可以改用xml的方式設定，增加修改的彈性
            configuration.DataBaseIntegration(c =>
            {
                // 資料庫選用 SQLite
                c.Dialect<MsSql7Dialect>();
                // 取用 .config 中的 "MyTestDB" 連線字串
                c.ConnectionStringName = "MyTestDB" + key;
                // Schema 變更時的處置方式
                c.SchemaAction = SchemaAutoAction.Update;
                // 交易隔離等級
                c.IsolationLevel = IsolationLevel.ReadCommitted;
            });
            return configuration;
        }
        public void AddMapping(string key,System.Type[] types)
        {
            var configuration = GetConfiguration(key);
            var mapping = GetMapping(types);
            configuration.AddMapping(mapping);
        }
        private HbmMapping GetMapping(System.Type[] types)
        {
            var mapper = new ModelMapper();
            mapper.BeforeMapClass +=(mi, t, map) =>
            {
                map.Id(x => { x.Column("TID"); x.Generator(Generators.Identity); });
            };
            mapper.BeforeMapProperty +=(mi, propertyPath, map) => 
            {
               map.Column(x=>{x.Name(propertyPath.ToColumnName());x.NotNullable(false);});
            };
            mapper.AddMappings(types);
            HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            return mapping;
        }

        

    }
}
