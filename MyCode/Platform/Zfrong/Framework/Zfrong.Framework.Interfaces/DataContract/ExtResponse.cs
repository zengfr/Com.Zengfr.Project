using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zfrong.Framework.Core.DataContract
{
    [DataContract(Name = "ExtPagingResponse_{0}")]
    [Serializable]
    public class ExtPagingResponse<T> : ExtResponse<T>
    {
        [DataMember]
        public int totalCount { get; set; }
        [DataMember]
        public IList<T> items { get; set; }
    }



    [DataContract(Name = "ExtResponse_{0}")]
    [Serializable]
    public class ExtResponse<T>
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public T data { get; set; }
    }
    [DataContract]
    [KnownType(typeof(ExtResponse))]
    [Serializable]
    public class ExtResponse : ExtResponse<string>
    {

    }
}
