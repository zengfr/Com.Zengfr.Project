using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// Ä£°åÁÐ
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(CellTemplate))]
   public class CellTemplate:Template
   {
       public List<OneAction> AreaActions = new List<OneAction>();
       public CellTemplate(string name) : base(name) { }
       public CellTemplate(string name, OneAction action)
       {
           Name = name;
           AreaActions.Add(action);
       }
       public CellTemplate(string name, List<OneAction> actions)
       {
           Name = name; 
           AreaActions = actions;
       }
    }
}
