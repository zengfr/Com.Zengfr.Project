using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common.NH
{
    public class SessionFactoryUtils
    {
        public static ISessionFactory Create(string xmlFileName, string xmlFactoryName)
        {
            if (string.IsNullOrWhiteSpace(xmlFileName))
                xmlFileName = "nhibernate.cfg.xml";
            var path = AppDomain.CurrentDomain.BaseDirectory;
            NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
            config.Configure(string.Format("{0}\\{1}", path, xmlFileName), xmlFactoryName);
            return config.BuildSessionFactory();
        }
        private static IDictionary<string, ISessionFactory> sessionFactoryDict = new Dictionary<string, ISessionFactory>();
        public static ISessionFactory CreateOrGet(string xmlFileName, string xmlFactoryName)
        {
            var key = string.Format("{0},{1}", xmlFactoryName, xmlFileName);
            if ((!sessionFactoryDict.ContainsKey(key))|| sessionFactoryDict[key].IsClosed)
            {
                sessionFactoryDict.Remove(key);
                sessionFactoryDict.Add(key, Create(xmlFactoryName, xmlFileName));
            }
            return sessionFactoryDict[key];
        }
    }
}
