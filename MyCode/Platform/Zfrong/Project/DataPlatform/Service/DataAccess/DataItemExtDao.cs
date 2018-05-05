using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.Core.IDao;
using Zfrong.Framework.Core.IDao.Plugin;
using Zfrong.Framework.CoreBase.Dao;
using Zfrong.Framework.Core.Model;
using DataPlatform.Model;
using NHibernate;

namespace DataPlatform.Dao
{
    public interface IDataItemExtDao : IExtDao<DataItem>
    {

    }
    public class DataItemExtDao : ExtDaoBase<DataItem>, IDataItemExtDao
    {
        public virtual ISessionFactory SessionFactory { get; set; }
        public DataItemExtDao(IExtDaoPlugin<DataItem> plugin)
            : base(plugin)
        {

        }
    }
}
