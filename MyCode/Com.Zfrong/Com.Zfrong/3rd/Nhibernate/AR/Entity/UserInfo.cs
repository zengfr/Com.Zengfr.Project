using System;
using System.Collections.Generic;
using System.Text;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using System.Security.Principal;
using System.Security;

namespace Com.Zfrong.Common.Data.AR.Entity
{
    [ActiveRecord("ut_UserInfo",DynamicUpdate=true)]
   public  class UserInfo//:EntityBase
   {
       int userID;
       [PrimaryKey(PrimaryKeyType.Foreign)]
       public virtual int UserID
       {
           get { return userID; }
           set { userID = value; }
       }
       User user;
       [OneToOne]
       public virtual User User
       {
           get { return user; }
           set {user=value; }
       }
       [Field]
       public  int Age;
       [Field]
       public bool Sex;
       [Field]
       public string RealName;
       [Field]
       public string IDCard;
       
       [Field]
        public string Tel;
       [Field]
        public string Phone;
       [Field]
        public string Msn;
       [Field]
        public string QQ;

       [Field]
        public string Country;
       [Field]
        public string Province;
       [Field]
        public string Area;
       [Field]
        public string City;

       [Field]
        public string Place;//住址
       [Field]
       public string ZipCode;// 邮编
       [Field]
       public string Birthday;//生日
       [Field]
       public byte Horoscope;//星座
       [Field]
       public byte Zodiac;//生肖
       [Field]
       public byte Blood;//血型
       [Field]
       public string Idiograph;//签名
       [Field]
       public string BankName;
       [Field]
       public string BankNum;

       [Field]
       public string HomePage;//主页
       [Field]
       public string Occupation;//职业
       [Field]
       public string Trade;//行业
    }
}
