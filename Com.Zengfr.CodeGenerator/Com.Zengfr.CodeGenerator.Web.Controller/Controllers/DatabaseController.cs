using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Com.Zengfr.CodeGenerator.Web;
using Com.Zengfr.CodeGenerator.Web.Models;
using Com.Zengfr.CodeGenerator.Web.Util;
using NHibernate.Criterion;

namespace Com.Zengfr.CodeGenerator.WebUI.Controllers
{
    public class DatabaseController : ApiController
    {
        [HttpGet]
        public IList<Database> GetList(long companyId)
        {
            IList<Database> results = new List<Database>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Database>();
                cri.Add(Expression.Eq("Company", new Company() { CompanyId= companyId }));
                results = cri.List<Database>();
            }
            return results;
        }

        [HttpGet]
        public IList<Database> GetList()
        {
            IList<Database> results = new List<Database> ();
            using (var session = NHibernateUtil.Session)
            {
                var cri=session.CreateCriteria<Database>();
                results=cri.List<Database>();
            }
            return results;
        }
        [HttpGet]
        public IList<Database> GetTree(long companyId, long databaseId)
        {
            IList<Database> results = new List<Database>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Database>();
                results = cri.List<Database>();
            }
            return results;
        }
        [HttpGet]
        public Database Get(int keyId)
        {
            Database result = new Database();
            using (var session = NHibernateUtil.Session)
            {
                result = session.Get<Database>(keyId);
            }
            return result;
        }
        [HttpPost]
        public void SaveOrUpdate(Database item)
        {
            using (var session = NHibernateUtil.Session)
            {
                session.SaveOrUpdate(item);
            }
        }
    }

    
}