using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;//
using System.Collections.Generic;
using CommonPack;
using CommonPack.HtmlAgilityPack;
using Com.Zfrong.Xml;
using Com.Zfrong.IO;
namespace ParserEngine
{
    public interface ISearcher
    {
        string Name
        {get;set;}
         string QueryUrl
        {get;set;}
         string QueryBaseUrl
        { get;set;}
         IDictionary<string, string> QueryParams
        { get;set;}
        List<Table> Tables
        { get;set;}
        List<TableTemplate> TableTemplates
        { get;set;}
    }
    /// <summary>
    /// 按模板采集类
    /// </summary>
    public class Searcher:ISearcher
    {
        public Searcher()
        {
            Tables = new List<Table>();
            TableTemplates =new List<TableTemplate>();
            QueryParams = new Dictionary<string, string>();
        }
        public Searcher(string name)
        {
            this.Name = name;//
        }
        #region 成员
 
        #endregion
        #region 接口
        public string Name{get;set;}
        public string QueryUrl
        { get; set; }
        public string QueryBaseUrl
        { get; set; }
        public IDictionary<string, string> QueryParams
        { get; set; }
        public List<Table> Tables
        { get; set; }

        public List<TableTemplate> TableTemplates
        { get; set; }
        public int TotalCount
        { get; set; }
        #endregion
        #region 保存/加载 配置
        public void SaveConfig()
        {
            SaveConfig("Config",this.Name);//
        }
        public void SaveConfig(string dir,string fileName)
        {
            MyConfig myConfig=MyConfigHelper.Get(dir,fileName);

            myConfig.SetParamValue("QueryBaseUrl", this.QueryBaseUrl);
            myConfig.Dictionary["QueryParams"]= this.QueryParams;

            myConfig.Save();
            Serialization.SaveToFile<List<TableTemplate>>(dir+@"\"+fileName + ".T.xml", this.TableTemplates);
        }
        public void LoadConfig()
        {
            LoadConfig("Config", this.Name);//
        }
        public void LoadConfig(string dir,string fileName)
        {
            MyConfig myConfig = MyConfigHelper.Get(dir, fileName);
            this.QueryBaseUrl=myConfig.GetParamValue("QueryBaseUrl");
            this.QueryParams=myConfig.Get("QueryParams");//

            this.TableTemplates = Serialization.LoadFromFile<List<TableTemplate>>(dir + @"\" + fileName + ".T.xml");
        }
        #endregion
        #region 静态方法 编码
        private static string EncodeKey(string q)
        {
            string encSearch;
            encSearch = q.Replace(" ", "+");
            encSearch = encSearch.Replace("#", "%23");
            return encSearch;
        }
        public static string Encode(string url)
        {
            return Encode(url, Encoding.Default);//
        }
        public static string Encode(string url, string encoding)
        {
            return Encode(url, Encoding.GetEncoding(encoding));//
        }
        public static string Encode(string url, Encoding e)
        {
            return System.Web.HttpUtility.UrlEncode(url, e);//
        }
        #endregion
        #region 方法
        public void GetValues()
        {
            this.QueryUrl = "";
            foreach (KeyValuePair<string, string> kv in this.QueryParams)
            {
                this.QueryUrl+="&"+kv.Key+"="+EncodeKey(kv.Value);//
            }
            this.QueryUrl = this.QueryBaseUrl + this.QueryUrl;

            this.GetValues(this.QueryUrl);
        }
        public void GetValues(string url)
        {
            this.QueryUrl = url;//
            HtmlWeb hw = new HtmlWeb();
            Console.WriteLine("查询:" + url);//
            HtmlDocument doc = hw.Load(url); hw = null;//
            this.GetValues(doc);
        }
        public void GetValues(HtmlDocument doc)
        {
            BuildTableResults(doc.Text);//
        }
        private void BuildTableResults(string doc)
        {
            
            if (this.TotalCount == 0)
                ParseTotalCount(doc);//
            Tables = TemplateParser.BuildTables(doc, this.TableTemplates) as List<Table>;//
        }
        protected virtual string GetTotalCountRegex()
        {
            return null;//"找到相关图片[约]{0,2}([0-9,]+)张"
        }
        private static RegexOptions regOpt = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline;
        unsafe private void ParseTotalCount(string content)
        {
            if (this.TotalCount == 0&&content!=null)
            {
                string r = GetTotalCountRegex();//
                if (r != null)
                {
                    Match m = Regex.Match(content, r, regOpt);
                    if (!string.IsNullOrEmpty(m.Groups[1].Value))
                    {
                        this.TotalCount = int.Parse(m.Groups[1].Value.Replace(",", ""));
                    }
                }
                if (TotalCount == 0)
                {
                    if (content.IndexOf("计算机软件的自动请求") != -1)
                    {//计算机软件的自动请求
                        Console.WriteLine("怀疑是计算机软件的自动请求...Sleep70秒");//
                        System.Threading.Thread.Sleep(70 * 1000);//
                    }
                }
            }
        }

        #endregion
       
        public void Print()
        {
            foreach (Table box in this.Tables)
            {
                foreach (Row Row in box.Rows)
                {
                    //Console.WriteLine(Row.Value);//
                    foreach (Cell cell in Row.Cells)
                    {
                        Console.WriteLine(cell.Name + "-----");//

                        Console.WriteLine(cell.Value);//
                    }
                    Console.WriteLine("----------------------------");//
                }
            }
        }
    }
    
    public class GoogleSearcher : Searcher
    {
        public GoogleSearcher()
            : base()
        {
            this.QueryBaseUrl = @"http://www.google.cn/search?&hl=zh-CN";
            //this.KeyParamName = @"&q=";
            //this.PageParamName = @"&start=";

            //this.KeyParamValue = @"google";
            //this.PageParamValue = @"0";

            RowTemplate t = new RowTemplate();//
            t.AreaAction=new ListAction(@"<div class=g>(.*?)</td></tr></table></div>",0);

            t.CellTemplates.Add(new CellTemplate("连接", new OneAction(ActionInput.A_Href ,0)));
            t.CellTemplates.Add(new CellTemplate("连接Text", new OneAction(ActionInput.A_InnerText, 0)));

            List<Replace> list = new  List<Replace>();
            list.Add(new Replace( "Cached - Similar pages",""));
            list.Add(new Replace("- 类似网页", ""));//
            t.CellTemplates.Add(new CellTemplate("描述", new OneAction(ActionInput.ALL, 0 ,list)));
            
            this.TableTemplates.Add(new TableTemplate(t));//
        }
    }
    public class BaiduSearcher : Searcher
    {
        public BaiduSearcher()
            : base()
        {
            this.QueryBaseUrl = @"http://www.baidu.com/s?";
            //this.KeyParamName = @"&wd=";
            //this.PageParamName = @"&pn=";

            //this.KeyParamValue = @"baidu";
            //this.PageParamValue = @"0";
            
            RowTemplate t = new RowTemplate();//
            t.AreaAction=new ListAction(@"<tr><td class=f>(.*?)<br><font color=#008000>",0);
            t.CellTemplates.Add(new CellTemplate("连接", new OneAction(ActionInput.A_Href, 0)));
            t.CellTemplates.Add(new CellTemplate("连接Text", new OneAction(ActionInput.A_InnerText, 0)));

            XmlSerializableList<Replace> list = new XmlSerializableList<Replace>();
            list.Add(new Replace("上的更多结果",""));
            list.Add(new Replace("- 百度快照",""));//

            t.CellTemplates.Add(new CellTemplate("描述", new OneAction(ActionInput.ALL, 0, list)));

            this.TableTemplates.Add(new TableTemplate(t));//
        }

    }

}