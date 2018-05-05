using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DB;
using ThreadWorker;
using HtmlAgilityPack;
using HLSSplit;
using System.Data;
using System.Text.RegularExpressions;
using CommonPack.HtmlAgilityPack;//
using BlackHen.Threading;//
namespace WordBuilder
{
    public class EnHotWorkItem : WorkItem
    {
        string Url;//
        public EnHotWorkItem(string url)
        {
            this.Url = url;//
        }
        public override void Perform()
        {
            worker_OnProcess(new Uri(Url));//
        }
        protected void worker_OnProcess(Uri url)
        {
            // Monitor.Enter(this);
            EnHotManager.ShowMessge("分析|" + url.ToString());//
            HtmlWeb hw = new HtmlWeb(); hw.UseCookies = true;//
            HtmlDocument doc = hw.Load(url.ToString());

            DocumentWithLinks nwl = new DocumentWithLinks(doc); nwl.IsTextContent = hw.IsTextContent;//
            EnHot obj = null; nwl.GetAllRefs();//
            foreach (KeyValuePair<string, string> e in nwl.RefsList)
            {
                bool b = e.Value.Length <= 10; b = b && e.Value.Length != 0;//
                if (b)
                {
                    if (!EnHot.Exists("NameHash=" + e.Value.GetHashCode()))
                    {
                        obj = new EnHot();//
                        obj.Name = e.Value;//
                        obj.NameHash = obj.Name.GetHashCode();//
                        obj.Save();//
                        EnHotManager.ShowMessge("短语|" + e.Value);//打印
                    }
                    else
                    {
                        DB.SqlHelper.ExecuteNonQuery("update [EnHot] set [count]=[count]+1 where NameHash=" + e.Value.GetHashCode());//
                    }

                }

                Uri u = new Uri(url, e.Key);
                if (regex.IsMatch(u.ToString()))
                {
                    if (!EnHotManager.BloomFilter.IsRepeat(u.ToString()))
                    {
                         EnHotManager.WorkQueue.Add(new EnHotWorkItem(u.ToString()));//
                    }
                }
            }
            #region
            //HtmlToText htt = new HtmlToText();
            //string s = htt.ConvertHtml(doc.Text);

            //byte iExtraCalcFlag = 0; //附加计算标志，不进行附加计算
            ////获得附加计算标识
            //iExtraCalcFlag |= (byte)SegOption.POS;//
            ////iExtraCalcFlag |= (byte)SegOption.KEYWORD;
            //iExtraCalcFlag |= (byte)SegOption.SEARCH;
            ////iExtraCalcFlag |= (byte)SegOption.FINGER;

            //HLParse p = new HLParse();
            //p.ExtraCalcFlag = iExtraCalcFlag;//
            //p.Parse(s);

            //foreach (KeyValuePair<string, POS> e in p.Words)
            //{
            //    bool b = e.Key.Length <= CnDict.Schema.Columns.GetColumn(CnDict.Columns.Word).MaxLength;
            //    if (b)
            //    {
            //        CnDict obj = new CnDict();
            //        obj.LoadByParam(CnDict.Columns.Word, e.Key);
            //        if (obj.Id > 0)
            //        {
            //            obj.Num += 1;
            //        }
            //        else
            //        {
            //            obj.Word = e.Key;//
            //            obj.Post = (int)e.Value;//

            //            Console.WriteLine("分词|" + e.Key);//打印
            //        }
            //        obj.Save();//
            //    }

            //}

            //}

            //p = null;//
            //htt = null;//
            doc = null;//
            //s = null;//

            //Monitor.Pulse(this);
            // Monitor.Exit(this);
            #endregion
        }
        private static Regex regex = new Regex("www.google.com/trends", regOpt);//
        private static RegexOptions regOpt = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline;

    }
}
