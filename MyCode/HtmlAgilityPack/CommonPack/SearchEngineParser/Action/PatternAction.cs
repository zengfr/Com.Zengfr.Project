using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CommonPack;
using Com.Zfrong.Xml;
namespace ParserEngine
{

    public partial class ActionInput
    {
      public static readonly string None=string.Empty;
      public static readonly ActionInput ALL = new ActionInput("(.*)",0);
      public static readonly ActionInput Tag = new ActionInput("<(.*?)>(.*?)</(.*?)>");
      public static readonly ActionInput Attr = new ActionInput("(.*?)=\"(.*?)\"");
      public static readonly ActionInput A_Href =new ActionInput( "<a(.*?)href=\"(.*?)\"(.*?)>",2);
      public static readonly ActionInput A_InnerText = new ActionInput("<a(.*?)href=\"(.*?)\"(.*?)>(.*?</a>",4);
      public static readonly ActionInput A_Title = new ActionInput("<a(.*?)title=\"(.*?)\"(.*?)>(.*?</a>", 4);
      public static readonly string Img_Src = "//img/@src";
      public static readonly string Img_Alt = "//img/@alt";
      public static readonly string Input_Value = "//input/@value";
      public static readonly string IP = "(.*)";
      public static readonly string Digital = "(.*)";
      public static readonly string Email = "(.*)";
      public static readonly string Phone = "(.*)";
      public static readonly string Tel = "(.*)";
      public static readonly string DateTime = "(.*)";
    }
    public partial class ActionInput
    {
        public string Input { get; set; }
        public int GroupIndex { get; set; }
        public ActionInput(string input)
        {
            Input = input;
            GroupIndex = 0;
        }
        public ActionInput(string input,int groupIndex)
        {
            Input=input;
            GroupIndex=groupIndex;
        }
    }
    public class  ActionBase 
    {
        public ActionInput ActionInput { get; set; }
        public List<Replace> Filters = new List<Replace>();
    }
    public class ListAction : ActionBase
    {
        public ListAction()
        { }
        public ListAction(string input, int groupIndex)
        {
            ActionInput = new ActionInput(input, groupIndex);
        }
    }
     public class OneAction :ActionBase
    {
          public int ListIndex { get; set; }
          public bool StripHTML { get; set; }
          public OneAction(string input, int groupIndex, int listIndex)
          {
              ActionInput = new ActionInput(input, groupIndex);
              ListIndex = listIndex; StripHTML = true;
          }
          public OneAction(ActionInput input, int listIndex)
        {
            ActionInput = input;
            ListIndex = listIndex; StripHTML = true;
        }
          public OneAction(ActionInput input, int listIndex, List<Replace> filters)
          {
              ActionInput = input;
              ListIndex = listIndex;
              Filters = filters; StripHTML = true;
          }
    }
    
     
}
