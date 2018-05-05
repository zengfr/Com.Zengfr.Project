using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{
    /// <summary>
    /// Ìæ»»Àà
    /// </summary>
    [Serializable]
    public class Replace  
    {
        public string OldText { get; set; }
        public string NewText { get; set; }
        public Replace(string oldText,string newText)
        {
            OldText = oldText;
            NewText = newText;
        }
    }
}
