using System;
using System.Collections.Generic;
using System.Text;
using Com.Zfrong.Common.Data.NHHelper.Utils;
namespace Com.Zfrong.Common.Data.NHHelper.Base
{
    public abstract class AbstractNHibernateDaoFactory
    {
        public AbstractNHibernateDaoFactory(string sessionFactoryConfigPath)
        {
            Check.Require(sessionFactoryConfigPath != null, "sessionFactoryConfigPath may not be null");
            SessionFactoryConfigPath = sessionFactoryConfigPath;
        }

        protected string SessionFactoryConfigPath {
            get {
                return sessionFactoryConfigPath;
            }
            set {
                sessionFactoryConfigPath = value;
            }
        }
        private string sessionFactoryConfigPath;
    }
}
