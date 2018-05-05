using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
using Com.Zfrong.Xml;
namespace ParserEngine
{
    public class BaiduSearcher_WB : Searcher
    {
        public BaiduSearcher_WB()
               : base()
           {
               this.QueryBaseUrl = @"http://www.baidu.com/s?rtt=2&tn=baiduwb&cl=2";
               //this.KeyParamName = @"&word=";
               //this.PageParamName = @"&pn=";

               //this.KeyParamValue = @"baidu";
               //this.PageParamValue = @"0";
               List<Replace> list = new List<Replace>();
               list.Add(new Replace("- 新浪微博 - 评论", ""));//
               list.Add(new Replace("\\[查看图片\\]", ""));//
               //list.Add(new Replace("(\\d+?)小时前", "()"));//
               list.Add(new Replace("=baidu_s_profile", ""));//

               RowTemplate t = new RowTemplate();//
               t.AreaAction=new ListAction(@"<li id=(.*?)- 新浪微博 -(.*?)</p></li>",0);
               t.CellTemplates.Add(new CellTemplate("连接", new OneAction(ActionInput.A_Href, -1,list)));



               t.CellTemplates.Add(new CellTemplate("Text", new OneAction(ActionInput.ALL,0, list)));

               

               this.TableTemplates.Add(new TableTemplate(t));//
           }
        protected override string GetTotalCountRegex()
        {
            return "找到相关新闻[约]{0,2}([0-9,]+)篇";
        }
        public static List<Table> Search(string key, int pn, ref int totalCount)
        {
            Searcher s = new BaiduSearcher_WB();//
            //s.QueryParams["zfrong"] = Guid.NewGuid().ToString();//
            s.QueryParams["word"] = Searcher.Encode(key, "gb2312");//
            s.QueryParams["pn"] = pn.ToString();//

            s.QueryParams["bs"]=s.QueryParams["word"];
            s.QueryParams["sr"]="0";
            s.QueryParams["cl"]="2";
            s.QueryParams["rn"]="20";
            s.QueryParams["ct"] ="0";
            s.QueryParams["from"] = "news";//
            s.TotalCount = totalCount;//
            s.GetValues();//
            totalCount = s.TotalCount;//

            return s.Tables;//
        }
    }
}
