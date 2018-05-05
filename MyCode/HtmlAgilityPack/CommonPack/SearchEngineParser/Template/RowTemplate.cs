using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// Ä£°åÐÐ
    /// </summary>
    [Serializable]
    public class RowTemplate : Template
    {
        public ListAction AreaAction = new ListAction();
        public List<CellTemplate> CellTemplates = new List<CellTemplate>();//
        public RowTemplate() : base() { }
        public RowTemplate(string name, ListAction action)
       {
           Name = name; 
           AreaAction = action;
       }
    }
}
