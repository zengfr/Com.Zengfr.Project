using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Com.Zengfr.CodeGenerator.Web;
using Com.Zengfr.CodeGenerator.Web.Models;
using Com.Zengfr.CodeGenerator.Web.Util;

namespace Com.Zengfr.CodeGenerator.WebUI.Controllers
{
    public class CompanyController : ApiController
    {
        [HttpGet]
        public IList<Company> GetList()
        {
            IList<Company> results = new List<Company> ();
            using (var session = NHibernateUtil.Session)
            {
                var cri=session.CreateCriteria<Company>();
                results=cri.List<Company>();
            }
            return results;
        }
        [HttpGet]
        public IList<Company> GetTree(long companyId)
        {
            IList<Company> results = new List<Company>();
            using (var session = NHibernateUtil.Session)
            {
                var cri = session.CreateCriteria<Company>();
                results = cri.List<Company>();
            }
            return results;
        }
        [HttpGet]
        public  Company  Get(int companyId)
        {
            Company result = new Company();
            using (var session = NHibernateUtil.Session)
            {
                result = session.Get<Company>(companyId);
            }
            return result;
        }
        [HttpPost]
        public void SaveOrUpdate(Company company)
        {
            using (var session = NHibernateUtil.Session)
            {
                session.SaveOrUpdate(company);
            }
        }
    }

    
}