using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Quartz;
using Quartz.Impl;
using System.Text.RegularExpressions;
using Spring.Rest.Client;
namespace TaskApplication.pro
{
    [ActiveRecord]
    public class oooapp : ModelBase
    {
        [Property]
        public virtual int tag { get; set; }
        [Property]
        public virtual string content { get; set; }

    }
    public class oooappJob : JobBase
    {
        public override void Execute(JobExecutionContext context)
        {
            try
            {
                IList<int> tags = gettags(); tags.Remove(0); 
                int i = 0;
                foreach (int tag in tags)
                {
                    ParseList(1, 0, tag);
                    i++;
                }
            }
            catch (Exception ex) { ShowException(ex); }
        }
        IList<int> gettags()
        {
            IList<int> tags = new List<int>();
            string url, rtn;
            url = "http://www.oooapp.com/time/weiboku/";
            RestTemplate rest = new RestTemplate();
            rtn = rest.GetForObject<string>(url);
            MatchCollection mColl = Regex.Matches(rtn, "checked=\"checked\" value=\"(.*?)\"", regexOptions);

            foreach (Match m in mColl)
            {
                if (m.Success)
                {
                    tags.Add(int.Parse(m.Groups[1].Value));
                }
            }
            return tags;
        }
        void ParseList( int page,int totalPage,int tag)//
        {
            Show("ParseList->Page:{0} TotalPage:{1} Tag:{2}", page,totalPage,tag);
            bool exist = false;
            string url,  rtn;
            url = "http://www.oooapp.com/time/weiboku/?order=0&tag={0}&page={1}";
            url = string.Format(url, tag, page);
            RestTemplate rest = new RestTemplate();
            //rest.GetForObjectAsync<string>(url,r =>{ParseItems(r.Response);});
            rtn = rest.GetForObject<string>(url);
            if (totalPage == 0) 
            { 
                totalPage = ParseTotalPage(rtn);
            }
            exist = ParseItems(rtn, tag); rtn = null;
            if (!exist)
            {   if(page<totalPage)
                    ParseList(++page,totalPage, tag);
            }
        }
        int ParseTotalPage(string source)
        {
            Match m = Regex.Match(source, ">(.*?)</a>(.*?)<a href='(.*?)'>下一页</a>", regexOptions|RegexOptions.RightToLeft);
            if(m.Success)
               return int.Parse(m.Groups[1].Value);
            return 0;
        }
        bool ParseItems(string source, int tag)
        {
            MatchCollection mColl = Regex.Matches(source, "<p class=\"cont_11\">(.*?)</p>", regexOptions);
            IList<oooapp> objs = new List<oooapp>();
            foreach (Match m in mColl)
            {
                if (m.Success)
                {
                    objs.Add(ParseItem(m, tag));
                }
            }
            return SaveItems<oooapp>(objs);
        }
        oooapp ParseItem(Match m, int tag)
        {
            oooapp item = new oooapp();
            item.content = m.Groups[1].Value;
            item.tag = tag;
            item.hash = item.content.GetHashCode();
            Show("ParseItem->Tag:{0} C:{1}", item.tag, item.content);
            return item;
        }

    }
}
