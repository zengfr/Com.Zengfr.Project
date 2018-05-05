using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Shards;
using NHibernate.Shards.Cfg;
using NHibernate.Shards.LoadBalance;
using NHibernate.Shards.Session;
using NHibernate.Shards.Strategy;
using NHibernate.Shards.Strategy.Access;
using NHibernate.Shards.Strategy.Resolution;
using NHibernate.Shards.Strategy.Selection;
using NHibernate.Tool.hbm2ddl;
using System;

namespace Com.Zengfr.Proj.Common.NH
{
    class NHShards
    {

        private void Run()
        {
            var connectionStringNames = new string[] { "Shard1", "Shard2", "Shard3" };
            bool createSchema = false;

            IList<IShardConfiguration> shardConfigs = PrepareConfiguration(connectionStringNames, createSchema);
            var sessionFactory = CreateShardedSessionFactory(shardConfigs, null);

            //var session = sessionFactory.OpenSession();
            //try
            //{
            //    ICriteria crit = session.CreateCriteria(typeof (WeatherReport),"weather");
            //    var count = crit.List();
            //    if (count != null) Console.WriteLine(count.Count);
            //    crit.Add(Restrictions.Gt("Temperature", 33));
            //    var reports = crit.List();
            //    if (reports != null) Console.WriteLine(reports.Count);
            //}
            //finally
            //{
            //    session.Close();
            //}


            Console.WriteLine("Done.");
            Console.ReadKey(true);
        }

        #region

        private static IShardStrategyFactory BuildShardStrategyFactory()
        {
            return new MyShardStrategyFactory();
        }

        private static Configuration GetConfigurationTemplate(string connectionStringName, int shardId, bool createSchema = false)
        {
            var cfg = new Configuration();

            cfg.SessionFactoryName("NHibernateShards" + shardId);
            cfg.Proxy(p =>
            {
                p.Validation = false;
                //p.ProxyFactoryFactory<ProxyFactoryFactory>();
            })
                .DataBaseIntegration(db =>
                {
                    db.Dialect<MsSql2008Dialect>();
                    db.ConnectionStringName = connectionStringName;
                })
                //.AddResource("NHibernate.Shards.Demo.Mappings.hbm.xml", Assembly.GetExecutingAssembly())
                .SetProperty(ShardedEnvironment.ShardIdProperty, shardId.ToString());
            if (createSchema)
            {
                new SchemaExport(cfg).Drop(false, true);
                new SchemaExport(cfg).Create(false, true);
            }
            return cfg;
        }
        #endregion

        #region

        public static IList<IShardConfiguration> PrepareConfiguration(string[] connectionStringNames, bool createSchema = false)
        {
            IList<IShardConfiguration> shardConfigs = new List<IShardConfiguration>();
            int index = 0;
            foreach (var connectionStringName in connectionStringNames)
            {
                shardConfigs.Add(new ShardConfiguration(GetConfigurationTemplate(connectionStringName, ++index, createSchema)));
            }
            return shardConfigs;
        }


        public static IShardedSessionFactory CreateShardedSessionFactory(IList<IShardConfiguration> shardConfigs, Dictionary<short, short> virtualShardMap)
        {
            IShardStrategyFactory shardStrategyFactory = BuildShardStrategyFactory();
            if (shardConfigs != null && shardConfigs.Count > 0)
            {
                Configuration prototypeConfig = GetConfigurationTemplate(shardConfigs[0].ConnectionStringName, 1);

                ShardedConfiguration shardedConfig = null;
                if (virtualShardMap != null)
                {
                    shardedConfig = new ShardedConfiguration(prototypeConfig, shardConfigs, shardStrategyFactory, virtualShardMap);
                }
                else
                {
                    shardedConfig = new ShardedConfiguration(prototypeConfig, shardConfigs, shardStrategyFactory);
                }
                return shardedConfig.BuildShardedSessionFactory();
            }
            return null;
        }
        #endregion

    }

    public class MyShardStrategyFactory : IShardStrategyFactory
    {
        #region IShardStrategyFactory Members

        public IShardStrategy NewShardStrategy(IEnumerable<ShardId> shardIds)
        {
            var loadBalancer = new RoundRobinShardLoadBalancer(shardIds); //RandomShardLoadBalancer
            var pss = new RoundRobinShardSelectionStrategy(loadBalancer);//轮询选择策略
            //ShardSelectionStrategy, 新增对象时，存储到哪个分区。 

            IShardResolutionStrategy prs = new AllShardsShardResolutionStrategy(shardIds);
            //ShardResolutionStrategy, 该策略用于查找单个对象时，判断它在哪个或哪几个分区上。

            IShardAccessStrategy pas = new SequentialShardAccessStrategy();
            //顺序策略：SequentialShardAccessStrategy， 每个query按顺序在所有分区上执行。 
            //平行策略：ParallelShardAccessStrategy， 每个query以多线程方式并发平行的在所有分区上执行。
            return new ShardStrategyImpl(pss, prs, pas);
        }

        #endregion

         
    }

}
