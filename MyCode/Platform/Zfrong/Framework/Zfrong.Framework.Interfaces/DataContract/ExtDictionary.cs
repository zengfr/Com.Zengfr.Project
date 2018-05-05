using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace Zfrong.Framework.Core.DataContract
{
    [CollectionDataContract
    (Name = "MyDictionary",
    ItemName = "Entry",
    KeyName = "K",
    ValueName = "V")]
    public  class ExtDictionary<K, V> : Dictionary<K, V>
    {

    }
}
