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
public class departmentsDao:DaoBase<departments>,IdepartmentsDao
{
	public virtual ISessionFactory SessionFactory { get; set; }
	public departmentsDao(IDaoPlugin<departments> plugin): base(plugin)
	{

	}
}
}
