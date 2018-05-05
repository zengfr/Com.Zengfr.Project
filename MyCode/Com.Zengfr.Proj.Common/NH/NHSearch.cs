
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Search;
using NHibernate.Search.Event;
namespace Com.Zengfr.Proj.Common.NH
{
    //    <section name="nhs-configuration" type="NHibernate.Search.Cfg.ConfigurationSectionHandler, NHibernate.Search" 
    //requirePermission="false" />
    //    <nhs-configuration xmlns='urn:nhs-configuration-1.0'>
    //        <search-factory  sessionFactoryName="NHibernateSearch.Demo">
    //            <property name='hibernate.search.default.directory_provider'>NHibernate.Search.Store.RAMDirectoryProviderFSDirectoryProvider, 
    //NHibernate.Search</property>
    //            <property name='hibernate.search.default.indexBase'>~/Index</property>
    //            <property name='hibernate.search.default.indexBase.create'>true</property>
    //        </search-factory>
    //    </nhs-configuration>
    /// <summary>
    /// 
    /// </summary>
    public class NHSearch
    {
        public static void Initialize()
        {
            Configuration configuration = new Configuration();
            configuration.SetListener(NHibernate.Event.ListenerType.PostUpdate, new FullTextIndexEventListener());
            configuration.SetListener(NHibernate.Event.ListenerType.PostInsert, new FullTextIndexEventListener());
            configuration.SetListener(NHibernate.Event.ListenerType.PostDelete, new FullTextIndexEventListener());

            configuration.SetListener(NHibernate.Event.ListenerType.PostCollectionRecreate, new FullTextIndexCollectionEventListener());
            configuration.SetListener(NHibernate.Event.ListenerType.PostCollectionRemove, new FullTextIndexCollectionEventListener());
            configuration.SetListener(NHibernate.Event.ListenerType.PostCollectionUpdate, new FullTextIndexCollectionEventListener());

            configuration.Configure();
            var sessionFactory = configuration.BuildSessionFactory();
            //SearchFactory.Initialize(configuration, sessionFactory);
        }
        public static IFullTextSession GetFullTextSession(ISessionFactory sessionFactory)
        {
            var fullTextSession = Search.CreateFullTextSession(sessionFactory.OpenSession());
            return fullTextSession;
        }
    }
}
