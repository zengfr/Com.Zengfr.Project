using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine
{
    /// <summary>
    /// ���ݽ������/��
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