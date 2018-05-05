using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;//
using ThreadWorker;
using HtmlAgilityPack;
using CommonPack;
using QiHe.CodeLib.Csv;
using CommonPack.HtmlAgilityPack;
namespace Builder.Data
{
    public class SubjectItem
    {
        public string Subject;
        public string Title;
        public string Url;
    }
    public class SubjectBuilder : Builder
    {
        static SubjectBuilder()
        {
            Save_TheadStart();//
        }

        public static IList<SubjectItem> List = new List<SubjectItem>();//
        SubjectItem item;

        public SubjectBuilder() { }
        public void Init(string url, DocumentWithLinks doc, string taskName, ModeType modeType)
        {
            base.Init(taskName, modeType);

            if (!this.IsTrue)
                return;//
            if (!doc.IsTextContent)
                return;
            Get(url, doc.Doc);
        }
        protected override string GetName()
        {
            return "Subject";//
        }
        private void Get(string url, HtmlDocument doc)
        {
            this.item = new SubjectItem();//
            item.Subject = CommonPack.HtmlAgilityPack.HtmlToSubject.Analytics2(doc);//
            if (this.IsTrue = item.Subject.Length < this.Config.GetParamValue_Int("MinLength"))
                return;

            this.IsTrue =item.Subject.IndexOf("请尝试以下操作") == -1 || item.Subject.IndexOf("HTTP 错误") != -1
                || item.Subject.IndexOf("为技术支持人员提供") != -1;

            if (!this.IsTrue)
                return;//
            item.Url = url;//
            item.Title=GetTitle(doc);// 
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
            lock (List)
            {
                if (item == null)
                    return;
                this.Replace(ref this.item.Subject);
                this.Replace(ref this.item.Title);

                if (!List.Contains(item))
                    List.Add(item);//
                Monitor.PulseAll(List);//
            }
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
                Thread.Sleep(1 * 60 * 1000);//

                if (List.Count > 10)
                    //SaveToCSVFileFile();//
                    SaveToDB();//
            }
        }
        public static void SaveToCSVFileFile()
        {
            lock (List)
            {
                if (List.Count == 0)
                    return;
                ShowMesage("SubjectToCSVFile Start...Count:" + List.Count);//
                CsvData csv = new CsvData();
                CsvRecord r;
                foreach (SubjectItem item in List)
                {
                    r = new CsvRecord();
                    r.Fields.Add(item.Url); r.Fields.Add(item.Title); r.Fields.Add(item.Subject);
                    csv.Records.Add(r);//
                }
                Dictionary<int, FieldFormatOption> formatOptions = new Dictionary<int, FieldFormatOption>();
                formatOptions[0] = new FieldFormatOption(true);
                formatOptions[1] = new FieldFormatOption(true);
                formatOptions[2] = new FieldFormatOption(true);

                StringBuilder sb = new StringBuilder().Append(CsvEncoder.Encode(csv, formatOptions));//
                string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\CSV\" + DateTime.Now.ToString("yyMMdd-HHmmss") + ".csv";//
                IOHelper.SaveTextFile(ref path, ref sb);//
                List.Clear();//
                Monitor.PulseAll(List);//
                ShowMesage("SubjectToCSVFile End...");
            }
        }
        public static void SaveToDB()
        {
            lock (List)
            {
                if (List.Count == 0)
                    return;
                ShowMesage("SubjectToDB Start...Count:" + List.Count);//
                DB.AdvData obj;
                foreach (SubjectItem item in List)
                {
                    obj = new DB.AdvData();
                    obj.Content = item.Subject;
                    obj.Link = item.Url;
                    obj.Title = item.Title;
                    obj.LinkHash = obj.Link.GetHashCode();//
                    obj.State = 0;
                    obj.Key = "";
                    obj.Create();//
                }
                List.Clear();//
                Monitor.PulseAll(List);//
                ShowMesage("SubjectToDB End...");
            }
        }
        #region


        #endregion
    }
}
