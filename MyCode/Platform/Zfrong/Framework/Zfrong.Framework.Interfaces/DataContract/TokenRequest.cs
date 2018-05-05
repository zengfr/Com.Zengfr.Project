using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Zfrong.Framework.Core.DataContract
{
     [DataContract]
     [Serializable]
    public class TokenRequest
    {
         [DataMember]
         public string User { get; set; }
         [DataMember]
         public string Pwd { get; set; }
         [DataMember]
         public string Company { get; set; }
         [DataMember]
         public string System { get; set; }
    }
     [DataContract][Serializable]
     public class TokenResponse
     {
         [DataMember]
         public long   UserID { get; set; }
         [DataMember]
         public string Token { get; set; }
         [DataMember]
         public string Message { get; set; }
         [DataMember]
         public int ResultCode { get; set; }
         [DataMember]
         public DateTime DateTimeLimit { get; set; }
         [DataMember]
         public long RequestCount { get; set; }
     }
}
