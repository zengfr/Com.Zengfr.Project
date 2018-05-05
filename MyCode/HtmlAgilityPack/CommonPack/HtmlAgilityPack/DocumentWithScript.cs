using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HtmlAgilityPack;
namespace CommonPack.HtmlAgilityPack
{
    public class DocumentWithScript
    {
        public static void Test()
        {
            HtmlWeb hw = new HtmlWeb();
            DocumentWithScript js = new DocumentWithScript(hw.Load("http://zhidao.baidu.com/?t=50"));

            Console.Write(js.Doc.Text);//
            Console.ReadKey();//zfr
        }
        public HtmlDocument Doc = new HtmlDocument();
        public IDictionary<int, string> Script = new Dictionary<int, string>();
        public IDictionary<int, string> Style = new Dictionary<int, string>();

        public DocumentWithScript(HtmlDocument doc)
        {
            Init(doc);
        }
        private void Init(HtmlDocument doc)
        {
            StringWriter sw = new StringWriter();
            try
            {
                ConvertTo(doc.DocumentNode, sw);
            }
            catch { }
            sw.Flush();
            this.Doc.LoadHtml(sw.ToString());
        }
        private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
        public void ConvertTo(HtmlNode node, TextWriter outText)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    outText.Write(node.OuterHtml);
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                   outText.Write(node.OuterHtml);
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name.ToLower())
                    {
                        case "script":
                            if(node.Attributes["src"]==null)
                                DoReplace(node, outText, ref  ScriptFormat, this.Script);
                            else
                                outText.Write(node.OuterHtml);
                            break;
                        case "style":
                            DoReplace(node, outText, ref StyleFormat, this.Style);
                            break;
                        default:
                            if (node.HasChildNodes)
                            {
                                outText.Write("<" + node.Name);
                                WriteAttributes(node,outText,false);
                                outText.Write(">");
                                if (node.Name == "body")
                                {  outText.Write("<!--#include file=\"/zfrong/top.shtml\" -->");
                                    outText.Write(ScriptFormat, "top-qq-bt-sex-girl-mp3");
                                }

                                ConvertContentTo(node, outText);

                                if (node.Name == "body")
                                    outText.Write(ScriptFormat, "foot-qq-bt-sex-girl-mp3");
                                outText.Write("</" + node.Name);
                                WriteAttributes(node, outText, true); 
                                outText.Write(">");
                            }
                            else
                            {
                                outText.Write(node.OuterHtml);
                            }
                            break;
                    }
                    break;
            }

        }
        internal void WriteAttributes(HtmlNode node,TextWriter outText,bool isClosing)
        {
            if (!isClosing)
            {
                if (node.Attributes != null)
                {
                    foreach (HtmlAttribute att in node.Attributes)
                    {
                        WriteAttribute(outText,node, att);
                    }
                }
            }
            else {
                if (node.ClosingAttributes != null)
                {
                    foreach (HtmlAttribute att in node.ClosingAttributes)
                    {
                        WriteAttribute(outText,node, att);
                    }
                }
            }
        }
        private void WriteAttribute(TextWriter outText, HtmlNode node, HtmlAttribute att)
        {
            string name;
            if (node.OwnerDocument.OptionOutputUpperCase)
				{
					name = att.Name.ToUpper();
				}
				else
				{
					name = att.Name;
				}

				if (att.Name.Length >= 4)
				{
					if ((att.Name[0] == '<') && (att.Name[1] == '%') &&
						(att.Name[att.Name.Length-1] == '>') && (att.Name[att.Name.Length-2] == '%'))
					{
						outText.Write(" " + name);
						return;
					}
				}
                if (node.OwnerDocument.OptionOutputOptimizeAttributeValues)
				{
					if (att.Value.IndexOfAny(new Char[]{(char)10, (char)13, (char)9, ' '}) < 0)
					{
						outText.Write(" " + name + "=" + att.Value);
					}
					else
					{
						outText.Write(" " + name + "=\"" + att.Value + "\"");
					}
				}
				else
				{
					outText.Write(" " + name + "=\"" + att.Value + "\"");
				}
        }
        private static void DoReplace(HtmlNode node, TextWriter outText,ref string format,IDictionary<int, string> outList)
        {
            int code = node.InnerHtml.GetHashCode();
            if (!outList.ContainsKey(code))
            {
                outList.Add(code, node.InnerHtml);
            }
            outText.Write(format, code);
        }
        private static string ScriptFormat = "<script language=\"javascript\" type=\"text/javascript\" src=\"/zfrong/js/{0}.js\"></script>";
        private static string StyleFormat = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/zfrong/css/{0}.js\" />";
    }
}
