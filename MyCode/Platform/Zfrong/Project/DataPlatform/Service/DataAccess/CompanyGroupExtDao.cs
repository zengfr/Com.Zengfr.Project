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
    public interface ICompanyGroupExtDao : IExtDao<CompanyGroup>
    {

    }
    public class CompanyGroupExtDao : ExtDaoBase<CompanyGroup>, ICompanyGroupExtDao
    {
        public virtual ISessionFactory SessionFactory { get; set; }
        public CompanyGroupExtDao(IExtDaoPlugin<CompanyGroup> plugin)
            : base(plugin)
        { 

        }
    }

}
