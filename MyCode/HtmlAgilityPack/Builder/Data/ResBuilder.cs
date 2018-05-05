using System;
using System.Collections.Generic;
using System.Text;
using ThreadWorker;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;//
using CommonPack.HtmlAgilityPack;
using NHibernate.Expression;
using Castle.ActiveRecord;
namespace Builder.Data
{
    public class ResBuilder : Builder
    {
        static ResBuilder()
        {
            Save_TheadStart();//
        }
        private string Text = "";//
        public ResBuilder() { }
        public void Init(string url, DocumentWithLinks doc, string taskName, ModeType modeType)
        {
            base.Init(taskName, modeType);
            Get(url, doc.Doc);
        }

        protected override string GetName()
        {
            return "Res";//
        }
        private void Get(string url, HtmlDocument doc)
        {
            if (doc.Text == null)
                return;//
            CommonPack.HtmlAgilityPack.HtmlToText ht = new CommonPack.HtmlAgilityPack.HtmlToText();
            this.Text = ht.ConvertHtml(doc.Text);//   
        }
        public void ThreadStart()
        {
            ThreadStart ts = new ThreadStart(this.Start);
            Thread t = new Thread(ts);
            t.Start();//

        }
        private void Start()
        {
            ResBuilder.GetEmail(ref this.Text, EmailList);//
            ResBuilder.GetMobile(ref this.Text, MobileList);//
        }
        private static void Save_TheadStart()
        {
            ThreadStart ts = new ThreadStart(Save);
            Thread t = new Thread(ts);//
            t.IsBackground = true;//
            t.Start();//
        }
        private static void Save()
        {
            while (!IsQuit)
            {
                Thread.Sleep(15 * 60 * 1000);//

                if (EmailList.Count > 100)
                    EmailToDataBase();//
                if (MobileList.Count > 100)
                    MobileToDataBase();//
            }

        }
        public static void ListToSave()
        {
            EmailToDataBase();//
            MobileToDataBase();//
        }
        private static void EmailToDataBase()
        {
            lock (EmailList)
            {
                ShowMesage("EmailToDataBase Start...Count:" + EmailList.Count);//
                foreach (string v in EmailList)
                {
                    DB.Email obj = ActiveRecordBase<DB.Email>.FindFirst(Expression.Eq("Content", v));
                      if (obj!=null&&obj.ID > 0)
                    {
                        obj.Num += 1; obj.Update();
                    }
                    else
                    {
						obj=new DB.Email();
                        obj.Content = v; obj.Create();
                        ShowMesage("E:" + v);//
                    }
                    
                }
                EmailList.Clear();//
                Monitor.PulseAll(EmailList);//
                ShowMesage("EmailToDataBase End...");
            }
        }
        private static void MobileToDataBase()
        {
            lock (MobileList)
            {
                ShowMesage("MobileToDataBase Start...Count:" + MobileList.Count);//
                foreach (string v in MobileList)
                {
                    DB.Mobile obj = ActiveRecordBase<DB.Mobile>.FindFirst(Expression.Eq("Content", v));
                     if (obj!=null&&obj.ID > 0)
                    {
                        obj.Num += 1; obj.Update();
                    }
                    else
                    {obj=new DB.Mobile();
                    obj.Content = v; obj.Create();
                        ShowMesage("M:" + v);//
                    }
                }
                MobileList.Clear();//
                Monitor.PulseAll(MobileList);//
                ShowMesage("MobileToDataBase End...");
            }
        }
        #region static

        public static void GetEmail(ref string text, IList<string> list)
        {
            lock (list)
            {
                MatchCollection Matchers = getInfo(ref text, 1);//
                foreach (Match m in Matchers)
                {
                    if (!list.Contains(m.Value))
                        list.Add(m.Value);//
                }
                Monitor.PulseAll(list);//
            }
        }
        public static void GetMobile(ref string text, IList<string> list)
        {
            lock (list)
            {
                MatchCollection Matchers = getInfo(ref text, 2);//
                foreach (Match m in Matchers)
                {
                    if (!list.Contains(m.Value))
                        list.Add(m.Value);//
                } Monitor.PulseAll(list);//
            }
        }

        private static MatchCollection getInfo(ref string data, Regex regex)
        {
            MatchCollection Matchers = regex.Matches(data);
            return Matchers;
        }
        private static Regex getRegex(string regexStr)
        {
            Regex regex = new Regex(regexStr, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return regex;
        }
        private static MatchCollection getInfo(ref string data, int type)
        {
            Regex regex = null;
            switch (type)
            {
                case 1://email
                    //Matchers=getInfo(data, "\\w+([-+.]\\w+)*[@#^]\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");//
                    //Matchers=getInfo(data,@"^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|"((?=[\x01-\x7f])[^"\\]|\\[\x01-\x7f])*"\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|"((?=[\x01-\x7f])[^"\\]|\\[\x01-\x7f])*")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$");//
                    //差Matchers=getInfo(data,@"([a-z0-9_]|\\-|\\.)+@(([a-z0-9_]|\\-)+\\.)+[a-z]{2,4}");//
                    //Matchers = getInfo(data, @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");//
                    //Matchers=getInfo(data,@"([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})");//
                    if (EmailRegex == null)
                        EmailRegex = getRegex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");//
                    regex = EmailRegex; break;//
                case 2:// Mobile
                    if (MobileRegex == null)
                        MobileRegex = getRegex("(13|15)\\d{9}");
                    regex = MobileRegex; break;//
                case 3://Phone
                    //写了一个，可验证如下27种格式：
                    ////110
                    //8888888
                    //88888888
                    //8888888-123
                    //88888888-23435
                    //0871-8888888-123
                    //023-88888888-23435
                    //86-0871-8888888-123
                    //8888888_123
                    //88888888_23435
                    //0871_8888888_123
                    //023_88888888_23435
                    //86_0871_8888888_123
                    //8888888－123
                    //88888888－23435
                    //0871－8888888－123
                    //023－88888888－23435
                    //86－0871－8888888－123
                    //8888888—123
                    //88888888—23435
                    //0871—8888888—123
                    //023—88888888—23435
                    //86—0871—8888888—123
                    //13588888888
                    //15988888888
                    //013588888888
                    //015988888888
                    //    分格时，用户可以输入中英文的-_－—
                    //正则表达式如下：
                    //(^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)
                    //"(\\d+-)?(\\d{4}-?\\d{7}|\\d{3}-?\\d{8}|^\\d{7,8})(-\\d+)?" 
                    //((\(\d{3}\)|\d{3}-)?\d{8}[-]\d+)|((\(\d{3}\)|\d{3}-)?\d{8})|((\(\d{4}\)|\d{4}-)?\d{7}[-]\d+)|((\(\d{4}\)
                    //(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)
                    //^(\d{3}-|\d{4}-)?(\d{8}|\d{7})?$       //国内电话
                    if (PhoneRegex == null)
                        PhoneRegex = getRegex("(13|15)\\d{9}");
                    regex = PhoneRegex; break;//
            }
            if (regex == null)
                return null;//
            MatchCollection Matchers = getInfo(ref data, regex);
            return Matchers;
        }
        private static Regex EmailRegex;
        private static Regex MobileRegex;
        private static Regex PhoneRegex;

        public static IList<string> EmailList = new List<string>();//
        public static IList<string> MobileList = new List<string>();//
        #endregion
    }
 
}
