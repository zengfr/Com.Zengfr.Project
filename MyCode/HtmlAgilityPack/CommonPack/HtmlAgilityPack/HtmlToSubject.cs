using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
namespace CommonPack.HtmlAgilityPack
{
    /// <summary>
    /// 分析出html文本主题
    /// </summary>
   public class HtmlToSubject
    {
#region
       public static string Analytics2(StreamReader sr)
       {
          string source = sr.ReadToEnd();
          return Analytics2(source);//
       }
       public static string Analytics2(string html)
       {
           DateTime startTime = DateTime.Now;
            return Analytics2(html, startTime);
       }
       public static string Analytics2(string html, DateTime startTime)
       {
           HtmlDocument doc = new HtmlDocument();
           doc.OptionOutputAsXml = true;
           doc.LoadHtml(html);//
           return Analytics2(doc, startTime);//
       }
       public static string Analytics2(HtmlDocument doc)
       {
           DateTime startTime = DateTime.Now;
         return  Analytics2(doc, startTime);
       }
       public static string Analytics2(HtmlDocument doc, DateTime startTime)
       {

           doc.OptionOutputAsXml = true;
           StringWriter sw = new StringWriter();
           ConvertTo(doc.DocumentNode, sw, startTime);
           sw.Flush();
           return DeleteReplace(sw.ToString());
       }

       private static void ConvertContentTo(HtmlNode node, TextWriter outText, DateTime startTime)
       {
           foreach (HtmlNode subnode in node.ChildNodes)
           {
               ConvertTo(subnode, outText, startTime);
           }
       }

       private static void ConvertTo(HtmlNode node, TextWriter outText, DateTime startTime)
       {
           string html;
           TimeSpan timeSpan = (TimeSpan)(DateTime.Now - startTime);
           if (timeSpan.Minutes > 3)
               return;//无限循环处理
           #region ######
           switch (node.NodeType)
           {
               case HtmlNodeType.Comment:
                    break;

               case HtmlNodeType.Document:
                   ConvertContentTo(node, outText, startTime);
                   break;

               case HtmlNodeType.Text:
                   // script and style must not be output
                   string parentName = node.ParentNode.Name;
                   if ((parentName == "script") || (parentName == "style"))
                       //if ((parentName == "script") || (parentName == "style") || (parentName == "a"))//zfr
                       break;
                   if (node.ParentNode.ParentNode != null && IsNotMainNode(node.ParentNode.ParentNode,startTime))
                       break;//
                   // get text
                   html = ((HtmlTextNode)node).Text;

                   // is it in fact a special closing node output as text?
                   if (HtmlNode.IsOverlappedClosingElement(html))
                       break;

                   // check the text is meaningful and not a bunch of whitespaces
                   if (html.Trim().Length > 0)
                   {
                       outText.Write(HtmlEntity.DeEntitize(html));
                   }
                   break;

               case HtmlNodeType.Element:
                   switch (node.Name)
                   {
                       case "p":
                       //    // treat paragraphs as crlf
                           outText.Write("\r\n"); 
                           break;
                       case "br"://zfr
                           outText.Write("\r\n"); 
                           break;
                       case "img"://zfr
                           if (node.ParentNode!= null && IsNotMainNode(node.ParentNode,startTime))
                               break;//
                           outText.Write(node.OuterHtml); 
                           break;
                   }

                   if (node.HasChildNodes)
                   {
                       ConvertContentTo(node, outText, startTime);
                   }
                   break;
           }
           #endregion
       }

