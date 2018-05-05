using System;
using System.Collections.Generic;
using System.Text;
using Com.Zfrong.Algorithms.LCS;
using HtmlAgilityPack;
using CommonPack.HtmlAgilityPack;
using System.Collections;
using Com.Zfrong.Algorithms.TFIDF;
namespace CommonPack.LCS
{
    /// <summary>
    /// 自动模板生成类
    /// </summary>
    public class TemplateBuilder
    {
        public enum BuilderType : byte
        {
            Html, Text
        }
        #region 构造方法
        public TemplateBuilder()
        {
            Urls = new List<string>();//
            Type = BuilderType.Html;//
        }
        #endregion
        #region 字段
        /// <summary>
        /// 采样url
        /// </summary>
        public IList<string> Urls;
        /// <summary>
        /// 模板保存路径
        /// </summary>
        public string TemplateFile;
        /// <summary>
        /// 默认Html
        /// </summary>
        public BuilderType Type;
        #endregion
        #region 接口
        public void AddUrl(string url)
        {
            this.Urls.Add(url);//
        }
        public void Start()
        {
            switch (Type)
            {
                case BuilderType.Html:
                    HtmlBuilder(Urls, TemplateFile); break;
                case BuilderType.Text:
                    TextBuilder(Urls, TemplateFile); break;
            }
        }
        #endregion
        #region 静态方法
        public static void HtmlBuilder(IList<string> urls, string file)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument lcs = hw.Load(urls[0]);
            HtmlDocument doc;
            for (int i = 1; i < urls.Count; i++)
            {
                doc = hw.Load(urls[i]);
                lcs.LoadHtml(LCSFinder.GetLCS(lcs.Text, doc.Text));//
            } GC.Collect();
            lcs.Save(file);//
        }
        public static void TextBuilder(IList<string> urls, string file)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlToText htt = new HtmlToText();
            HtmlDocument lcs = hw.Load(urls[0]);
            lcs.LoadHtml(htt.ConvertHtml(lcs.Text));//

            HtmlDocument doc;
            for (int i = 1; i < urls.Count; i++)
            {
                doc = hw.Load(urls[i]);
                doc.LoadHtml(htt.ConvertHtml(doc.Text));//

                lcs.LoadHtml(LCSFinder.GetLCS(lcs.Text, doc.Text));//
            } GC.Collect();
            lcs.Save(file);//
        }
        #endregion
    }

    /// <summary>
    /// 采样
    /// </summary>
    public class SampleBuilder
    {
        public static IList<IList<string>> GetSampleUrl(string url)
        {
           return GetSampleUrl(new Uri(url));
        }
        public static IList<IList<string>> GetSampleUrl(Uri uri)
        {
            DocumentWithLinks obj = new DocumentWithLinks(uri);
            obj.GetAllRefs();//
            List<string> list = new List<string>(); 
            list.Add(uri.ToString());//
            foreach (KeyValuePair<string, string> kv in obj.RefsList)
                list.Add(kv.Key);
            URLTFIDFMeasure TFIDF = new URLTFIDFMeasure(list.ToArray());
            TFIDF.Init();//
            TFIDF.GetMaxSimilarityDoc();//
            IList<IList<string>> objList = new List<IList<string>>();
            foreach (KeyValuePair<float, int> kv in TFIDF.VList)
            {
                if (kv.Value > 1)
                    objList.Add(TFIDF.GetSimilarityDoc(kv.Key));
            }
            return objList;//
        }
        public static void Test()
        {
            IList<IList<string>> obj = GetSampleUrl("http://news.baidu.com/");//
            for (int i = 0; i < obj.Count; i++)
            {
                Console.WriteLine("-".PadLeft(40,'-'));
                for (int j= 0; j < obj[i].Count; j++)
                {
                    Console.WriteLine(obj[i][j]);
                }
            }
        }

    }
}