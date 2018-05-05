using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                    var data = EnumerableToDataTable(results);
                    Com.Zengfr.Proj.Common.CsvFileHelper.SaveCSV(data, path);
                }
                lastTime = DateTime.Now;
                results.Clear();
                return true;
            }
            return false;
        }
        public static DataTable EnumerableToDataTable<T>(IEnumerable<T> list)
        {
            DataTable dt = new DataTable();
            var ps = typeof(T).GetProperties();
            foreach (PropertyInfo info in ps )
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in ps)
                {
                    row[info.Name] = info.GetValue(t, null)??string.Empty;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        public static void SaveCSV(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            bool flag = !fi.Directory.Exists;
            if (flag)
            {
                fi.Directory.Create();
            }
            var isNewfile = !fi.Exists;
            if (isNewfile)
            {
                fi.Create().Close();
            }
            int columnsCount = dt.Columns.Count;
            int rowsCount = dt.Rows.Count;
            using (FileStream fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    string data = "";
                    if (isNewfile)
                    {
                        for (int i = 0; i < columnsCount; i++)
                        {
                            data += dt.Columns[i].ColumnName.ToString();
                            bool flag2 = i < columnsCount - 1;
                            if (flag2)
                            {
                                data += ",";
                            }
                        }
                        sw.WriteLine(data);
                    }
                    for (int j = 0; j < rowsCount; j++)
                    {
                        data = "";
                        for (int k = 0; k < columnsCount; k++)
                        {
                            string str = dt.Rows[j][k].ToString();
                            str = str.Replace("\"", "\"\"");
                            bool flag3 = str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n');
                            if (flag3)
                            {
                                str = string.Format("\"{0}\"", str);
                            }
                            data += str;
                            bool flag4 = k < columnsCount - 1;
                            if (flag4)
                            {
                                data += ",";
                            }
                        }
                        sw.WriteLine(data);
                    }
                }
            }
        }
    }
}
