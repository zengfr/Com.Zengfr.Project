using System;
using System.IO;
using HtmlAgilityPack;
namespace CommonPack.HtmlAgilityPack
{
    public enum ConvertType : byte
    {
        Text = 0x00,
        Img = 0x02,
        Script = 0x04,
        Comment=0x08,

    }
	public class HtmlToText
    {
       public static void Test()
        {
            HtmlWeb hw = new HtmlWeb();
            string url = @"http://top.baidu.com";

            //str=@"http://www.qihoo.com";
            url = "http://www.baidu.com/s?wd=" + "ÃÀÅ®";
           // url = "http://zhidao.baidu.com/question/2824280.html?fr=idrm";//
           // url = "http://www.qqzhi.com/article/QQZone/html/5946.html";//
            //url = "http://www.qqzhi.com/article/teach/html/5246.html";//
           // url = "http://news.baidu.com/n?cmd=2&page=%68%74%74%70%3a%2f%2f%73%70%6f%72%74%73%2e%71%71%2e%63%6f%6d%2f%61%2f%32%30%30%37%31%32%31%36%2f%30%30%30%32%38%31%2e%68%74%6d&pn=1&cls=top";
            //url = "http://news.china.com.cn/chinanet/07news/china.cgi?docid=3594197722098196236,10467017965313168866,0&server=192.168.3.137&port=5757";//
            url = "http://zhidao.baidu.com/question/41790117.html?fr=idfn";//
            url = "http://home.donews.com/donews/article/1/121570.html";//
            url = "http://home.donews.com/donews/article/1/121569.html";//
           HtmlDocument doc = hw.Load(url);

            
            //HtmlToText htt = new HtmlToText();
           string s =HtmlToSubject.Analytics(doc);// htt.Convert(@"..\..\mshome.htm");
           //HtmlToSubject hts = new HtmlToSubject();
          // s = hts.ConvertDoc(doc).Text;//
            //s = htt.ConvertHtml(doc.Text); doc = null;//
            //while (s.IndexOf("{}") != -1) { s = s.Replace("{}", ""); }
            //StreamWriter sw = new StreamWriter("mshome.txt");
            //sw.Write(s);
            //sw.Flush();
            //sw.Close();
            Console.Write(s);//
            Console.ReadKey();//zfr
        }

        public bool ShowImg = true;//

		public HtmlToText()
		{

		}
		public string Convert(string path)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.Load(path);

			StringWriter sw = new StringWriter();
			ConvertTo(doc.DocumentNode, sw);
			sw.Flush();
			return sw.ToString();
		}
        public string ConvertHtml(string html)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);

			StringWriter sw = new StringWriter();
			ConvertTo(doc.DocumentNode, sw);
			sw.Flush();
			return sw.ToString();
		}

		private void ConvertContentTo(HtmlNode node, TextWriter outText)
		{
			foreach(HtmlNode subnode in node.ChildNodes)
			{
				ConvertTo(subnode, outText);
			}
		}

		public void ConvertTo(HtmlNode node, TextWriter outText)
		{
			string html;
			switch(node.NodeType)
			{
				case HtmlNodeType.Comment:
					// don't output comments
                   // outText.Write(node.InnerText);//zfr
					break;

				case HtmlNodeType.Document:
					ConvertContentTo(node, outText);
					break;

				case HtmlNodeType.Text:
					// script and style must not be output
					string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                    //if ((parentName == "script") || (parentName == "style") || (parentName == "a"))//zfr
						break;

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
					switch(node.Name)
					{
						case "p":
							// treat paragraphs as crlf
                            outText.Write("\r\n"); break;
                        case "br"://zfr
                            outText.Write("\r\n"); break;
                        case "img"://zfr
                            if(ShowImg)
                            outText.Write(node.OuterHtml);break;
					}

					if (node.HasChildNodes)
                    {
                        //if(node.InnerText!="")
                        //    outText.Write("{");//zfr
						ConvertContentTo(node, outText);
                        //if (node.InnerText != "")
                        //   /outText.Write("}");//zfr
					}
					break;
			}
		}
	}
}
