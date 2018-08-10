using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common;

namespace TaobaoTask
{
    public  class CSVUtil
    {
        public static bool SaveToFile<T>(ref DateTime lastTime, int minutes, List<T> results)
        {
            return SaveToFile<T>(ref lastTime, minutes, results, "0");
        }
        public static bool SaveToFile<T>(ref DateTime lastTime, int minutes, List<T> results,string ftype)
        {
            if ((DateTime.Now - lastTime).TotalMinutes >= minutes)
            {
                if (results.Any())
                {
                    var path = string.Format("{0}\\data\\{1}_{2}.csv", AppDomain.CurrentDomain.BaseDirectory, ftype,DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    var data = CsvFileHelper.EnumerableToDataTable(results);
                    Com.Zengfr.Proj.Common.CsvFileHelper.SaveCSV(data, path);
                }
                lastTime = DateTime.Now;
                results.Clear();
                return true;
            }
            return false;
        }
        public static void SaveCSVCreate(DataTable dt, string fullPath)
        {
            CsvFileHelper.SaveCSV(dt, fullPath);
        }
        public static void SaveCSVAppend(DataTable dt, string fullPath)
        {
            CsvFileHelper.SaveCSVAppend(dt,   fullPath);
        }
    }
}
