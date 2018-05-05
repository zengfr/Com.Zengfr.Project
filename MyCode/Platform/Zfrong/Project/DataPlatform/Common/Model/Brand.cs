using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.CoreBase.Model.Mapping;
//using System.ComponentModel.DataAnnotations; 
namespace DataPlatform.Model
{
   public class Brand : MyModelBase
    {
         //public Brand(){
         //    CompanyGroup=new CompanyGroup();
         //}
        public virtual string Name { get; set; }
        public virtual CompanyGroup CompanyGroup { get; set; }
    }
}
