using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Zengfr.Proj.Common
{
    public class CsvFileHelper
    {
        public static DataTable EnumerableToDataTable<T>(IEnumerable<T> list)
        {
            DataTable dt = new DataTable();
            var ps = typeof(T).GetProperties();
            foreach (PropertyInfo info in ps)
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in ps)
                {
                    row[info.Name] = info.GetValue(t, null) ?? string.Empty;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(DataTable dt, string fullPath)
        {
            SaveCSV(dt, fullPath, FileMode.Create);
        }
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSVAppend(DataTable dt, string fullPath)
        {
            SaveCSV(dt, fullPath, FileMode.Append);
        }
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(DataTable dt, string fullPath, FileMode fileMode)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
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
            using (FileStream fs = new FileStream(fullPath, fileMode, System.IO.FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    string data = "";
                    if (isNewfile|| fileMode== FileMode.Create)
                    {
                        //写出列名称
                        for (int i = 0; i < columnsCount; i++)
                        {
                            data += dt.Columns[i].ColumnName.ToString();
                            if (i < columnsCount - 1)
                            {
                                data += ",";
                            }
                        }
                    }
                    sw.WriteLine(data);
                    //写出各行数据
                    for (int i = 0; i < rowsCount; i++)
                    {
                        data = "";
                        for (int j = 0; j < columnsCount; j++)
                        {
                            string str = dt.Rows[i][j].ToString();
                            str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                            if (str.Contains(',') || str.Contains('"')
                                || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                            {
                                str = string.Format("\"{0}\"", str);
                            }

                            data += str;
                            if (j < columnsCount - 1)
                            {
                                data += ",";
                            }
                        }
                        sw.WriteLine(data);
                    }
                    sw.Close();
                }
                fs.Close();
            }

        }

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
            DataTable dt = new DataTable();
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists) return dt;
            Encoding encoding = Encoding.UTF8;
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read,FileShare.ReadWrite);

            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
