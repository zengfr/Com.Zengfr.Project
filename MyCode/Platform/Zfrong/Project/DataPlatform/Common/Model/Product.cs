using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.CoreBase.Model.Mapping;
namespace DataPlatform.Model
{
  public  class Product : MyModelBase
    {// public Product(){
    //         Brand=new CompanyGroup();
    //     }
        public virtual string Name { get; set; }
        public virtual Brand Brand { get; set; }
    }
}
