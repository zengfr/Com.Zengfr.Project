using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Zfrong.Framework.Core.DataContract
{
    [DataContract(Name = "ExtRequest_{0}")]
    [Serializable]
    public class ExtRequest<T>
    {
        [DataMember]
        public T data { get; set; }
    }
    [DataContract]
    [KnownType(typeof(ExtRequest))]
    [Serializable]
    public class ExtRequest : ExtRequest<string>
    {

    }
}
