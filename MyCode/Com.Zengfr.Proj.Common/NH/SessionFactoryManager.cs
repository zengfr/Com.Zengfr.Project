using System;
using System.Reflection;
using System.Web;
using Com.Zengfr.Proj.Common.NH;
using Com.Zengfr.Proj.Common.NH.Audit;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
namespace Services.NHJunk
{
    public class SessionFactoryManager
    {
        public class DefaultClassConvention : IClassConvention
        {
            public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
            {
                instance.DynamicInsert();
                instance.DynamicUpdate();
            }
        }
        public class DefaultPrimarykeyConvention : IIdConvention
        {
            /// <summary>  
            /// 指定Entity Class到Table ID的生成规则，及ID的规则  
            /// </summary>  
            /// <param name="instance">生成实体的对象</param>  
            public void Apply(FluentNHibernate.Conventions.Instances.IIdentityInstance instance)
            {
                instance.Column(instance.EntityType.Name + "ID");
                instance.GeneratedBy.Identity();
                instance.UnsavedValue("0");
            }
        }
        public class DefaultPropertyConvention : IPropertyConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                if (instance.Name.EndsWith("ID"))
                {
                    instance.Index(string.Format("index_{0}_{1}", instance.EntityType.Name, instance.Name));
                }
                if (instance.Name.EndsWith("ChangeLastTime"))
                {
                    instance.Index(string.Format("index_{0}_{1}", instance.EntityType.Name, instance.Name));
                }
            }
        }
        public class DefaultJoinConvention : IJoinConvention
        {
            public void Apply(IJoinInstance instance)
            {
                // instance.Optional();
            }
        }
        public class DefaultReferenceConvention : IReferenceConvention
        {
            public virtual bool CascadeAll { get; set; }
            public DefaultReferenceConvention(bool cascadeAll)
            {
                CascadeAll = cascadeAll;
            }
            /// <summary>  
            /// 指定对象外键的生成规则  
            /// </summary>  
            /// <param name="instance">外键对象</param>  
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column(instance.Name + "ID");
                instance.Fetch.Join();
                if (CascadeAll)
                {
                    instance.Cascade.All();
                }
                else
                {
                    instance.Cascade.None();
                }
                instance.Nullable();
                instance.Insert();
                instance.Update();
                instance.Index(string.Format("index_{0}_{1}{2}", instance.EntityType.Name, instance.Name, "ID"));
                //instance.OptimisticLock();
                instance.NotFound.Ignore();
                instance.LazyLoad();

            }

        }
        public ISessionFactory CreateSessionFactory(bool autoGeneratorData)
        {
            var assemblyName = "Com.Zengfr.Proj.Domain.Entity.Map";
            var connectionStringKey = "ConnectionString";
            return CreateSessionFactory(assemblyName, connectionStringKey, autoGeneratorData);
        }
        public ISessionFactory CreateSessionFactory(string assemblyName, string connectionStringKey, bool autoGeneratorData)
        {
            ISessionFactory sessionFactory = null;
            try
            {
                sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c =>
                        c.FromConnectionStringWithKey(connectionStringKey)).Driver<ProfiledSql2008ClientDriver>()
                        .UseOuterJoin()
                        .AdoNetBatchSize(50)
                        .ShowSql().FormatSql())
                    .Mappings(x =>
                    {
                        var assembly = Assembly.Load(assemblyName);
                        x.FluentMappings.Add(typeof(AuditMetadataLogMap));
                        x.FluentMappings.AddFromAssembly((assembly))
                            .Conventions.Setup(c =>
                            {
                                c.Add(new DefaultClassConvention());
                                // c.Add(new DefaultJoinConvention());
                                c.Add(new DefaultReferenceConvention(autoGeneratorData));
                                c.Add(new DefaultPrimarykeyConvention());
                                c.Add(new DefaultPropertyConvention());

                            }).ExportTo(SchemaUpdateUtils.GetBaseDirectoryPath("hbm"));
                    }
                    )
                    .ExposeConfiguration(x =>
                    {
                        //x.SetProperty("hbm2ddl.auto", "create-drop");
                        //x.SetProperty("hbm2ddl.keywords", "auto-quote");
                        x.SetProperty("command_timeout","60");
                        x.SetProperty("show_sql", "true");
                        x.SetProperty("use_sql_comments", "true");
                        x.SetProperty("format_sql", "true");
                        x.SetProperty("generate_statistics", "true");
                        x.SetProperty("current_session_context_class", "call");
                        x.SetProperty("use_outer_join", "true");

                        //new SchemaExport(x).SetOutputFile(fileName).Drop(false, true);
                        //new SchemaExport(x).Drop(false, true);

                        //new SchemaUpdate(x).Execute(false, true);
                        x.Interceptor = new HintOptionInterceptor();

                        //x.AppendListeners(ListenerType.PostUpdate,
                        //    new IPostUpdateEventListener[]{new AuditLogEventListener()
                        //});

                        //x.AppendListeners(ListenerType.PreUpdate,
                        //   new IPreUpdateEventListener[]{new AuditLogEventListener()
                        //});

                        //var enversConf = new NHibernate.Envers.Configuration.Fluent.FluentConfiguration();
                        //enversConf.Audit(x.ClassMappings.Select(t => t.MappedClass).Where(t => t.Name.EndsWith("PayBank")));
                        //x.IntegrateWithEnvers(enversConf);

                        var enversConf = new NHibernate.Envers.Configuration.Attributes.AttributeConfiguration();
                        x.SetEnversProperty(ConfigurationKey.StoreDataAtDelete, true);
                        x.IntegrateWithEnvers(enversConf);

                        if (!autoGeneratorData)
                        {
                            SchemaUpdateUtils.ExportSql(x);
                            SchemaUpdateUtils.ExportUpdateSql(x);
                            use0insteadofnullforforeignkeys(x);
                        }
                    }
                    )
                    .BuildSessionFactory();

                if (autoGeneratorData)
                {
                    AutoDataGenerator.GeneratorData(sessionFactory, 111);
                }
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Application["NHStatistics"] = sessionFactory.Statistics;
                }
                sessionFactory.Statistics.OperationThreshold = TimeSpan.FromMilliseconds(300);//1秒


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sessionFactory;
        }
        private static void use0insteadofnullforforeignkeys(Configuration config)
        {
            foreach (var persistentClass in config.ClassMappings)
            {
                persistentClass.AddTuplizer(EntityMode.Poco, typeof(NullableTuplizer2).AssemblyQualifiedName);
            }
        }

    }
}