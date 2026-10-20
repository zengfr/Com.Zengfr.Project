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
    public class ModelAttrController : ApiController
    {
        [HttpGet]
        public IList<ModelAttr> GetList(long databaseId)
        {
            IList<ModelAttr> results = new List<ModelAttr>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Model>();
                cri.Add(Expression.Eq("Database", new Database() { DatabaseId = databaseId }));
                results = cri.List<ModelAttr>();
            }
            return results;
        }

        [HttpGet]
        public IList<ModelAttr> GetList()
        {
            IList<ModelAttr> results = new List<ModelAttr>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<ModelAttr>();
                results = cri.List<ModelAttr>();
            }
            return results;
        }
        [HttpGet]
        public ModelAttr Get(int keyId)
        {
            ModelAttr result = new ModelAttr();
            using (var session = NHibernateUtil.Session)
            {
                result = session.Get<ModelAttr>(keyId);
            }
            return result;
        }
        [HttpPost]
        public void SaveOrUpdate(ModelAttr item)
        {
            using (var session = NHibernateUtil.Session)
            {
                session.SaveOrUpdate(item);
            }
        }
    }

    
}