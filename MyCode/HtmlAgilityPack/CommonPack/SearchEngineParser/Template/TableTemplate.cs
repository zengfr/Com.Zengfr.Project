using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// ÇøÓòÄ£°å
    /// </summary>
    [Serializable]
   public class TableTemplate:Template
   {
      public List<OneAction> AreaActions = new List<OneAction>();
      public RowTemplate RowTemplate=new RowTemplate();
      public TableTemplate(string name) : base(name) { }
      public TableTemplate(RowTemplate rowTemplate)  {
          RowTemplate = rowTemplate;
      }
      public TableTemplate(string name, List<OneAction> actions)
       {
           Name = name; 
           AreaActions = actions;
       }
    }
}
