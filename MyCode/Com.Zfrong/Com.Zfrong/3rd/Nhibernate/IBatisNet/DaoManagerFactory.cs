using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.Configuration;
namespace Com.Zfrong.Common.Data.IBatisNet
{
    public class DaoManagerFactory
        {
            static private object _synRoot = new Object();
        static private DaoManagerFactory _instance;

            private IDictionary<string,IDaoManager> daoManagerMap =new Dictionary<string,IDaoManager>();

            /// <summary>
            /// Remove public constructor. prevent instantiation.
            /// </summary>
        private DaoManagerFactory() { }

        public static DaoManagerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            
                            try
                            {
                                Build();//
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            _instance = new DaoManagerFactory();
                        }
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Reset the singleton
        /// </summary>
        /// <remarks>
        /// Must verify ConfigureHandler signature.
        /// </remarks>
        /// <param name="obj">
        /// </param>
        public static void Reset()
        {
            Reset(null);
        }
        private static void Reset(object obj)
        {
            _instance = null;
        }

        public static void Build()
        {
            Build(null);//
        }
        public static void Build(string daoConfigFile)
        {
            ConfigureHandler handler = new ConfigureHandler(Reset);
            DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
            if (daoConfigFile == null)
                builder.ConfigureAndWatch(handler);
            else
                builder.ConfigureAndWatch(daoConfigFile, handler);
        }

        public IDaoManager Get()
        {
            return Get("");
        }
        public IDaoManager Get(string contextName)
        {
            if (!daoManagerMap.ContainsKey(contextName))
            {
                IDaoManager obj;
                if (contextName.Length == 0)
                    obj = DaoManager.GetInstance();//
                else
                    obj = DaoManager.GetInstance(contextName);
                daoManagerMap[contextName] = obj;//
            }
            return daoManagerMap[contextName];
        }
        public IDaoManager Get(IDao dao)
        {
            string key = dao.GetType().Name;
            if (!daoManagerMap.ContainsKey(key))
            {
                IDaoManager obj = DaoManager.GetInstance(dao);
                daoManagerMap[key] = obj;//
            }
            return daoManagerMap[key];
        }
    }
}