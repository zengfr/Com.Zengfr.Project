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
    public class TableController : ApiController
    {
        [HttpGet]
        public IList<Model> GetList(long databaseId)
        {
            IList<Model> results = new List<Model>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Model>();
                cri.Add(Expression.Eq("Database", new Database() { DatabaseId = databaseId }));
                results = cri.List<Model>();
            }
            return results;
        }

        [HttpGet]
        public IList<Model> GetList()
        {
            IList<Model> results = new List<Model>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Model>();
                results = cri.List<Model>();
            }
            return results;
        }
        [HttpGet]
        public IList<Model> GetTree(long companyId, long databaseId,long modelId)
        {
            IList<Model> results = new List<Model>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Model>();
                results = cri.List<Model>();
            }
            return results;
        }
        [HttpGet]
        public Model Get(int keyId)
        {
            Model result = new Model();
            using (var session = NHibernateUtil.Session)
            {
                result = session.Get<Model>(keyId);
            }
            return result;
        }
        [HttpPost]
        public void SaveOrUpdate(Model item)
        {
            using (var session = NHibernateUtil.Session)
            {
                session.SaveOrUpdate(item);
            }
        }
    }

    
}