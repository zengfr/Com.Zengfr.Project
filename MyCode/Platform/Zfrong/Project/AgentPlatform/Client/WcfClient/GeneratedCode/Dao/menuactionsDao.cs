using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model.Base;
using NHibernate;
using NHibernate.Criterion;
using Zfrong.Framework.Core.Dao.Base;
using Zfrong.Framework.Core.Dao;
using GeneratedCode.Model;
namespace GeneratedCode.Dao{
public class menuactionsDao:DaoBase<menuactions>,ImenuactionsDao
{
	public virtual ISessionFactory SessionFactory { get; set; }
	public menuactionsDao(IDaoPlugin<menuactions> plugin): base(plugin)
	{

	}
}
}
