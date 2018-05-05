using System;
using System.Collections.Generic;
using System.Text;
using Com.Zfrong.Common.Data.NH.ActiveRecords;
using Com.Zfrong.Common.Data.NH.DAL;//
namespace Com.Zfrong.Common.Data.NH.Test
{
    class test
    {
        public class CustomerDAL : DALObjectBase<Customer>//, ICustomerDao
        {
          public CustomerDAL()
          {
              //this.
          }
        }
        public class Customer 
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
                   // Check.Require(!string.IsNullOrEmpty(value), "A valid company name must be provided");
                    companyName = value;
                }
            }

            public string ContactName
            {
                get { return contactName; }
                set { contactName = value; }
            }

            

            #endregion
            #region Members

           
            private string companyName = "";
            private string contactName = "";
            #endregion
        }
    }
}
