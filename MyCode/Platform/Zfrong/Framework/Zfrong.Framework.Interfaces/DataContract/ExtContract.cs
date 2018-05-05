using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Zfrong.Framework.Core.DataContract
{
    [DataContract]
    [Serializable]
    public class ExtKVItem : ExtKVItem<string>
    {
        public ExtKVItem(string key, string value) { Key = key; Value = value; }
    }
    [DataContract(Name = "ExtKVItem_{0}")]
    [Serializable]
    public class ExtKVItem<T>
    {
        public ExtKVItem() { }
        public ExtKVItem(string key, T value) { Key = key; Value = value; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public T Value { get; set; }
    }
    [DataContract]
    [KnownType(typeof(ExtFilterItem))]
    [Serializable]
    public class ExtFilterItem
    {
        public ExtFilterItem() { Data = new ExtFilterData(); }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public ExtFilterData Data { get; set; }
    }

    [DataContract]
    [KnownType(typeof(ExtFilterData))]
    [Serializable]
    public class ExtFilterData
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Comparison { get; set; }
    }
}
