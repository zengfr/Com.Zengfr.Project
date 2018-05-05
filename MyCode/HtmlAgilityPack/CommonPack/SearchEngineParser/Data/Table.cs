using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// ����/���ݽ������
    /// </summary>
    [Serializable]
    public class Table : NameValue
    {
        public List<Row> Rows = new List<Row>();
        public Table() : base() { }
        public Table(string name) : base(name) { }
        public Table(string name, string value) : base(name, value) { }
    }
}
