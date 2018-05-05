using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;//
using System.IO;
using ThreadWorker;
using CommonPack;
using CommonPack.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Threading;//
using System.Net;
namespace Builder.Data
{
    public class PageItem
    {
        public  bool IsTextContent;//
        public string Url;
        public string FilePath;
        public StringBuilder Text = new StringBuilder(512);
    }
    public class PageBuilder : Builder
    {
        PageItem Item;
        public PageBuilder() { }
        public void Init(string url, DocumentWithLinks doc, string taskName, ModeType modeType)
        {
            base.Init(taskName, modeType);
            Get(url, doc);
        }
        protected override string GetName()
        {
            return "Page";//
        }
        private void Get(string url, DocumentWithLinks doc)
        {
            if (!this.IsTrue)
                return;//
            #region
            this.Item = new PageItem();//
            this.Item.IsTextContent = doc.IsTextContent;//
            this.Item.Url = url;//
            //·��ת��
            this.ConvertToFilePath(ref this.Item.FilePath, new Uri(UrlReWritor.ReWriteUrl(this.Item.Url)));//
            #endregion
            if (this.Item.IsTextContent)
            { 
                #region URL�滻
                doc.Doc.OptionAutoCloseOnEnd = true;
                doc.Doc.OptionCheckSyntax = true;
                doc.Doc.OptionOutputAsXml = true;//
                this.Item.Text.Append(doc.Doc.Text);//
                foreach (string str in doc.AllLinks)
                {
                    this.Item.Text.Replace("\"" + str + "\"", "\"" + UrlReWritor.ReWriteUrl(str) + "\"");//urlת��
                }
                 #endregion
            }
        }
        public void ThreadStart()
        {
            if (!this.IsTrue)
                return;//
            ThreadStart ts = new ThreadStart(this.Start);
            Thread t = new Thread(ts);
            t.Start();//
        }
        private void Start()
        {
             if (this.Item.IsTextContent)
            {
                #region
                #region Text/Node�滻
                this.Replace(this.Item.Text);//�滻

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(this.Item.Text.ToString());

                this.PrependChildNode(doc);//RemoveNode
                this.AppendChildNode(doc);//RemoveNode
                this.RemoveAllChildrenNode(doc);//RemoveNode
                #endregion
                #region JS�滻
                DocumentWithScript dwj = new DocumentWithScript(doc); doc = null;//JS�滻

                this.Item.Text.Remove(0, this.Item.Text.Length);//
                this.Item.Text.Append(dwj.Doc.Text);//
                #endregion
                IOHelper.SaveTextFile(ref this.Item.FilePath, this.Item.Text, this.Config.GetParamValue_Bool("IsExistUpdate"));//����ҳ���ļ�
                #region JS/Css����
                 string path;
                Uri u = new Uri(this.Item.Url);
                foreach (KeyValuePair<int, string> kv in dwj.Script)
                {
                    path = @"/zfrong/js/" + kv.Key + ".js"; u = new Uri(u, path);//

                   this.ConvertToFilePath(ref path, u);//
                    
                    IOHelper.SaveTextFile(ref path, kv.Value, false);//����js�ļ�
                }
                foreach (KeyValuePair<int, string> kv in dwj.Style)
                {
                    path = @"/zfrong/css/" + kv.Key + ".css"; u = new Uri(u, path);//

                    this.ConvertToFilePath(ref path, u);//

                    IOHelper.SaveTextFile(ref path, kv.Value, false);//����css�ļ�
                }
                #endregion
                #endregion
            }
            else
            {
                IOHelper.SaveBinaryFile(ref this.Item.FilePath, ref this.Item.Url, false);//����
            }
            this.Item = null;//
        }
        #region ConvertToFilePath
        private void ConvertToFilePath(ref string path, Uri uri)
        {
            ConvertToFilePath(ref path, this.Config.GetParamValue_String("OutputPath"), uri);
        }
        private void ConvertToFilePath(ref string path, string baseUrl, Uri uri)
        {
            path = UrlReWritor.ConvertToFilePath(baseUrl, uri);//
            this.Replace(ref path);
            UrlReWritor.RemoveForbidCharToPath(ref path);//
        }
        #endregion
    }
}