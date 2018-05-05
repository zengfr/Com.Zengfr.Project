using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine
{
    /// <summary>
    /// 数据结果集合/列
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(Cell))]
    public class Cell:NameValue
    {
       public  Cell() : base(){}
       public  Cell(string name) : base(name) { }
        public Cell(string name,string value) : base(name,value) { }
    }
}