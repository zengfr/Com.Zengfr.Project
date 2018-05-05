using System;
using System.Collections.Generic;
using System.Text;
using Com.Zfrong.Common.Data.NHHelper.Utils;
using Com.Zfrong.Common.Data.NHHelper.Base.Domain;
using Com.Zfrong.Common.Data.NHHelper.Base;
namespace Com.Zfrong.Common.Data.NHHelper.Test
{
    class test
    {
        public class CustomerDao : AbstractNHibernateDao<Customer, string>//, ICustomerDao
        {
            public CustomerDao(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath) 
            {
            }
        }
        public class Customer : DomainObject<string>, IHasAssignedId<string>
        {
            #region Constructors
            /// <summary>
            /// Needed by ORM for reflective creation.
            /// </summary>
            private Customer() { }
            #endregion

            #region Properties

            public string CompanyName
            {
                get { return companyName; }
                set
                {
                    Check.Require(!string.IsNullOrEmpty(value), "A valid company name must be provided");
                    companyName = value;
                }
            }

            public string ContactName
            {
                get { return contactName; }
                set { contactName = value; }
            }

            

            #endregion

            public void SetAssignedIdTo(string assignedId)
            {
                Check.Require(!string.IsNullOrEmpty(assignedId), "assignedId may not be null or empty");

                // As an alternative to Check.Require, which throws an exception, the 
                // Validation Application Block could be used to validate the following
                Check.Require(assignedId.Trim().Length == 5, "assignedId must be exactly 5 characters");

                ID = assignedId.Trim().ToUpper();
            }

            /// <summary>
            /// Hash code should ONLY contain the "business value signature" of the object and not the ID
            /// </summary>
            public override int GetHashCode()
            {
                return (GetType().FullName + "|" +
                        CompanyName + "|" +
                        ContactName).GetHashCode();
            }
            

            #region Members

           
            private string companyName = "";
            private string contactName = "";
            #endregion
        }
    }
}

//<?xml version="1.0"?>
//<configuration>
//  <configSections>
//    <section name="nhibernateSettings"
//            type="Com.Zfrong.Common.Data.NHHelper.Base.OpenSessionInViewSection, Com.Zfrong.Common.Data.NHHelper.Base" />
//  </configSections>

//  <nhibernateSettings>
//    <!-- List every session factory that will be needed; transaction management and closing sessions 
//        will be managed with the open-session-in-view module -->
//    <sessionFactories>
//      <clearFactories />
//      <sessionFactory name="northwind" factoryConfigPath="C:\EnterpriseSample\EnterpriseSample.Web\Config\NorthwindNHibernate.config" isTransactional="true" />
//    </sessionFactories>
//  </nhibernateSettings>
//</configuration>
