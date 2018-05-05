using System;
using System.Collections.Generic;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{

	/// <summary>
    /// 数据结果集合/行　Each Row in the result list coming from the Search
	/// </summary>
    [Serializable]
	public class Row:NameValue
    {
        public List<Cell> Cells = new List<Cell>();			// Some complementory text coming with the result
       public  Row() : base(){}
        public Row(string name) : base(name) { }
        public Row(string name,string value) : base(name,value) { }
        public Row(List<Cell> cells)
        {
            this.Cells = cells;
        }
	}
}
