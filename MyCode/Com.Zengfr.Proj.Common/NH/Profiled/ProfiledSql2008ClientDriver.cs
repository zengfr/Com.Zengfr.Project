using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Driver;

namespace Com.Zengfr.Proj.Common.NH
{
    /// <summary>
    /// MiniProfiler.NHibernateSqlDriver
    /// </summary>
    public class ProfiledSql2008ClientDriver : Sql2008ClientDriver, IEmbeddedBatcherFactoryProvider
    {
        public override IDbCommand CreateCommand()
        {
            return (DbCommand)
                    new DbCommandProxy((DbCommand)base.CreateCommand())
                    .GetTransparentProxy();

            //return base.CreateCommand();
        }

       
    }
}