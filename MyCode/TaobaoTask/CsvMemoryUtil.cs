using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace TaobaoTask
{
    public class CsvMemoryUtil
    {
        static CsvMemoryUtil csvMemoryUtil;
        public static CsvMemoryUtil Instance
        {
            get
            {
                if (csvMemoryUtil == null)
                    csvMemoryUtil = new CsvMemoryUtil();
                return csvMemoryUtil;
            }
        }
        protected IDictionary<string, bool> Data = new Dictionary<string, bool>();
        public long TotalCount
        {
            get
            {
                return Data.Count;
            }
        }
        protected int SaveIntervalMinute { get; set; }
        protected int SaveIntervalCount { get; set; }
        protected DateTime SaveLastTime { get; set; }
        public void Load(string dir, string colName)
        {

            var fs = Directory.EnumerateFiles(dir, "*.csv", SearchOption.AllDirectories);
            foreach (var f in fs)
            {
                var dt = Com.Zengfr.Proj.Common.CsvFileHelper.OpenCSV(f);
                foreach (DataRow r in dt.Rows)
                {

                    var v = "" + r[colName];
                    if (!Data.ContainsKey(v))
                        Data[v] = true;
                    else
                        Console.WriteLine("duplicate Key:{0},{1}", Path.GetFileName(f), v);
                }
            }
        }
        public string GetLastCreationFile(string dir, string ext)
        {
            var directory = new DirectoryInfo(dir);
            var myFile = directory.GetFiles(ext, SearchOption.AllDirectories)
                         .OrderByDescending(f => f.CreationTime)
                         .FirstOrDefault();
            return myFile == null ? string.Empty : myFile.FullName;
        }
        public void SaveToEnqueue<T>(IList<T> items, Func<T, string> keyFunc)
        {
            lock (Data)
            {
                items = items.Where(t => !Data.ContainsKey(keyFunc.Invoke(t))).ToList();
                foreach (var item in items)
                {
                    var key = keyFunc.Invoke(item);
                    if (!Data.ContainsKey(key))
                    {
                        Data[key] = false;
                    }
                }
            }
            if (items.Count > 0)
            {
                ComLib.Queue.Queues.Enqueue(items);
                Process<T>(true);
            }
        }
        protected bool CheckIsDoProcess<T>()
        {
            var checkIsDoProcess = (this.SaveIntervalMinute > 0 && (DateTime.Now - this.SaveLastTime).TotalMinutes > this.SaveIntervalMinute);
            checkIsDoProcess = checkIsDoProcess || (this.SaveIntervalCount > 0 && ComLib.Queue.Queues.GetQueue<T>().Count > this.SaveIntervalCount);
            return checkIsDoProcess;
        }
        public void Process<T>(bool checkTimeAndCount)
        {
            lock (Data)
            {
                if ((!checkTimeAndCount) || CheckIsDoProcess<T>())
                {
                    do
                    {
                        lock (Data)
                        {
                            ComLib.Queue.Queues.Process<T>();
                            SaveLastTime = DateTime.Now;
                        }
                    }
                    while (ComLib.Queue.Queues.GetQueue<T>().Count > 0);
                }
            }
        }
        public void AddProcessorFor<T>(Action<IList<T>> action, int saveIntervalMinute, int saveIntervalCount)
        {
            this.SaveLastTime = DateTime.Now;
            this.SaveIntervalMinute = saveIntervalMinute;
            this.SaveIntervalCount = saveIntervalCount;
            ComLib.Queue.Queues.AddProcessorFor(500, (IList<T> items) =>
             {
                 if (items.Count > 0)
                     action.Invoke(items);
             });
        }
    }
}
