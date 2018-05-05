using System;
using System.Collections;//
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using System.Collections;
namespace Com.Zfrong.Common.Data.NH
{
    class test2
    {
        public static void Main(string[] args)
        {
            string sql="select * from Test where id<500";//

            NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
            //config.AddAssembly(typeof(MyNamespace.Data.Test).Assembly);//
            //config.AddFile("Test.hbm.xml");//
            //Com.Zfrong.Common.Data.NH.NHHelper.SessionFactory = config.BuildSessionFactory();//
            Com.Zfrong.Common.Data.NH.NHHelper.Execute(sql);//

            Page.PageHelper helper = new Page.PageHelper();
            helper.Session = config.BuildSessionFactory().OpenSession();

            ICriteria c = null;// helper.Session.CreateCriteria(typeof(MyNamespace.Data.Test));//
            c.Add(Expression.Lt("id", 500));//
            helper.GetPagedQuery(c, 3);//
            Page.Page p = helper.GetPagedQuery(helper.CreateSQLQuery(sql), 3);//

        }
       
    }
}
