using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using log4net;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.NH
{
    public class AutoDataGenerator
    {
        static ILog log = LogManager.GetLogger(typeof(AutoDataGenerator));
        public static void GeneratorData(Configuration config, int count)
        {
            var sessionFactory = config.BuildSessionFactory();
            GeneratorData(sessionFactory, count);
        }
        public static void GeneratorData(ISessionFactory sessionFactory, int count)
        {
            var allClassMetadata = ((NHibernate.Impl.SessionFactoryImpl)(sessionFactory)).GetAllClassMetadata();

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 20;

            Parallel.ForEach(allClassMetadata.Values, parallelOptions, (classMetadata) =>
            {
                try
                {
                    using (var session = sessionFactory.OpenStatelessSession())
                    {
                        session.SetBatchSize(100);
                        var t = classMetadata.GetMappedClass(EntityMode.Poco);
                        var realCount = session.CreateCriteria(t).SetProjection(Projections.RowCount()).UniqueResult<int>();
                        if (realCount < count)
                        {
                            using (ITransaction tx = session.BeginTransaction())
                            {
                                for (int index = realCount; index < count; index++)
                                {
                                    try
                                    {
                                        var obj = System.Activator.CreateInstance(t);
                                        AutoDataGenerator.SetPropertiesRandomValue(obj);
                                        session.Insert(obj);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex); //throw ex;
                                    }
                                }
                                tx.Commit();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex); //throw ex;
                }
            });
        }
        public static void SetPropertiesRandomValue(object obj)
        {
            if (obj == null) return;
            var type = obj.GetType();
            var ps = type.GetProperties();
            Random random = new Random();
            string randomValue;
            foreach (var p in ps)
            {
                randomValue = random.Next(0, 1024).ToString();
                if (p.Name.IndexOf("Rate") != -1)
                {
                    randomValue = random.Next(0, 99).ToString();
                }
                switch (p.PropertyType.Name)
                {
                    case "String":
                        randomValue = random.Next(0, 1024 * 10).ToString();
                        p.SetValue(obj, "String" + randomValue, null); break;
                    case "Int64":
                        p.SetValue(obj, long.Parse(random.Next(1, 11).ToString()), null); break;
                    case "Int32":
                        p.SetValue(obj, int.Parse(randomValue), null); break;
                    case "Int16":
                        p.SetValue(obj, short.Parse(randomValue), null); break;
                    case "Decimal":
                        p.SetValue(obj, decimal.Parse(randomValue), null); break;
                    case "Double":
                        p.SetValue(obj, double.Parse(randomValue), null); break;
                    case "Single":
                        p.SetValue(obj, float.Parse(randomValue), null); break;
                    case "Byte":
                        randomValue = random.Next(0, 127).ToString();
                        p.SetValue(obj, byte.Parse(randomValue), null); break;
                    case "DateTime":
                        p.SetValue(obj, DateTime.Now.AddDays(int.Parse(randomValue)), null); break;
                    case "Boolean":
                        p.SetValue(obj, random.Next(0, 127) > 63 ? true : false, null); break;
                    default:
                        if (p.Name != "Item" && p.PropertyType.IsClass)
                        {
                            SetPropertiesRandomValue(p.GetValue(obj, null));
                        }
                        break;
                }
            }
        }
    }
}