       private static bool IsNotMainNode(HtmlNode node)
       {
           DateTime startTime = DateTime.Now;
           return IsNotMainNode(node, startTime);
       }
       private static bool IsNotMainNode(HtmlNode node, DateTime startTime)
       {
           int k;
           
           k=0;//
           GetNodeChildCount_A(node, 4, ref k);//
           if (k >=5)
               return true;

           k = 0;//
           GetNodeChildCount_Input(node,ref k);//
           if (k >= 2)
               return true;

           k = Analytics2(node.InnerText,startTime).Length;
           if (k < 200)
               return true;
           return false;//
       }
        private static void GetNodeChildCount_A(HtmlNode node,ref int count)
       { 
            GetNodeChildCount(node,"a",5,ref count);//
       }
       private static void GetNodeChildCount_A(HtmlNode node,  int minTextLen, ref int count)
       {
           GetNodeChildCount(node, "a", minTextLen, 5, ref count);//
       }
       private static void GetNodeChildCount_Input(HtmlNode node, ref int count)
       {
           GetNodeChildCount(node, "input",2, ref count);//
       }
       private static void GetNodeChildCount(HtmlNode node,string name,int maxbreak,ref int count)
       {
           GetNodeChildCount(node, name, 0, maxbreak, ref count);//
       }
       private static void GetNodeChildCount(HtmlNode node,string name, int minTextLen,int maxbreak,ref int count)
       {
           foreach (HtmlNode n in node.ChildNodes)
           {
               if (n.Name.ToLower() == name)
               {
                        count++;//
                        if (minTextLen != 0 && n.InnerText.Length < minTextLen)
                            count--;//当..时忽略
               }
               if (count <= maxbreak)
                   GetNodeChildCount(n, name, minTextLen, maxbreak, ref count);//
               else
                   break;//
           }
       }
       /// <summary>
       /// 删除不可见字符
       /// </summary>
       /// <param name="sourceString"></param>
       /// <returns></returns>
       public static string DeleteUnVisibleChar(string sourceString)
       {
           StringBuilder sBuilder =new StringBuilder(131);
           for (int i = 0; i < sourceString.Length; i++)
           {
               int Unicode = sourceString[i];
               if (Unicode >= 16)
               {
                   sBuilder.Append(sourceString[i].ToString());
               }
           }
           return sBuilder.ToString();
       }
       public static string DeleteReplace(string str)
       {
           int len1, len2 = 0;//防止无限循环
           len1 = str.Length;
           while (str.IndexOf("&nbsp;&nbsp;") != -1 && len2 != len1)
           {
               len1 = str.Length;
               str = str.Replace("&nbsp;&nbsp;", "&nbsp;");
               len2 = str.Length;
           }
           len2 = 0;
           while (str.IndexOf("  ") != -1 && len2 != len1)
           {
               len1 = str.Length;
               str = str.Replace("  ", " ");
               len2 = str.Length;
           }
           len2 = 0;
           while (str.IndexOf("\t\t") != -1 && len2 != len1)
           {
               len1 = str.Length;
               str = str.Replace("\t\t", "\t");
               len2 = str.Length;
           }
           len2 = 0;
           while (str.IndexOf("\r\r") != -1 && len2 != len1)
           {
               len1 = str.Length;
               str = str.Replace("\r\r", "\r"); 
               len2 = str.Length;
           }
           len2 = 0;
           while (str.IndexOf("\n\n") != -1 && len2 != len1)
           {
               len1 = str.Length;
               str = str.Replace("\n\n", "\n");
               len2 = str.Length;
           } 
           len2 = 0;
           while (str.IndexOf("\r\n\r\n") != -1 && len2 != len1)
           {
               len1 = str.Length;
               str = str.Replace("\r\n\r\n", "\r\n"); 
               len2 = str.Length;
           }
           return str;//
       }
       /// <summary>
       /// 分析出html文本主题 按文本密度
       /// </summary>
       /// <param name="doc"></param>
       /// <returns></returns>
       public static string Analytics(HtmlDocument doc)
       {
           List<HtmlNode> nodes = new List<HtmlNode>();
           foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//*"))
           {
               if(node.NodeType!=HtmlNodeType.Comment)
                    nodes.Add(node);//
           }
           nodes.Sort(new StructComparer());//

           nodes = GetMaxScore(nodes);//
           StringBuilder  sb =new StringBuilder(255);//
           foreach (HtmlNode node in nodes)
           {
              sb.Append(node.InnerText);//
           }
           
           string str =DeleteReplace(sb.ToString()); sb.Remove(0, sb.Length);//
           
           return str;//
       }
       /// <summary>
       /// 过滤 style/script/head/html/body
       /// </summary>
       /// <param name="nodes"></param>
       /// <returns></returns>
       public static List<HtmlNode> GetMaxScore(List<HtmlNode> nodes)
       {
           List<HtmlNode> list = new List<HtmlNode>();
           string name = "style/script/head/html/body";//
           for (int i = nodes.Count-1; i >= 0; i--)
           {
               if (name.IndexOf(nodes[i].Name.ToLower())!=-1)
                   continue;
               if (list.Count > 0 && list[0].Depth != nodes[i].Depth && list[0].Score != nodes[i].Score)
                   break;//
                 list.Add(nodes[i]);//
           }
           return list;//
       }
       class StructComparer : IComparer,IComparer<HtmlNode>
       {
           #region IComparer 成员

           public int Compare(object x, object y)
           {
               return Compare(x as HtmlNode, y as HtmlNode);
           }
           public int Compare(HtmlNode a, HtmlNode b)
           {
               return a.Score.CompareTo(b.Score);//
           }
           #endregion
       }
#endregion
    
    }
}
