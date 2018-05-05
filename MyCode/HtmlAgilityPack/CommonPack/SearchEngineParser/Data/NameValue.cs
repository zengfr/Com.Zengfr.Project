using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// 数据结果基类
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(NameValue))]
    public class NameValue : XmlSerializable
    {
       public string Name="";
        public string Value="";
         public NameValue()
        {

        }
        public NameValue(string name)
        {
            this.Name = name;
        }
        public NameValue(string name,string  value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
