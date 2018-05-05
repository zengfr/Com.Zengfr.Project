using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;//
using ThreadWorker;
using HtmlAgilityPack;
using Lucene.Net.Store;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Cn;
using CommonPack.HtmlAgilityPack;//
namespace DataBuilder
{
   public class IndexBuilder
    {

      public static IndexWriter writer;//
        // new IndexWriter("", new StandardAnalyzer(), false);
       
       public StringBuilder Text = new StringBuilder(256);
       public string Url;
       public string Title;
       static IndexBuilder()
       {
           string path = @"E:\index\";
           FSLockConfig.LockDirectory = path;//
           if (writer == null)
               writer = new IndexWriter(path, new ChineseAnalyzer(), true);
       }
       public static void Close()
       {
           if (writer != null)
           {
               writer.Optimize();
               writer.Close();
           }
       }
       public IndexBuilder()
       {

       }
       public IndexBuilder(ThreadWork threadWork)
       {
           if (threadWork.DocWithLinks.IsTextContent)
           {
               threadWork.DocWithLinks.Doc.OptionOutputAsXml = true;//
               this.Text.Append(HtmlToSubject.Analytics(threadWork.DocWithLinks.Doc));//
               GetTitle(threadWork.DocWithLinks.Doc, out this.Title);//
           }
       }
      
        public void ThreadStart()
       {
           ThreadStart ts = new ThreadStart(this.Start);
           Thread t = new Thread(ts);
           t.Start();//
       }
       public void Start()
       {
           if (this.Text.Length < 90)
               return;//
           Document doc = new Document();
           doc.Add(new Field("url", this.Url, true, false, false));//Field.UnIndexed
           doc.Add(new Field("title", this.Title, true, false, false));//Field.UnIndexed
           doc.Add(new Field("subject", this.Text.ToString(), true, true, true));//Field.Text
           try
           {
               writer.AddDocument(doc);
           }
           catch 
           { 

           }
           finally
           {
               
           }
       }
       private static void GetTitle(HtmlDocument doc, out string title)
       {
           HtmlNode node = doc.DocumentNode.SelectSingleNode("//title");//
           if (node == null)
               node = doc.DocumentNode.SelectSingleNode("//Title");//
           if (node == null)
               title = "";//

           title = node.InnerText;//
       }
    }
}
