using System;
using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
namespace CommonPack.HtmlAgilityPack
{
	class GetDocLinks
	{
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    HtmlWeb hw = new HtmlWeb();
        //    string url = @"http://www.baidu.com";
        //    HtmlDocument doc = hw.Load(url);
        //    doc.Save("mshome.htm");

        //    DocumentWithLinks nwl = new DocumentWithLinks(doc);
        //    Console.WriteLine("Linked urls:");
        //    for(int i=0;i<nwl.Links.Count;i++)
        //    {
        //        Console.WriteLine(nwl.Links[i]);
        //    }

        //    Console.WriteLine("Referenced urls:");
        //    foreach(KeyValuePair<string,string> v in nwl.Refs)
        //    {
        //        Console.WriteLine(v.Key);
        //    }
        //    Console.ReadKey();//zfr
        //}
	}

	/// <summary>
	/// 获取doc的urs集合
	/// </summary>
	public class DocumentWithLinks
	{
        /// <summary>
        /// "//*[@background or @lowsrc or @src or @href]"
        /// </summary>
        public List<string> AllLinks= new List<string>();
        /// <summary>
        /// [A连接，连接InnerText] "//a[@href]" "//iframe[@src]"
        /// </summary>
        public Dictionary<string, string> RefsList= new Dictionary<string, string>();
        public Dictionary<string, string> ImgsList = new Dictionary<string, string>();
		public HtmlDocument Doc;
        public bool IsTextContent;//zfr
		/// <summary>
		/// Creates an instance of a DocumentWithLinkedFiles.
		/// </summary>
		/// <param name="doc">The input HTML document. May not be null.</param>
		public DocumentWithLinks(HtmlDocument doc)
		{
			if (doc == null)
			{
				throw new ArgumentNullException("doc");
			}
			this.Doc = doc;
		}
        public DocumentWithLinks(Uri uri)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            this.Doc=htmlWeb.Load(uri.ToString());
            this.IsTextContent = htmlWeb.IsTextContent;//
        }
        public DocumentWithLinks() { }
        public string GetText()
        {
            HtmlToText ht = new HtmlToText();
            ht.ShowImg = false;//
            return ht.ConvertHtml(this.Doc.Text);//
        }
        /// <summary>
        /// "background" "href" "src" "lowsrc"
        /// </summary>
		public void GetAllLinks()
        {
            if (!this.IsTextContent) return;//

			HtmlNodeCollection atts = Doc.DocumentNode.SelectNodes("//*[@background or @lowsrc or @src or @href]");
			if (atts == null)
				return;

			foreach(HtmlNode n in atts)
			{
				ParseLink(n, "background");
				ParseLink(n, "href");
				ParseLink(n, "src");
				ParseLink(n, "lowsrc");
			}
		}
        /// <summary>
        /// "//a[@href]" "//iframe[@src]"
        /// </summary>
		public void GetAllRefs()
		{
            if (!this.IsTextContent) return;//

            HtmlNodeCollection coll = Doc.DocumentNode.SelectNodes("//a[@href]");
			if (coll == null)
				return;

			foreach(HtmlNode n in coll)
			{
                ParseLink(n, "href");
			}

            coll = Doc.DocumentNode.SelectNodes("//iframe[@src]");
            if (coll == null)
                return;

            foreach (HtmlNode n in coll)
            {
                ParseLink(n, "src");
            }
		}
        public void GetAllImgs()
        {
            if (!this.IsTextContent) return;//

            HtmlNodeCollection coll = Doc.DocumentNode.SelectNodes("//img[@src]");
            if (coll == null)
                return;

            foreach (HtmlNode n in coll)
            {
                ParseLink(n, "src");
            }
        }

		private void ParseLink(HtmlNode node, string name)
		{
			HtmlAttribute att = node.Attributes[name];
			if (att == null)
				return;
            AllLinks.Add(att.Value);
            if ("src,href".IndexOf(att.Name) == -1)
                return;//
            if ("a;iframe".IndexOf(node.Name.ToLower()) != -1)
            {
                if (!RefsList.ContainsKey(att.Value))
                    RefsList.Add(att.Value, node.InnerText.Trim());
            }
            if (node.Name.ToLower()=="img")
            {
                if (!ImgsList.ContainsKey(att.Value))
                {
                    string alt="";
                    if (node.Attributes["alt"] != null)
                        alt = node.Attributes["alt"].Value;//
                    ImgsList.Add(att.Value, alt);
                }
            }
			// if name = href, we are only interested by <link> tags
			//if ((name == "href")) //&& (node.Name != "link"))//zfr
				//return;
			
		}
	}
}
