using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model
{
   public class SearchTerm : MyModelBase
   {
       //public SearchTerm()
       //{
       //      Product=new CompanyGroup();
       //  }
        public virtual string Name { get; set; }
        public virtual Product Product { get; set; }
    }
}
